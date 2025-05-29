namespace PocketCounter.Domain.ValueObjects;

public class PhotoFilesList
{
    public List<PhotoFile> Photos { get; set; } = new List<PhotoFile>();

    public PhotoFilesList() { }

    public PhotoFilesList(List<PhotoFile> photos)
    {
        Photos = photos;
    }
}


