using System.Diagnostics.CodeAnalysis;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Newtonsoft.Json;

namespace Calendify.client;

public class OAuthUser(string user)
{
    public UserCredential? Credential { get; private set; }

    public void AuthenticateUser()
    {
        var stream = new FileStream("client/client_secrets.json", FileMode.Open, FileAccess.Read);
        var jsonObject = JsonConvert.DeserializeObject<Root>(new StreamReader(stream).ReadToEnd());
        var secrets = new ClientSecrets{ClientId = jsonObject!.installed.client_id, ClientSecret = jsonObject!.installed.client_secret};
        stream.Close();
        
        var task = GoogleWebAuthorizationBroker.AuthorizeAsync(
            secrets, [CalendarService.Scope.Calendar], user,
            CancellationToken.None, OAuthService.CalendifyDataStore); ;
        task.Wait();
        
        Credential = task.Result;
        OAuthService.ActiveUserNames!.Add(user);
    }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public record Installed(
        string client_id,
        string project_id,
        string auth_uri,
        string token_uri,
        string auth_provider_x509_cert_url,
        string client_secret,
        IReadOnlyList<string> redirect_uris
    );

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public record Root(
        Installed installed
    );
}