using System.Collections.Generic;

namespace ThetanSearch
{
  public interface ITokenAPIPriceProvider
  {
    IDictionary<string, double> GetListCurrencyToken(string[] slugs, string currency = "USD");
  }
}