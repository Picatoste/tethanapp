using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ThetanCore
{
  [Serializable]
  public class ROIProfit : ICloneable
  {
    public bool IsPositive { get; set; }
    public WinRateType WinRate { get; set; }
    public double TotalRevenue { get; set; }
    public double TotalProfit { get; set; }
    public double TotalWinBattles { get; set; }
    public double TotalLoseBattles { get; set; }
    public double TotalWinTHC { get; set; }
    public double TotalLoseTHC { get; set; }
    public double TotalTHC { get; set; }
    public double RevenueWin { get; set; }
    public double RevenueLose { get; set; }
    public double ClaimFee { get; set; }
    
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
