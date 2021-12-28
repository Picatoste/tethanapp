using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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
      int size = 100)
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
  
  public class SystemCurrency
  {
    public int type { get; set; }
    public string name { get; set; }
    public long value { get; set; }
    public int decimals { get; set; }
  }

  public class ThetanData
  {
    [JsonPropertyName("id")]
    public string Id { get; set; }
    public DateTime created { get; set; }
    public DateTime lastModified { get; set; }
    public string ownerId { get; set; }
    public string ownerAddress { get; set; }
    public string tokenId { get; set; }
    public string refId { get; set; }
    public int refType { get; set; }
    public long price { get; set; }
    public SystemCurrency systemCurrency { get; set; }
    public int hp { get; set; }
    public int dmg { get; set; }
    public int heroTypeId { get; set; }
    public ThetanHeroRole heroRole { get; set; }
    public ThetanHeroRarity heroRarity { get; set; }
    public int skinId { get; set; }
    public ThetanSkinRarity skinRarity { get; set; }
    public int status { get; set; }
    public int marketType { get; set; }
    public int level { get; set; }
    public int trophyClass { get; set; }
    public int battleCap { get; set; }
    public string imageAvatar { get; set; }
    public string imageFull { get; set; }
    public string name { get; set; }
    public string skinName { get; set; }
    public int battleCapMax { get; set; }

  }

  public class Page
  {
    public int total { get; set; }
  }

  public class RequestResultThetanData
  {
    public bool success { get; set; }
    public List<ThetanData> data { get; set; }
    public Page page { get; set; }
  }

}
