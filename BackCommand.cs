using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace BackRM4
{
    public class BackCommand : IRocketCommand
    {
        public void Execute(IRocketPlayer player, params string[] context)
        {
            UnturnedPlayer target;
            bool otherUser;

            if (context.Length >= 1 && Back.Instance.Configuration.Instance.allowOtherPlayer)
            {
                target = UnturnedPlayer.FromName(context[0]);
                otherUser = true;
            }
            else
            {
                if (context.Length >= 1 && !Back.Instance.Configuration.Instance.allowOtherPlayer)
                {
                    UnturnedChat.Say(player,"Other user functionality is disabled in the config");
                    return;
                }
                else
                {
                    target = UnturnedPlayer.FromName(player.DisplayName);
                    otherUser = false;
                }
            }

            if (!(target is UnturnedPlayer paramType))
            {
                UnturnedChat.Say(player, "Invalid parameter");
                return;
            }
            
            if (Back.Instance.deaths.ContainsKey(target))
            {
                target.Teleport(Back.Instance.deaths[target], target.Rotation);
                UnturnedChat.Say(player, "You have been teleported back to your last death");
            }
            else
            {
                UnturnedChat.Say(player, otherUser ? "This player hasn't died yet!" : "You haven't died yet!");
            }
        }

        public AllowedCaller AllowedCaller { get; } = AllowedCaller.Player;
        public string Name { get; } = "back";
        public string Help { get; } = "Teleports you to your last death location";
        public string Syntax { get; } = "/back";
        public List<string> Aliases { get; }
        public List<string> Permissions => new List<string>() {"back"};
    }
}
