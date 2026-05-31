# Sprint 8 - Spec técnica (Notícias)

## Entidade

```csharp
public sealed class NewsItem : BaseEntity
{
    public string Title { get; private set; }
    public string Url { get; private set; }
    public string UrlHash { get; private set; }           // SHA256 da URL
    public string Source { get; private set; }            // "CoinDesk", "InfoMoney", ...
    public NewsCategory Category { get; private set; }    // Crypto, FinancialBR, FinancialIntl
    public string[] Tags { get; private set; }            // BTC, ETH, ADA, IBOV, FED
    public DateTimeOffset PublishedAt { get; private set; }
    public string? Summary { get; private set; }
    public string? ImageUrl { get; private set; }
}
```

## Endpoint

```
GET /api/v1/noticias
  ?category=Crypto|FinancialBR|FinancialIntl|All
  &tag=BTC,ETH
  &search=ethereum
  &page=1&pageSize=20
```

Resposta:

```json
{
  "items": [
    {
      "id": "...",
      "title": "Ethereum hits new high after ETF approval",
      "url": "https://www.coindesk.com/...",
      "source": "CoinDesk",
      "category": "Crypto",
      "tags": ["ETH"],
      "publishedAt": "2026-08-15T13:24:00Z",
      "summary": "...",
      "imageUrl": "https://..."
    }
  ],
  "total": 1234,
  "page": 1,
  "pageSize": 20
}
```

## Fontes default

```json
"News": {
  "Sources": [
    { "name": "CoinDesk",      "url": "https://www.coindesk.com/arc/outboundfeeds/rss/", "category": "Crypto" },
    { "name": "CoinTelegraph", "url": "https://cointelegraph.com/rss",                     "category": "Crypto" },
    { "name": "Decrypt",       "url": "https://decrypt.co/feed",                           "category": "Crypto" },
    { "name": "TheBlock",      "url": "https://www.theblock.co/rss.xml",                   "category": "Crypto" },
    { "name": "InfoMoney",     "url": "https://www.infomoney.com.br/feed/",                "category": "FinancialBR" },
    { "name": "Valor",         "url": "https://valor.globo.com/rss/",                      "category": "FinancialBR" },
    { "name": "Investing",     "url": "https://br.investing.com/rss/news.rss",             "category": "FinancialIntl" }
  ],
  "TagDictionary": {
    "BTC":  ["bitcoin", "btc"],
    "ETH":  ["ethereum", "eth", "ether"],
    "ADA":  ["cardano", "ada"],
    "SOL":  ["solana", "sol"],
    "BNB":  ["binance coin", "bnb"],
    "IBOV": ["ibovespa", "ibov"],
    "FED":  ["federal reserve", "powell"],
    "Selic":["selic", "copom"],
    "IPCA": ["ipca", "inflação"]
  }
}
```
