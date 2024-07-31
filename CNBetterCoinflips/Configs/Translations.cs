using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace BetterCoinflips.Configs
{
    public class Translations : ITranslation
    {
        [Description("如果硬币损坏，这将被添加到效果消息中。")]
        public string CoinBreaksMessage { get; set; } = "\n此外，你的硬币使用过度，已经损坏了。";

        [Description("当硬币注册时没有使用次数时的广播消息。")]
        public string CoinNoUsesMessage { get; set; } = "你的硬币一开始就没有使用次数！";

        public List<string> HintMessages { get; set; } = new()
        {
            "你的硬币落在了反面。",
            "你的硬币落在了正面。"
        };

        [Description("在这里你可以为这些好的硬币效果设置消息。")]
        public string TossOnCooldownMessage { get; set; } = "你还不能扔硬币。";
        public string RedCardMessage { get; set; } = "你获得了一张设施经理钥匙卡！";
        public string ContainmentEngineerCardMessage { get; set; } = "你获得了一张收容工程师钥匙卡！";
        public string MediKitMessage { get; set; } = "你收到了一个医疗包！";
        public string TpToEscapeMessage { get; set; } = "你现在可以逃脱了！这就是你想要的，对吧？";
        public string MagicHealMessage { get; set; } = "你被魔法治愈了！";
        public string HealthIncreaseMessage { get; set; } = "你获得了10%的额外生命值！";
        public string NeatHatMessage { get; set; } = "你得到了一顶整洁的帽子！";
        public string RandomGoodEffectMessage { get; set; } = "你获得了一个随机效果。";
        public string OneAmmoLogicerMessage { get; set; } = "你得到了一把枪。";
        public string LightbulbMessage { get; set; } = "你得到了一个闪亮的小灯泡！";
        public string PinkCandyMessage { get; set; } = "你得到了一个漂亮的糖果！";
        public string BadRevoMessage { get; set; } = "这是什么鬼东西！？";
        public string EmptyHidMessage { get; set; } = "你刚刚得到了一个微型 HID！？";
        public string SizeChangeMessage { get; set; } = "你变成了小矮人。";
        public string RandomItemMessage { get; set; } = "你得到了一个随机物品！";


        [Description("在这里你可以为这些坏的硬币效果设置消息。")]
        public string HpReductionMessage { get; set; } = "你的生命值减少了30%。";
        public string TpToClassDCellsMessage { get; set; } = "你被传送到了 D 级囚犯的牢房。";
        public string TpToClassDCellsAfterWarheadMessage { get; set; } = "你被传送到了一个放射性区域。";
        public string RandomBadEffectMessage { get; set; } = "你获得了一个随机效果。";
        public string LightsOutMessage { get; set; } = "灯灭了。";
        public string LiveGrenadeMessage { get; set; } = "小心你的头！";
        public string TrollFlashMessage { get; set; } = "你听到了什么吗？";
        public string TpToRandomScpMessage { get; set; } = "你被传送到了一个 SCP 那里。";
        public string SmallDamageMessage { get; set; } = "你失去了15点生命值。";
        public string HugeDamageMessage { get; set; } = "你失去了很多生命值。";
        public string PrimedVaseMessage { get; set; } = "你的奶奶来看你了！";
        public string ShitPantsMessage { get; set; } = "你尿裤子了。";
        public string InventoryResetMessage { get; set; } = "你失去了你的物品。";
        public string ClassSwapMessage { get; set; } = "这就是我所说的 UNO 反转卡！";
        public string InstantExplosionMessage { get; set; } = "你被炸了。";
        public string PlayerSwapMessage { get; set; } = "你的物品被随机玩家交换了。";
        public string PlayerSwapIfOneAliveMessage { get; set; } = "你本应该和某人交换位置，但已经没有其他人活着了！";
        public string KickMessage { get; set; } = "再见！";
        public string SpectSwapPlayerMessage { get; set; } = "你刚刚让某人的回合变得更好了！";
        public string SpectSwapSpectMessage { get; set; } = "你被选为一个随机观众来替换这个玩家！";
        public string SpectSwapNoSpectsMessage { get; set; } = "你很幸运，因为没有观众来取代你的位置。";
        public string TeslaTpMessage { get; set; } = "所以你是电力的粉丝？";
        public string TeslaTpAfterWarheadMessage { get; set; } = "你被传送到了一个放射性区域。";

        [Description("这条消息将广播给双方玩家。")]
        public string InventorySwapMessage { get; set; } = "你的物品被随机玩家交换了。";
        public string InventorySwapOnePlayerMessage { get; set; } = "你不能和任何人交换，所以你失去了生命值。";
        public string RandomTeleportMessage { get; set; } = "你被随机传送了。";
        public string RandomTeleportWarheadDetonatedMessage { get; set; } = "核弹已经引爆，所以你只得到了一个糖果。";
        public string HandcuffMessage { get; set; } = "你因涉嫌犯下战争罪...或其他事情被逮捕了。";
    }
}
