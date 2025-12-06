// ═══════════════════════════════════════════════════════════════
// CHAT FEATURE - VERTICAL SLICE
// ═══════════════════════════════════════════════════════════════
// Contains: Endpoint, Request, Response, Handler, Validator
// All in one file for cohesion.
// ═══════════════════════════════════════════════════════════════

using Aigents.Domain.Entities;
using Aigents.Infrastructure.Data;
using Aigents.Infrastructure.Services.AI;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aigents.Api.Features.Chat;

// ───────────────────────────────────────────────────────────────
// ENDPOINT
// ───────────────────────────────────────────────────────────────

public class SendMessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/chat", async (SendMessageRequest request, ISender sender) =>
        {
            var result = await sender.Send(new SendMessageCommand(
                request.ConversationId,
                request.UserId,
                request.Messages,
                request.Mode
            ));
            
            return Results.Ok(result);
        })
        .WithName("SendMessage")
        .WithOpenApi()
        .Produces<SendMessageResponse>()
        .ProducesValidationProblem();
    }
}

// ───────────────────────────────────────────────────────────────
// REQUEST / RESPONSE
// ───────────────────────────────────────────────────────────────

public record SendMessageRequest(
    Guid? ConversationId,
    Guid UserId,
    List<MessageDto> Messages,
    string Mode
);

public record MessageDto(string Role, string Content);

public record SendMessageResponse(
    Guid ConversationId,
    string Content,
    int TokensUsed
);

// ───────────────────────────────────────────────────────────────
// COMMAND
// ───────────────────────────────────────────────────────────────

public record SendMessageCommand(
    Guid? ConversationId,
    Guid UserId,
    List<MessageDto> Messages,
    string Mode
) : IRequest<SendMessageResponse>;

// ───────────────────────────────────────────────────────────────
// VALIDATOR
// ───────────────────────────────────────────────────────────────

public class SendMessageValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");

        RuleFor(x => x.Messages)
            .NotEmpty()
            .WithMessage("At least one message is required");

        RuleFor(x => x.Mode)
            .Must(m => m == "buy" || m == "sell")
            .WithMessage("Mode must be 'buy' or 'sell'");
    }
}

// ───────────────────────────────────────────────────────────────
// HANDLER
// ───────────────────────────────────────────────────────────────

public class SendMessageHandler : IRequestHandler<SendMessageCommand, SendMessageResponse>
{
    private readonly AigentsDbContext _db;
    private readonly IAiService _aiService;
    private readonly ILogger<SendMessageHandler> _logger;

    public SendMessageHandler(
        AigentsDbContext db, 
        IAiService aiService,
        ILogger<SendMessageHandler> logger)
    {
        _db = db;
        _aiService = aiService;
        _logger = logger;
    }

    public async Task<SendMessageResponse> Handle(SendMessageCommand request, CancellationToken ct)
    {
        // Get or create conversation
        Conversation conversation;
        
        if (request.ConversationId.HasValue)
        {
            conversation = await _db.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == request.ConversationId.Value, ct)
                ?? throw new InvalidOperationException("Conversation not found");
        }
        else
        {
            var mode = request.Mode.ToLower() == "sell" ? AgentMode.Sell : AgentMode.Buy;
            
            conversation = new Conversation
            {
                UserId = request.UserId,
                Mode = mode
            };
            
            _db.Conversations.Add(conversation);
        }

        // Save user message
        var lastMessage = request.Messages.Last();
        var userMessage = new Message
        {
            ConversationId = conversation.Id,
            Role = MessageRole.User,
            Content = lastMessage.Content
        };
        _db.Messages.Add(userMessage);

        // Call AI
        var aiMessages = request.Messages.Select(m => new ChatMessage
        {
            Role = m.Role,
            Content = m.Content
        });

        var aiResponse = await _aiService.ChatAsync(aiMessages, request.Mode, ct);

        // Save AI response
        var assistantMessage = new Message
        {
            ConversationId = conversation.Id,
            Role = MessageRole.Assistant,
            Content = aiResponse.Content,
            TokensUsed = aiResponse.TokensUsed,
            ModelUsed = aiResponse.Model
        };
        _db.Messages.Add(assistantMessage);

        // Update conversation
        conversation.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Chat message processed. ConversationId: {ConversationId}, Tokens: {Tokens}",
            conversation.Id, aiResponse.TokensUsed);

        return new SendMessageResponse(
            conversation.Id,
            aiResponse.Content,
            aiResponse.TokensUsed
        );
    }
}
