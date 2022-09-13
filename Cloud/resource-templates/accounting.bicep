@allowed([
  'Test'
  'Sandbox'
])
param environment string

param sqlAdminLogin string

@secure()
param sqlAdminLoginPassword string

param applicationBase string = 'NZBAAccounting'

@allowed([
  'F1 Free'
  'B1 Basic'
  // 'D1 Shared' does not support linux as the appservice plan OS
])
param planSku string = 'F1 Free'

@allowed([
  'Basic Basic'
])
param dbSku string = 'Basic Basic'

@allowed([
  5
])
param dbCapacity int = 5

param suffix string = '-${substring(uniqueString(resourceGroup().id), 5, 4)}-'

// resources that require a globally unique name
// use the suffix to help ensure uniqueness
var appServiceName = '${applicationBase}App${suffix}${environment}'
var appServicePlanName = '${applicationBase}Plan${environment}'
var dbServerName = '${applicationBase}DbServer${suffix}${environment}'
var dbName = '${applicationBase}Db'
var location = resourceGroup().location
var dotnetVersion = 'DOTNETCORE|5.0'

var dbConnection = 'Server=${dbServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${dbName};MultipleActiveResultSets=true;;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;UID=${sqlAdminLogin};PWD=${sqlAdminLoginPassword}'

// app service plan
resource plan 'Microsoft.Web/serverfarms@2020-09-01' = {
  name: appServicePlanName
  location: resourceGroup().location
  kind: 'linux'
  properties: {
    reserved: true
  }
  sku: {
    tier: first(skip(split(planSku, ' '), 1))
    name: first(split(planSku, ' '))
  }
}

// app service
resource app 'Microsoft.Web/sites@2016-08-01' = {
  name: appServiceName
  location: resourceGroup().location
  properties: {
    siteConfig: {
      linuxFxVersion: dotnetVersion
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: environment
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: reference(insights.id, '2015-05-01').InstrumentationKey
        }  
      ]
      connectionStrings: [
        {
          name: 'SQLServer'
          connectionString: dbConnection
          type: 'SQLAzure'
        }
      ]
    }
    serverFarmId: '/subscriptions/${subscription().subscriptionId}/resourcegroups/${resourceGroup().name}/providers/Microsoft.Web/serverfarms/${appServicePlanName}'
  }

  tags: {
    'hidden-related:/subscriptions/${subscription().subscriptionId}/resourcegroups/${resourceGroup().name}/providers/Microsoft.Web/serverfarms/${appServicePlanName}': 'empty'
  }
}

// application insights
resource insights 'Microsoft.Insights/components@2015-05-01' = {
  name: appServiceName
  kind: 'web'
  location: location
  tags: {
    'hidden-link:${resourceGroup().id}/providers/Microsoft.Web/sites/${appServiceName}': 'Resource'
  }
  properties: {
    Application_Type: 'web'
  }
}

// database server
resource dbServer 'Microsoft.Sql/servers@2020-02-02-preview' = {
  name: dbServerName
  location: location
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminLoginPassword
  }
}

// database
resource db 'Microsoft.Sql/servers/databases@2020-08-01-preview' = {
  name: '${dbServer.name}/${dbName}'
  location: location
  sku: {
    tier: first(skip(split(dbSku, ' '), 1))
    name: first(split(dbSku, ' '))
    capacity: dbCapacity
  }
}

// firewall rule allows the pipeline to access the database 
// in order to apply migrations
resource firewall 'Microsoft.Sql/servers/firewallRules@2020-11-01-preview' = {
  name: '${dbServer.name}/AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0' // '0.0.0.0' represents Azure services
    endIpAddress: '0.0.0.0'
  }
}

output appServiceName string = app.name
output connectionString string = dbConnection
output appServiceUrl string = 'https://${app.properties.defaultHostName}'

