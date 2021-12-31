using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ThethanApp.Models
{
  public class ThetanModel
  {
    [Display(Name = "#")]
    public string Id { get; set; }
    [Display(Name = "Name")]
    public string Name { get; set; }
    [Display(Name = "Avatar")]
    public Uri avatarSmall { get; set; }
    [Display(Name = "Avataer")]
    public Uri avatarBig { get; set; }
    [Display(Name = "Battles Remaining.")]
    public int BattleCap { get; set; }
    [Display(Name = "Battles")]
    public int BattleCapMax { get; set; }
    [Display(Name = "ROI (50%)")]
    public IEnumerable<ROIProfitModel> ROIProfit { get; set; }
    [Display(Name = "WBNB Price")]
    public double Price { get; set; }
    [Display(Name = "USD Price")]
    public double PriceConverted { get; set; }
    [Display(Name = "Go to market")]
    public Uri LinkMarket { get; set; }
    public DateTime LastModified { get; set; }
    public string LastModifiedAgo { get; set; }
    [Display(Name = "ROI (50% WinRate)")]
    public double Roi50PerCent { get; set; }
    public TierROI Roi50PerCentGrade { get; set; }
  }
  public enum TierROI
  {
    [Description("white")]
    Error,
    [Description("#FF7EFD")]
    F,
    [Description("#807FFE")]
    E,
    [Description("#7FBFFD")]
    D,
    [Description("#7EFF80")]
    C,
    [Description("#FFFB7C")]
    B,
    [Description("#FFBD80")]
    A,
    [Description("#FD7E7D")]
    S
  }
  public class ROIProfitModel
  {
    public bool IsPositive { get; set; }
    public WinRateType WinRate { get; set; }
    public double TotalRevenue { get; set; }
    public double TotalProfit { get; set; }
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
