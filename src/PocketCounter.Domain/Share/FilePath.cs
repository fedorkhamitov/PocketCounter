using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using System.Linq;

namespace PocketCounter.Domain.Share;

public record FilePath
{
    public string Path { get; }

    [JsonConstructor]
    private FilePath(string path)
    {
        Path = path;
    }

    public static Result<FilePath, Error> Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path.ToString()))
            return Errors.General.ValueIsRequired("File path");
        
        var pathExtension = System.IO.Path.GetExtension(path)?
            .TrimStart('.')
            .ToLower();
        
        if (string.IsNullOrWhiteSpace(pathExtension) ||
            !Enum.GetNames(typeof(PhotoFileExtensionFormat))
                .Any(ex => ex.Equals(pathExtension, StringComparison.OrdinalIgnoreCase)))
            return Errors.General.ValueIsInvalid("Path for PhotoFile");
        
        return new FilePath(path);
    }
}