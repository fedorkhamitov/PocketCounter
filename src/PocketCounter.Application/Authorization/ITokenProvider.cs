using PocketCounter.Application.Authorization.DataModels;

namespace PocketCounter.Application.Authorization;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}