using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Newtonsoft.Json;

namespace Calendify.client;

public class OAuthUser
{
    public UserCredential? Credential { get; private set; }

    private readonly string _user;

    public OAuthUser(string user)
    {
        _user = user;
        AuthenticateUser();
    }

    private async void AuthenticateUser()
    {
        await using var stream = new FileStream("client/client_secrets.json", FileMode.Open, FileAccess.Read);
        Credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            (await GoogleClientSecrets.FromStreamAsync(stream)).Secrets,
            [CalendarService.Scope.Calendar],
            _user, CancellationToken.None, OAuthService.CalendifyDataStore);
        OAuthService.ActiveUserNames.Add(_user);
    }
}