using System.Collections.Generic;

namespace ThetanCore
{
  public interface IRoiProfitServices
  {
    void FillRoi(IEnumerable<Thetan> thetans, IDictionary<string, double> convertCurrency);
    void FillRoi(Thetan thetan, IDictionary<string, double> convertCurrency);
  }
}