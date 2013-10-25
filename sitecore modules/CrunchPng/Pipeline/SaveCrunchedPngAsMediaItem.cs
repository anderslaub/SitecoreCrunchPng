#region Dependencies

using System.IO;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;

#endregion

namespace LaubPlusCo.CrunchPng.Pipeline
{
  public class SaveCrunchedPngAsMediaItem
  {
    public bool ReplaceExisting
    {
      get { return Sitecore.Configuration.Settings.GetBoolSetting("CrunchPng.ReplaceExisting", false); }
    }

    public void Process(CrunchPngPipelineArgs args)
    {
      if (!File.Exists(args.TemporaryDestinationFile))
      {
        args.AddMessage("File is not a real png image", PipelineMessageType.Error);
        args.AbortPipeline();
        return;
      }
      var path = args.ImageToCrunch.InnerItem.Paths.FullPath;
      using (new SecurityDisabler())
      {
        var options = new MediaCreatorOptions
          {
            AlternateText = args.ImageToCrunch.Alt,
            Database =  args.ImageToCrunch.Database,
            Destination = ReplaceExisting ? path : string.Concat(path, "_crunched"),
            FileBased =  args.ImageToCrunch.FileBased,
            Language =  args.ImageToCrunch.InnerItem.Language,
            IncludeExtensionInItemName =  false,
            KeepExisting = false
          };
        MediaManager.Creator.CreateFromFile(args.TemporaryDestinationFile, options);
      }
    }
  }
}
