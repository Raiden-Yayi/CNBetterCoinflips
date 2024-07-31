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
        public string Description { get; } = "��ȡָ����ҳ��е�Ӳ�ҵ�ʹ�ô�����";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("bc.coinuses.get"))
            {
                response = "��û��ʹ�ô������Ȩ��";
                return false;
            }

            if (arguments.Count == 0)
            {
                Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(sender);
                if (player.CurrentItem == null || player.CurrentItem.Type != ItemType.Coin)
                {
                    response = "������û�г���Ӳ�ҡ�";
                    return false;
                }

                if (!EventHandlers.CoinUses.ContainsKey(player.CurrentItem.Serial))
                {
                    response = $"�����е�Ӳ����δע�ᣬ��Ϊ����û�б�ʹ�ù���";
                    return false;
                }

                response = $"�����е�Ӳ�һ��� {EventHandlers.CoinUses[player.CurrentItem.Serial]} ��ʹ�ô�����";
                return true;
            }

            if (arguments.Count == 1)
            {
                Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(arguments.ElementAt(0));
                if (player == null)
                {
                    response = $"�޷��� {arguments.ElementAt(0)} ����Ϊ��Ч��Ŀ�ꡣ";
                    return false;
                }
                if (player.CurrentItem == null || player.CurrentItem.Type != ItemType.Coin)
                {
                    response = $"{player.Nickname} ����û�г���Ӳ�ҡ�";
                    return false;
                }

                if (!EventHandlers.CoinUses.ContainsKey(player.CurrentItem.Serial))
                {
                    response = $"{player.Nickname} ���е�Ӳ����δע�ᣬ��Ϊ����û�б�ʹ�ù���";
                    return false;
                }

                response = $"{player.Nickname} ���е�Ӳ�һ��� {EventHandlers.CoinUses[player.CurrentItem.Serial]} ��ʹ�ô�����";
                return true;
            }

            response = "�÷�: coinuses get player [id/name]";
            return false;
        }
    }
}
