#region Dependencies

using System.IO;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;

#endregion

namespace LaubPlusCo.CrunchPng.Pipeline
{
  public class LocatePngCrunchExecutable
  {
    public void Process(CrunchPngPipelineArgs args)
    {
      args.PngCrunchExecutable = HttpContext.Current.Server.MapPath("/bin/pngcrush_1_7_67_w64.exe");
      if (!string.IsNullOrEmpty(args.PngCrunchExecutable) && File.Exists(args.PngCrunchExecutable))
        return;
      Log.Warn("Cannot find pngcrunch executable", this);
      args.AddMessage("Cannot find pngcrunch executable", PipelineMessageType.Error);
      args.AbortPipeline();
    }
  }
}