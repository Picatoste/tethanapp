using System;

namespace ThetanSearch
{
  class Program
  {
    static void Main(string[] args)
    {
      var thetans = new ThetanAPIProvider().GeThetans(ThetanSortType.Latest, heroRarity: new[] { ThetanHeroRarity.Legendary }, size:10);

      //var priceUSD = new TokenPriceProvider().GetTokenPriceBySlug("thetan-coin");
      //priceUSD = new TokenPriceProvider().GetTokenPriceBySlug("wbnb");
      //priceUSD = new TokenPriceProvider().GetTokenPriceBySlug("thetan-coin");

    }
  }
}
