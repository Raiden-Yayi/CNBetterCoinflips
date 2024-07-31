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
        public string Description { get; } = "��ȡָ�����кŵ�Ӳ��ʹ�ô�����";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("bc.coinuses.get"))
            {
                response = "��û��Ȩ��ʹ�ô�����";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "�÷�: coinuses get serial [serial]";
                return false;
            }
            bool flag1 = ushort.TryParse(arguments.ElementAt(0), out ushort serial);
            if (!flag1)
            {
                response = $"�޷��� {arguments.ElementAt(0)} ����Ϊ���кš�";
                return false;
            }

            if (!EventHandlers.CoinUses.ContainsKey(serial))
            {
                response = $"�Ҳ������к�Ϊ {serial} ����ע��Ӳ�ҡ�";
                return false;
            }

            response = $"���к�Ϊ {serial} ��Ӳ�һ��� {EventHandlers.CoinUses[serial]} ��ʹ�ô�����";
            return true;
        }
    }
}
