using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;

namespace PocketCounter.Domain.ValueObjects;

public class Dimensions : ValueObject
{
    public double Width { get; }
    public double Height { get; }
    public double Depth { get; }

    private Dimensions()
    {
    }

    private Dimensions(double width, double height, double depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }

    public static Result<Dimensions, Error> Create(double width, double height, double depth)
    {
        if (double.IsNaN(width) || double.IsNaN(height) || double.IsNaN(depth) ||
            double.IsInfinity(width) || double.IsInfinity(height) || double.IsInfinity(depth) ||
            width < 0 || height < 0 || depth < 0)
        {
            return Errors.General.ValueIsInvalid("Dimensions");
        }

        return new Dimensions(width, height, depth);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Width;
        yield return Height;
        yield return Depth;
    }
}