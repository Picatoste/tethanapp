using LiteDB;
using System.Linq;
using System.Collections.Generic;
using ThetanSearch;
using System;
using ThetanCore.Extensions;
using AutoMapper;
using Microsoft.Extensions.Options;
using ThethanCore;

namespace ThetanCore
{

  public class ThetanProvider : IThetanProvider
  {
    private readonly ITokenAPIPriceProvider tokenPriceProvider;
    private readonly IThetanAPIProvider thetanProvider;
    private readonly IThetanRoiProfitServices roiProfitServices;
    private readonly IDictionary<string, Thetan> thetansSaved;

    private readonly IMapper mapper;

    public ThetanProvider(
        ITokenAPIPriceProvider tokenService,
        IThetanAPIProvider thetanProvider,
        IThetanRoiProfitServices roiProfitServices, 
        IOptions<ThetanConfig> thetanConfig,
        IDictionary<string, Thetan> thetansSaved)
    {
      this.tokenPriceProvider = tokenService;
      this.thetanProvider = thetanProvider;
      this.roiProfitServices = roiProfitServices;
      this.thetansSaved = thetansSaved;


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

      RemoveThetansAfterHours(afterHours, thetansCandidateToAdd);

      this.roiProfitServices.FillRoi(
        thetansCandidateToAdd,
        this.tokenPriceProvider.GetListCurrencyToken(new[] { "thetan-coin", "wbnb" }));


      AddThetans(thetansCandidateToAdd);


      return thetansSaved.Values.AsEnumerable()
          .OrderByDescending(x => x.LastModified);
    }

    private void RemoveThetansAfterHours(int afterHours, IList<Thetan> thetansCandidateToAdd)
    {
      foreach (var thetansToAdd in thetansCandidateToAdd)
      {
        var thetansToRemove = thetansSaved.Values.Where(x =>
         x.LastModified.AddHours(afterHours) < DateTime.UtcNow)
            .ToList().Clone();

        foreach (Thetan thetanToDeleted in thetansToRemove)
        {
          thetansSaved.Remove(thetanToDeleted.Id);
        }
      }
    }

    private void AddThetans(IEnumerable<Thetan> thetansAdd)
    {
      foreach (Thetan thetanCandidateAdd in thetansAdd)
      {
        if (CheckIfExistThethan(thetanCandidateAdd))
        {
          thetansSaved.Add(thetanCandidateAdd.Id, thetanCandidateAdd);
        }
      }
    }

    private bool CheckIfExistThethan(Thetan thetanCandidateAdd)
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

      return addThetan;
    }
  }
}
