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

        // ReSharper ����һ�� FieldCanBeMadeReadOnly.Global
        public static Dictionary<ushort, int> CoinUses = new();

        // ������������Ӳ��Ч�����ʵ��ֵ䣬��������
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

        // �������и���Ӳ��Ч�����ʵ��ֵ䣬��������
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

        // ��������
        public static void SendBroadcast(Player pl, string message, bool showHint = false, bool isTails = false)
        {
            pl.Broadcast(new Exiled.API.Features.Broadcast(message, Config.BroadcastTime), true);

            if (showHint && Config.HintDuration > 0)
            {
                pl.ShowHint(isTails ? Translations.HintMessages.First() : Translations.HintMessages.ElementAt(1), Config.HintDuration);
            }
        }

        // ��Ҫ����߼�
        public void OnCoinFlip(FlippingCoinEventArgs ev)
        {
            // �㲥��Ϣ
            string message = "";
            // ����������������Ƴ�Ӳ�ң���Ϊ������ִ��Ч��֮ǰ�ᱻ���
            bool helper = false;
            // �������Ƿ�����ȴ��
            bool flag = _cooldownDict.ContainsKey(ev.Player.RawUserId)
                        && (DateTime.UtcNow - _cooldownDict[ev.Player.RawUserId]).TotalSeconds < Config.CoinCooldown;
            if (flag)
            {
                ev.IsAllowed = false;
                SendBroadcast(ev.Player, Translations.TossOnCooldownMessage);
                Log.Debug($"{ev.Player.Nickname} tried to throw a coin on cooldown.");
                return;
            }

            // Ϊ���������ȴʱ��
            _cooldownDict[ev.Player.RawUserId] = DateTime.UtcNow;

            // ���Ӳ���Ƿ���ע���ʹ�ô���
            if (!CoinUses.ContainsKey(ev.Player.CurrentItem.Serial))
            {
                CoinUses.Add(ev.Player.CurrentItem.Serial, _rd.Next(Config.MinMaxDefaultCoins[0], Config.MinMaxDefaultCoins[1]));
                Log.Debug($"Registered a coin, Uses Left: {CoinUses[ev.Player.CurrentItem.Serial]}");
                // �����ע���Ӳ���Ƿ���ʹ�ô���
                if (CoinUses[ev.Player.CurrentItem.Serial] < 1)
                {
                    // ��ʹ���б����Ƴ�Ӳ��
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

            // ����Ӳ��ʹ�ô���
            CoinUses[ev.Player.CurrentItem.Serial]--;
            Log.Debug($"Uses Left: {CoinUses[ev.Player.CurrentItem.Serial]}");

            // �����ע���ʹ�ô����Ƿ�������Ϊ0���Ա���ִ��Ч�����Ƴ�Ӳ��
            if (CoinUses[ev.Player.CurrentItem.Serial] < 1)
            {
                helper = true;
            }

            Log.Debug($"Is tails: {ev.IsTails}");

            if (!ev.IsTails)
            {
                int totalChance = _goodEffectChances.Values.Sum();
                int randomNum = _rd.Next(1, totalChance + 1);
                // Ϊ�����¼�����Ĭ��ֵ
                int headsEvent = 2;

                // ħ��ѭ����ȷ�������¼�������ö���ֵ���ÿ����Ŀ�ĸ���
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

                // ʹ�������¼�ѡ��Ч����ִ��
                var effect = CoinFlipEffect.GoodEffects[headsEvent];
                effect.Execute(ev.Player);
                message = effect.Message;
            }
            if (ev.IsTails)
            {
                int totalChance = _badEffectChances.Values.Sum();
                int randomNum = _rd.Next(1, totalChance + 1);
                // Ϊ�����¼�����Ĭ��ֵ
                int tailsEvent = 13;

                // ħ��ѭ����ȷ�������¼�������ö���ֵ���ÿ����Ŀ�ĸ���
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

                // ʹ�÷����¼�ѡ��Ч����ִ��
                var effect = CoinFlipEffect.BadEffects[tailsEvent];
                effect.Execute(ev.Player);
                message = effect.Message;
            }

            // ���Ӳ����0��ʹ�ô������Ƴ���
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

        // �Ƴ�Ĭ��Ӳ��
        public void OnSpawningItem(SpawningItemEventArgs ev)
        {
            if (Config.DefaultCoinsAmount != 0 && ev.Pickup.Type == ItemType.Coin)
            {
                Log.Debug($"Removed a coin, coins left to remove {Config.DefaultCoinsAmount}");
                ev.IsAllowed = false;
                Config.DefaultCoinsAmount--;
            }
        }

        // �Ƴ���������ɵ�Ӳ�Ҳ���SCP�������滻ѡ������Ʒ
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
