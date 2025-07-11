﻿namespace PocketCounter.Infrastructure.Authentication;

public class JwtOptions
{
    public const string JWT = nameof(JWT);
    
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string Key { get; init; }
    public string ExpiredMinutesTime { get; init; }
}