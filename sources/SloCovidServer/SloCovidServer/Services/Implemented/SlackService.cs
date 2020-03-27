using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SloCovidServer.Services.Abstract;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SloCovidServer.Services.Implemented
{
    public class SlackService : ISlackService
    {
        static readonly JsonSerializerOptions serializationOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        readonly ILogger<SlackService> logger;
        readonly string secret;
        readonly HttpClient client;
        public SlackService(ILogger<SlackService> logger, IConfiguration configuration, HttpClient client)
        {
            this.logger = logger;
            secret = configuration["Slack_Secret"];
            this.client = client;
        }
        public async Task SendNotificationAsync(string text, CancellationToken ct)
        {
            var payload = new Payload("alert", text);
            string content = JsonSerializer.Serialize(payload, serializationOptions);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://slack.com/api/chat.postMessage")
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json"),
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", secret);
            try
            {
                var response = await client.SendAsync(request, ct);
                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning($"Failed to send warning to slack: {response.ReasonPhrase}");
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var slackResponse = JsonSerializer.Deserialize<SlackResponse>(responseContent, serializationOptions);
                    if (!slackResponse.Ok)
                    {
                        logger.LogWarning($"Failed to send warning to slack: {slackResponse.Error}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to send warning to slack");
            }
        }

        struct SlackResponse
        {
            public bool Ok { get; set; }
            public string Channel { get; set; }
            public string Error { get; set; }
        }

        class Payload
        {
            public string Channel { get; }
            public string Text { get; }
            public Payload(string channel, string text)
            {
                Channel = channel;
                Text = text;
            }
        }
    }
}
