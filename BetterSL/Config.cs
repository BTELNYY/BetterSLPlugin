using BetterSL.Resources;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using PlayerRoles;
using PluginAPI.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Policy;

namespace BetterSL
{
    public class Config
    {
        [Description("Enable the plugin?")]
        public bool PluginEnabled { get; set; } = true;

        [Description("Should dummy players be enabled?")]
        public bool DummiesEnabled { get; set; } = false;

        [Description("How much percent of all chaos must die in order for the round to end?")]
        public float ChaosDeadPercent { get; set; } = 0.5f;

        [Description("Broadcast to all SCPs regarding chaos being targets")]
        public string ChaosTargetsBroadcast { get; set; } = $"You must kill alive chaos to end the round!";

        [Description("Should the MTF team have classes reassigned with subclasses?")]
        public bool MTFAssignSubclasses { get; set; } = true;

        [Description("Minimum and maximum in between which guards will spawn in LCZ, value is chosen at random at round start")]
        public int GuardSpawnMinTime { get; set; } = 140;
        public int GuardSpawnMaxTime { get; set; } = 170;

        [Description("Maximum amount of guards to be spawned once they do spawn. Note that if there are no enough spectators, the game will only spawn the amount it can.")]
        public uint MaxGuardSpawns { get; set; } = 5;

        [Description("list of rooms where guards can spawn")]
        public List<RoomName> GuardSpawnableRooms { get; set; } =  new List<RoomName>() 
        {
            RoomName.Lcz173,
            RoomName.LczGlassroom,
            RoomName.LczComputerRoom
        };

        [Description("Should corpses of guards spawn in HCZ and EZ? (Custom names and death reasons in other config files)")]
        public bool ShouldGuardCorpsesSpawn { get; set; } = true;

        [Description("Amount of corpses present in the facility maximum")]
        public int GuardCorpseAmount { get; set; } = 5;

        [Description("List of allowed rooms where guard corpses can spawn. Note that duplicate entries mean multiple corpses can spawn.")]
        public List<RoomName> GuardCorpseSpawnableRooms { get; set; } = new List<RoomName>() 
        {
            RoomName.HczServers,
            RoomName.HczTestroom,
            RoomName.Hcz939,
            RoomName.HczWarhead,
            RoomName.HczMicroHID,
            RoomName.HczCheckpointA,
            RoomName.HczCheckpointB,
            RoomName.EzEvacShelter,
            RoomName.EzOfficeSmall,
            RoomName.EzOfficeLarge,
            RoomName.EzOfficeStoried,
            RoomName.EzOfficeStoried,
            RoomName.EzCollapsedTunnel,
        };

        [Description("What should spawn on guards in HCZ? Note that the amount of things spawned works just as expected unless the item is a gun, at which point the amount counts as ammo.")]
        public Dictionary<ItemType, int> HczGuardCorpseContents { get; set; } = new Dictionary<ItemType, int>() 
        {
            [ItemType.KeycardGuard] = 1,
            [ItemType.GunCOM18] = 0,
            [ItemType.Ammo9x19] = 30,
            [ItemType.ArmorLight] = 1
        };

        [Description("What should spawn on guards in EZ? Note that the amount of things spawned works just as expected unless the item is a gun, at which point the amount counts as ammo.")]
        public Dictionary<ItemType, int> EzGuardCorpseContents { get; set; } = new Dictionary<ItemType, int>()
        {
            [ItemType.KeycardGuard] = 1,
            [ItemType.GunFSP9] = 0,
            [ItemType.Ammo9x19] = 40,
            [ItemType.ArmorLight] = 1
        };



        [Description("Should the crossvecs in LCZ armory be replaced with FSP9s?")]
        public bool LczArmoryReplaceCrossvecWithFSP9 { get; set; } = true;

        [Description("Should all Armor in LCZ armory be downgraded one tier?")]
        public bool LczArmoryDowngradeArmor { get; set; } = false;

        [Description("Should HCZ armory spawn with combat armor?")]
        public bool HczArmorySpawnCombatArmor { get; set; } = true;

        [Description("How much combat armor to spawn in HCZ Armory?")]
        public int HczArmoryCombatArmorAmount { get; set; } = 1;

        [Description("Should SCP 500 give you back all your stamina on use?")]
        public bool Scp500GivesStamina { get; set; } = true;

        [Description("Should a facility manager keycard spawn in 049's room?")]
        public bool FacilityManagerReplacesGuardCardIn049 { get; set; } = true;

        [Description("Should the 049 glass be impossible to break?")]
        public bool Hcz049GlassCantBeBroken { get; set; } = true;

        [Description("Should the 049 gate be locked until generators are actived? (Note, make sure if this is enabled, 049 will be teleported as they will be trapped in the room!)")]
        public bool Lock049GateUntilGeneratorsAreActive { get; set; } = true;

        [Description("How many generators must be enabled in order for the HCZ 049 gate to unlock?")]
        public int Unlock049GateGeneratorEngagedAmount { get; set; } = 2;

        [Description("Should the HCZ 049 Armory need a keycard? (the button will still show the text but no keycard is actually needed)")]
        public bool Hcz049ArmoryDoesNotNeedKeycard { get; set; } = true;

