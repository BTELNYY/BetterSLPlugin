using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Policy;

namespace BetterSL
{
    public class Config
    {
        [Description("Enable the plugin?")]
        public bool PluginEnabled { get; set; } = true;
        
        [Description("How much damage should 096's base attack do?")]
        public float Scp096SwingDamage { get; set; } = 65f;

        [Description("SCP 106's starting health")]
        public float Scp106MaxHp { get; set; } = 1800;

        [Description("How much damage should SCP 106 inflict on hit?")]
        public float Scp106AttackDamage { get; set; } = 45;

        [Description("How much vigor should SCP 106 get upon killing someone?")]
        public float Scp106OnKillVigor { get; set; } = 0.2f;

        [Description("How much vigor should SCP 106 get upon clicking someone?")]
        public float Scp106OnAttackVigor { get; set; } = 0.3f;

        [Description("How long (in seconds) should SCP 106's damage over time last?")]
        public float Scp106AttackDamageOverTimeDuration { get; set; } = 4f;

        [Description("How much should SCP 106's damage over time attack do per tick?")]
        public float Scp106AttackDamageOverTimeDamage { get; set; } = 5f;

        [Description("How often should the damage from SCP 106 over time effect tick? (seconds)")]
        public float Scp106TickEvery { get; set; } = 1f;

        [Description("Amount of time before a body auto drops from PD")]
        public int Scp106BodyDropDelay { get; set; } = 45;

        [Description("SCP 049's damage resistence? (default is 20 in vanilla)")]
        public int Scp049Armour { get; set; } = 35;

        [Description("How many times can SCP 049 revive a instance of SCP 049-2?")]
        public int Scp049Max0492Ressurection { get; set; } = 2;

        [Description("Max speed of SCP-049-2's Lobotomized Bloodlust ability")]
        public float Scp0492BloodlustMaxSpeed { get; set; } = 5.4f;

        [Description("What message should be sent to all SCPs when a generator is turned on and SCP 079 pings it?")]
        public string Scp079GeneratorAlertMessage { get; set; } = "<color=purple>SCP 079</color> has pinged a generator in {room}";

        [Description("What range should the SCP 079 Ping for a generator have?")]
        public float Scp079GeneratorPingRange { get; set; } = 25f;

        [Description("Termination reward multipliers for SCP 079.")]
        public Dictionary<Team, float> Scp079TerminationRewardMultipliers { get; set; } = new Dictionary<Team, float>{ 
            [Team.ClassD] = 0.5f,
            [Team.Scientists] = 0.5f,
            [Team.FoundationForces] = 1f,
            [Team.ChaosInsurgency] = 1f,
            [Team.OtherAlive] = 1f,
            [Team.SCPs] = 1f,
            [Team.Dead] = 1f
        };

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

        [Description("How many doors can SCP 079 have locked at the same time?")]
        public int Scp079MaxLockedDoors { get; set; } = 1;

        [Description("How much AP should SCP 079 lose when locking doors per tier?")]
        public float[] Scp079DoorLockCostPercent { get; set; } = { 0.2f, 0.17f, 0.15f, 0.12f, 0.1f };

        [Description("Limit of players 939 can damage with one claw swipe")]
        public int Scp939MaxAoePlayerHits { get; set; } = 2;

        [Description("SCP 939's base claw cooldown (vanilla is 0.6)")]
        public float Scp939ClawCooldown { get; set; } = 0.7f;

        [Description("Should SCP 939 be able to deploy cloud in elevators?")]
        public bool Scp939CanUseCloudInElevator { get; set; } = false;

        [Description("Message to show to 939 players who try to use gas in elevators")]
        public string Scp939GasInElevatorMessage { get; set; } = "You can't deploy amnestic cloud in elevators!";

        [Description("How fast should SCP 049 'sprint' at when using the sense ability?")]
        public float Scp049SenseAbilitySpeed { get; set; } = 5.8f;

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
