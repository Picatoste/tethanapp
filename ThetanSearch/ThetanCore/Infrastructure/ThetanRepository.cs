using LiteDB;
using System.Linq;
using System.Collections.Generic;
using System;
using ThetanCore.Interfaces;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace ThetanCore
{
  public class ThetanRepository : IThetanRepository
  {
    private readonly string connectionString;
    private readonly IEqualityComparer<Thetan> thetanComparer = new ThetanComparer();

    public ThetanRepository(IOptions<ThetanConfig> thetanConfig)
    {
      connectionString = thetanConfig.Value.ConnectionStringLiteDB;
    }

    private ILiteCollection<Thetan> GetDBThetans()
    {
      var db = new LiteDatabase(this.connectionString);
      var col = db.GetCollection<Thetan>("thetans");
      col.EnsureIndex(x => x.Id);
      return col;
    }

    public ICollection<Thetan> SelectAll()
    {
      return GetDBThetans().Query()
        .OrderByDescending(x => x.LastModified)
        .ToList();
    }
    
    public int SelectAllCount()
    {
      return GetDBThetans().Query().Count();
    }


    public ICollection<K> Select<K>(Expression<Func<Thetan, K>> selector)
    {
      return GetDBThetans().Query()
        .Select(selector)
        .ToList().ToList();
    }

    public ICollection<Thetan> SelectWhere(Expression<Func<Thetan, bool>> selector)
    {
      return GetDBThetans().Query()
        .Where(selector).ToList();
    }


    public Thetan Select(string id)
    {
      return GetDBThetans().FindOne(x => x.Id == id);
    }

    public int Delete(Expression<Func<Thetan, bool>> selector)
    {
      return GetDBThetans().DeleteMany(selector);
    }

    public int Delete(IEnumerable<Thetan> thetans)
    {
      return GetDBThetans().DeleteMany(x => thetans.Contains(x, this.thetanComparer));
    }
    public int DeleteAll()
    {
      return GetDBThetans().DeleteAll();
    }

    public bool Upsert(Thetan thetan)
    {
      return GetDBThetans().Upsert(thetan);
    }

    public int Upsert(IEnumerable<Thetan> thetans)
    {
      return GetDBThetans().Upsert(thetans);
    }

  }
}

