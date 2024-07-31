using System;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace BetterCoinflips.Commands.CoinUses.Set
{
    public class Set : ParentCommand
    {
        public Set() => LoadGeneratedCommands();
        
        public override string Command { get; } = "set";
        public override string[] Aliases { get; } = { };
        public override string Description { get; } = "����ָ����һ����кų��е�Ӳ��ʹ�ô�����";
        
        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Player());
            RegisterCommand(new Serial());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("bc.coinuses.set"))
            {
                response = "��û��ʹ�ô������Ȩ��";
                return false;
            }
            
            response = "��Ч����������õ������player, serial";
            return false;
        }
    }
}
