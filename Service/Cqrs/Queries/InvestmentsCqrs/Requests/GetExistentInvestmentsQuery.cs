using Domain.Domains.Investments;
using MediatR;

namespace Service.Cqrs.Queries.InvestmentsCqrs.Requests;

public record struct GetExistentInvestmentsQuery(): IRequest<List<Investments>>;