using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using PlayerRoles;

namespace BetterCoinflips.Configs
{
    public class Config : IConfig
    {
        [Description("插件是否应启用。默认：true")]
        public bool IsEnabled { get; set; } = true;

        [Description("是否应显示调试日志。默认：false")]
        public bool Debug { get; set; } = false;

        [Description("应移除的基础游戏生成的硬币数量。默认：4")]
        public int DefaultCoinsAmount { get; set; } = 4;

        [Description("要替换为硬币的物品的ItemType及其替换数量，该物品应为SCP底座中找到的物品。")]
        public Dictionary<ItemType, int> ItemToReplace { get; set; } = new()
        {
            { ItemType.SCP500, 2 }
        };

        [Description("每个硬币在破裂前抛掷次数的随机范围边界。上限是独占的。")]
        public List<int> MinMaxDefaultCoins { get; set; } = new()
        {
            1,
            4
        };

        [Description("硬币抛掷之间的时间（秒）。")]
        public double CoinCooldown { get; set; } = 5;

        [Description("通知您有关'奖励'的广播持续时间。默认：3")]
        public ushort BroadcastTime { get; set; } = 3;

        [Description("提示您是正面还是反面的持续时间。设置为0或更少以禁用。")]
        public float HintDuration { get; set; } = 3;

        [Description("地图黑屏持续时间。默认：10")]
        public float MapBlackoutTime { get; set; } = 10;

        [Description("掉在您头上的手榴弹的引信时间。默认：3.25")]
        public double LiveGrenadeFuseTime { get; set; } = 3.25;

        [Description("可以应用于玩家的负面效果列表。列表可在以下网址获取：https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.EffectType.html")]
        public HashSet<EffectType> BadEffects { get; set; } = new()
        {
            EffectType.Asphyxiated,
            EffectType.Bleeding,
            EffectType.Blinded,
            EffectType.Burned,
            EffectType.Concussed,
            EffectType.Corroding,
            EffectType.CardiacArrest,
            EffectType.Deafened,
            EffectType.Decontaminating,
            EffectType.Disabled,
            EffectType.Ensnared,
            EffectType.Exhausted,
            EffectType.Flashed,
            EffectType.Hemorrhage,
            EffectType.Hypothermia,
            EffectType.InsufficientLighting,
            EffectType.Poisoned,
            EffectType.PocketCorroding,
            EffectType.SeveredHands,
            EffectType.SinkHole,
            EffectType.Stained,
            EffectType.Traumatized
        };

        [Description("可以应用于玩家的正面效果列表。列表可在以下网址获取：https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.EffectType.html")]
        public HashSet<EffectType> GoodEffects { get; set; } = new()
        {
            EffectType.BodyshotReduction,
            EffectType.DamageReduction,
            EffectType.Invigorated,
            EffectType.Invisible,
            EffectType.MovementBoost,
            EffectType.RainbowTaste,
            EffectType.Scp1853,
            EffectType.Scp207,
            EffectType.Vitality
        };

        [Description("收到设施经理钥匙卡而不是收容工程师钥匙卡的百分比几率。")]
        public int RedCardChance { get; set; } = 15;

        [Description("PlayerSwap效果（#17）忽略的角色列表")]
        public HashSet<RoleTypeId> PlayerSwapIgnoredRoles { get; set; } = new()
        {
            RoleTypeId.Spectator,
            RoleTypeId.Filmmaker,
            RoleTypeId.Overwatch,
            RoleTypeId.Scp079,
            RoleTypeId.Tutorial,
        };

        [Description("InventorySwap效果（#17）忽略的角色列表")]
        public HashSet<RoleTypeId> InventorySwapIgnoredRoles { get; set; } = new()
        {
            RoleTypeId.Spectator,
            RoleTypeId.Filmmaker,
            RoleTypeId.Overwatch,
            RoleTypeId.Scp079,
            RoleTypeId.Tutorial,
            RoleTypeId.Scp049,
            RoleTypeId.Scp079,
            RoleTypeId.Scp096,
            RoleTypeId.Scp106,
            RoleTypeId.Scp173,
            RoleTypeId.Scp0492,
            RoleTypeId.Scp939,
            RoleTypeId.Scp3114,
        };

