using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;

namespace PocketCounter.Domain.ValueObjects;

public class PhotoFile : ValueObject
{
    public FilePath PathToStorage { get; }

    [JsonConstructor]
    private PhotoFile(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }

    public static Result<PhotoFile, Error> Create(FilePath path)
    {
        if (string.IsNullOrWhiteSpace(path.Path))
            return Errors.General.ValueIsRequired("Path");
        return new PhotoFile(path);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PathToStorage;
    }
}