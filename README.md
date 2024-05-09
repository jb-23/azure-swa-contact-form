# Azure Static Web App Contact Form

A contact form written in HTML/CSS & C#, using
Azure Communication Services to send the visitor's message.

To prevent automated form spam, a honeypot technique is used.


## Creating Email Communication Service resources

- Create a Resource Group on Azure
- Create a Communication Service resource in the new resource group
- Also create an Email Communication Service resource
- Go to the new Email Communication Service
- Open "Provision Domains" blade
- Choose (+ Add Domain) > Azure Domain
- Go to the Communication Service resource
- "Email | Domains" blade > [Connect Domain]
- Select the Email Communication Service and click [Connect]
- **"Settings | Keys" blade > copy primary or secondary connection string**
  - Copied value becomes *AZ_EMAIL_CONNECTION_STRING*
- Go to the Email Communication Services Domain resource
- **"Email services | MailFrom addresses" blade > ... > Copy MailFrom address**
  - Copied value becomes *AZ_MAILFROM_ADDRESS*


## Local Testing

For local testing, add the file api/local.settings.json with
the following contents, replacing the values as indicated:

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",

    "AZ_MAILFROM_ADDRESS": " --mailfrom address from Azure portal-- ",
    "AZ_EMAIL_CONNECTION_STRING": " --connection string from Azure portal-- ",
    "CONTACT_EMAIL_ADDRESS": " --your email address to receive messages-- "
  }
}
```


## Deploying to Azure

- Create a git repository, commit and push project
- On Azure, create a Static Web App and deploy this project
  - Build Preset `HTML`
  - App location: `/www`
  - Api location: `/api`
  - Output location: `/`
- Under Settings | Environment variables, add the settings
  AZ_EMAIL_CONNECTION_STRING, AZ_MAILFROM_ADDRESS, CONTACT_EMAIL_ADDRESS
  as in local.settings.json above


## Notes

User will receive a 500 error if they send more than 100 MB, this is determined
by the FUNCTIONS_REQUEST_BODY_SIZE_LIMIT environment variable which Azure
currently forbids setting manually for Static Web App functions, although this
can be played with locally by adding it to local.settings.json.


## License

This project is licensed under the MIT License.
