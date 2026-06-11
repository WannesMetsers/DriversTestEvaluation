using System;
using System.Collections.Generic;
using System.Text;
using DriversTestEvaluation.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using DriversTestEvaluation.Data.Context;

namespace DriversTestEvaluation.Business.Analyzers
{
  

    public class LlavaVisionAnalyzer : IVisionAnalyzer
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        

        public LlavaVisionAnalyzer(IConfiguration config)
        {
            _httpClient = new HttpClient();

            _baseUrl = config["Ollama:BaseUrl"]
                ?? "http://localhost:11434/api/chat";

           
           
        }

        public async Task<JsonEntry> AnalyzeJsonEntry(JsonEntry previousJson)
        {
            Console.WriteLine("Generating next JsonEntry...");

            if (previousJson == null)
            {
                previousJson = new JsonEntry
                {
                    Position = new double[] { 0, 0 },
                    Speed_kmh = 0,
                    SpaceCarInFront = 50,
                    ColorTrafficLight = "Green",
                    InFrontOfTrafficLight = false,
                    SpeedLimit = 50
                };
            }

            var previousStateJson = JsonSerializer.Serialize(previousJson);



            var requestBody = new
            {
                model = "llava", // or whichever model you use
                messages = new[]
                {
            new
            {
                role = "user",
                content =
$@"
You are a traffic simulation API.

You will receive the previous vehicle state.

Generate the NEXT vehicle state.

SCHEMA:

{{
  ""Position"": [x, y],
  ""Speed_kmh"": number,
  ""SpaceCarInFront"": number,
  ""ColorTrafficLight"": ""Red"" | ""Yellow"" | ""Green"",
  ""InFrontOfTrafficLight"": boolean,
  ""SpeedLimit"": number
}}

RULES:

- Generate ONLY the next state.
- Position should move realistically.
- Position x has to be a number.
- Position y has to be a number.
- Speed changes should be gradual.
- Traffic light should usually stay the same.
- If the light is red and the car is approaching it, normally slow down.
- If the space in front is low, normally slow down.

HUMAN ERROR RULE (IMPORTANT):

Every 5–10 steps MUST include a mistake.
Mistakes must be VARIED and NOT repetitive.

You MUST rotate between these categories:

1. TRAFFIC VIOLATIONS
- Run red light
- Run yellow light aggressively
- Ignore speed limit completely (> +30 km/h)

2. DISTANCE ERRORS
- Follow dangerously close (< 5m)
- Sudden tailgating after acceleration

3. IMPULSIVE BEHAVIOR
- Hard acceleration when approaching obstacle
- Hard braking too late (delayed reaction)

4. BAD JUDGMENT
- Speed up when traffic light is turning red
- Fail to slow down when space is shrinking
- Maintain speed in unsafe conditions

5. RANDOM HUMAN ERROR (rare but important)
- Inconsistent speed (jerky driving)
- Overcorrection (speed up → brake → speed up quickly)

IMPORTANT RULES:
- Do NOT repeat the same mistake twice in a row
- Ensure at least 3 different mistake types appear every 10 outputs
- Mistakes must be realistic but clearly unsafe
- Sometimes combine 2 mistakes in one state

OUTPUT RULES:
- Output ONLY JSON
- No markdown
- No explanation

PREVIOUS STATE:

{previousStateJson}
"
            }
        },
                stream = false
            };

            var json = JsonSerializer.Serialize(requestBody);

            var response = await _httpClient.PostAsync(
                _baseUrl,
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(responseString);

            var doc = JsonDocument.Parse(responseString);

            var text = doc.RootElement
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(text))
                return new JsonEntry();

            text = ExtractJson(text);

            try
            {
                return JsonSerializer.Deserialize<JsonEntry>(
                    text,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                ) ?? new JsonEntry();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(text);
                return new JsonEntry();
            }
        }
        public async Task<VisionResult> AnalyzeAsync(byte[] screenshot)
        {
            Console.WriteLine("Analyzing with LLAVA...");

            var base64Image = Convert.ToBase64String(screenshot);

            var requestBody = new
            {
                model = "llava",
                messages = new[]
                {
                new
                {
                    role = "user",
                    content =
//==========================================
//for fake jsons
//==========================================
                    @"You are a machine API. Please disregard the image being send Make up a json for testing perpuses

trafficLight must be exactly one of:
-""red""
- ""yellow""
- ""green""


There can only be a trafficLight color or one TRUE 

RULES:
-Output ONLY JSON
-No text
- No markdown
- No explanation


If you output anything else, it is invalid.

Return EXACTLY:

{
  ""trafficLight"": ""red"",
  ""speeding"": false,
  ""laneDeparture"": false,
  ""collisionRisk"": false,
  ""visibleCars"": 0
}"
//==========================================
//for ai
//==========================================
//                    @"You are a machine API. That is also a driver evaluator.


//trafficLight must be exactly one of:
//- ""red""
//- ""yellow""
//- ""green""
//- ""none""

//Use ""none"" if:
//- no traffic light is visible
//- the traffic light is too far away to identify
//- the traffic light is not applicapble to the player

//RULES:
//- Output ONLY JSON
//- No text
//- No markdown
//- No explanation

//If you output anything else, it is invalid.

//Return EXACTLY:

//{
//  ""trafficLight"": ""red"",
//  ""speeding"": false,
//  ""laneDeparture"": false,
//  ""collisionRisk"": false,
//  ""visibleCars"": 0
//}"
,
                    images = new[]
                    {
                        base64Image
                    }
                }
            },
                stream = false
            };

            var json = JsonSerializer.Serialize(requestBody);

            var response = await _httpClient.PostAsync(
                _baseUrl,
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            var responseString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responseString))
            {
                Console.WriteLine("Empty response from Ollama");
                return new VisionResult();
            }
           

            if (string.IsNullOrWhiteSpace(responseString))
            {
                Console.WriteLine("Empty response from Ollama");
                return new VisionResult();
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ollama API error: {responseString}");
            }

            var doc = JsonDocument.Parse(responseString);

            var text = doc.RootElement
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(text))
                return new VisionResult();

            // remove markdown
            text = text.Replace("```json", "")
                       .Replace("```", "")
                       .Trim();

            // extract JSON safely
            var finalJson = ExtractJson(text);

            if (string.IsNullOrWhiteSpace(finalJson))
            {
                Console.WriteLine("No valid JSON returned:");
                Console.WriteLine(text);
                return new VisionResult();
            }

            try
            {
                var result = JsonSerializer.Deserialize<VisionResult>(
                    finalJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new VisionResult();

                Console.WriteLine("VISION RESULT:");
                Console.WriteLine(JsonSerializer.Serialize(result));

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("JSON parse failed:");
                Console.WriteLine(finalJson);
                Console.WriteLine(ex.Message);
                return new VisionResult();
            }
        }

        static string ExtractJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            // remove markdown fences
            input = input.Replace("```json", "")
                         .Replace("```", "")
                         .Trim();

            // find first {
            int start = input.IndexOf('{');
            int end = input.LastIndexOf('}');

            if (start == -1 || end == -1 || end <= start)
                return "";

            return input.Substring(start, end - start + 1);
        }
    }
}
