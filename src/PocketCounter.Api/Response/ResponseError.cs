namespace PocketCounter.Api.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);