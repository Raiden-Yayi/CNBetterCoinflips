using System;
using System.Collections.Generic;
using Exiled.API.Features;
using System.Linq;
using BetterCoinflips.Configs;
using BetterCoinflips.Types;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using UnityEngine;

namespace BetterCoinflips
{
    public class EventHandlers
    {
        private static Config Config => Plugin.Instance.Config;
        private static Configs.Translations Translations => Plugin.Instance.Translation;
        private readonly System.Random _rd = new();

        // ReSharper 禁用一次 FieldCanBeMadeReadOnly.Global
        public static Dictionary<ushort, int> CoinUses = new();

        // 包含所有正面硬币效果概率的字典，带有索引
        private readonly Dictionary<int, int> _goodEffectChances = new()
        {
            { 0, Config.KeycardChance },
            { 1, Config.MedicalKitChance },
            { 2, Config.TpToEscapeChance },
            { 3, Config.HealChance },
            { 4, Config.MoreHpChance },
            { 5, Config.HatChance },
            { 6, Config.RandomGoodEffectChance },
            { 7, Config.OneAmmoLogicerChance },
            { 8, Config.LightbulbChance },
            { 9, Config.PinkCandyChance },
            { 10, Config.BadRevoChance },
            { 11, Config.EmptyHidChance },
            { 13, Config.SizeChangeChance },
            { 14, Config.RandomItemChance }
        };

        // 包含所有负面硬币效果概率的字典，带有索引
        private readonly Dictionary<int, int> _badEffectChances = new()
        {
            { 0, Config.HpReductionChance },
            { 1, Config.TpToClassDCellsChance },
            { 2, Config.RandomBadEffectChance },
            { 3, Config.WarheadChance },
            { 4, Config.LightsOutChance },
            { 5, Config.LiveHeChance },
            { 6, Config.TrollFlashChance },
            { 7, Config.ScpTpChance },
            { 8, Config.OneHpLeftChance },
            { 9, Config.PrimedVaseChance },
            { 10, Config.ShitPantsChance },
            { 13, Config.InventoryResetChance },
            { 14, Config.ClassSwapChance },
            { 15, Config.InstantExplosionChance },
            { 16, Config.PlayerSwapChance },
            { 18, Config.SpectSwapChance },
            { 19, Config.TeslaTpChance },
            { 20, Config.InventorySwapChance },
            { 21, Config.RandomTeleportChance },
            { 22, Config.HandcuffChance },
        };

        private readonly Dictionary<string, DateTime> _cooldownDict = new();

        // 辅助方法
        public static void SendBroadcast(Player pl, string message, bool showHint = false, bool isTails = false)
        {
            pl.Broadcast(new Exiled.API.Features.Broadcast(message, Config.BroadcastTime), true);

            if (showHint && Config.HintDuration > 0)
            {
                pl.ShowHint(isTails ? Translations.HintMessages.First() : Translations.HintMessages.ElementAt(1), Config.HintDuration);
            }
        }

