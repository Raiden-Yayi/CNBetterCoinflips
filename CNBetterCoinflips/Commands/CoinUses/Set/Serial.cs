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
        public string Description { get; } = "�������к�����Ӳ�ҵ�ʹ�ô�����";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(sender);
            if (!player.CheckPermission("bc.coinuses.set"))
            {
                response = "��û��ʹ�ô������Ȩ��";
                return false;
            }
            
            if (arguments.Count != 2)
            {
                response = "�÷�: coinuses set serial [serial] [amount]";
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
                response = $"�Ҳ������к�Ϊ {serial} ��Ӳ�ҡ�";
                return false;
            }

            bool flag2 = int.TryParse(arguments.ElementAt(1), out int amount);
            if (!flag2)
            {
                response = $"�޷��� {arguments.ElementAt(1)} ����Ϊ������";
                return false;
            }

            EventHandlers.CoinUses[serial] = amount;
            string message = player.DoNotTrack ? $"{player.Nickname}({player.RawUserId})" : $"{player.Nickname}";
            Log.Debug($"{message} �ոս����к�Ϊ {serial} ��Ӳ��ʹ�ô�������Ϊ {amount}��");
            response = $"�ɹ���Ӳ�ҵ�ʹ�ô�������Ϊ {amount}��";
            return true;
        }
    }
}
