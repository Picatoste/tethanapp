namespace ThetanCore.Interfaces
{
  public class SMTPConfig
  {
    public string Host { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public bool EnableSSL { get; set; }
  }

  public class ThetanEmailNotificationConfig
  {
    public SMTPConfig SMTPConfig { get; set; }
    public string EmailTo { get; set; }
  }

  public class ThetanConfig
  {
    public string ConnectionStringLiteDB { get; set; }
  }

  public class ThetanHostedServiceConfig
  {
    public bool Enabled { get; set; }
    public int PeriodInSeconds { get; set; }
  }
}
