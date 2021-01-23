﻿using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.EmailService
{
    public class EmailSender : IEmailSender
    {

        private readonly EmailConfig _emailConfig;

        public EmailSender(EmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public async Task SendEmailAsync(Message message)
        {
            var emailMesage = CreateEmailMessage(message);
            await SendAsync(emailMesage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("%from%",_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };
            if (message.Attachments != null && message.Attachments.Any())
            {
                int i = 1;
                foreach (var attachment in message.Attachments)
                {
                    bodyBuilder.Attachments.Add($"face-{i}.jpg", attachment);
                    i++;
                }
            }
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage emailMesage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, false);
                    await client.SendAsync(emailMesage);
                }
                catch (Exception ex)
                {
                    //This needs to be logged actually
                    Console.Out.WriteLine("" + ex.Message);
                    throw;

                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
