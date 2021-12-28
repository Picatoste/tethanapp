using System.Collections.Generic;

namespace ThetanSearch
{
  public interface ITokenPriceProvider
  {
    IDictionary<string, double> GetListCurrencyToken(string[] slugs, string currency = "USD");
  }
}