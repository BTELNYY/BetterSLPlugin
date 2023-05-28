using System.ComponentModel;


namespace BetterSL
{
    public class Config
    {
        [Description("Enable the plugin?")]
        public bool PluginEnabled { get; set; } = true;

        [Description("How much health should 106 start with?")]
        public float Scp106StarterHealth { get; set; } = 1800f;
    }
}
