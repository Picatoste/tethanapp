using System.Collections.Generic;

namespace ThetanCore
{
  public interface IThetanProvider
  {
    IEnumerable<Thetan> GetAllThetans(int afterHours = 1);
  }
}