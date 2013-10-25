#region Dependencies

using System;
using System.Linq;
using LaubPlusCo.CrunchPng.Pipeline;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

#endregion

namespace LaubPlusCo.CrunchPng.InfraStructure
{
  public class CrunchPngCommand : Command
  {
    public override void Execute(CommandContext context)
    {
      if (!context.Items.Any())
        return;
      Assert.IsNotNull(context.Items[0], "Error, no item in context");
      var pipelineArgs = new CrunchPngPipelineArgs(context.Items[0]);
      CorePipeline.Run("crunchPng", pipelineArgs);
      if (!pipelineArgs.Aborted && string.IsNullOrEmpty(pipelineArgs.Message))
        return;
      var errorMessage = pipelineArgs.GetMessages().FirstOrDefault(m => m.Type == PipelineMessageType.Error);
      SheerResponse.Alert(errorMessage != null ? errorMessage.Text : "An error occurred", true);
    }

    public override CommandState QueryState(CommandContext context)
    {
      Error.AssertObject(context, "context");
      if (context.Items.Length == 0)
        return CommandState.Hidden;
      if (!IsPngMediaItem(context.Items[0]))
        return CommandState.Hidden;
      return base.QueryState(context);
    }

    protected bool IsPngMediaItem(Item item)
    {
      var mediaItem = new MediaItem(item);
      return mediaItem.Extension.Equals("png", StringComparison.InvariantCultureIgnoreCase)
             || mediaItem.MimeType.Equals("image/png", StringComparison.InvariantCultureIgnoreCase);
    }
  }
}