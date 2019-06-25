using System.Threading.Tasks;
using Rocket.API.Configuration;
using Rocket.API.DependencyInjection;
using Rocket.API.Eventing;
using Rocket.API.Permissions;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;

namespace Back
{
    public class Back : Plugin<BackConfiguration>
    {
        private readonly IEventBus _eventBus;
        private readonly IPermissionChecker _permissionChecker;
        private readonly IConfiguration _config;
        private BackConfiguration _configuration;
        
        public Back(IDependencyContainer container, IEventBus eventBus, IPermissionChecker permissionChecker, IConfiguration config) : base ("Return to your last death!", container)
        {
            _eventBus = eventBus;
            _permissionChecker = permissionChecker;
            _config = config;
        }

        protected override async Task OnActivate(bool isFromReload)
        {
            _configuration = _config.Get<BackConfiguration>();
            _eventBus.AddEventListener(this, new EventHandler(_permissionChecker, _configuration));
            Logger.LogInformation("Back plugin by hiilikewiki loaded!");
        }
    }
}