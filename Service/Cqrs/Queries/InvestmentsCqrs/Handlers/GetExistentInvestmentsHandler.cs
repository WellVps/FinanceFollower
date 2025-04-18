using System;
using Domain.Domains.Investments;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using MongoDB.Driver;
using Service.Cqrs.Queries.InvestmentsCqrs.Requests;

namespace Service.Cqrs.Queries.InvestmentsCqrs.Handlers;

public class GetExistentInvestmentsHandler(IInvestmentsRepository investmentsRepository) : IRequestHandler<GetExistentInvestmentsQuery, List<Investments>>
{
    private readonly IInvestmentsRepository _investmentsRepository = investmentsRepository;

    public async Task<List<Investments>> Handle(GetExistentInvestmentsQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterDefinitionBuilder<Investments>().Gt(x => x.TotalAmount, 0);

        var investments = (await _investmentsRepository.GetAll(filter, cancellationToken: cancellationToken)).ToList();

        if (investments is null || investments.Count == 0)
            throw new KeyNotFoundException("No investments found.");

        return investments;
    }
}
