using System.Collections.Generic;

namespace ThetanCore
{
  public class ThetanComparer : EqualityComparer<Thetan>
  {
    public override bool Equals(Thetan x, Thetan y)
    {
      return x.Id == y.Id;
    }

    public override int GetHashCode(Thetan obj)
    {
      if (obj == null) return 0;
      return obj.LinkMarket.GetHashCode() ^ obj.Id.GetHashCode();
    }
  }
}
