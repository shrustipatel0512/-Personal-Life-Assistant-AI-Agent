using MediatR;
using PersonalLifeAssistant.Application.Common.Models;
using PersonalLifeAssistant.Application.Features.Chat.Models;

namespace PersonalLifeAssistant.Application.Features.Chat.Commands.SendChatMessage;

public record SendChatMessageCommand(string Message) : IRequest<Result<ChatReplyDto>>;
