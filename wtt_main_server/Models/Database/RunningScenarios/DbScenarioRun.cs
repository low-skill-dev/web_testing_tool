using CommonLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models.Database.RunningScenarios;

#pragma warning disable CS8618

public class DbScenarioRun : ObjectWithGuid
{
	public Guid OriginalScenarioGuid { get; set; }

	public DateTime? Started { get; set; }
	public DateTime? Completed { get; set; }

	public bool? IsSucceeded { get; set; }
	public string? ErrorMessage { get; set; }


	/* Обозначает, сколько сценарий реально находился "в работе",
	 * исключая время простаивания в ожидании тех или иных ответов.
	 * Может подойти для создания особых методов тарификации за
	 * процессорное время, а не за период времени. В целом,
	 * данный вид тарификации выглядит даже наиболее разумным.
	 */
	public TimeSpan? ProcessorTime { get; set; }


	public ScenarioRunReasons? RunReason { get; set; }

	/* Сам по себе сценарий может меняться, писать алгоритм сохранения
	 * сценариев путем не полного сохранения, а сохранения различий - 
	 * нереально долго. Хранить же все версии сценария тоже не
	 * вариант ввиду ограничения по памяти. Пользователю даётся
	 * возможность клонировать сценарии.
	 * 
	 * Данные поля будут удаляться по истечении какого-то времени,
	 * либо когда будет нехватать места для записи новых,
	 * либо по какому-либо другому условию. Соответственно, в них
	 * будет записываться нул.
	 */
	[Column(TypeName = "jsonb")]
	public string? ScenarioJsonSnapshot { get; set; }

	/* Данное поле будет хранить всё... Вообще всё что связано с данным
	 * запуском. Буквально "unpredictionable data structures" - идеальный
	 * юзкейс для jsonb.
	 */
	[Column(TypeName = "jsonb")]
	public string? ScenarioJsonResult { get; set; }

	public Dictionary<string, string>? InputValues { get; set; } // name to value
	public Dictionary<string, string>? OutputValues { get; set; } // name to value
}