        public HashSet<ItemType> ItemsToGive { get; set; } = new()
        {
            ItemType.Adrenaline,
            ItemType.Coin,
            ItemType.Flashlight,
            ItemType.Jailbird,
            ItemType.Medkit,
            ItemType.Painkillers,
            ItemType.Radio,
            ItemType.ArmorCombat,
            ItemType.ArmorHeavy,
            ItemType.ArmorLight,
            ItemType.GrenadeFlash,
            ItemType.GrenadeHE,
            ItemType.GunA7,
            ItemType.GunCom45,
            ItemType.GunCrossvec,
            ItemType.GunLogicer,
            ItemType.GunRevolver,
            ItemType.GunShotgun,
            ItemType.GunAK,
            ItemType.GunCOM15,
            ItemType.GunCOM18,
            ItemType.GunE11SR,
            ItemType.GunFSP9,
            ItemType.GunFRMG0,
        };

        public HashSet<RoomType> RoomsToTeleport { get; set; } = new()
        {
            RoomType.EzCafeteria,
            RoomType.EzCheckpointHallway,
            RoomType.EzCollapsedTunnel,
            RoomType.EzConference,
            RoomType.EzCrossing,
            RoomType.EzCurve,
            RoomType.EzDownstairsPcs,
            RoomType.EzGateA,
            RoomType.EzGateB,
            RoomType.EzIntercom,
            RoomType.EzPcs,
            RoomType.EzStraight,
            RoomType.EzTCross,
            RoomType.EzUpstairsPcs,
            RoomType.EzVent,
            RoomType.Hcz049,
            RoomType.Hcz079,
            RoomType.Hcz096,
            RoomType.Hcz106,
            RoomType.Hcz939,
            RoomType.HczArmory,
            RoomType.HczCrossing,
            RoomType.HczCurve,
            RoomType.HczElevatorA,
            RoomType.HczElevatorB,
            RoomType.HczEzCheckpointA,
            RoomType.HczEzCheckpointB,
            RoomType.HczHid,
            RoomType.HczNuke,
            RoomType.HczServers,
            RoomType.HczStraight,
            RoomType.HczTCross,
            RoomType.HczTesla,
            RoomType.HczTestRoom,
            RoomType.Lcz173,
            RoomType.Lcz330,
            RoomType.Lcz914,
            RoomType.LczAirlock,
            RoomType.LczArmory,
            RoomType.LczCafe,
            RoomType.LczCheckpointA,
            RoomType.LczCheckpointB,
            RoomType.LczClassDSpawn,
            RoomType.LczCrossing,
            RoomType.LczCurve,
            RoomType.LczGlassBox,
            RoomType.LczPlants,
            RoomType.LczStraight,
            RoomType.LczTCross,
            RoomType.LczToilets,
            RoomType.Surface,
        };

        [Description("这些正面效果发生的几率。这是一个比例几率，不是百分比几率。")]
        public int KeycardChance { get; set; } = 20;
        public int MedicalKitChance { get; set; } = 35;
        public int TpToEscapeChance { get; set; } = 5;
        public int HealChance { get; set; } = 10;
        public int MoreHpChance { get; set; } = 10;
        public int HatChance { get; set; } = 10;
        public int RandomGoodEffectChance { get; set; } = 30;
        public int OneAmmoLogicerChance { get; set; } = 1;
        public int LightbulbChance { get; set; } = 15;
        public int PinkCandyChance { get; set; } = 10;
        public int BadRevoChance { get; set; } = 5;
        public int EmptyHidChance { get; set; } = 5;
        public int ForceRespawnChance { get; set; } = 15;
        public int SizeChangeChance { get; set; } = 20;
        public int RandomItemChance { get; set; } = 35;

        [Description("这些负面效果发生的几率。这是一个比例几率，不是百分比几率。")]
        public int HpReductionChance { get; set; } = 20;
        public int TpToClassDCellsChance { get; set; } = 5;
        public int RandomBadEffectChance { get; set; } = 20;
        public int WarheadChance { get; set; } = 10;
        public int LightsOutChance { get; set; } = 20;
        public int LiveHeChance { get; set; } = 30;
        public int TrollFlashChance { get; set; } = 50;
        public int ScpTpChance { get; set; } = 20;
        public int OneHpLeftChance { get; set; } = 15;
        public int PrimedVaseChance { get; set; } = 20;
        public int ShitPantsChance { get; set; } = 40;
        public int FakeCassieChance { get; set; } = 50;
        public int TurnIntoScpChance { get; set; } = 30;
        public int InventoryResetChance { get; set; } = 20;
        public int ClassSwapChance { get; set; } = 10;
        public int InstantExplosionChance { get; set; } = 10;
        public int PlayerSwapChance { get; set; } = 20;
        public int SpectSwapChance { get; set; } = 10;
        public int TeslaTpChance { get; set; } = 15;
        public int InventorySwapChance { get; set; } = 20;
        public int HandcuffChance { get; set; } = 10;
        public int RandomTeleportChance { get; set; } = 15;
    }
}

