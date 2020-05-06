using System.Collections.Generic;
using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using UnityEngine;

namespace BackRM4
{
    public class Back : RocketPlugin<BackConfiguration>
    {

        public static Back Instance;
        
        public override void LoadPlugin()
        {
            Instance = this;
            UnturnedPlayerEvents.OnPlayerDead += UnturnedPlayerEventsOnOnPlayerDead;
        }
        
        public override void UnloadPlugin()
        {
            Instance = null;
            UnturnedPlayerEvents.OnPlayerDead -= UnturnedPlayerEventsOnOnPlayerDead;
        }
        
        
        public Dictionary<UnturnedPlayer, Vector3> deaths = new Dictionary<UnturnedPlayer, Vector3>();

        private void UnturnedPlayerEventsOnOnPlayerDead(UnturnedPlayer player, Vector3 position)
        {
            if (!DeathShouldBeLogged(player))
                return;

            if (!deaths.ContainsKey(player))
                deaths.Add(player, position);
            else
                deaths[player] = position;
        }


        public bool DeathShouldBeLogged(UnturnedPlayer player)
        {
            return Instance.Configuration.Instance.allowOtherPlayer || player.HasPermission("back");
        }
        
    }
}
