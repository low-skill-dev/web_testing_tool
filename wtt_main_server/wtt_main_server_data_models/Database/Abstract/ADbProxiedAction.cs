using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wtt_main_server_data.Database.Abstract;

public abstract class ADbProxiedAction : ADbAction
{
	/* Идентификатор прокси из базы данных. Вопрос - что делать
	 * в случае удаления прокси. Предполагается делать запрос
	 * на обязательную перенастройку пресета. 
	 * ! Также для избегания проблем требуется валидировать 
	 * ! сценарий на доступность всех инструкментов ПЕРЕД
	 * ! его непосредственно запуском, что не исключает
	 * ! ошибки в ходе выполнения.
	 */
	public Guid? ProxyGuid { get; set; }
}
