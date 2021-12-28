using System.Collections.Generic;

namespace ThetanSearch
{
  public class RequestResultThetanData
  {
    public bool success { get; set; }
    public List<ThetanData> data { get; set; }
    public Page page { get; set; }
  }

}
