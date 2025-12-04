using Azure.Messaging.ServiceBus;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.ServiceBus;

namespace MyRecipeBook.Infrastructure.Services.ServiceBus;

public class DeleteUserQueue(ServiceBusSender serviceBusSender) : IDeleteUserQueue
{
    public async Task SendMessage(User user)
    {
        await serviceBusSender.SendMessageAsync(new ServiceBusMessage(user.UserIdentifier.ToString()));
    }
}