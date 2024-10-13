@description('The name of the resource group')
param resourceGroupName string

@description('The location of the resources')
param location string = resourceGroup().location

@description('The name of the Key Vault')
param keyVaultName string

@description('The name of the Azure Cognitive Search resource')
param searchServiceName string

@description('The name of the SQL Server')
param sqlServerName string

@description('The name of the SQL Database')
param sqlDatabaseName string

@description('The admin username for the SQL Server')
param sqlAdminUsername string

@description('The admin password for the SQL Server')
@secure()
param sqlAdminPassword string

@description('The name of the Storage Account')
param storageAccountName string

@description('The name of the OpenAI resource')
param openAIResourceName string

resource keyVault 'Microsoft.KeyVault/vaults@2021-06-01-preview' = {
  name: keyVaultName
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: []
  }
}

resource searchService 'Microsoft.Search/searchServices@2020-08-01' = {
  name: searchServiceName
  location: location
  sku: {
    name: 'standard'
  }
  properties: {
    replicaCount: 1
    partitionCount: 1
  }
}

resource sqlServer 'Microsoft.Sql/servers@2021-02-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdminUsername
    administratorLoginPassword: sqlAdminPassword
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2021-02-01-preview' = {
  name: '${sqlServerName}/${sqlDatabaseName}'
  location: location
  properties: {
    sku: {
      name: 'S0'
      tier: 'Standard'
    }
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
  }
}

resource openAIResource 'Microsoft.CognitiveServices/accounts@2021-04-30' = {
  name: openAIResourceName
  location: location
  sku: {
    name: 'S0'
  }
  kind: 'OpenAI'
  properties: {
    apiProperties: {
      qnaRuntimeEndpoint: 'https://<your-openai-endpoint>'
    }
  }
}

output keyVaultId string = keyVault.id
output searchServiceId string = searchService.id
output sqlServerId string = sqlServer.id
output sqlDatabaseId string = sqlDatabase.id
output storageAccountId string = storageAccount.id
output openAIResourceId string = openAIResource.id
