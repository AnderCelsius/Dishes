using AutoMapper;
using Dishes.Common.Models;
using Dishes.Webhook.Data;

namespace Dishes.Webhook.Utils;

public class Profiles : Profile
{
    public Profiles()
    {
        CreateMap<WebhookSubscriptionDTO, WebhookSubscription>().ReverseMap();
    }
}
