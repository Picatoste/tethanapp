using System;
using System.Collections.Generic;

namespace ThetanCore
{
  public interface IThetanNotification
  {
    bool Notify(IEnumerable<Thetan> thetansPendingProcess, string emailTo, Func<Thetan, bool> predicateWhere);
  }
}