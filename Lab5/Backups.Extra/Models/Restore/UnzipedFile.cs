namespace Backups.Extra.Models.Restore;

public class UnzipedFile
{
    public UnzipedFile(byte[] data, string name)
    {
        Name = name;
        Data = data;
    }

    public byte[] Data { get; }
    public string Name { get; }
}