using Domain.Domains.Investments;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using Service.Cqrs.Commands.InvestmentsCqrs.Requests;

namespace Service.Cqrs.Commands.InvestmentsCqrs.Handlers;

public class AddInvestmentEntryHandler: IRequestHandler<AddInvestmentEntryCommand, bool>
{
    private readonly IInvestmentEntriesRepository _investmentRepository;
    private readonly IMediator _mediator;

    public AddInvestmentEntryHandler(
        IInvestmentEntriesRepository investmentRepository,
        IMediator mediator
    )
    {
        _investmentRepository = investmentRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(
        AddInvestmentEntryCommand request,
        CancellationToken cancellationToken
    )
    {
        var investmentsEntriesPayload = request.InvestmentsEntries;
        var firstInvestmentEntry = investmentsEntriesPayload.FirstOrDefault();

        if (firstInvestmentEntry == null)
        {
            return false;
        }

        var investmentEntry = new InvestmentEntry()
        {
            UserId = firstInvestmentEntry.UserId,
            InvestmentDate = firstInvestmentEntry.InvestmentDate,
            TotalAmount = investmentsEntriesPayload.Sum(x => x.Amount),
            TotalTax = investmentsEntriesPayload.Sum(x => x.Tax),
            CreatedAt = DateTime.UtcNow,
            Assets = [.. investmentsEntriesPayload.Select(x => new InvestmentEntry.AssetsList()
            {
                AssetId = x.AssetId,
                Price = x.Price,
                Quantity = x.Quantity,
                Amount = x.Amount,
                Tax = x.Tax
            })]
        };

        investmentEntry.Validate();

        var result = await _investmentRepository.Save(investmentEntry, cancellationToken);

        if(result)
            investmentEntry.Assets.ForEach(async asset =>
                await _mediator.Send(
                    new RecalculateUserInvestmentCommand(
                        investmentEntry.UserId, 
                        investmentEntry.InvestmentDate, 
                        asset
                    ), 
                    cancellationToken));

        return result;
    }
}
