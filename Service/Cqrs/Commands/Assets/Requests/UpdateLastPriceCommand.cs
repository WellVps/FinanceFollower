using MediatR;

namespace Service.Cqrs.Commands.Assets.Requests;

public record struct UpdateLastPriceCommand (
    string Ticker,
    double LastPrice
) : IRequest<bool>;
