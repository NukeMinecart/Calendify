using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Newtonsoft.Json;

namespace Calendify.client;

public class OAuthUser(string user)
{
    public UserCredential? Credential { get; private set; }

    public async Task AuthenticateUser()
    {
        Console.WriteLine(user);
        await using var stream = new FileStream("client/client_secrets.json", FileMode.Open, FileAccess.Read);
        Credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            (await GoogleClientSecrets.FromStreamAsync(stream)).Secrets,
            [CalendarService.Scope.Calendar],
            user, CancellationToken.None, OAuthService.CalendifyDataStore);
        OAuthService.ActiveUserNames.Add(user);
    }
}