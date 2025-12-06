// ═══════════════════════════════════════════════════════════════
// AIGENTS - BOOTSTRAP INFRASTRUCTURE
// ═══════════════════════════════════════════════════════════════
// Run this FIRST to create the Container Registry
// Then run main.bicep for the full deployment
// ═══════════════════════════════════════════════════════════════

targetScope = 'subscription'

@description('Azure region')
param location string = 'australiaeast'

@description('Resource group name')
param resourceGroupName string = 'aigents-rg'

@description('Azure Container Registry name (must be globally unique)')
param acrName string = 'aigentsacr${uniqueString(subscription().id)}'

// ───────────────────────────────────────────────────────────────
// RESOURCE GROUP
// ───────────────────────────────────────────────────────────────

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: resourceGroupName
  location: location
  tags: {
    application: 'aigents'
    purpose: 'AI Real Estate Platform'
  }
}

// ───────────────────────────────────────────────────────────────
// CONTAINER REGISTRY
// ───────────────────────────────────────────────────────────────

module acr 'acr.bicep' = {
  name: 'acr-deployment'
  scope: rg
  params: {
    name: acrName
    location: location
  }
}

// ───────────────────────────────────────────────────────────────
// OUTPUTS
// ───────────────────────────────────────────────────────────────

output resourceGroupName string = rg.name
output acrName string = acr.outputs.acrName
output acrLoginServer string = acr.outputs.loginServer
