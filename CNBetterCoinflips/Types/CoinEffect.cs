using System;
using System.Collections.Generic;
using System.Linq;
using BetterCoinflips.Configs;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using InventorySystem.Items.Firearms.Attachments;
using MEC;
using PlayerRoles;
using Respawning;
using UnityEngine;
using Player = Exiled.API.Features.Player;

namespace BetterCoinflips.Types
{
    public class CoinFlipEffect
    {
        private static Config Config => Plugin.Instance.Config;
        private static Configs.Translations Translations => Plugin.Instance.Translation;
        private static readonly System.Random Rd = new();

        public Action<Player> Execute { get; set; }
        public string Message { get; set; }

        public CoinFlipEffect(string message, Action<Player> execute)
        {
            Execute = execute;
            Message = message;
        }

        private static readonly Dictionary<string, string> _scpNames = new()
        {
            { "1 7 3", "SCP-173"},
            { "9 3 9", "SCP-939"},
            { "0 9 6", "SCP-096"},
            { "0 7 9", "SCP-079"},
            { "0 4 9", "SCP-049"},
            { "1 0 6", "SCP-106"}
        };

        private static bool flag1 = Config.RedCardChance > Rd.Next(1, 101);

        // 好效果列表
        public static List<CoinFlipEffect> GoodEffects = new()
        {
            // 0: 生成一张红色钥匙卡或 containment engineer 钥匙卡
            new CoinFlipEffect(flag1 ? Translations.RedCardMessage : Translations.ContainmentEngineerCardMessage, player =>
            {
                Pickup.CreateAndSpawn(flag1 ? ItemType.KeycardFacilityManager : ItemType.KeycardContainmentEngineer, player.Position, new Quaternion());
            }),

            // 1: 为玩家生成一个医疗包和止痛药
            new CoinFlipEffect(Translations.MediKitMessage, player =>
            {
                Pickup.CreateAndSpawn(ItemType.Medkit, player.Position, new Quaternion());
                Pickup.CreateAndSpawn(ItemType.Painkillers, player.Position, new Quaternion());
            }),

            // 2: 将玩家传送到逃生次要门
            new CoinFlipEffect(Translations.TpToEscapeMessage, player =>
            {
                player.Teleport(Door.Get(DoorType.EscapeSecondary));
            }),

            // 3: 为玩家治疗 25 点生命值
            new CoinFlipEffect(Translations.MagicHealMessage, player =>
            {
                player.Heal(25);
            }),

            // 4: 增加玩家 10% 的生命值
            new CoinFlipEffect(Translations.HealthIncreaseMessage, player =>
            {
                player.Health *= 1.1f;
            }),

            // 5: 为玩家生成 SCP-268（Neat Hat）
            new CoinFlipEffect(Translations.NeatHatMessage, player =>
            {
                Pickup.CreateAndSpawn(ItemType.SCP268, player.Position, new Quaternion());
            }),

            // 6: 为玩家应用一个随机的好效果
            new CoinFlipEffect(Translations.RandomGoodEffectMessage, player =>
            {
                var effect = Config.GoodEffects.ToList().RandomItem();
                player.EnableEffect(effect, 5, true);
                Log.Debug($"选择的随机效果: {effect}");
            }),

            // 7: 为玩家生成一把带有一发子弹的 Logicer
            new CoinFlipEffect(Translations.OneAmmoLogicerMessage, player =>
            {
                Firearm gun = (Firearm)Item.Create(ItemType.GunLogicer);
                gun.Ammo = 1;
                gun.CreatePickup(player.Position);
            }),

            // 8: 为玩家生成 SCP-2176（灯泡）
            new CoinFlipEffect(Translations.LightbulbMessage, player =>
            {
                Pickup.CreateAndSpawn(ItemType.SCP2176, player.Position, new Quaternion());
            }),

            // 9: 为玩家生成粉色糖果（SCP-330）
            new CoinFlipEffect(Translations.PinkCandyMessage, player =>
            {
                Scp330 candy = (Scp330)Item.Create(ItemType.SCP330);
                candy.AddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);
                candy.CreatePickup(player.Position);
            }),

            // 10: 为玩家生成一把带有附件的定制左轮手枪
            new CoinFlipEffect(Translations.BadRevoMessage, player =>
            {
                Firearm revo = (Firearm)Item.Create(ItemType.GunRevolver);
                revo.AddAttachment(new[]
                    {AttachmentName.CylinderMag8, AttachmentName.ShortBarrel, AttachmentName.ScopeSight});
                revo.CreatePickup(player.Position);
            }),

