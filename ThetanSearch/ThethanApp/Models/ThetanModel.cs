using System;
using System.Collections.Generic;
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
}
