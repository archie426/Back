using System.Threading.Tasks;
using Rocket.API.Commands;
using Rocket.API.Permissions;
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


        public BackCommand(EventHandler eventHandler, IPermissionChecker permissionChecker)
        {
            _eventHandler = eventHandler;
            _permissionChecker = permissionChecker;
        }

        public async Task ExecuteAsync(ICommandContext context)
        {
            UnturnedPlayer player = ((UnturnedUser)context.User).Player;
            
            PermissionResult permResult = await _permissionChecker.CheckPermissionAsync(context.User, "back");

            switch(permResult)
            {
                case PermissionResult.Grant:
                    if (_eventHandler.deaths.ContainsKey(context.User))
                    {
                        await player.Entity.TeleportAsync(_eventHandler.deaths[context.User], player.Entity.Rotation);
                        await context.User.SendMessageAsync("You have been teleported back to your last death");
                    }
                    else
                    {
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