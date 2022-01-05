using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using ThetanSearch;
using AutoMapper;
using ThethanCore.Mappers;
using Microsoft.Extensions.Options;
using ThetanCore.Interfaces;

namespace ThetanCore
{
  public class ThetanHostedService : IHostedService, IDisposable
  { 
    private readonly object notifyLock = new object();
    private readonly Task _completedTask = Task.CompletedTask;
    private int executionCount = 0;
    private Timer _timer = null;
    private readonly double periodInSeconds;
    private readonly bool enabled;


    private readonly ITokenPriceProvider tokenService;
    private readonly IThetanProvider thetanProvider;
    private readonly IRoiProfitServices rOIServices;
    private readonly IThetanNotification thetanNotification;
    private readonly IOptions<ThetanEmailNotificationConfig> thetanEmailNotificationConfig;

    private readonly IMapper mapper;

    public ThetanHostedService(
        ITokenPriceProvider tokenService,
        IThetanProvider thetanProvider,
        IRoiProfitServices rOIServices,
        IThetanNotification thetanNotification,
        IOptions<ThetanConfig> thetanConfig,
        IOptions<ThetanEmailNotificationConfig> thetanEmailNotificationConfig,
        IOptions<ThetanHostedServiceConfig> thetanHostedServiceConfig)
    {
      this.periodInSeconds = thetanHostedServiceConfig.Value.PeriodInSeconds;
      this.enabled = thetanHostedServiceConfig.Value.Enabled;

      this.tokenService = tokenService;
      this.thetanProvider = thetanProvider;
      this.rOIServices = rOIServices;
      this.thetanNotification = thetanNotification;
      this.thetanEmailNotificationConfig = thetanEmailNotificationConfig;

      var config = new MapperConfiguration(cfg => {
        cfg.CreateMap<ThetanData, Thetan>();
        cfg.AddProfile(new TethanProfile(tokenService));
      });

      mapper = config.CreateMapper();
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
      if (enabled)
      {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(periodInSeconds));
      }
      return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
      var count = Interlocked.Increment(ref executionCount);
      
      var thetansData = this.thetanProvider.GeThetans();

      var thetansToInsert = mapper.Map<IEnumerable<ThetanData>, IEnumerable<Thetan>>(thetansData);


      var convertCurrency = this.tokenService.GetListCurrencyToken(new[] { "thetan-coin", "wbnb" });
      rOIServices.FillRoi(thetansToInsert, convertCurrency);
      lock (notifyLock)
      {
        this.thetanNotification.Notify(thetansToInsert, this.thetanEmailNotificationConfig.Value.EmailTo, (Thetan thetan) =>
        {
          return thetan.PriceConverted >= 5F
              && thetan.PriceConverted <= 75F
              && thetan.Roi50PerCent >= 150
              && thetan.ROIProfit.First(x => x.WinRate == WinRateType.PerCent30).IsPositive;
        });
      }
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
  }
}
