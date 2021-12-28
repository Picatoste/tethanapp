using System.Collections.Generic;

namespace ThetanCore
{
  public interface IThetanNotification
  {
    bool Notify(IEnumerable<Thetan> thetansPendingProcess, string emailTo, double minPrice, double maxPrice, double minRoiProfit50Percent);
  }
}