using CourseTech.ChatGptApi.Constants;
using CourseTech.ChatGptApi.Interfaces;
using CourseTech.Domain.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

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
            var requestBody = new
            {
                model = _chatGptSettings.ChatGptModel,
                messages = new[]
                {
                    new { role = "user", content = prompt }
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
            JObject jsonObject = JObject.Parse(responseBody);

            return jsonObject[ChatGptReponsePropertyConstants.FirstLevelResponseProperty][0]
                [ChatGptReponsePropertyConstants.SecondLevelResponseProperty]
                [ChatGptReponsePropertyConstants.ThirdLevelResponseProperty].ToString();

        }
    }
}
