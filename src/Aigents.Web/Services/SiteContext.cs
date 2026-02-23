using System;

namespace Aigents.Web.Services;

public interface ISiteContext
{
    SitePersona GetPersona(Uri uri);
}

public enum SitePersona
{
    Agent, // Default
    Buy,
    Sell,
    Rent
}

public class SiteContext : ISiteContext
{
    public SitePersona GetPersona(Uri uri)
    {
        try
        {
            if (uri == null) return SitePersona.Agent;

            // 1. Check Query Param (Simple string check)
            var query = uri.Query;
            if (!string.IsNullOrEmpty(query))
            {
                if (query.Contains("site=buy", StringComparison.OrdinalIgnoreCase)) return SitePersona.Buy;
                if (query.Contains("site=sell", StringComparison.OrdinalIgnoreCase)) return SitePersona.Sell;
                if (query.Contains("site=rent", StringComparison.OrdinalIgnoreCase)) return SitePersona.Rent;
                if (query.Contains("site=agent", StringComparison.OrdinalIgnoreCase)) return SitePersona.Agent;
            }

            // 2. Check Path Segment
            var path = uri.AbsolutePath;
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith("/buy", StringComparison.OrdinalIgnoreCase)) return SitePersona.Buy;
                if (path.StartsWith("/sell", StringComparison.OrdinalIgnoreCase)) return SitePersona.Sell;
                if (path.StartsWith("/rent", StringComparison.OrdinalIgnoreCase)) return SitePersona.Rent;
                if (path.StartsWith("/agent", StringComparison.OrdinalIgnoreCase)) return SitePersona.Agent;
            }

            // 3. Check Hostname
            var host = uri.Host;
            if (!string.IsNullOrEmpty(host))
            {
                host = host.ToLowerInvariant();
                if (host.StartsWith("buy.")) return SitePersona.Buy;
                if (host.StartsWith("sell.")) return SitePersona.Sell;
                if (host.StartsWith("rent.")) return SitePersona.Rent;
                if (host.StartsWith("agent.")) return SitePersona.Agent;
            }

            return SitePersona.Agent; // Default
        }
        catch
        {
            return SitePersona.Agent;
        }
    }
}
