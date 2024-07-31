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
        public string Description { get; } = "��ȡ��Ʒ�����кš�";
        public string[] Usage { get; } = { "id/name (��ѡ)" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("bc.getserial"))
            {
                response = "��û��ʹ�ô������Ȩ��";
                return false;
            }
            if (arguments.Count == 0)
            {
                Item item = Player.Get(sender).CurrentItem;
                if (item == null)
                {
                    response = "��û�г����κ���Ʒ��";
                    return false;
                }

                response = $"��Ʒ: {item.Base.name}, ���к�: {item.Serial}";
                return true;
            }

            if (arguments.Count == 1)
            {
                Player player = Player.Get(arguments.ElementAt(0));
                if (player == null)
                {
                    response = $"δ�ҵ���� {arguments.ElementAt(0)}��";
                    return false;
                }

                Item item = player.CurrentItem;
                if (item == null)
                {
                    response = "ָ�������û�г����κ���Ʒ��";
                    return false;
                }

                response = $"��Ʒ: {item.Base.name}, ���к�: {item.Serial}";
                return true;
            }

            response = "ʹ�÷�������ȷ��";
            return false;
        }
    }
}
