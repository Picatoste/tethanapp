using System;
using System.Text.Json.Serialization;

namespace ThetanSearch
{
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

}
