using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ThetanSearch
{


  public class ThetanProvider : IThetanProvider
  {
    private const string BaseUrl = "https://data.thetanarena.com/";

    public IEnumerable<ThetanData> GeThetans(
      ThetanSortType sort = ThetanSortType.Latest,
      ThetanHeroRarity[] heroRarity = null,
      ThetanSkinRarity[] skinRarity = null,
      ThetanHeroRole[] heroRole = null,
      double? priceMinWNBNB = null,
      double? priceMaxWNBNB = null,
      int size = 10)
    {
      RequestResultThetanData thetans = null;
      var request = new RestRequest("/thetan/v1/nif/search", Method.GET);
      request.AddQueryParameter("size", size.ToString());
      request.AddQueryParameter("sort", sort.ToString());

      if (heroRarity != null)
      { 
      request.AddQueryParameter("heroRarity", string.Join(",", heroRarity.Select(x => ((int)x).ToString())));
      }

      if (skinRarity != null)
      {
        request.AddQueryParameter("skinRarity", string.Join(",", heroRarity));
      }

      if (priceMinWNBNB != null)
      {
        request.AddQueryParameter("priceMin", String.Format("{0:0}", priceMinWNBNB * 100000000));
      }
      
      if (priceMaxWNBNB != null)
      {
        request.AddQueryParameter("priceMax", String.Format("{0:0}", priceMaxWNBNB * 100000000));
      }

      if (heroRole != null)
      {
        request.AddQueryParameter("heroRole", string.Join(",", heroRarity));
      }
      var response = new RestClient(BaseUrl).Execute(request);
      if(response.IsSuccessful)
      {
        thetans = JsonSerializer.Deserialize<RequestResultThetanData>(response.Content);
      }
      return thetans?.data;
    }
    
  }

}
