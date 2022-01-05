using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThetanCore;
using ThetanSearch;

namespace ThethanCore.Mappers
{
  public class TethanProfile : Profile
  {
    private readonly IDictionary<string, double> dictCurrency;
    public TethanProfile(
        ITokenPriceProvider tokenProvider) : base()
    {
      this.dictCurrency = tokenProvider.GetListCurrencyToken(new[] { "thetan-coin", "wbnb" });

      CreateMap<ThetanData, Thetan>()
      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
      .ForMember(dest => dest.BattleCap, opt => opt.MapFrom(src => src.battleCap))
      .ForMember(dest => dest.BattleCapMax, opt => opt.MapFrom(src => src.battleCapMax))
      .ForMember(dest => dest.Rarity, opt => opt.MapFrom(src => src.heroRarity))
      .ForMember(dest => dest.Price, opt => opt.MapFrom(src => ((double)src.price) / 100000000))
      .ForMember(dest => dest.PriceConverted, opt => opt.MapFrom(src => (((double)src.price) / 100000000) * this.dictCurrency["wbnb"]))
      .ForMember(dest => dest.avatarSmall, opt => opt.MapFrom(src => $"https://assets.thetanarena.com/{src.imageAvatar}".Replace("avatar", "smallavatar")))
      .ForMember(dest => dest.avatarBig, opt => opt.MapFrom(src => $"https://assets.thetanarena.com/{src.imageFull}"))
      .ForMember(dest => dest.LinkMarket, opt => opt.MapFrom(src => $"https://marketplace.thetanarena.com/item/{src.refId}"))
      .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.created))
      .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.lastModified.ToUniversalTime()));
    }
  }
}
