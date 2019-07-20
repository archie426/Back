using System.Threading.Tasks;
using Rocket.API.Commands;
using Rocket.API.Configuration;
using Rocket.API.Permissions;
using Rocket.API.Player;
using Rocket.API.User;
using Rocket.Core.User;
using Rocket.Unturned.Player;

namespace Back
{
    public class BackCommand : ICommand
    {
        public bool SupportsUser(IUser user) => true;

        public string[] Aliases { get; } = null;

        public string Description { get; } = "Sends you back to your last death";

        public string Name { get; } = "Back";

        public string Summary { get; } = "Sends you back to your last death";

        public string Syntax { get; } = null;

        public IChildCommand[] ChildCommands { get; } = null;


        private readonly EventHandler _eventHandler;
        private readonly IPermissionChecker _permissionChecker;
        private readonly IConfiguration _configuration; 
        private readonly BackConfiguration _config;


        public BackCommand(EventHandler eventHandler, IPermissionChecker permissionChecker, IConfiguration configuration)
        {
            _eventHandler = eventHandler;
            _permissionChecker = permissionChecker;
            _configuration = configuration;
            _config = _configuration.Get<BackConfiguration>();
        }

        public async Task ExecuteAsync(ICommandContext context)
        {
            PermissionResult permResult = await _permissionChecker.CheckPermissionAsync(context.User, "back");

            IUser target;
            bool otherUser;

            if (context.Parameters.Length >= 1 && _config.allowOtherPlayer)
            {
                target = await context.Parameters.GetAsync<IUser>(0);
                otherUser = true;
            }
            else
            {
                if (context.Parameters.Length >= 1 && !_config.allowOtherPlayer)
                {
                    await context.User.SendMessageAsync("Other user functionality is disabled in the config");
                    return;
                }
                else
                {
                    target = context.User;
                    otherUser = false;
                }
            }

            if (!(target is IUser paramType))
            {
                await context.User.SendMessageAsync("Invalid parameter");
                return;
            }
            
            UnturnedPlayer player = ((UnturnedUser)target).Player;
            
            switch(permResult)
            {
                case PermissionResult.Grant:
                    if (_eventHandler.deaths.ContainsKey(target))
                    {
                        await player.Entity.TeleportAsync(_eventHandler.deaths[target], player.Entity.Rotation);
                        await context.User.SendMessageAsync("You have been teleported back to your last death");
                    }
                    else
                    {
                        if (otherUser)
                            await context.User.SendMessageAsync("This player hasn't died yet!");
                        else
                            await context.User.SendMessageAsync("You haven't died yet!");
                    }
                    break;
                case PermissionResult.Deny:
                    await context.User.SendMessageAsync("You do not have permission to use this command!"); //I swear there was a built in way for this?
                    break;
                case PermissionResult.Default:
                    await context.User.SendMessageAsync("You do not have permission to use this command!");
                    break;              
            }
        }
    }
}