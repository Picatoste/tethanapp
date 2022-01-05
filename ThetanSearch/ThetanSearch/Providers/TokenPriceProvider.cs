using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ThetanSearch
{
  public class TokenPriceProvider : ITokenPriceProvider
  {
    private const string BaseUrl = "https://pro-api.coinmarketcap.com/v1/";
    private readonly IList<TokenPriceData> distTokenPriceData;

    public TokenPriceProvider()
    {
      this.distTokenPriceData = new List<TokenPriceData>();
    }

    public IDictionary<string, double> GetListCurrencyToken(string[] slugs, string currency = "USD")
    {
      var dictTokenPrices = new Dictionary<string, double>();
#if DEBUG
      dictTokenPrices.Add("thetan-coin", 0.114);
      dictTokenPrices.Add("wbnb", 511.26);
#else
      foreach (var slug in slugs)
      {
        var tokenPrice = this.distTokenPriceData.FirstOrDefault(
            x => x.Slug.Equals(slug) 
            && x.Currency.Equals(currency)
            &&  x.LastUpdated.AddHours(1) >= DateTime.Now);
        if (tokenPrice == null)
        {
          distTokenPriceData.Remove(tokenPrice);
          tokenPrice = new TokenPriceData()
          {
            Slug = slug,
            Currency = currency,
            Price = GetTokenPriceBySlug(slug, currency).GetValueOrDefault(),
            LastUpdated = DateTime.Now
          };
          distTokenPriceData.Add(tokenPrice);
        }
        dictTokenPrices.Add(tokenPrice.Slug, tokenPrice.Price);
      }
#endif

      return dictTokenPrices;
    }

    private double? GetTokenPriceBySlug(string slug, string currency = "USD")
    {
      double price = 0;
      try
      {
        var request = new RestRequest("cryptocurrency/quotes/latest", Method.GET);
        request.AddHeader("X-CMC_PRO_API_KEY", "f32714eb-4198-49ec-af3e-6dceab0dfb75");
        request.AddQueryParameter("slug", slug);
        request.AddQueryParameter("convert", currency);
        var response = new RestClient(BaseUrl).Execute(request);
        if (response.IsSuccessful)
        {
          var tokenPrice = JObject.Parse(response.Content);
          double.TryParse(tokenPrice["data"].First?.First?["quote"][currency]["price"].ToString(), out price);
        }
        return price;

      }
      catch
      {
        return distTokenPriceData.FirstOrDefault(x => x.Slug == slug && x.Currency == currency)?.Price;
      }
    }

    private class TokenPriceData
    {
      public string Slug { get; set; }
      public double Price { get; set; }
      public string Currency { get; set; }
      public DateTime LastUpdated { get; set; }
    }
  }


  public class Status
  {
    public DateTime timestamp { get; set; }
    public int error_code { get; set; }
    public object error_message { get; set; }
    public int elapsed { get; set; }
    public int credit_count { get; set; }
    public object notice { get; set; }
  }

  public class Platform
  {
    public int id { get; set; }
    public string name { get; set; }
    public string symbol { get; set; }
    public string slug { get; set; }
    public string token_address { get; set; }
  }
  
  public class USD
  {
    public double price { get; set; }
    public double volume_24h { get; set; }
    public double volume_change_24h { get; set; }
    public double percent_change_1h { get; set; }
    public double percent_change_24h { get; set; }
    public double percent_change_7d { get; set; }
    public double percent_change_30d { get; set; }
    public double percent_change_60d { get; set; }
    public double percent_change_90d { get; set; }
    public double market_cap { get; set; }
    public int market_cap_dominance { get; set; }
    public double fully_diluted_market_cap { get; set; }
    public DateTime last_updated { get; set; }
  }

  public class Quote
  {
    public USD USD { get; set; }
  }

  public class WBNB
  {
    public int id { get; set; }
    public string name { get; set; }
    public string symbol { get; set; }
    public string slug { get; set; }
    public int num_market_pairs { get; set; }
    public DateTime date_added { get; set; }
    public List<string> tags { get; set; }
    public object max_supply { get; set; }
    public double circulating_supply { get; set; }
    public double total_supply { get; set; }
    public Platform platform { get; set; }
    public int is_active { get; set; }
    public int cmc_rank { get; set; }
    public int is_fiat { get; set; }
    public DateTime last_updated { get; set; }
    public Quote quote { get; set; }
  }

  public class Data
  {
    public WBNB WBNB { get; set; }
  }

  public class TokenPrice
  {
    public Status status { get; set; }
    public Data data { get; set; }
  }

}
