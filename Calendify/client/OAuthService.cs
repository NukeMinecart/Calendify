
using Google.Apis.Util.Store;

namespace Calendify.client;

public static class OAuthService
{
    public static HashSet<string> ActiveUserNames { get; private set; } = [];
    
    public static readonly List<OAuthUser> ActiveUsers = [];

    public static readonly FileDataStore CalendifyDataStore =
        new (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Calendify");

    public static async Task AuthenticateUsers()
    {
        var fetchUserData = await CalendifyDataStore.GetAsync<HashSet<string>>("activeUsers");
        ActiveUserNames = fetchUserData ?? ActiveUserNames;

        foreach (var user in ActiveUserNames.Select(userName => new OAuthUser(userName)))
        {
            await user.AuthenticateUser();
            ActiveUsers.Add(user);
            await user.Credential.RefreshTokenAsync(CancellationToken.None);
        }
        Console.WriteLine("Active users refreshed");
    }

    public static void SaveUsers()
    {
        CalendifyDataStore.StoreAsync("activeUsers", ActiveUserNames).Wait();
    }
    
}