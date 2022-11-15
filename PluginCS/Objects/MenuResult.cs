using System.Collections.Generic;

namespace PluginCS.Objects
{
  public class MenuResult
  {
    public List<string> Args { get; init; }

    public string Command { get; init; }

    public bool Break { get; set; }
  }
}