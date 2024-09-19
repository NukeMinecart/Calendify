
using Google.Apis.Util.Store;

namespace Calendify.client;

public static class OAuthService
{
    public static HashSet<string> ActiveUserNames { get; private set; } = [];
    
    public static readonly List<OAuthUser> ActiveUsers = [];

    public static readonly FileDataStore CalendifyDataStore =
        new (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Calendify");

    public static void AuthenticateUsers()
    {
        var fetchUserData = CalendifyDataStore.GetAsync<HashSet<string>>("activeUsers");
        fetchUserData.Wait();
        ActiveUserNames = fetchUserData.Result ?? ActiveUserNames;

        foreach (var userName in ActiveUserNames)
        {
            ActiveUsers.Add(new OAuthUser(userName));
            Console.WriteLine($"User: {userName}");
        }
    }

    public static void SaveUsers()
    {
        CalendifyDataStore.StoreAsync("activeUsers", ActiveUserNames).Wait();
    }
    
}