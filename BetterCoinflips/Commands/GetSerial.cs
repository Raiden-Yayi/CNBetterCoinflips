using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Permissions.Extensions;

namespace BetterCoinflips.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class GetSerial : ICommand, IUsageProvider
    {
        public string Command { get; } = "getserial";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "获取物品的序列号。";
        public string[] Usage { get; } = { "id/name (可选)" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("bc.getserial"))
            {
                response = "您没有使用此命令的权限";
                return false;
            }
            if (arguments.Count == 0)
            {
                Item item = Player.Get(sender).CurrentItem;
                if (item == null)
                {
                    response = "您没有持有任何物品。";
                    return false;
                }

                response = $"物品: {item.Base.name}, 序列号: {item.Serial}";
                return true;
            }

            if (arguments.Count == 1)
            {
                Player player = Player.Get(arguments.ElementAt(0));
                if (player == null)
                {
                    response = $"未找到玩家 {arguments.ElementAt(0)}。";
                    return false;
                }

                Item item = player.CurrentItem;
                if (item == null)
                {
                    response = "指定的玩家没有持有任何物品。";
                    return false;
                }

                response = $"物品: {item.Base.name}, 序列号: {item.Serial}";
                return true;
            }

            response = "使用方法不正确。";
            return false;
        }
    }
}
