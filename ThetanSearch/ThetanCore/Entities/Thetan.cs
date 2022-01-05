using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ThetanCore
{
  [Serializable]
  public class Thetan : ICloneable
  {
    public String Id { get; set; }
    public String Name { get; set; }
    public Uri avatarSmall { get; set; }
    public Uri avatarBig { get; set; }
    public int BattleCap { get; set; }
    public int BattleCapMax { get; set; }
    public Rarity Rarity { get; set; }
    public IEnumerable<ROIProfit> ROIProfit { get; set; }
    public double Price { get; set; }
    public double PriceConverted { get; set; }
    public Uri LinkMarket { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
    public double Roi50PerCent { get; set; }

    public object Clone()
    {
      using (MemoryStream stream = new MemoryStream())
      {
        if (this.GetType().IsSerializable)
        {
          BinaryFormatter formatter = new BinaryFormatter();
          formatter.Serialize(stream, this);
          stream.Position = 0;
          return formatter.Deserialize(stream);
        }
        return null;
      }
    }
  }
}
