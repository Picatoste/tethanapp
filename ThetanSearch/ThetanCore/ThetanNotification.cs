using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThetanCore.Interfaces;

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


    private readonly SMTPConfig sMTPConfig;

    private static ManualResetEvent mre = new ManualResetEvent(false);
    public ThetanNotification(
        IOptions<ThetanEmailNotificationConfig> thetanEmailNotificationConfig)
    {
      this.sMTPConfig = thetanEmailNotificationConfig.Value.SMTPConfig;
    }
    public bool Notify(IEnumerable<Thetan> thetansPendingProcess, string emailTo, Func<Thetan, bool> predicateWhere)
    {
      bool notify = true;
      var thetansCandidateToNotify = thetansPendingProcess.Where(predicateWhere);

      foreach (Thetan thetanToNotify in thetansCandidateToNotify.Where(thetan => !this.thetanNotified.Contains(thetan)))
      {
        this.thetanNotified.Add(thetanToNotify);
        Task.Factory.StartNew(() => SendEmail(emailTo, thetanToNotify));
        mre.WaitOne();
      }
      return notify;
    }
    
    private bool SendEmail(string emailTo, Thetan thetan)
    {
      try
      {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(sMTPConfig.User));
        email.To.Add(MailboxAddress.Parse(emailTo));
        email.Subject = $"{ crossed_swords_symbol} NEW! { crossed_swords_symbol}| {chart_up_dollar_sign_symbol} : { thetan.Roi50PerCent.ToString("0") }% / ${ thetan.ROIProfit.FirstOrDefault(x => x.WinRate == WinRateType.PerCent50)?.TotalProfit.ToString("0") }| ${ thetan.PriceConverted.ToString("0") }  |{link_sign_symbol} { thetan.LinkMarket }";
        email.Body = new TextPart(TextFormat.Html)
        {
          Text =
            $@"<h1>New { thetan.Name.ToUpperInvariant() } thetan notified! </h1>
            <div style='float:left; margin:5px;'>
              <img width='100' height='100' src='{ thetan.avatarSmall}' alt='Image' />
            </div>
            <div style='float:left;margin:5px;'>
            <div style='clear:both;float:left'>
            <strong>ROI50 %</strong> { thetan.Roi50PerCent.ToString("0") }%
            </div>
            <div style='clear:both;float:left'>
            <strong>ROI50 TotalProfit</strong> ${ thetan.ROIProfit.FirstOrDefault(x => x.WinRate == WinRateType.PerCent50)?.TotalProfit.ToString("0")}
            </div>
            <div style='clear:both;float:left'>
            <strong>Price</strong> ${ thetan.PriceConverted }
            </div>
            <div style='clear:both;float:left'>
            <strong>Link</strong> { thetan.LinkMarket.ToString() } 
            </div>
            <div style='clear:both;float:left'>
            ROI 30% in this thetan is POSITIVE (<strong>ROI30 Total Profit</strong>: ${ thetan.ROIProfit.FirstOrDefault(x => x.WinRate == WinRateType.PerCent30)?.TotalProfit.ToString("0")}).
            </div>
            </div>"
        };

        // send email
        using (var smtp = new SmtpClient())
        {
          smtp.Connect(sMTPConfig.Host, sMTPConfig.Port, (sMTPConfig.EnableSSL) ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
          smtp.Authenticate(sMTPConfig.User, sMTPConfig.Password);
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
  }
}
