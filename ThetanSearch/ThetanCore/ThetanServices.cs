using LiteDB;
using System.Linq;
using System.Collections.Generic;
using ThetanSearch;
using System;
using ThetanCore.Extensions;
using AutoMapper;
using ThethanCore.Mappers;
using ThetanCore.Interfaces;
using Microsoft.Extensions.Options;

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
        IRoiProfitServices roiProfitServices, 
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
          thetansSaved.Add(thetanCandidateAdd.Id, thetanCandidateAdd);
        }
      }
    }
  }
}
