using CommonLibrary.Models;
using CommonLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Database.Abstract;

public abstract class ADbAction : ObjectWithGuid
{
	public abstract string Type { get; } 

	public string Name { get; set; } = "Action";
	public string Description { get; set; } = "";
	public Guid? Next { get; set; }
	public bool ContinueExecutionInCaseOfCriticalError { get; set; } = false;

	public int ColumnId { get; set; } = 0;
	public int InColumnId {  get; set; } = 0;
}
