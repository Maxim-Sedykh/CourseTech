using CourseTech.ChatGptApi.Interfaces;
using CourseTech.ChatGptApi.Models;
using CourseTech.Domain.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ILogger = Serilog.ILogger;

namespace CourseTech.ChatGptApi;

/// <summary>
/// Сервис для работы с ChatGPT
/// </summary>
public class ChatGptService : IChatGptService
{
    private readonly HttpClient _httpClient;
    private readonly ChatGptSettings _settings;
    private readonly ILogger _logger;

    public ChatGptService(
        HttpClient httpClient,
        IOptions<ChatGptSettings> options,
        ILogger logger)
    {
        _settings = options.Value;
        _logger = logger;

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
    }

    /// <inheritdoc/>
    public async Task<string> SendMessageToChatGPT(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt)) return string.Empty;

        var requestBody = new GptRequest(_settings.ChatGptModel,
        [
            new GptMessage(_settings.Role, prompt)
        ]);

        try
        {
            using var response = await _httpClient.PostAsJsonAsync("", requestBody);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GptResponse>();

            return result?.Choices.FirstOrDefault()?.Message.Content ?? string.Empty;
        }
        catch (HttpRequestException ex)
        {
            _logger.Error(ex, "GPT API request failed for prompt: {Prompt}", prompt);
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error while communicating with GPT API");
            throw;
        }
    }
}
