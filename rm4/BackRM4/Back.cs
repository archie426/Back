using System.Collections.Generic;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using UnityEngine;

namespace BackRM4
{
    public class Back : RocketPlugin<BackConfiguration>
    {

        public Back Instance;
        
        protected override void Load()
        {
            Instance = this;
            UnturnedPlayerEvents.OnPlayerDead += UnturnedPlayerEventsOnOnPlayerDead;
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
            //Need to figure out getting perms in RM4
            
            /*if (Instance.Configuration.Instance.allowOtherPlayer)
            {
                return true;
            } */ 

            return true;
        }
        
    }
}