using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ThetanCore
{
  public interface IThetanRepository
  {
    int Delete(Expression<Func<Thetan, bool>> selector);
    int Delete(IEnumerable<Thetan> thetans);
    int DeleteAll();
    Thetan Select(string id);
    ICollection<K> Select<K>(Expression<Func<Thetan, K>> selector);
    ICollection<Thetan> SelectAll();
    int SelectAllCount();
    ICollection<Thetan> SelectWhere(Expression<Func<Thetan, bool>> selector);
    int Upsert(IEnumerable<Thetan> thetans);
    bool Upsert(Thetan thetan);
  }
}