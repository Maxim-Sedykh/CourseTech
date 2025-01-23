using CourseTech.ChatGptApi.Constants;
using CourseTech.ChatGptApi.Interfaces;
using CourseTech.ChatGptApi.Models.RequestModels;
using CourseTech.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace CourseTech.ChatGptApi
{
    public class ChatGptService : IChatGptService
    {
        private readonly HttpClient _httpClient;
        private readonly ChatGptSettings _chatGptSettings;

        public ChatGptService(IOptions<ChatGptSettings> chatGptOptions)
        {
            _chatGptSettings = chatGptOptions.Value;
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _chatGptSettings.ApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        }

        public async Task<string> SendMessageToChatGPT(string prompt)
        {
            var requestBody = new ChatGptRequest
            {
                Model = _chatGptSettings.ChatGptModel,
                Messages =
                [
                    new Message { Role = _chatGptSettings.Role, Content = prompt }
                ]
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, MediaTypeNames.Application.Json);

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
                throw new ApplicationException("An error occurred while communicating with the ChatGPT API.", e);
            }
            catch (JsonException e)
            {
                throw new ApplicationException("An error occurred while processing the response from the ChatGPT API.", e);
            }
            finally
            {
                response?.Dispose();
            }
        }

        private string ParseChatGptResponse(string responseBody)
        {
            using JsonDocument jsonDocument = JsonDocument.Parse(responseBody);

            return jsonDocument.RootElement.GetProperty(ChatGptReponsePropertyConstants.FirstLevelResponseProperty)[0]
                .GetProperty(ChatGptReponsePropertyConstants.SecondLevelResponseProperty)
                .GetProperty(ChatGptReponsePropertyConstants.ThirdLevelResponseProperty).GetString();
        }
    }
}
