using CourseTech.ChatGptApi.Interfaces;
using CourseTech.Domain.Constants.ChatGptConstants;
using CourseTech.Domain.Dto.Analyzer;
using CourseTech.Domain.Interfaces.UserQueryAnalyzers;
using System.Text.Json;

namespace CourseTech.DAL.UserQueryAnalyzers
{
    public class ChatGptQueryAnalyzer(IChatGptService chatGptService) : IChatGptQueryAnalyzer
    {
        public async Task<ChatGptAnalysResponseDto> AnalyzeUserQuery(string exceptionMessage,
            string userQuery,
            string rightQuery,
            float maxGradeForQuestion)
        {
            var chatGptRequest = string.Format(ChatGptRequestConstants.ChatGptRequest, exceptionMessage, rightQuery, userQuery, maxGradeForQuestion);

            var chatGptResponseJson = await chatGptService.SendMessageToChatGPT(chatGptRequest);

            var result = JsonSerializer.Deserialize<ChatGptAnalysResponseDto>(chatGptResponseJson);

            return result;
        }
    }
}
