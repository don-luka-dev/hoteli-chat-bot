using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Services;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {

        private readonly Kernel _kernel;
        private readonly IChatCompletionService _chatCompletionService;
        private readonly ApplicationDbContext _dbContext;

        public ChatBotController(Kernel kernel, IChatCompletionService chatCompletionService, ApplicationDbContext dbContext)
        {
            _kernel = kernel;
            _chatCompletionService = chatCompletionService;
            _dbContext = dbContext;
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string>> AIEndpoint([FromForm] AIRequestDto request, CancellationToken ct)
        {
            var sessionId = request.SessionId;
            var userInput = request.Message ?? string.Empty;

            string? dataUrl = null;
            if (request.UrlSlike is not null && request.UrlSlike.Length > 0)
            {
                dataUrl = await ConvertImageToBase64Async(request.UrlSlike);
            }

            // 1) Učitaj povijest
            var history = new ChatHistory();
            var messages = await _dbContext.ChatMessages
                .Where(m => m.SessionId == sessionId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync(ct);

            foreach (var m in messages)
            {
                if (m.Role == "user") history.AddUserMessage(m.Content);
                else if (m.Role == "assistant") history.AddAssistantMessage(m.Content);
            }

            // 2) Spremi trenutnu user poruku (bez sentinela)
            var userMsg = new ChatMessage
            {
                SessionId = sessionId,
                Role = "user",
                Content = userInput,
                UrlSlike = dataUrl, // privremeno dok ne izdvojiš u posebnu tablicu
                Timestamp = DateTime.UtcNow
            };
            _dbContext.ChatMessages.Add(userMsg);
            await _dbContext.SaveChangesAsync(ct);

            // 3) Dodaj u runtime history tek tekst poruke
            history.AddUserMessage(userInput);

            var settings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            var requestKernel = _kernel.Clone(); // SK ima Clone()
            requestKernel.Data["sessionId"] = sessionId;

            var result = await _chatCompletionService.GetChatMessageContentAsync(
                history,
                settings,
                requestKernel,
                ct
            );

            var assistantResponse = result.Content ?? string.Empty;

            _dbContext.ChatMessages.Add(new ChatMessage
            {
                SessionId = sessionId,
                Role = "assistant",
                Content = assistantResponse,
                Timestamp = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync(ct);

            return Ok(assistantResponse);
        }


        private static async Task<string> ConvertImageToBase64Async(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var mimeType = file.ContentType;
            var base64 = Convert.ToBase64String(fileBytes);
            return $"data:{mimeType};base64,{base64}";
        }



    }

}
