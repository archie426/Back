using System.Collections.Generic;
using System.Threading.Tasks;
using Rocket.API.Configuration;
using Rocket.API.Eventing;
using Rocket.API.Permissions;
using Rocket.API.User;
using Rocket.Unturned.Player.Events;
using Vector3 = System.Numerics.Vector3;

namespace Back
{
    public class EventHandler : IEventListener<UnturnedPlayerDeadEvent>
    {
        
        private readonly IPermissionChecker _permissionChecker;
        private readonly BackConfiguration _config;
        public Dictionary<IUser, Vector3> deaths;
        
        
        public EventHandler(IPermissionChecker checker, BackConfiguration config)
        {
            _permissionChecker = checker;
            _config = config;
        }


        public async Task<bool> DeathShouldBeLogged(UnturnedPlayerDeadEvent @event)
        {
            if (_config.allowOtherPlayer)
            {
                return true;
            }
            return !_config.allowOtherPlayer && _permissionChecker.CheckPermissionAsync(@event.Player.User, "back").Result == PermissionResult.Grant;
        }
        

        [Rocket.Core.Eventing.EventHandler]
        public async Task HandleEventAsync(IEventEmitter emitter, UnturnedPlayerDeadEvent @event)
        {
            
            Vector3 location = @event.Position;

            if (await DeathShouldBeLogged(@event))
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