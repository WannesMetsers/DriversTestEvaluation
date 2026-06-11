//using DriversTestEvaluation.Core.Models;
//using Microsoft.Extensions.Configuration;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;

//public class GeminiVisionAnalyzer : IVisionAnalyzer
//{
//    private readonly HttpClient _httpClient;
//    private readonly string _apiKey;
//    private readonly string _baseUrl;

//    public GeminiVisionAnalyzer(IConfiguration config)
//    {
//        _httpClient = new HttpClient();
//        _apiKey = config["GeminiApi:ApiKey"]
//            ?? throw new InvalidOperationException("GeminiApi:ApiKey is missing");

//        _baseUrl = config["GeminiApi:BaseUrl"]
//            ?? throw new InvalidOperationException("GeminiApi:BaseUrl is missing");

//    }

//    public async Task<VisionResult> AnalyzeAsync(byte[] screenshot)
//    {
//        Console.WriteLine("Analyzing");
//        var base64Image = Convert.ToBase64String(screenshot);

//        var requestBody = new
//        {
//            generationConfig = new
//            {
//                responseMimeType = "application/json"
//            },
//            contents = new[]
//            {
//                new
//                {
//                    parts = new object[]
//                    {
//                        new
//                        {
//                            text =
//                            @"You are a driving test examiner AI.
//Analyze this image from a driving simulation.

//Return ONLY valid JSON in this format:
//{
//  ""trafficLight"": ""red|yellow|green|none"",
//  ""speeding"": boolean,
//  ""laneDeparture"": boolean,
//  ""collisionRisk"": boolean,
//  ""visibleCars"": number
//}"
//                        },
//                        new
//                        {
//                            inline_data = new
//                            {
//                                mime_type = "image/png",
//                                data = base64Image
//                            }
//                        }
//                    }
//                }
//            }
//        };

//        var url = $"{_baseUrl}{_apiKey}";

//        var json = JsonSerializer.Serialize(requestBody);

//        var response = await _httpClient.PostAsync(
//            url,
//            new StringContent(json, Encoding.UTF8, "application/json")
//        );

//        var responseString = await response.Content.ReadAsStringAsync();

//        if (!response.IsSuccessStatusCode)
//        {
//            throw new Exception($"Gemini API error: {responseString}");
//        }

//        using var doc = JsonDocument.Parse(responseString);
//        Console.WriteLine(responseString);
//        var text = doc.RootElement
//            .GetProperty("candidates")[0]
//            .GetProperty("content")
//            .GetProperty("parts")[0]
//            .GetProperty("text")
//            .GetString();

//        // Gemini returns JSON as a string → parse it
//        text = text!
//    .Replace("```json", "")
//    .Replace("```", "")
//    .Trim();

//        var visionResult = JsonSerializer.Deserialize<VisionResult>(text,
//            new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });

//        return visionResult!;
//    }
//}