// ═══════════════════════════════════════════════════════════════
// AZURE CONTAINER REGISTRY MODULE
// ═══════════════════════════════════════════════════════════════

@description('ACR name')
param name string

@description('Location')
param location string

resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: name
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
  tags: {
    application: 'aigents'
  }
}

output acrName string = acr.name
output loginServer string = acr.properties.loginServer
