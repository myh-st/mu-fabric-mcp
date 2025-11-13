// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// This Bicep file creates the test resources required for Fabric.Mcp.Tools.Analytics live tests
// Currently, Fabric resources are managed through the Fabric portal and REST APIs
// This file serves as a placeholder for future infrastructure automation

@description('The name of the test environment')
param environmentName string = 'fabricanalyticstest'

@description('Location for all resources')
param location string = resourceGroup().location

@description('The tags to apply to all resources')
param tags object = {}

// Output values that would be used by the tests
output FABRIC_TENANT_ID string = subscription().tenantId
output RESOURCE_GROUP_NAME string = resourceGroup().name
output ENVIRONMENT_NAME string = environmentName
