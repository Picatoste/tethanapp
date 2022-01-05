using LiteDB;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ThetanCore.Extensions
{
  public static class Extensions
  {
    public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
    {
      return listToClone.Select(item => (T)item.Clone()).ToList();
    }
  }
}

