using Rocket.API;

namespace BackRM4
{
    public class BackConfiguration : IRocketPluginConfiguration
    {

        public bool allowOtherPlayer;
        
        public void LoadDefaults()
        {
            allowOtherPlayer = false;
        }
    }
}