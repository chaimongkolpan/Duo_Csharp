using System;
using DuoUniversal;
using Microsoft.Extensions.Configuration;

namespace DuoSecurity
{
    public interface IDuoClientProvider
    {
        public Client GetDuoClient();
    }

    public class DuoClientProvider : IDuoClientProvider
    {
        private string ClientId { get; }
        private string ClientSecret { get; }
        private string ApiHost { get; }
        private string RedirectUri { get; }

        public DuoClientProvider(IConfiguration config)
        {
            ClientId = config.GetValue<string>("Client ID");
            ClientSecret = config.GetValue<string>("Client Secret");
            ApiHost = config.GetValue<string>("API Host");
            RedirectUri = config.GetValue<string>("Redirect URI");
        }

        public Client GetDuoClient()
        {
            if (string.IsNullOrWhiteSpace(ClientId))
            {
                throw new DuoException("A 'Client ID' configuration value is required in the appsettings file.");
            }
            if (string.IsNullOrWhiteSpace(ClientSecret))
            {
                throw new DuoException("A 'Client Secret' configuration value is required in the appsettings file.");
            }
            if (string.IsNullOrWhiteSpace(ApiHost))
            {
                throw new DuoException("An 'Api Host' configuration value is required in the appsettings file.");
            }
            if (string.IsNullOrWhiteSpace(RedirectUri))
            {
                throw new DuoException("A 'Redirect URI' configuration value is required in the appsettings file.");
            }

            return new ClientBuilder(ClientId, ClientSecret, ApiHost, RedirectUri).Build();
        }
    }
}



