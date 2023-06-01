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

        [Description("How much AP should 079 spawn with?")]
        public int Scp079SpawnAp { get; set; } = 0;

        [Description("SCP 079 regen per tier, goes from tier 0 to 5.")]
        public float[] Scp079RegenRate { get; set; } = { 1.7f, 2.5f, 4.1f, 5.6f, 7.1f };

        [Description("How long should SCP 079's lockdown last?")]
        public float Scp079LockdownLength { get; set; } = 10f;

        [Description("How much should lockdown cost to use? (Precent of total AP)")]
        public float Scp079LockdownCostPercent { get; set; } = 0.8f;

        [Description("SCP 079's AP regen rate when lockdown is used. 0 will result in no regen.")]
        public float[] Scp079LockdownRegenMultiplier { get; set; } = { 0, 0, 0, 0.25f, 0.25f };

        [Description("SCP 939's base claw cooldown (vanilla is 0.6)")]
        public float Scp939ClawCooldown { get; set; } = 0.7f;

        [Description("Should SCP 939 be able to deploy cloud in elevators?")]
        public bool Scp939CanUseCloudInElevator { get; set; } = false;

        [Description("Message to show to 939 players who try to use gas in elevators")]
        public string Scp939GasInElevatorMessage { get; set; } = "You can't deploy amnestic cloud in elevators!";

        [Description("How fast should SCP 049 'sprint' at when using the sense ability?")]
        public float Scp049SenseAbilitySpeed { get; set; } = 5.8f;

        [Description("How many times can SCP 049 ressurct 049-2 instances?")]
        public int Scp049Max0492Ressurection { get; set; } = 1;

        [Description("Should SCP 500 give you back all your stamina on use?")]
        public bool Scp500GivesStamina { get; set; } = true;

        [Description("Should a facility manager keycard spawn in 049's room?")]
        public bool FacilityManagerSpawnsIn049 { get; set; } = true;    

        [Description("Player join broadcast text")]
        public string PlayerJoinBroadcastText { get; set; } = "Welcome to the \'rebalanced\' SL. All content is still in early development, things may change.";

        [Description("Player join broadcast length (ushort)")]
        public ushort PlayerJoinBroadcastDuration { get; set; } = 20;
    }
}
