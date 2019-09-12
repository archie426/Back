using System.Collections.Generic;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Player;

namespace BackRM4
{
    public class BackCommand : IRocketCommand
    {
        public void Execute(IRocketPlayer player, string[] context)
        {
            bool otherUser;
            
            //watch this space
            
        }

        public AllowedCaller AllowedCaller { get; }
        public string Name { get; } = "back";
        public string Help { get; } = "Teleports you to your last death location";
        public string Syntax { get; } = "/back";
        public List<string> Aliases { get; }
        public List<string> Permissions => new List<string>() {"back"};
    }
}