            // 11: 为玩家生成一个没有能量的 MicroHID
            new CoinFlipEffect(Translations.EmptyHidMessage, player =>
            {
                MicroHIDPickup item = (MicroHIDPickup)Pickup.Create(ItemType.MicroHID);
                item.Position = player.Position;
                item.Spawn();
                item.Energy = 0;
            }),

            // 13: 改变玩家的大小
            new CoinFlipEffect(Translations.SizeChangeMessage, player =>
            {
                player.Scale = new Vector3(1.13f, 0.5f, 1.13f);
            }),

            // 14: 为玩家生成一个随机物品
            new CoinFlipEffect(Translations.RandomItemMessage, player =>
            {
                Item.Create(Config.ItemsToGive.ToList().RandomItem()).CreatePickup(player.Position);
            }),
        };


        // 坏效果列表
        public static List<CoinFlipEffect> BadEffects = new()
        {
            // 0: 减少玩家 30% 的生命值
            new CoinFlipEffect(Translations.HpReductionMessage, player =>
            {
                if ((int) player.Health == 1)
                    player.Kill(DamageType.CardiacArrest);
                else
                    player.Health *= 0.7f;
            }),

            // 1: 将玩家传送到 D 级牢房
            new CoinFlipEffect(Warhead.IsDetonated ? Translations.TpToClassDCellsAfterWarheadMessage : Translations.TpToClassDCellsMessage, player =>
            {
                player.DropHeldItem();
                player.Teleport(Door.Get(DoorType.PrisonDoor));

                if (Warhead.IsDetonated)
                {
                    player.Kill(DamageType.Decontamination);
                }
            }),

            // 2: 为玩家应用一个随机的坏效果
            new CoinFlipEffect(Translations.RandomBadEffectMessage, player =>
            {
                var effect = Config.BadEffects.ToList().RandomItem();
                
                // 防止玩家无限留在 PD 中
                if (effect == EffectType.PocketCorroding)
                    player.EnableEffect(EffectType.PocketCorroding);
                else
                    player.EnableEffect(effect, 5, true);

                Log.Debug($"选择的随机效果: {effect}");
            }),

            // 4: 关闭所有灯光
            new CoinFlipEffect(Translations.LightsOutMessage, player =>
            {
                Map.TurnOffAllLights(Config.MapBlackoutTime);
            }),

            // 5: 生成一个活的手榴弹
            new CoinFlipEffect(Translations.LiveGrenadeMessage, player =>
            {
                ExplosiveGrenade grenade = (ExplosiveGrenade) Item.Create(ItemType.GrenadeHE);
                grenade.FuseTime = (float) Config.LiveGrenadeFuseTime;
                grenade.SpawnActive(player.Position + Vector3.up, player);
            }),

            // 6: 生成一个短引信时间的闪光弹，将闪光弹的所有者设置为玩家，以便可能致盲其他人
            new CoinFlipEffect(Translations.TrollFlashMessage, player =>
            {
                FlashGrenade flash = (FlashGrenade) Item.Create(ItemType.GrenadeFlash, player);
                flash.FuseTime = 1f;
                flash.SpawnActive(player.Position);
            }),

            // 7: 将玩家传送到一个随机的 SCP 或如果没有 SCP 存在则造成伤害
            new CoinFlipEffect(Player.Get(Side.Scp).Any(x => x.Role.Type != RoleTypeId.Scp079) ? Translations.TpToRandomScpMessage : Translations.SmallDamageMessage, player =>
            {
                if (Player.Get(Side.Scp).Any(x => x.Role.Type != RoleTypeId.Scp079))
                {
                    Player scpPlayer = Player.Get(Side.Scp).Where(x => x.Role.Type != RoleTypeId.Scp079).ToList().RandomItem();
                    player.Position = scpPlayer.Position;
                    return;
                }
                player.Hurt(15);
            }),

            // 8: 将玩家生命值设置为 1 或如果已经是 1 则杀死
            new CoinFlipEffect(Translations.HugeDamageMessage, player =>
            {
                if ((int) player.Health == 1)
                    player.Kill(DamageType.CardiacArrest);
                else
                    player.Health = 1;
            }),

            // 9: 为玩家生成一个已激活的 SCP-244 花瓶
            new CoinFlipEffect(Translations.PrimedVaseMessage, player =>
            {
                Scp244 vase = (Scp244)Item.Create(ItemType.SCP244a);
                vase.Primed = true;
                vase.CreatePickup(player.Position);
            }),

            // 10: 在玩家位置生成一个 tantrum
            new CoinFlipEffect(Translations.ShitPantsMessage, player =>
            {
                player.PlaceTantrum();
            }),
            
            // 13: 重置玩家库存
            new CoinFlipEffect(Translations.InventoryResetMessage, player =>
            {
                player.DropHeldItem();
                player.ClearInventory();
            }),

            // 14: 将玩家角色翻转到相反角色
            new CoinFlipEffect(Translations.ClassSwapMessage, player =>
            {
                player.DropItems();
                switch (player.Role.Type)
                {
                    case RoleTypeId.Scientist:
                        player.Role.Set(RoleTypeId.ClassD, RoleSpawnFlags.AssignInventory);
                        break;
                    case RoleTypeId.ClassD:
                        player.Role.Set(RoleTypeId.Scientist, RoleSpawnFlags.AssignInventory);
                        break;
                    case RoleTypeId.ChaosConscript:
                    case RoleTypeId.ChaosRifleman:
                        player.Role.Set(RoleTypeId.NtfSergeant, RoleSpawnFlags.AssignInventory);
                        break;
                    case RoleTypeId.ChaosMarauder:
                    case RoleTypeId.ChaosRepressor:
                        player.Role.Set(RoleTypeId.NtfCaptain, RoleSpawnFlags.AssignInventory);
                        break;
                    case RoleTypeId.FacilityGuard:
                        player.Role.Set(RoleTypeId.ChaosRifleman, RoleSpawnFlags.AssignInventory);
                        break;
                    case RoleTypeId.NtfPrivate:
                    case RoleTypeId.NtfSergeant:
                    case RoleTypeId.NtfSpecialist:
                        player.Role.Set(RoleTypeId.ChaosRifleman, RoleSpawnFlags.AssignInventory);
                        break;
                    case RoleTypeId.NtfCaptain:
                        List<RoleTypeId> roles = new List<RoleTypeId>
                        {
                            RoleTypeId.ChaosMarauder,
                            RoleTypeId.ChaosRepressor
                        };
                        player.Role.Set(roles.RandomItem(), RoleSpawnFlags.AssignInventory);
                        break;
                }

                // 防止玩家无限留在 PD 中
                if (player.CurrentRoom.Type == RoomType.Pocket)
                {
                    player.EnableEffect(EffectType.PocketCorroding);
                }
            }),

            // 15: 生成一个引信时间非常短的 HE 手榴弹
            new CoinFlipEffect(Translations.InstantExplosionMessage, player =>
            {
                ExplosiveGrenade instaBoom = (ExplosiveGrenade) Item.Create(ItemType.GrenadeHE);
                instaBoom.FuseTime = 0.1f;
                instaBoom.SpawnActive(player.Position, player);
            }),

            // 16: 与另一个随机玩家交换位置
            new CoinFlipEffect(Player.List.Count(x => x.IsAlive && !Config.PlayerSwapIgnoredRoles.Contains(x.Role.Type)) <= 1 ? Translations.PlayerSwapIfOneAliveMessage : Translations.PlayerSwapMessage, player =>
            {
                var playerList = Player.List.Where(x => x.IsAlive && !Config.PlayerSwapIgnoredRoles.Contains(x.Role.Type)).ToList();
                playerList.Remove(player);

                if (playerList.IsEmpty())
                {
                    return;
                }

                var targetPlayer = playerList.RandomItem();
                var pos = targetPlayer.Position;

                targetPlayer.Teleport(player.Position);
                player.Teleport(pos);

                EventHandlers.SendBroadcast(targetPlayer, Translations.PlayerSwapMessage);
            }),

            // 18: 与一个观察者交换
            new CoinFlipEffect(Player.List.Where(x => x.Role.Type == RoleTypeId.Spectator).IsEmpty() ? Translations.SpectSwapNoSpectsMessage : Translations.SpectSwapPlayerMessage, player =>
            {
                var spectList = Player.List.Where(x => x.Role.Type == RoleTypeId.Spectator).ToList();

                if (spectList.IsEmpty())
                {
                    return;
                }
                
                var spect = spectList.RandomItem();
                
                spect.Role.Set(player.Role.Type, RoleSpawnFlags.None);
                spect.Teleport(player);
                spect.Health = player.Health;
                
                List<ItemType> playerItems = player.Items.Select(item => item.Type).ToList();

                foreach (var item in playerItems)
                {
                    spect.AddItem(item);
                }
                
                
                // 将玩家的弹药给观察者，必须在ClearInventory()之前完成，否则弹药会掉在地上
                for (int i = 0; i < player.Ammo.Count; i++)
                {
                    spect.AddAmmo(player.Ammo.ElementAt(i).Key.GetAmmoType(), player.Ammo.ElementAt(i).Value);
                    player.SetAmmo(player.Ammo.ElementAt(i).Key.GetAmmoType(), 0);
                }

                player.ClearInventory();
                player.Role.Set(RoleTypeId.Spectator);

                EventHandlers.SendBroadcast(spect, Translations.SpectSwapSpectMessage);
            }),

            // 19: 如果未引爆核弹，传送到随机的特斯拉门
            new CoinFlipEffect(Warhead.IsDetonated ? Translations.TeslaTpAfterWarheadMessage : Translations.TeslaTpMessage, player =>
            {
                player.DropHeldItem();

                player.Teleport(Exiled.API.Features.TeslaGate.List.ToList().RandomItem());

                if (Warhead.IsDetonated)
                {
                    player.Kill(DamageType.Decontamination);
                }
            }),

            // 20: 与另一个随机玩家交换库存和弹药
            new CoinFlipEffect(Player.List.Where(x => !Config.InventorySwapIgnoredRoles.Contains(x.Role.Type)).Count(x => x.IsAlive) <= 1 ? Translations.InventorySwapOnePlayerMessage : Translations.InventorySwapMessage, player =>
            {
                List<Player> playerList = Player.List.Where(x => x != player && !Config.InventorySwapIgnoredRoles.Contains(x.Role.Type)).ToList();

                if (playerList.Count(x => x.IsAlive) <= 1)
                {
                    player.Hurt(50);
                    return;
                }

                var target = playerList.Where(x => x != player).ToList().RandomItem();

                // 保存物品
                List<ItemType> items1 = player.Items.Select(item => item.Type).ToList();
                List<ItemType> items2 = target.Items.Select(item => item.Type).ToList();

                // 保存并移除弹药
                Dictionary<AmmoType, ushort> ammo1 = new();
                Dictionary<AmmoType, ushort> ammo2 = new();
                for (int i = 0; i < player.Ammo.Count; i++)
                {
                    ammo1.Add(player.Ammo.ElementAt(i).Key.GetAmmoType(), player.Ammo.ElementAt(i).Value);
                    player.SetAmmo(ammo1.ElementAt(i).Key, 0);
                }
                for (int i = 0; i < target.Ammo.Count; i++)
                {
                    ammo2.Add(target.Ammo.ElementAt(i).Key.GetAmmoType(), target.Ammo.ElementAt(i).Value);
                    target.SetAmmo(ammo2.ElementAt(i).Key, 0);
                }

                // 设置物品
                target.ResetInventory(items1);
                player.ResetInventory(items2);

                // 设置弹药
                foreach (var ammo in ammo2)
                {
                    player.SetAmmo(ammo.Key, ammo.Value);
                }
                foreach (var ammo in ammo1)
                {
                    target.SetAmmo(ammo.Key, ammo.Value);
                }

                EventHandlers.SendBroadcast(target, Translations.InventorySwapMessage);
            }),

            // 21: 根据核弹状态生成红色糖果或传送到随机房间
            new CoinFlipEffect(Warhead.IsDetonated ? Translations.RandomTeleportWarheadDetonatedMessage : Translations.RandomTeleportMessage, player =>
            {
                if (Warhead.IsDetonated)
                {
                    Scp330 candy = (Scp330)Item.Create(ItemType.SCP330);
                    candy.AddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Red);
                    candy.CreatePickup(player.Position);
                    return;
                }

                player.Teleport(Room.Get(Config.RoomsToTeleport.GetRandomValue()));
            }),

            // 22: 手铐玩家并丢弃他们的物品
            new CoinFlipEffect(Translations.HandcuffMessage, player =>
            {
                player.Handcuff();
                player.DropItems();
            }),
        };
    }
}

