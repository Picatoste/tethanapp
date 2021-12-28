using System.Collections.Generic;

namespace ThetanCore
{
  public interface IROIServices
  {
    void FillRoi(IEnumerable<Thetan> thetans, IDictionary<string, double> convertCurrency);
  }
}