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
        public override string Description { get; } = "��ȡָ����һ����кų��е�Ӳ�ҵ�ʹ�ô�����";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Player());
            RegisterCommand(new Serial());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("bc.coinuses.get"))
            {
                response = "��û��ʹ�ô������Ȩ��";
                return false;
            }

            response = "��Ч����������õ������player, serial";
            return false;
        }
    }
}