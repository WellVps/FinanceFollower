using System;
using System.Linq.Expressions;
using Domain.Domains.Investments;
using Infraestructure.Repositories.Interfaces;
using MediatR;
using Service.Cqrs.Commands.InvestmentsCqrs.Requests;
using Service.Cqrs.Queries.InvestmentsCqrs.Requests;

namespace Service.Cqrs.Commands.InvestmentsCqrs.Handlers;

public class RecalculateUserInvestmentHandler : IRequestHandler<RecalculateUserInvestmentCommand, bool>
{
    private readonly IInvestmentsRepository _investmentsRepository;
    private readonly IMediator _mediator;

    public RecalculateUserInvestmentHandler(IInvestmentsRepository investmentsRepository, IMediator mediator)
    {
        _investmentsRepository = investmentsRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(RecalculateUserInvestmentCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var asset = request.Asset;

        Investments? investment;

        try
        {
            investment = await _mediator.Send(new GetInvestmentByUserIdAndAssetQuery(userId, asset.AssetId), cancellationToken);
        }
        catch (KeyNotFoundException)
        {
            investment = null;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the investment.", ex);
        }

        if(investment == null)
        {
            investment = new Investments(
                userId, 
                asset.AssetId,
                request.BoughtDate,
                asset.Quantity,
                asset.Price,
                asset.Amount,
                asset.Price,
                asset.Amount
            );

            return await _investmentsRepository.Save(investment, cancellationToken);
        }

        // calculate the new average price
        var newAveragePrice = (investment.AveragePrice * investment.Quantity + asset.Price * asset.Quantity) /
                              (investment.Quantity + asset.Quantity);

        var quantity = investment.Quantity + asset.Quantity;

        var updateValues = new List<(Expression<Func<Investments, object>>, object)>
        {
            (investment => investment.AveragePrice, newAveragePrice),
            (investment => investment.Quantity, quantity),
            (investment => investment.TotalInvested, investment.TotalInvested + asset.Amount),
            (investment => investment.Price, asset.Price),
            (investment => investment.TotalAmount, quantity * asset.Price),
            (investment => investment.LastBought, request.BoughtDate)
        };
        
        return await _investmentsRepository.Update(
            x => x.Id == investment.Id,
            cancellationToken,
            [.. updateValues]
        );
    }
}
