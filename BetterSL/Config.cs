using System.ComponentModel;


namespace BetterSL
{
    public class Config
    {
        [Description("Enable the plugin?")]
        public bool PluginEnabled { get; set; } = true;

        [Description("How much health should 106 start with?")]
        public float Scp106StarterHealth { get; set; } = 1800f;
        
        [Description("How much damage should 096's base attack do?")]

        public float Scp096BitchSlapDamage { get; set; } = 65f;
        
        [Description("Max speed of SCP-049-2's Lobotomized Bloodlust ability")]

        public float LobotomizedBloodlustMaxSpeed { get; set; } = 5.4f;
    }
}
