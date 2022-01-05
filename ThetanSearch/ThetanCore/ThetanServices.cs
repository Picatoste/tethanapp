//using LiteDB;
using System.Linq;
using System.Collections.Generic;
using ThetanSearch;
using System;
using ThetanCore.Extensions;
using AutoMapper;
using ThethanCore.Mappers;
using System.Collections.Concurrent;

namespace ThetanCore
{

  public class ThetanServices : IThetanServices
  {
    private readonly ITokenPriceProvider tokenPriceProvider;
    private readonly IThetanProvider thetanProvider;
    private readonly IRoiProfitServices roiProfitServices;
    private readonly IDictionary<string, Thetan> thetansSaved;

    private readonly IMapper mapper;

    public ThetanServices(
        ITokenPriceProvider tokenService,
        IThetanProvider thetanProvider,
        IRoiProfitServices roiProfitServices)
    {
      this.tokenPriceProvider = tokenService;
      this.thetanProvider = thetanProvider;
      this.roiProfitServices = roiProfitServices;

      //// We know how many items we want to insert into the ConcurrentDictionary.
      //// So set the initial capacity to some prime number above that, to ensure that
      //// the ConcurrentDictionary does not need to be resized while initializing it.
      //int initialCapacity = 101;

      //// The higher the concurrencyLevel, the higher the theoretical number of operations
      //// that could be performed concurrently on the ConcurrentDictionary.  However, global
      //// operations like resizing the dictionary take longer as the concurrencyLevel rises.
      //// For the purposes of this example, we'll compromise at numCores * 2.
      //int numProcs = Environment.ProcessorCount;
      //int concurrencyLevel = numProcs * 2;


      //this.thetansSaved = new ConcurrentDictionary<string, Thetan>(concurrencyLevel, initialCapacity);

      //this.thetansSaved = new ConcurrentDictionary<string, Thetan>();

      this.thetansSaved = new Dictionary<string, Thetan>();


      var config = new MapperConfiguration(cfg =>
      {
        cfg.CreateMap<ThetanData, Thetan>();
        cfg.AddProfile(new TethanProfile(this.tokenPriceProvider));
      });

      mapper = config.CreateMapper();

    }


    public IEnumerable<Thetan> GetAllThetans(int afterHours = 1)
    {
      var thetansCandidateToAdd = mapper.Map<IEnumerable<ThetanData>, IEnumerable<Thetan>>(
          thetanProvider.GeThetans()
      ).ToList().Clone();

      foreach (var thetansToAdd in thetansCandidateToAdd)
      {
        var clonedDeletedThetans = thetansSaved.Values.Where(x =>
         x.LastModified.AddHours(afterHours) < DateTime.UtcNow)
            .ToList().Clone();

        foreach (Thetan thetanToDeleted in clonedDeletedThetans)
        {
          thetansSaved.Remove(thetanToDeleted.Id);
        }
      }
      
      this.roiProfitServices.FillRoi(
        thetansCandidateToAdd,
        this.tokenPriceProvider.GetListCurrencyToken(new[] { "thetan-coin", "wbnb" }));
        

      AddThetans(thetansCandidateToAdd);


      return thetansSaved.Values.AsEnumerable()
          .OrderByDescending(x => x.LastModified);
    }


    private void AddThetans(IEnumerable<Thetan> thetansAdd)
    {
      foreach (Thetan thetanCandidateAdd in thetansAdd)
      {
        bool addThetan = false;
        if (thetansSaved.ContainsKey(thetanCandidateAdd.Id))
        {
          var thetanIntersect = thetansSaved[thetanCandidateAdd.Id];
          if (thetanIntersect != null)
          {
            if (thetanCandidateAdd.Price != thetanIntersect.Price
                && thetanCandidateAdd.ROIProfit != thetanIntersect.ROIProfit)
            {
              thetansSaved[thetanCandidateAdd.Id] = thetanCandidateAdd;
            }
          }
          else
          {
            addThetan = true;
          }
        }
        else
        {
          addThetan = true;
        }

        if(addThetan)
        {
          //thetanCandidateAdd.LastModified = TimeZoneInfo.ConvertTimeFromUtc(thetanCandidateAdd.LastModified, TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));
          thetansSaved.Add(thetanCandidateAdd.Id, thetanCandidateAdd);
        }
      }
    }


    //public IEnumerable<Thetan> GetAllThetans(int afterHours = 1)
    //{
    //  string fileDb = thetanConfig.Value.LiteDbFilePath;
    //  lock (fileDb)
    //  {
    //    using (var db = new LiteDatabase(fileDb))
    //    {
    //      // Get a collection (or create, if doesn't exist)
    //      var col = db.GetCollection<Thetan>("thetans");

    //      // Index document using document Name property
    //      col.EnsureIndex(x => x.Id);

    //      // Use LINQ to query documents (filter, sort, transform)
    //      return col.Query()
    //        .OrderByDescending(x => x.LastModified)
    //        .ToList();
    //    }
    //  }
    //}

  }


}

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

