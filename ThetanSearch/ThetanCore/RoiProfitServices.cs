using System;
using System.Collections.Generic;
using System.Linq;

namespace ThetanCore
{
  public class RoiProfitServices : IRoiProfitServices
  {
    private const double HTCReward_Win = 6;
    private const double HTCReward_Draw = 2;
    private const double HTCReward_Lose = 1;


    private readonly Dictionary<Rarity, double> HTCReward_WinBonus =
      new Dictionary<Rarity, double>()
      {
        { Rarity.Common, 3.25F },
        { Rarity.Epic, 6.50F },
        { Rarity.Legendary, 23.55F },
      };

    public RoiProfitServices()
    {
    }

    public void FillRoi(IEnumerable<Thetan> thetans, IDictionary<string, double> convertCurrency)
    {
      foreach (var thetan in thetans)
      {
        FillRoi(thetan, convertCurrency);
      }

    }

    public void FillRoi(Thetan thetan, IDictionary<string, double> convertCurrency)
    {

      IList<ROIProfit> roiProfit = new List<ROIProfit>();
      foreach (WinRateType rate in Enum.GetValues(typeof(WinRateType)))
      {
        roiProfit.Add(CreateROIProfit(thetan, rate, convertCurrency));
      }
      thetan.ROIProfit = roiProfit;
      thetan.Roi50PerCent = CalculaRoiByWinRate(thetan, WinRateType.PerCent50, convertCurrency);
    }

    private double CalculaRoiByWinRate(Thetan thetan, WinRateType rate, IDictionary<string, double> convertCurrency)
    {
      return (thetan.ROIProfit.FirstOrDefault(x => x.WinRate == rate).TotalRevenue * 100) / (thetan.Price * convertCurrency["wbnb"]);
    }
    private ROIProfit CreateROIProfit(Thetan thetan, WinRateType rate, IDictionary<string, double> convertCurrency)
    {
      double totalWinBattles = Convert.ToInt32(thetan.BattleCap * ((double)rate / 10));
      double totalLoseBattles = thetan.BattleCap - totalWinBattles;
      double totalTHCWin = totalWinBattles * (HTCReward_Win + HTCReward_WinBonus[thetan.Rarity]);
      double revenueWin = totalTHCWin * convertCurrency["thetan-coin"];

      double totalTHCLose = totalLoseBattles * HTCReward_Lose;
      double revenueLose = totalTHCLose * convertCurrency["thetan-coin"];

      double totalTHC = totalTHCWin + totalTHCLose;
      double claimFee = (totalTHC * 0.04) * convertCurrency["thetan-coin"];

      double revenueTotal = revenueWin + revenueLose ;
      double revenueTotalWithClaimFee = revenueTotal - claimFee;

      double price = thetan.Price * convertCurrency["wbnb"];
      double totalProfit = (revenueTotal - price);
      double totalProfitWithClaimFee = (revenueTotal - price) - claimFee;
      return new ROIProfit()
      {
        WinRate = rate,
        IsPositive = totalProfitWithClaimFee >= 0,
        TotalRevenue = revenueTotalWithClaimFee,
        TotalProfit = totalProfitWithClaimFee,
        
      };
    }
  }
}
