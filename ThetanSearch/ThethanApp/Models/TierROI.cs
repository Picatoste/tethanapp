using System.ComponentModel;

namespace ThethanApp.Models
{
  public enum TierROI
  {
    [Description("white")]
    Error,
    [Description("#FF7EFD")]
    F,
    [Description("#807FFE")]
    E,
    [Description("#7FBFFD")]
    D,
    [Description("#7EFF80")]
    C,
    [Description("#FFFB7C")]
    B,
    [Description("#FFBD80")]
    A,
    [Description("#FD7E7D")]
    S
  }
}
