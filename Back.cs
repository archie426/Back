using System.Threading.Tasks;
using Rocket.API.DependencyInjection;
using Rocket.API.Eventing;
using Rocket.API.Permissions;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;

namespace Back
{
    public class Back : Plugin
    {
        private readonly IEventBus _eventBus;
        private readonly IPermissionChecker _permissionChecker;
        
        public Back(IDependencyContainer container, IEventBus eventBus, IPermissionChecker permissionChecker) : base ("Return to your last death!", container)
        {
            _eventBus = eventBus;
            _permissionChecker = permissionChecker;
        }

        protected override async Task OnActivate(bool isFromReload)
        {
            Logger.LogInformation("Back plugin by hiilikewiki loaded!");
            _eventBus.AddEventListener(this, new EventHandler(_permissionChecker));
        }
    }
}