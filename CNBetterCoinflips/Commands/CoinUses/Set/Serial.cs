using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace BetterCoinflips.Commands.CoinUses.Set
{
    public class Serial : ICommand
    {
        public string Command { get; } = "serial";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "根据序列号设置硬币的使用次数。";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(sender);
            if (!player.CheckPermission("bc.coinuses.set"))
            {
                response = "您没有使用此命令的权限";
                return false;
            }
            
            if (arguments.Count != 2)
            {
                response = "用法: coinuses set serial [serial] [amount]";
                return false;
            }

            bool flag1 = ushort.TryParse(arguments.ElementAt(0), out ushort serial);
            if (!flag1)
            {
                response = $"无法将 {arguments.ElementAt(0)} 解析为序列号。";
                return false;
            }

            if (!EventHandlers.CoinUses.ContainsKey(serial))
            {
                response = $"找不到序列号为 {serial} 的硬币。";
                return false;
            }

            bool flag2 = int.TryParse(arguments.ElementAt(1), out int amount);
            if (!flag2)
            {
                response = $"无法将 {arguments.ElementAt(1)} 解析为数量。";
                return false;
            }

            EventHandlers.CoinUses[serial] = amount;
            string message = player.DoNotTrack ? $"{player.Nickname}({player.RawUserId})" : $"{player.Nickname}";
            Log.Debug($"{message} 刚刚将序列号为 {serial} 的硬币使用次数设置为 {amount}。");
            response = $"成功将硬币的使用次数设置为 {amount}。";
            return true;
        }
    }
}
