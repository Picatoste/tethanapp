using AutoMapper;
using EnumsNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThetanCore;
using ThetanSearch;
using ThethanApp.Models;

namespace ThethanApp.Mappers
{
  public class TethanProfile : Profile
  {
    public TethanProfile()
    {
      CreateMap<Thetan, ThetanModel>()
      .ForMember(dest => dest.LastModifiedAgo, opt => opt.MapFrom(src => GetTimeAgo(src.LastModified)))
      .ForMember(dest => dest.Roi50PerCentGrade, opt => opt.MapFrom(src => GetGradeRoi(src.Roi50PerCent)));
      CreateMap<ROIProfit, ROIProfitModel>();
    }

    private GradeROI GetGradeRoi(double RoiGrade)
    {
      if (RoiGrade <= 0)
      {
        return GradeROI.E;
      }
      else if (RoiGrade > 0 && RoiGrade <= 50)
      {
        return GradeROI.D;
      }
      else if (RoiGrade > 50 && RoiGrade <= 100)
      {
        return GradeROI.C;
      }
      else if (RoiGrade > 100 && RoiGrade <= 115)
      {
        return GradeROI.B;
      }
      else if (RoiGrade > 115 && RoiGrade <= 135)
      {
        return GradeROI.BPlus;
      }
      else if (RoiGrade > 135 && RoiGrade <= 200)
      {
        return GradeROI.A;
      }
      else if (RoiGrade > 200)
      {
        return GradeROI.APlus;
      }
      else
      {
        return GradeROI.Error;
      } 
    }

    private string GetTimeAgo(DateTime time)
    {
      const int SECOND = 1;
      const int MINUTE = 60 * SECOND;
      const int HOUR = 60 * MINUTE;
      const int DAY = 24 * HOUR;
      const int MONTH = 30 * DAY;

      var ts = new TimeSpan(DateTime.UtcNow.Ticks - time.ToUniversalTime().Ticks);
      double delta = Math.Abs(ts.TotalSeconds);

      if (delta < 1 * MINUTE)
        return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

      if (delta < 2 * MINUTE)
        return "a minute ago";

      if (delta < 45 * MINUTE)
        return ts.Minutes + " minutes ago";

      if (delta < 90 * MINUTE)
        return "an hour ago";

      if (delta < 24 * HOUR)
        return ts.Hours + " hours ago";

      if (delta < 48 * HOUR)
        return "yesterday";

      if (delta < 30 * DAY)
        return ts.Days + " days ago";

      if (delta < 12 * MONTH)
      {
        int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
        return months <= 1 ? "one month ago" : months + " months ago";
      }
      else
      {
        int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
        return years <= 1 ? "one year ago" : years + " years ago";
      }
    }
  }
}
