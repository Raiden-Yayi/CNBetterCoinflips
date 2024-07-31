using System;
using System.Linq;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace BetterCoinflips.Commands.CoinUses.Get
{
    public class Serial : ICommand
    {
        public string Command { get; } = "serial";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "获取指定序列号的硬币使用次数。";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("bc.coinuses.get"))
            {
                response = "您没有权限使用此命令";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "用法: coinuses get serial [serial]";
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
                response = $"找不到序列号为 {serial} 的已注册硬币。";
                return false;
            }

            response = $"序列号为 {serial} 的硬币还有 {EventHandlers.CoinUses[serial]} 次使用次数。";
            return true;
        }
    }
}
