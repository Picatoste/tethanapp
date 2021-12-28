﻿using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using System.Threading;
using System.Threading.Tasks;

namespace ThetanCore
{
  public class ThetanNotification : IThetanNotification
  {
    private const string crossed_swords_symbol = "=?UTF-8?Q?=E2=9A=94?=";
    private const string money_with_wings_symbol = "=?UTF-8?Q?=F0=9F=92=B8?=";
    private const string banknote_with_dollar_sign_symbol = "=?UTF-8?Q?=F0=9F=92=95?=";
    private const string chart_up_dollar_sign_symbol = "=?UTF-8?Q?=F0=9F=93=88?=";
    private const string link_sign_symbol = "=?UTF-8?Q?=F0=9F=94=97?=";
    private readonly HashSet<Thetan> thetanNotified = new HashSet<Thetan>(new ThetanComparer());

    private static ManualResetEvent mre = new ManualResetEvent(false);
    public ThetanNotification()
    {

    }
    public bool Notify(IEnumerable<Thetan> thetansPendingProcess, string emailTo, double minPrice, double maxPrice, double minRoiProfit50Percent)
    {
      bool notify = true;
      var thetansCandidateToNotify = thetansPendingProcess.Where(thetan =>
          thetan.PriceConverted >= minPrice
          && thetan.PriceConverted <= maxPrice
          && thetan.Roi50PerCent >= minRoiProfit50Percent);

      foreach (Thetan thetanToNotify in thetansCandidateToNotify.Where(thetan => !this.thetanNotified.Contains(thetan)))
      {
        this.thetanNotified.Add(thetanToNotify);
        Task.Factory.StartNew(() => SendEmail(emailTo, thetanToNotify));
        mre.WaitOne();
      }
      return notify;
    }
    //  <?xml version = "1.0" encoding="utf-8" ?>
    //  <configuration>
    //  <host>smtp.gmail.com</host>
    //  <port>587</port>
    //  <user>thetanmailer@gmail.com</user>
    //  <password>thetanthetan</password>
    //  <enableSsl>true</enableSsl>
    // </configuration>
    private bool SendEmail(string emailTo, Thetan thetan)
    {
      try
      {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("thetanmailer@gmail.com"));
        email.To.Add(MailboxAddress.Parse(emailTo));
        email.Subject = $"{ crossed_swords_symbol} NEW! { crossed_swords_symbol}| {chart_up_dollar_sign_symbol} : { thetan.Roi50PerCent.ToString("0") }% | ${ thetan.PriceConverted.ToString("0") } | {link_sign_symbol} { thetan.LinkMarket }";
        email.Body = new TextPart(TextFormat.Html)
        {
          Text =
            $@"<h1>New { thetan.Name.ToUpperInvariant() } thetan notified! </h1>
            <div style='float:left'>
              <img width='100' height='100' src='{ thetan.avatarSmall}' alt='Image' />
            </div>
            <div style='float:left'>
            <strong>ROI50</strong> { thetan.Roi50PerCent }%
            </div>
            <div style='float:left'>
            <strong>Price</strong> ${ thetan.PriceConverted }
            </div>
            <div style='float:left'>
            <strong>Link</strong> { thetan.LinkMarket.ToString() } 
            </div>"
        };

        // send email
        using (var smtp = new SmtpClient())
        {
          smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
          smtp.Authenticate("thetanmailer@gmail.com", "thetanthetan");
          smtp.Send(email);
          smtp.Disconnect(true);
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        mre.Set();
      }
      return true;
    }
    public class ThetanComparer : EqualityComparer<Thetan>
    {
      public override bool Equals(Thetan x, Thetan y)
      {
        return x.Id == y.Id;
      }

      public override int GetHashCode(Thetan obj)
      {
        if (obj == null) return 0;
        return obj.LinkMarket.GetHashCode() ^ obj.Id.GetHashCode();
      }
    }
  }
}
