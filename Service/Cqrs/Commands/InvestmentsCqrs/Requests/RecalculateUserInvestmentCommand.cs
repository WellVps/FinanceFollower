using System;
using Domain.Domains.Investments;
using MediatR;

namespace Service.Cqrs.Commands.InvestmentsCqrs.Requests;

public record struct RecalculateUserInvestmentCommand(
    string UserId,
    DateTime BoughtDate,
    InvestmentEntry.AssetsList Asset
) : IRequest<bool>;