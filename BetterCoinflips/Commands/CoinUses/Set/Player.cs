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
        public string Description { get; } = "����ָ����ҳ��е�Ӳ�ҵ�ʹ�ô�����";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(sender);
            if (!sender.CheckPermission("bc.coinuses.set"))
            {
                response = "��û��ʹ�ô������Ȩ��";
                return false;
            }

            if (arguments.Count == 1)
            {
                Item coin = GetCoinByPlayer(player);
                if (coin == null)
                {
                    response = "��û�г���Ӳ�ҡ�";
                    return false;
                }

                bool flag1 = int.TryParse(arguments.ElementAt(0), out int amount);

                if (!flag1)
                {
                    response = $"�޷��� {arguments.ElementAt(0)} ����Ϊ������";
                    return false;
                }

                EventHandlers.CoinUses[coin.Serial] = amount;
                response = $"�ɹ���Ӳ�ҵ�ʹ�ô�������Ϊ {amount}��";
                return true;
            }

            if (arguments.Count == 2)
            {
                Exiled.API.Features.Player target = Exiled.API.Features.Player.Get(arguments.ElementAt(0));
                if (target == null)
                {
                    response = $"�޷��� {arguments.ElementAt(0)} ����Ϊ��Ч��Ŀ�ꡣ";
                    return false;
                }

                Item coin = GetCoinByPlayer(target);
                if (coin == null)
                {
                    response = $"{target.Nickname} û�г���Ӳ�ҡ�";
                    return false;
                }

                bool flag1 = int.TryParse(arguments.ElementAt(1), out int amount);

                if (!flag1)
                {
                    response = $"�޷��� {arguments.ElementAt(1)} ����Ϊ������";
                    return false;
                }

                EventHandlers.CoinUses[coin.Serial] = amount;
                string message = player.DoNotTrack ? $"{player.Nickname}({player.RawUserId})" : $"{player.Nickname}";
                Log.Debug($"{message} �ոս�Ӳ�� # {coin.Serial} ��ʹ�ô�������Ϊ {amount}��");
                response = $"�ɹ���Ӳ�ҵ�ʹ�ô�������Ϊ {amount}��";
                return true;
            }

            response = "\n�÷�: coinuses set player [id/name] [amount]\n����: coinuses set player [amount]";
            return false;
        }

        private Item GetCoinByPlayer(Exiled.API.Features.Player pl)
        {
            return pl.CurrentItem is { Type: ItemType.Coin } ? pl.CurrentItem : null;
        }
    }
}
