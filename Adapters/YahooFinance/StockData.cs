using BaseApiAdapter.Api;
using YahooFinanceApi;

namespace YahooFinance;

public class StockData: BaseApi
{
    public override async Task<double> GetAssetPrice(string symbol)
    {
        var sec = await Yahoo.Symbols(symbol).Fields(Field.RegularMarketPrice).QueryAsync();

        var stock = sec[symbol];

        return stock.RegularMarketPrice;
    }
}