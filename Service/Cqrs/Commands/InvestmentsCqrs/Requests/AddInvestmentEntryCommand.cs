using MediatR;
using Service.DTOs.InvestmentsDTOs;

namespace Service.Cqrs.Commands.InvestmentsCqrs.Requests;

public record struct AddInvestmentEntryCommand(
    List<InvestmentsEntriesDTO> InvestmentsEntries
): IRequest<bool>;