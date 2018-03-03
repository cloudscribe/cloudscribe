using cloudscribe.Messaging.Email;
using cloudscribe.Messaging.Email.ElasticEmail;
using cloudscribe.Messaging.Email.Mailgun;
using cloudscribe.Messaging.Email.SendGrid;
using cloudscribe.Messaging.Email.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeEmailSenders(
            this IServiceCollection services, 
            IConfiguration configuration
            )
        {
            services.TryAddScoped<ISmtpOptionsProvider, ConfigSmtpOptionsProvider>();
            services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
            services.AddScoped<IEmailSender, SmtpEmailSender>();

            services.TryAddScoped<ISendGridOptionsProvider, ConfigSendGridOptionsProvider>();
            services.Configure<SendGridOptions>(configuration.GetSection("SendGridOptions"));
            services.AddScoped<IEmailSender, SendGridEmailSender>();

            services.TryAddScoped<IMailgunOptionsProvider, ConfigMailgunOptionsProvider>();
            services.Configure<MailgunOptions>(configuration.GetSection("MailgunOptions"));
            services.AddScoped<IEmailSender, MailgunEmailSender>();

            services.TryAddScoped<IElasticEmailOptionsProvider, ConfigElasticEmailOptionsProvider>();
            services.Configure<ElasticEmailOptions>(configuration.GetSection("ElasticEmailOptions"));
            services.AddScoped<IEmailSender, ElasticEmailSender>();
            
            return services;
        }

    }
}
