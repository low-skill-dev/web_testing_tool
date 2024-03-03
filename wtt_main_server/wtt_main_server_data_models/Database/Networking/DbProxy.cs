using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Database.Abstract;
using wtt_main_server_data.Enums;

namespace wtt_main_server_data.Database.Networking;

#pragma warning disable CS8618


/* Данные прокси добавлены не юзером, а принадлежат
 * самому сервису, однако возможно в будущем будет
 * добавлена возможность инстансировать прокси, и,
 * уж точно, это можно сделать сделать для IMAP
 * аккаунтов, которые хоть и принадлежат сервису, но
 * использоваться могут только конкретным юзером.
 */
public class DbProxy : DbUserProxy
{
	public SubscriptionTypes SubscriptionRequired { get; set; }
}
