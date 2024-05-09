using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

using Azure;
using Azure.Communication.Email;

namespace JasonBamford.SWAContactForm
{
    public class SendMessage
    {
        [Function("send-message")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            var contactAddress = Environment.GetEnvironmentVariable("CONTACT_EMAIL_ADDRESS");
            var mailFromAddress = Environment.GetEnvironmentVariable("AZ_MAILFROM_ADDRESS");
            var connectionString = Environment.GetEnvironmentVariable("AZ_EMAIL_CONNECTION_STRING");

            var form = await req.ReadFormAsync();

            int botScore = 0;
            string name = "", email = "", message = "", honey = "";

            var emailFields = form["email"];

            if (emailFields.Count != 2) {
                botScore = 99;
            } else {
                name = form["name"].ToString();
                email = form["email"][0] ?? "";
                message = form["message"].ToString();
                honey = form["email"][1] ?? "";

                if (honey != "") ++botScore;
            }

            if (botScore > 0) {  /* make bot happy so they go away */
                return new OkObjectResult("Success, thank you.");
            }

            /* EmailClient.SendAsync html-escapes the plainTextContent argument,
               which is wrapped in <pre> tags and sent in an HTML email,
               so sanitising emailContent is not required. */

            var emailSubject = "Contact form message";
            var emailContent = $"Message from {name} <{email}>\n\n{message}";

            try {
                var emailClient = new EmailClient(connectionString);

                var emailSendOperation = await emailClient.SendAsync(
                    wait: WaitUntil.Started,
                    senderAddress: mailFromAddress,
                    recipientAddress: contactAddress,
                    subject: emailSubject,
                    htmlContent: null,
                    plainTextContent: emailContent);

                return new RedirectResult("/receipt");
            }
            catch (Exception) {
                return new RedirectResult("/failure");
            }
        }
    }
}
