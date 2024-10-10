
using Google.Apis.Util.Store;

namespace Calendify.client;

public static class OAuthService
{
    public static HashSet<string>? ActiveUserNames { get; private set; }

    public static List<OAuthUser> ActiveUsers { get; set; } = [];

    public static readonly FileDataStore CalendifyDataStore =
        new (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Calendify");
    
    public static async Task FetchActiveUsers()
    {
        var fetchUserData = await CalendifyDataStore.GetAsync<HashSet<string>>("activeUsers");
        ActiveUserNames ??= fetchUserData;
    }
    
    public static async Task AuthenticateUsers()
    {
        ActiveUsers.Clear();
        foreach (var user in ActiveUserNames!.Select(userName => new OAuthUser(userName)))
        {
            user.AuthenticateUser();

            if(user.Credential == null) continue;
            
            if(user.Credential.Token.IsStale)
                Console.WriteLine(await user.Credential!.RefreshTokenAsync(CancellationToken.None));
            
            ActiveUsers.Add(user);
        }
    }

    public static void SaveUsers()
    {
        CalendifyDataStore.StoreAsync("activeUsers", ActiveUserNames).Wait();
    }
    
}