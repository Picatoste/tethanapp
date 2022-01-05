using System.Collections.Generic;

namespace ThetanCore
{
  public interface IThetanServices
  {
    IEnumerable<Thetan> GetAllThetans(int afterHours = 1);
  }
}