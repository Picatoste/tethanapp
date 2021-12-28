using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using LiteDB;
using ThetanSearch;
using AutoMapper;
using ThethanCore.Mappers;

namespace ThetanCore
{
  public class ThetanHostedService : IHostedService, IDisposable
  {
    private readonly Task _completedTask = Task.CompletedTask;
    private int executionCount = 0;
    private Timer _timer = null;
    private readonly double periodInSeconds;


    private readonly ITokenPriceProvider tokenService;
    private readonly IThetanProvider thetanProvider;
    private readonly IRoiProfitServices rOIServices;
    private readonly IThetanNotification thetanNotification;
    private readonly IMapper mapper;

    public ThetanHostedService(
        ITokenPriceProvider tokenService,
        IThetanProvider thetanProvider,
        IRoiProfitServices rOIServices,
        IThetanNotification thetanNotification)
    {
      this.periodInSeconds = 5;

      this.tokenService = tokenService;
      this.thetanProvider = thetanProvider;
      this.rOIServices = rOIServices;
      this.thetanNotification = thetanNotification;

      var config = new MapperConfiguration(cfg => {
        cfg.CreateMap<ThetanData, Thetan>();
        cfg.AddProfile(new TethanProfile(tokenService));
      });

      mapper = config.CreateMapper();
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
      _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(periodInSeconds));

      return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
      var count = Interlocked.Increment(ref executionCount);
      
      var thetansData = this.thetanProvider.GeThetans();

      var thetansToInsert = mapper.Map<IEnumerable<ThetanData>, IEnumerable<Thetan>>(thetansData);


      var convertCurrency = this.tokenService.GetListCurrencyToken(new[] { "thetan-coin", "wbnb" });
      rOIServices.FillRoi(thetansToInsert, convertCurrency);

      this.thetanNotification.Notify(thetansToInsert, "dabenito@gmail.com", 10F, 80F, 150F);
      SetThetans(thetansToInsert);
      
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
      _timer?.Change(Timeout.Infinite, 0);

      return _completedTask;
    }
    
    public void Dispose()
    {
      _timer?.Dispose();
    }

    public void SetThetans(IEnumerable<Thetan> thetans, int afterHours = 1)
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
          col.DeleteMany(x => 
            x.LastModified.ToUniversalTime().AddHours(afterHours) < DateTime.UtcNow
          );
          col.Upsert(thetans);
        }
      }
    }
  }
}
