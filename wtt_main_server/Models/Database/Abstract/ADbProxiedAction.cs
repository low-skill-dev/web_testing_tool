using Reinforced.Typings.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Database.Abstract;

//[TsClass(IncludeNamespace = false, Order = 200)]
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
	public string? ProxyUrl { get; set; }
	public string? ProxyUsername { get; set; }
	public string? ProxyPassword { get; set; }
}
