using System.Collections.Generic;

namespace ThetanSearch
{
  public interface IThetanAPIProvider
  {
    IEnumerable<ThetanData> GeThetans(
        ThetanSortType sort = ThetanSortType.Latest, 
        ThetanHeroRarity[] heroRarity = null, 
        ThetanSkinRarity[] skinRarity = null, 
        ThetanHeroRole[] heroRole = null, 
        double? priceMinWNBNB = null, 
        double? priceMaxWNBNB = null, 
        int size = 10);
  }
}