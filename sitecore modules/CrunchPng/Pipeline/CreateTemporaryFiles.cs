#region Dependencies

using System.IO;

#endregion

namespace LaubPlusCo.CrunchPng.Pipeline
{
  public class CreateTemporaryFiles
  {
    public void Process(CrunchPngPipelineArgs args)
    {
      CreateFiles(args);
      WriteMediaStreamToSourceFile(args);
    }

    protected virtual void CreateFiles(CrunchPngPipelineArgs args)
    {
      args.TemporarySourceFile = CreateTempPngFile();
      args.TemporaryDestinationFile = args.TemporarySourceFile.Replace(".png", "_crunched.png");
    }

    protected virtual string CreateTempPngFile()
    {
      var tempFilePath = Path.GetTempFileName();
      var tempFilePngPath = tempFilePath.Replace(".tmp", ".png");
      File.Move(tempFilePath, tempFilePngPath);
      return tempFilePngPath;
    }

    protected virtual void WriteMediaStreamToSourceFile(CrunchPngPipelineArgs args)
    {
      var mediaStream = args.ImageToCrunch.GetMediaStream();
      var fileStream = File.Open(args.TemporarySourceFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
      mediaStream.CopyTo(fileStream);
      fileStream.Flush(true);
      fileStream.Close();
    }
  }
}