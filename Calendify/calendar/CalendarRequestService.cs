using Calendify.client;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace Calendify.calendar;

public static class CalendarRequestService
{
    public static CalendarService Service { get; } = new (new BaseClientService.Initializer
        { ApplicationName = "Calendify" });
    
    public static TReturn MakeRequest<TRequest, TReturn>(TRequest request) where TRequest : CalendarBaseServiceRequest<TReturn>
    {
        request.AddCredential(OAuthService.ActiveUsers[0].Credential);
        return request.Execute();
    }
}