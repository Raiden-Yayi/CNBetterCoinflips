using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Permissions.Extensions;

namespace BetterCoinflips.Commands.CoinUses.Set
{
    public class Player : ICommand
    {
        public string Command { get; } = "player";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "设置指定玩家持有的硬币的使用次数。";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(sender);
            if (!sender.CheckPermission("bc.coinuses.set"))
            {
                response = "您没有使用此命令的权限";
                return false;
            }

            if (arguments.Count == 1)
            {
                Item coin = GetCoinByPlayer(player);
                if (coin == null)
                {
                    response = "您没有持有硬币。";
                    return false;
                }

                bool flag1 = int.TryParse(arguments.ElementAt(0), out int amount);

                if (!flag1)
                {
                    response = $"无法将 {arguments.ElementAt(0)} 解析为数量。";
                    return false;
                }

                EventHandlers.CoinUses[coin.Serial] = amount;
                response = $"成功将硬币的使用次数设置为 {amount}。";
                return true;
            }

            if (arguments.Count == 2)
            {
                Exiled.API.Features.Player target = Exiled.API.Features.Player.Get(arguments.ElementAt(0));
                if (target == null)
                {
                    response = $"无法将 {arguments.ElementAt(0)} 解析为有效的目标。";
                    return false;
                }

                Item coin = GetCoinByPlayer(target);
                if (coin == null)
                {
                    response = $"{target.Nickname} 没有持有硬币。";
                    return false;
                }

                bool flag1 = int.TryParse(arguments.ElementAt(1), out int amount);

                if (!flag1)
                {
                    response = $"无法将 {arguments.ElementAt(1)} 解析为数量。";
                    return false;
                }

                EventHandlers.CoinUses[coin.Serial] = amount;
                string message = player.DoNotTrack ? $"{player.Nickname}({player.RawUserId})" : $"{player.Nickname}";
                Log.Debug($"{message} 刚刚将硬币 # {coin.Serial} 的使用次数设置为 {amount}。");
                response = $"成功将硬币的使用次数设置为 {amount}。";
                return true;
            }

            response = "\n用法: coinuses set player [id/name] [amount]\n或者: coinuses set player [amount]";
            return false;
        }

        private Item GetCoinByPlayer(Exiled.API.Features.Player pl)
        {
            return pl.CurrentItem is { Type: ItemType.Coin } ? pl.CurrentItem : null;
        }
    }
}
