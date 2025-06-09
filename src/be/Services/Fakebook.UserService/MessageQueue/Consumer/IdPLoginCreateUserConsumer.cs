using Fakebook.MessageQueueHandler.Consumer;
using Fakebook.MessageQueueHandler.Models.Core;
using Fakebook.MessageQueueHandler.Utils;
using Fakebook.SynchronousModel.Models.IdPService.Users;
using Fakebook.UserService.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client.Events;
using System.Text;

namespace Fakebook.UserService.MessageQueue.Consumer
{
    public class IdPLoginCreateUserConsumer : IConsumer<IdPLoginCreateUserModel>
    {
        public static string RoutingKey => RoutingQueueConstants.UserService.UserService_IPD_Login_Create_User;

        public async Task HandlerAsync(object sender, BasicDeliverEventArgs e, Message<IdPLoginCreateUserModel> message, IServiceProvider serviceProvider)
        {
            System.Console.WriteLine("===================== Start IdPLoginCreateUserConsumer");

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;

                var userService = scopedServiceProvider.GetRequiredService<IUserService>();

                await userService.SyncCreatedIdPUserAsync(message!.Data);
            }

            System.Console.WriteLine("===================== End IdPLoginCreateUserConsumer");
        }
    }
}