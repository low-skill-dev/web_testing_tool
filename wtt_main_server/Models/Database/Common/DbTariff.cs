using CommonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;
using CommonLibrary.Models;

namespace Models.Database.Common;

#pragma warning disable CS8618

public class DbTariff : ObjectWithGuid
{
	public string Name { get; set; }
	public string Description { get; set; }

	/// <summary>
	/// Максимальное количество вложенных сценариев.
	/// Когда один сценарий вызыавет другой - его
	/// значение Depth увеличивается на единицу.
	/// </summary>
	public int MaximumExecutionDepth { get; set; } = 4;

	/// <summary>
	/// Общее максимальное число действий на одно
	/// исполнение сценария. Включая дочерние
	/// сценарии. Предполагается реализовавть
	/// через ref struct / сложение.
	/// </summary>
	public int MaximumActionsPerRun { get; set; } = 1024;

	/// <summary>
	/// Ограничение по памяти для JS-интерпритатора,
	/// выполняеющего обновление контекста после
	/// действия.
	/// </summary>
	public int MaximumScriptMemoryBytes { get; set; } = 4 * 1024 * 1024;

	/// <summary>
	/// Ограничение по времени для JS-интерпритатора,
	/// выполняеющего обновление контекста после
	/// действия.
	/// </summary>
	public int MaximumScriptTimeSecs { get; set; } = 3;
}