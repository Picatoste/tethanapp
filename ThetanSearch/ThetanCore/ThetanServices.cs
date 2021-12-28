using AutoMapper;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThetanSearch;
using ThethanCore.Mappers;

namespace ThetanCore
{

  public class ThetanServices : IThetanServices
  {
    private readonly IThetanProvider thetanProvider;
    public ThetanServices(
        IThetanProvider thetanProvider)
    {
      this.thetanProvider = thetanProvider;
      
    }

    public IEnumerable<Thetan> GetThetans()
    {
        var thetans = GetAllThetans();
        return thetans;
      
    }

    public IEnumerable<Thetan> GetAllThetans(int afterHours = 1)
    {
      string fileDb = @"C:\Temp\Thetans.db";
      lock (fileDb)
      {
        using (var db = new LiteDatabase(fileDb))
        {
          // Get a collection (or create, if doesn't exist)
          var col = db.GetCollection<Thetan>("thetans");

          // Index document using document Name property
          col.EnsureIndex(x => x.Id);

          // Use LINQ to query documents (filter, sort, transform)
          return col.Query()
            .OrderByDescending(x => x.LastModified)
            .ToList();
        }
      }
    }
    
  }
}