        // 主要插件逻辑
        public void OnCoinFlip(FlippingCoinEventArgs ev)
        {
            // 广播消息
            string message = "";
            // 用于在用完次数后移除硬币，因为它们在执行效果之前会被检查
            bool helper = false;
            // 检查玩家是否在冷却中
            bool flag = _cooldownDict.ContainsKey(ev.Player.RawUserId)
                        && (DateTime.UtcNow - _cooldownDict[ev.Player.RawUserId]).TotalSeconds < Config.CoinCooldown;
            if (flag)
            {
                ev.IsAllowed = false;
                SendBroadcast(ev.Player, Translations.TossOnCooldownMessage);
                Log.Debug($"{ev.Player.Nickname} tried to throw a coin on cooldown.");
                return;
            }

            // 为玩家设置冷却时间
            _cooldownDict[ev.Player.RawUserId] = DateTime.UtcNow;

            // 检查硬币是否有注册的使用次数
            if (!CoinUses.ContainsKey(ev.Player.CurrentItem.Serial))
            {
                CoinUses.Add(ev.Player.CurrentItem.Serial, _rd.Next(Config.MinMaxDefaultCoins[0], Config.MinMaxDefaultCoins[1]));
                Log.Debug($"Registered a coin, Uses Left: {CoinUses[ev.Player.CurrentItem.Serial]}");
                // 检查新注册的硬币是否有使用次数
                if (CoinUses[ev.Player.CurrentItem.Serial] < 1)
                {
                    // 从使用列表中移除硬币
                    CoinUses.Remove(ev.Player.CurrentItem.Serial);
                    Log.Debug("Removed the coin");
                    if (ev.Player.CurrentItem != null)
                    {
                        ev.Player.RemoveHeldItem();
                    }
                    SendBroadcast(ev.Player, Translations.CoinNoUsesMessage);
                    return;
                }
            }

            // 减少硬币使用次数
            CoinUses[ev.Player.CurrentItem.Serial]--;
            Log.Debug($"Uses Left: {CoinUses[ev.Player.CurrentItem.Serial]}");

            // 检查已注册的使用次数是否已设置为0，以便在执行效果后移除硬币
            if (CoinUses[ev.Player.CurrentItem.Serial] < 1)
            {
                helper = true;
            }

            Log.Debug($"Is tails: {ev.IsTails}");

            if (!ev.IsTails)
            {
                int totalChance = _goodEffectChances.Values.Sum();
                int randomNum = _rd.Next(1, totalChance + 1);
                // 为正面事件设置默认值
                int headsEvent = 2;

                // 魔法循环以确定正面事件，考虑枚举字典中每个项目的概率
                foreach (KeyValuePair<int, int> kvp in _goodEffectChances)
                {
                    if (randomNum <= kvp.Value)
                    {
                        headsEvent = kvp.Key;
                        break;
                    }

                    randomNum -= kvp.Value;
                }

                Log.Debug($"headsEvent = {headsEvent}");

                // 使用正面事件选择效果并执行
                var effect = CoinFlipEffect.GoodEffects[headsEvent];
                effect.Execute(ev.Player);
                message = effect.Message;
            }
            if (ev.IsTails)
            {
                int totalChance = _badEffectChances.Values.Sum();
                int randomNum = _rd.Next(1, totalChance + 1);
                // 为正面事件设置默认值
                int tailsEvent = 13;

                // 魔法循环以确定正面事件，考虑枚举字典中每个项目的概率
                foreach (KeyValuePair<int, int> kvp in _badEffectChances)
                {
                    if (randomNum <= kvp.Value)
                    {
                        tailsEvent = kvp.Key;
                        break;
                    }

                    randomNum -= kvp.Value;
                }

                Log.Debug($"tailsEvent = {tailsEvent}");

                // 使用反面事件选择效果并执行
                var effect = CoinFlipEffect.BadEffects[tailsEvent];
                effect.Execute(ev.Player);
                message = effect.Message;
            }

            // 如果硬币有0次使用次数，移除它
            if (helper)
            {
                if (ev.Player.CurrentItem != null)
                {
                    ev.Player.RemoveHeldItem();
                }
                message += Translations.CoinBreaksMessage;
            }

            if (message != null)
            {
                SendBroadcast(ev.Player, message, true, ev.IsTails);
            }
        }

        // 移除默认硬币
        public void OnSpawningItem(SpawningItemEventArgs ev)
        {
            if (Config.DefaultCoinsAmount != 0 && ev.Pickup.Type == ItemType.Coin)
            {
                Log.Debug($"Removed a coin, coins left to remove {Config.DefaultCoinsAmount}");
                ev.IsAllowed = false;
                Config.DefaultCoinsAmount--;
            }
        }

        // 移除储物柜生成的硬币并在SCP底座中替换选定的物品
        public void OnFillingLocker(FillingLockerEventArgs ev)
        {
            if (ev.Pickup.Type == ItemType.Coin && Config.DefaultCoinsAmount != 0)
            {
                Log.Debug($"Removed a locker coin, coins left to remove {Config.DefaultCoinsAmount}");
                ev.IsAllowed = false;
                Config.DefaultCoinsAmount--;
            }
            else if (ev.Pickup.Type == Config.ItemToReplace.ElementAt(0).Key
                     && Config.ItemToReplace.ElementAt(0).Value != 0)
            {
                Log.Debug($"Placed a coin, coins left to place: {Config.ItemToReplace.ElementAt(0).Value}. Replaced item: {ev.Pickup.Type}");
                ev.IsAllowed = false;
                Pickup.CreateAndSpawn(ItemType.Coin, ev.Pickup.Position, new Quaternion());
                Config.ItemToReplace[Config.ItemToReplace.ElementAt(0).Key]--;
            }
        }
    }
}
