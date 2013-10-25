#region Dependencies

using System;
using System.Diagnostics;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;

#endregion

namespace LaubPlusCo.CrunchPng.Pipeline
{
  public class CrunchPng
  {
    public bool UseBrute
    {
      get { return Sitecore.Configuration.Settings.GetBoolSetting("CrunchPng.UseBrute", false); }
    }

    public void Process(CrunchPngPipelineArgs args)
    {
      var startInfo = new ProcessStartInfo
        {
          CreateNoWindow = true,
          UseShellExecute = false,
          FileName = args.PngCrunchExecutable,
          WindowStyle = ProcessWindowStyle.Hidden,
          Arguments = GetCrunchArguments(args)
        };
      try
      {
        using (var exeProcess = System.Diagnostics.Process.Start(startInfo))
        {
          exeProcess.WaitForExit();
        }
      }
      catch (Exception exception)
      {
        Log.Error("Could not call png crunch", exception, this);
        args.AddMessage("Could not call png crunch", PipelineMessageType.Error);
        args.AbortPipeline();
      }
    }

    protected virtual string GetCrunchArguments(CrunchPngPipelineArgs args)
    {
      return string.Concat(UseBrute ? " -brute " : string.Empty, args.TemporarySourceFile, " ", args.TemporaryDestinationFile);
    }
  }
}