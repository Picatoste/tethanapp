namespace ThethanApp.Models
{
  public class ROIProfitModel
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
  }
}