        [Description("Should SCP 2176 Affect elevators?")]
        public bool Scp2176AffectsElevators { get; set; } = true;

        [Description("How much damage should 096's base attack do?")]
        public float Scp096SwingDamage { get; set; } = 70f;

        [Description("Amount of time before a body auto drops from PD")]
        public int Scp106BodyDropDelay { get; set; } = 45;

        [Description("SCP 049's damage resistence? (default is 20 in vanilla)")]
        public int Scp049Armour { get; set; } = 35;

        [Description("How many times can SCP 049 revive a instance of SCP 049-2?")]
        public int Scp049Max0492Ressurection { get; set; } = 3;

        [Description("How fast should SCP 049 'sprint' at when using the sense ability?")]
        public float Scp049SenseAbilitySpeed { get; set; } = 5.8f;

        [Description("Should SCP 049 be teleported to top of his containment (past elevator) when the round starts?")]
        public bool Scp049TeleportToTopOfRoom { get; set; } = true;

        [Description("Max speed of SCP-049-2's Lobotomized Bloodlust ability")]
        public float Scp0492BloodlustMaxSpeed { get; set; } = 5.4f;

        [Description("What message should be sent to all SCPs when a generator is turned on and SCP 079 pings it?")]
        public string Scp079GeneratorAlertMessage { get; set; } = "<color=purple>SCP 079</color> has pinged a generator in {room}";

        [Description("What range should the SCP 079 Ping for a generator have?")]
        public float Scp079GeneratorPingRange { get; set; } = 25f;

        [Description("Termination reward multipliers for SCP 079.")]
        public Dictionary<Team, float> Scp079TerminationRewardMultipliers { get; set; } = new Dictionary<Team, float>{ 
            [Team.ClassD] = 1f,
            [Team.Scientists] = 1f,
            [Team.FoundationForces] = 0.7f,
            [Team.ChaosInsurgency] = 0.7f,
            [Team.OtherAlive] = 1f,
            [Team.SCPs] = 1f,
            [Team.Dead] = 1f
        };

        [Description("How much AP should 079 spawn with?")]
        public int Scp079SpawnAp { get; set; } = 50;

        [Description("SCP 079 regen per tier, goes from tier 0 to 5.")]
        public float[] Scp079RegenRate { get; set; } = { 1.9f, 2.8f, 4.1f, 5.6f, 7.1f };

        [Description("How much should a SCP 079 standard room blackout cost?")]
        public int Scp079BlackoutCost { get; set; } = 25;

        [Description("SCP 079 blackout cost multipliers per zone.")]
        public Dictionary<FacilityZone, float> Scp079BlackoutCostsPerZoneMultiplier { get; set; } = new Dictionary<FacilityZone, float>
        {
            [FacilityZone.None] = 1f,
            [FacilityZone.LightContainment] = 1f,
            [FacilityZone.HeavyContainment] = 1f,
            [FacilityZone.Entrance] = 1f,
            [FacilityZone.Surface] = 2f
        };

        [Description("How long should the cooldown be between blackouts for SCP 079? (Between blackouts means this amount of time counted after the blackout ends)")]
        public float Scp079SurfaceZoneBlackoutCooldown { get; set; } = 30f;

        [Description("How long should SCP 079's lockdown last?")]
        public float Scp079LockdownLength { get; set; } = 10f;

        [Description("How much should lockdown cost to use? (Precent of total AP)")]
        public float Scp079LockdownCostPercent { get; set; } = 0.4f;

        [Description("SCP 079's AP regen rate when lockdown is used. 0 will result in no regen.")]
        public float[] Scp079LockdownRegenMultiplier { get; set; } = { 0, 0, 0, 0.25f, 0.25f };

        [Description("Limit of players 939 can damage with one claw swipe")]
        public int Scp939MaxAoePlayerHits { get; set; } = 2;

        [Description("SCP 939's base claw cooldown (vanilla is 0.6)")]
        public float Scp939ClawCooldown { get; set; } = 0.7f;

        [Description("Should SCP 939 be able to deploy cloud in elevators?")]
        public bool Scp939CanUseCloudInElevator { get; set; } = false;

        [Description("Message to show to 939 players who try to use gas in elevators")]
        public string Scp939GasInElevatorMessage { get; set; } = "You can't deploy amnestic cloud in elevators!";

        [Description("Player join broadcast text")]
        public string PlayerJoinBroadcastText { get; set; } = "Welcome to the \'rebalanced\' SL. All content is still in early development, things may change.";

        [Description("Player join broadcast length (ushort)")]
        public ushort PlayerJoinBroadcastDuration { get; set; } = 20;

        [Description("What should the broadcast regarding being a subclass say? use {subclass} to refer to the displayname, {description} to refer to the class description, and {filename} for the class file name on server disk.")]
        public string PlayerSubclassNotificationText { get; set; } = "You are {subclass}! \n {description}";

        [Description("How long should the subclass notification last for?")]
        public ushort PlayerSubclassNotificationDuration { get; set; } = 5;
    }
}