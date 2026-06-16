using DriversTestEvaluation.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DriversTestEvaluation.Business.Analyzers
{
    public class SimulatorAnalyzer
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public SimulatorAnalyzer(IConfiguration config)
        {
            _httpClient = new HttpClient();

            _baseUrl = config["SimulatorApi:BaseUrl"];
                



        }

        public async Task<JsonEntry> AnalyzeJsonEntry()
        {
            var response = await _httpClient.GetAsync(_baseUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"API error: {await response.Content.ReadAsStringAsync()}"
                );
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<JsonEntry>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new JsonEntry();
        }
    }
}
