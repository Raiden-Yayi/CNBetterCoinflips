using System;
using System.Linq;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace BetterCoinflips.Commands.CoinUses.Get
{
    public class Player : ICommand
    {
        public string Command { get; } = "player";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "获取指定玩家持有的硬币的使用次数。";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("bc.coinuses.get"))
            {
                response = "您没有使用此命令的权限";
                return false;
            }

            if (arguments.Count == 0)
            {
                Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(sender);
                if (player.CurrentItem == null || player.CurrentItem.Type != ItemType.Coin)
                {
                    response = "您现在没有持有硬币。";
                    return false;
                }

                if (!EventHandlers.CoinUses.ContainsKey(player.CurrentItem.Serial))
                {
                    response = $"您持有的硬币尚未注册，因为它还没有被使用过。";
                    return false;
                }

                response = $"您持有的硬币还有 {EventHandlers.CoinUses[player.CurrentItem.Serial]} 次使用次数。";
                return true;
            }

            if (arguments.Count == 1)
            {
                Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(arguments.ElementAt(0));
                if (player == null)
                {
                    response = $"无法将 {arguments.ElementAt(0)} 解析为有效的目标。";
                    return false;
                }
                if (player.CurrentItem == null || player.CurrentItem.Type != ItemType.Coin)
                {
                    response = $"{player.Nickname} 现在没有持有硬币。";
                    return false;
                }

                if (!EventHandlers.CoinUses.ContainsKey(player.CurrentItem.Serial))
                {
                    response = $"{player.Nickname} 持有的硬币尚未注册，因为它还没有被使用过。";
                    return false;
                }

                response = $"{player.Nickname} 持有的硬币还有 {EventHandlers.CoinUses[player.CurrentItem.Serial]} 次使用次数。";
                return true;
            }

            response = "用法: coinuses get player [id/name]";
            return false;
        }
    }
}
