﻿using RestSharp;
using ApiBrasil.Domain;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ApiBrasil
{
    public class GenericCaller
    {
        private readonly ApiBrasilConfiguration _config;

        public GenericCaller(IOptions<ApiBrasilConfiguration> config)
        {
            _config = config.Value;
        }

        public async Task<string> SaveAsync(string type, string action)
        {
            var options = new RestClientOptions("https://cluster-01.apigratis.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/api/v1/{type}/{action}", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("SecretKey", _config.SecretKey ?? "");
            request.AddHeader("PublicToken", _config.PublicToken ?? "");
            request.AddHeader("DeviceToken", _config.DeviceToken ?? "");
            request.AddHeader("Authorization", $"Bearer {_config.Authorization}");
            var body = "";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);

            return JsonSerializer.Serialize(response.Content);
        }
    }

}

