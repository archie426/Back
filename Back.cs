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
        private readonly IPermissionProvider _permissionProvider;
        private readonly IPermissionChecker _permissionChecker;
        
        public Back(IDependencyContainer container, IEventBus eventBus, IPermissionProvider permissionProvider, IPermissionChecker permissionChecker) : base ("Return to your last death!", container)
        {
            _eventBus = eventBus;
            _permissionProvider = permissionProvider;
            _permissionChecker = permissionChecker;
        }

        protected override async Task OnActivate(bool isFromReload)
        {
            Logger.LogInformation("Back plugin by hiilikewiki loaded!");
            _eventBus.AddEventListener(this, new EventHandler(_permissionProvider, _permissionChecker));
        }
    }
}