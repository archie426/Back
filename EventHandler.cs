using System.Collections.Generic;
using System.Threading.Tasks;
using Rocket.API.Eventing;
using Rocket.API.Permissions;
using Rocket.API.User;
using Rocket.Unturned.Player.Events;
using Vector3 = System.Numerics.Vector3;

namespace Back
{
    public class EventHandler : IEventListener<UnturnedPlayerDeadEvent>
    {

        private readonly IPermissionProvider _permissionProvider;
        private readonly IPermissionChecker _permissionChecker;

        public Dictionary<IUser, Vector3> deaths;
        
        
        public EventHandler(IPermissionProvider provider, IPermissionChecker checker)
        {
            _permissionProvider = provider;
            _permissionChecker = checker;
        }

        [Rocket.Core.Eventing.EventHandler]
        public async Task HandleEventAsync(IEventEmitter emitter, UnturnedPlayerDeadEvent @event)
        {
            
            Vector3 location = @event.Position;

            if (_permissionChecker.CheckPermissionAsync(@event.Player.User, "back").Result == PermissionResult.Grant)
            {
                if (!deaths.ContainsKey(@event.Player.User))
                {
                    deaths.Add(@event.Player.User, location);  
                }
                else
                {
                    deaths[@event.Player.User] = location;
                }
            }
        }
        
    }
}