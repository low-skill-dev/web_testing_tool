using CommonLibrary.Models;
using CommonLibrary.Models;
using Models.Enums;
using Reinforced.Typings.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Database.Abstract;

////[TsClass(IncludeNamespace = false, Order = 100)]
public abstract class ADbAction : ObjectWithGuid
{
	public abstract ActionTypes Type { get; set; } 

	public string Name { get; set; } = "Action";
	public Guid? Next { get; set; }

	public int ColumnId { get; set; } = 0;
	public int RowId {  get; set; } = 0;

	public bool Bypass { get; set; } = false;

	public bool ContinueExecutionInCaseOfCriticalError { get; set; } = false;

	public string? AfterRunScript { get; set; }

	// Отказ от разработки по причине нехватки времени
	// public bool ScriptInTryBlock { get; set; } = false;

	/* После долги размышлений я пришел к выводу, что все прочие
	 * варианты работы являются овернинжинирингов и проще навесить
	 * на юзера обязанность написать одно (буквально одно) лишнее
	 * слово в текстбоксе, чем писать дополнительные сотни (если не
	 * тысячи) строк кода, каждая из которых может содержать ошибки.
	 * 
	 * Окончательно принимаю вариант, когда пользователь обращается
	 * к ответу через заранее захардкоженные переменные с названиями
	 * 'body', 'cookies', 'headers', в которых уже содержаться 
	 * определенные члены. Тело ответа - это JSON объект произвольной
	 * структуры, куки и заголовки - строго являются словорями, к членам
	 * которых можно обратиться через индекс-строку, аля 
	 * headers['last-modified'].
	 */
	public Dictionary<string, string>? VariableToPath { get; set; }
}