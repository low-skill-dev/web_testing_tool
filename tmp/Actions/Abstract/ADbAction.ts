import ADbObjectWithGuid from "src/models/Common/ObjectWithGuid";

export default abstract class ADbAction extends ADbObjectWithGuid
{
	Type: string;
	Name: string;
	Description: string;
	Next: string | null = null;
	ContinueExecutionInCaseOfCriticalError: boolean = false;
	ColumnId: number;
	InColumnId: number;

	constructor(type: string, name: string, description: string, columnId: number, inColumnId: number)
	{
		super();
		this.Type = type;
		this.Name = name;
		this.Description = description;
		this.ColumnId = columnId;
		this.InColumnId = inColumnId;
	}
}

