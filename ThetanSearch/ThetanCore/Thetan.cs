using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThetanCore
{
  public class Thetan
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
  }

  public class ROIProfit
  {
    public bool IsPositive { get; set; }
    public WinRateType WinRate { get; set; }
    public double TotalRevenue { get; set; }
    public double TotalProfit { get; set; }
  }

  public enum Rarity
  {
    Common = 0,
    Epic = 1,
    Legendary = 2
  }


  public enum WinRateType
  {
    PerCent10 = 1,
    PerCent20 = 2,
    PerCent30 = 3,
    PerCent40 = 4,
    PerCent50 = 5,
    PerCent60 = 6,
    PerCent70 = 7,
    PerCent80 = 8,
    PerCent90 = 9,
    PerCent100 = 10,
  }
}
