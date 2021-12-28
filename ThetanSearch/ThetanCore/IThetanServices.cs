using System.Collections.Generic;

namespace ThetanCore
{
  public interface IThetanServices
  {
    IEnumerable<Thetan> GetThetans();
  }
}