using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ThetanCore;
using ThetanSearch;
using ThethanApp.Mappers;
using ThethanApp.Models;

namespace ThethanApp.Controllers
{
  public class HomeController : Controller
  {
    private readonly IThetanProvider thetanServices;

    public HomeController(IThetanProvider thetanServices)
    {
      this.thetanServices = thetanServices;
    }
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Thetan()
    {
      var config = new MapperConfiguration(cfg => {
        cfg.CreateMap<ThetanData, ThetanModel>();
        cfg.AddProfile<TethanProfile>();
      });


      var mapper = config.CreateMapper();

      ViewBag.LastUpdate = DateTime.Now.ToLongTimeString();
      return View(mapper.Map<IEnumerable<Thetan>, IEnumerable<ThetanModel>>(thetanServices.GetAllThetans()));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
