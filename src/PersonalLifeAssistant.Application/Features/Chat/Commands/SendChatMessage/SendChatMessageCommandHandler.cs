using MediatR;
using PersonalLifeAssistant.Application.Common.Interfaces;
using PersonalLifeAssistant.Application.Common.Models;
using PersonalLifeAssistant.Application.Features.Chat.Models;
using PersonalLifeAssistant.Application.Features.Tasks.Models;
using PersonalLifeAssistant.Domain.Entities;

namespace PersonalLifeAssistant.Application.Features.Chat.Commands.SendChatMessage;

public class SendChatMessageCommandHandler : IRequestHandler<SendChatMessageCommand, Result<ChatReplyDto>>
{
    private readonly IAiService _aiService;
    private readonly IChatHistoryRepository _chatHistoryRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public SendChatMessageCommandHandler(
        IAiService aiService,
        IChatHistoryRepository chatHistoryRepository,
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _aiService = aiService;
        _chatHistoryRepository = chatHistoryRepository;
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ChatReplyDto>> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
    {
        var recentMessages = await _chatHistoryRepository.GetRecentByUserAsync(_currentUserService.UserId, 10, cancellationToken);
        var chatReply = await _aiService.SendChatAsync(
            request.Message,
            $"timezone={_currentUserService.TimeZone};user={_currentUserService.Email}",
            recentMessages.Select(m => $"{m.Role}:{m.Message}").ToArray(),
            cancellationToken);

        TaskItem? createdTask = null;
        if (TryBuildTaskRequest(request.Message, out var createTaskRequest))
        {
            var aiAnalysis = await _aiService.AnalyzeTaskAsync(
                createTaskRequest,
                $"timezone={_currentUserService.TimeZone};email={_currentUserService.Email}",
                cancellationToken);

            createdTask = new TaskItem
            {
                UserId = _currentUserService.UserId,
                Title = createTaskRequest.Title,
                Description = createTaskRequest.Description,
                DueDateUtc = createTaskRequest.DueDateUtc,
                EstimatedMinutes = createTaskRequest.EstimatedMinutes,
                Priority = aiAnalysis.SuggestedPriority,
                Category = aiAnalysis.SuggestedCategory,
                AiReasoning = aiAnalysis.Reasoning
            };

            await _taskRepository.AddAsync(createdTask, cancellationToken);
        }

        await _chatHistoryRepository.AddAsync(new ChatHistory
        {
            UserId = _currentUserService.UserId,
            Role = "user",
            Message = request.Message,
            Intent = "user_input"
        }, cancellationToken);

        await _chatHistoryRepository.AddAsync(new ChatHistory
        {
            UserId = _currentUserService.UserId,
            Role = "assistant",
            Message = chatReply.Response,
            Intent = chatReply.Intent
        }, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (createdTask is not null)
        {
            chatReply = new ChatReplyDto
            {
                Response = $"{chatReply.Response} I also created a task for \"{createdTask.Title}\".",
                Intent = chatReply.Intent,
                SuggestedActions = chatReply.SuggestedActions
            };
        }

        return Result<ChatReplyDto>.Success(chatReply);
    }

    private static bool TryBuildTaskRequest(string message, out CreateTaskRequest request)
    {
        var normalized = message.Trim();
        request = new CreateTaskRequest();

        if (string.IsNullOrWhiteSpace(normalized))
        {
            return false;
        }

        normalized = StripLeadingPromptPhrases(normalized);

        var prefixes = new[]
        {
            "create task",
            "create a task",
            "create new task",
            "create a new task",
            "add task",
            "add a task",
            "new task",
            "make task",
            "make a task",
            "remind me to",
            "todo",
            "to do",
            "task"
        };

        var matchedPrefix = prefixes.FirstOrDefault(prefix =>
            normalized.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

        if (matchedPrefix is null)
        {
            return false;
        }

        var title = normalized[matchedPrefix.Length..].Trim(' ', ':', '-', '.');
        title = RemoveLeadingTaskFillers(title);
        if (string.IsNullOrWhiteSpace(title))
        {
            return false;
        }

        request = new CreateTaskRequest
        {
            Title = title,
            Description = message,
            EstimatedMinutes = 30
        };

        return true;
    }

    private static string StripLeadingPromptPhrases(string message)
    {
        var value = message.Trim();
        var leadingPhrases = new[]
        {
            "please ",
            "can you ",
            "could you ",
            "would you ",
            "will you ",
            "hey ",
            "hi "
        };

        var changed = true;
        while (changed)
        {
            changed = false;
            foreach (var phrase in leadingPhrases)
            {
                if (value.StartsWith(phrase, StringComparison.OrdinalIgnoreCase))
                {
                    value = value[phrase.Length..].TrimStart();
                    changed = true;
                }
            }
        }

        return value;
    }

    private static string RemoveLeadingTaskFillers(string title)
    {
        var value = title.Trim();
        var fillers = new[]
        {
            "for ",
            "to ",
            "that says "
        };

        var changed = true;
        while (changed)
        {
            changed = false;
            foreach (var filler in fillers)
            {
                if (value.StartsWith(filler, StringComparison.OrdinalIgnoreCase))
                {
                    value = value[filler.Length..].TrimStart();
                    changed = true;
                }
            }
        }

        return value;
    }
}
