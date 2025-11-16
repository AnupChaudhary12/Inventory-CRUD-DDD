using Inventory.Application.Contracts.DTOs;
using MediatR;

namespace Inventory.Application.Features.Inventory.Command.AddMillionProductTest;

public record AddMillionProductTestCommand : IRequest<GeneralResponseDto<string>>
{
}
