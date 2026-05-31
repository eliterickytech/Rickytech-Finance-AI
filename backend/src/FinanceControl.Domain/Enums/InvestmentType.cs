namespace FinanceControl.Domain.Enums;

public enum InvestmentType
{
    RendaFixa = 1,
    Acao = 2,
    FII = 3,
    ETF = 4,
    Cripto = 5,
    Outro = 99
}

public enum OperationSide
{
    Buy = 1,
    Sell = 2,
    Fee = 3,
    Yield = 4
}

public enum IntegrationProvider
{
    Binance = 1,
    OpenFinance = 2,
    Custom = 99
}

public enum NewsCategory
{
    Crypto = 1,
    FinancialBR = 2,
    FinancialIntl = 3
}
