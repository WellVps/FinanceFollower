using System;

namespace Service.DTOs.InvestmentsDTOs;

public record struct InvestmentsEntriesDTO(
    string UserId,
    string AssetId,
    double Price,
    double Quantity,
    double Amount,
    double Tax,
    DateTime InvestmentDate
);