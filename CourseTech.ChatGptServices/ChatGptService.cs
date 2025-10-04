using CourseTech.ChatGptApi.Constants;
using CourseTech.ChatGptApi.Interfaces;
using CourseTech.Domain.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using ILogger = Serilog.ILogger;

namespace CourseTech.ChatGptApi;

public class ChatGptService : IChatGptService
{
    private readonly HttpClient _httpClient;
    private readonly ChatGptSettings _chatGptSettings;
    private readonly ILogger _logger;

    public ChatGptService(IOptions<ChatGptSettings> chatGptOptions, ILogger logger)
    {
        _chatGptSettings = chatGptOptions.Value;
        _httpClient = new HttpClient();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _chatGptSettings.ApiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        _logger = logger;
    }

    public async Task<string> SendMessageToChatGPT(string prompt)
    {
        var requestBody = new
        {
            model = _chatGptSettings.ChatGptModel,
            messages = new[]
            {
                new { role = _chatGptSettings.Role, content = prompt }
            }
        };

        string json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = null;
        try
        {
            response = await _httpClient.PostAsync(_chatGptSettings.BaseUrl, content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return ParseChatGptResponse(responseBody);
        }
        catch (HttpRequestException e)
        {
            _logger.Error(e, e.Message);

            throw;
        }
        finally
        {
            response?.Dispose();
        }
    }

    private static string ParseChatGptResponse(string responseBody)
    {
        JObject jsonObject = JObject.Parse(responseBody);

        return jsonObject[ChatGptReponsePropertyConstants.FirstLevelResponseProperty][0]
            [ChatGptReponsePropertyConstants.SecondLevelResponseProperty]
            [ChatGptReponsePropertyConstants.ThirdLevelResponseProperty].ToString();

    }
}
