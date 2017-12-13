
http://asp.net-hacker.rocks/2016/07/22/deploy-aspnetcore-to-azure.html

http://azure.microsoft.com/en-us/blog/deploy-to-azure-button-for-azure-websites-2/

http://blogs.msdn.com/b/webdev/archive/2015/09/16/deploy-to-azure-from-github-with-application-insights.aspx

Automated command-line deployment to windows azure using management API.
https://github.com/endjin/DeployToAzure

## Configure Blob Storage for Data protection keys

Create a blob storage 
https://docs.microsoft.com/en-us/azure/storage/common/storage-create-storage-account

Get the connection string for your blob storage and put it in appsettings.json or environment variables on azure
https://docs.microsoft.com/en-us/azure/storage/common/storage-configure-connection-string

You can find your storage account's connection strings in the Azure portal. Navigate to SETTINGS > Access keys in your storage account's menu blade to see connection strings for both primary and secondary access keys.

 "AppSettings": {
    "UseSsl": false,
    "UseAzureBlobForDataProtection": true,
    "DataProtectionBlobStorageConnectionString": "yourconnectionstring"

  }
  
  for environment variables the syntax is like this:
  AppSettings:UseAzureBlobForDataProtection
  AppSettings:DataProtectionBlobStorageConnectionString