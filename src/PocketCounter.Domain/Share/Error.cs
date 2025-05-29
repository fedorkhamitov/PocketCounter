﻿using System.Runtime.InteropServices.JavaScript;

namespace PocketCounter.Domain.Share;

public record Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public static Error Validation(string code, string message) => new Error(code, message, ErrorType.Validation);
    public static Error NotFound(string code, string message) => new Error(code, message, ErrorType.NotFound);
    public static Error Failure(string code, string message) => new Error(code, message, ErrorType.Failure);
    public static Error Conflict(string code, string message) => new Error(code, message, ErrorType.Conflict);
    public static Error Authentication(string code, string message) => new Error(code, message, ErrorType.Authentication);
    
    public string Serialize()
    {
        return string.Join(Constants.SEPARATOR, Code, Message, Type);
    }
    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split(Constants.SEPARATOR);
        if (parts.Length < 3)
            throw new ArgumentException("Invalid serialized format");
        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
            throw new ArgumentException("Invalid serialized format");
        return new Error(parts[0], parts[1], type);
    }
}