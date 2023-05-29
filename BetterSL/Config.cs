using System.ComponentModel;


namespace BetterSL
{
    public class Config
    {
        [Description("Enable the plugin?")]
        public bool PluginEnabled { get; set; } = true;
        
        [Description("How much damage should 096's base attack do?")]
        public float Scp096SwingDamage { get; set; } = 65f;
        
        [Description("Max speed of SCP-049-2's Lobotomized Bloodlust ability")]
        public float LobotomizedBloodlustMaxSpeed { get; set; } = 5.4f;

        [Description("SCP 106's starting health")]
        public float Scp106MaxHP { get; set; } = 1800;

        [Description("SCP 049's damage resistence? (default is 20 in vanilla)")]
        public int Scp049Armour { get; set; } = 35;

        [Description("Player join broadcast text")]
        public string PlayerJoinBroadcastText { get; set; } = "Welcome to the \'rebalanced\' SL. All content is still in early development, things may change.";

        [Description("Player join broadcast length (ushort)")]
        public ushort PlayerJoinBroadcastDuration { get; set; } = 20;
    }
}
