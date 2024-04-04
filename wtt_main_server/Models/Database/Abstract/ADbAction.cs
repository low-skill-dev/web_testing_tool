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
	public bool ScriptInTryBlock { get; set; } = false;
}