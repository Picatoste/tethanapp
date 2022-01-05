namespace ThethanApp.Models
{
  public class ROIProfitModel
  {
    public bool IsPositive { get; set; }
    public WinRateType WinRate { get; set; }
    public double TotalRevenue { get; set; }
    public double TotalProfit { get; set; }
  }
}
