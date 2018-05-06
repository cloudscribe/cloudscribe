using cloudscribe.Email;
using cloudscribe.Email.ElasticEmail;
using cloudscribe.Email.Mailgun;
using cloudscribe.Email.Senders;
using cloudscribe.Email.SendGrid;
using cloudscribe.Email.Smtp;
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

            services.TryAddScoped<IEmailSenderResolver, ConfigEmailSenderResolver>();

            services.TryAddSingleton<IServiceClientProvider, DefaultServiceClientProvider>();

            return services;
        }

    }
}
