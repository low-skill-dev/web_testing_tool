import ADbObjectWithGuid from "src/models/Common/ADbObjectWithGuid";

export default interface ADbAction extends ADbObjectWithGuid
{
	type: string;
	name: string;
	description: string;
	next: string | null;
	continueExecutionInCaseOfCriticalError: boolean;
	columnId: number;
	inColumnId: number;
}

