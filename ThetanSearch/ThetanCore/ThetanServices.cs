using LiteDB;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using ThetanCore.Interfaces;
using ThetanSearch;

namespace ThetanCore
{

  public class ThetanServices : IThetanServices
  {
    private readonly IOptions<ThetanConfig> thetanConfig;

    public ThetanServices(
        IOptions<ThetanConfig> thetanConfig)
    {
      this.thetanConfig = thetanConfig;
    }

    public IEnumerable<Thetan> GetThetans()
    {
        var thetans = GetAllThetans();
        return thetans;
      
    }

    public IEnumerable<Thetan> GetAllThetans(int afterHours = 1)
    {
      string fileDb = thetanConfig.Value.LiteDbFilePath;
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
