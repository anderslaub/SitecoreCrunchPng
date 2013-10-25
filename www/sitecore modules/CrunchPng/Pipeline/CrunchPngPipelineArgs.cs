#region Dependencies

using Sitecore.Data.Items;
using Sitecore.Pipelines;

#endregion

namespace LaubPlusCo.CrunchPng.Pipeline
{
  public class CrunchPngPipelineArgs : PipelineArgs
  {
    public CrunchPngPipelineArgs(Item mediaItem)
    {
      ImageToCrunch = new MediaItem(mediaItem);
    }

    public MediaItem ImageToCrunch { get; set; }
    public string TemporarySourceFile { get; set; }
    public string TemporaryDestinationFile { get; set; }
    public string PngCrunchExecutable { get; set; }
  }
}