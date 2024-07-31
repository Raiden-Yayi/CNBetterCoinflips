using System;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace BetterCoinflips.Commands.CoinUses.Get
{
    public class Get : ParentCommand
    {
        public Get() => LoadGeneratedCommands();

        public override string Command { get; } = "get";
        public override string[] Aliases { get; } = { };
        public override string Description { get; } = "获取指定玩家或序列号持有的硬币的使用次数。";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Player());
            RegisterCommand(new Serial());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("bc.coinuses.get"))
            {
                response = "您没有使用此命令的权限";
                return false;
            }

            response = "无效的子命令。可用的子命令：player, serial";
            return false;
        }
    }
}