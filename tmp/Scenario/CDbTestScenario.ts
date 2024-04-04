import ADbObjectWithRelatedUser from "../Common/ObjectWithUser";
import DbTestScenario from "./DbTestScenario";
import TestScenarioArgTypes from "./TestScenarioArgTypes";

export default class CDbTestScenario implements DbTestScenario
{
	Guid: string;
	UserGuid: string | null;
	Created: string;
	Changed: string | null;

	name: string;
	description: string;
	enableEmailNotifications: boolean;
	actionsJson: string;
	entryPoint: string;
	argTypes: TestScenarioArgTypes[];
	argNames: string[];

	constructor(
		guid: string,
		userGuid: string | null,
		created: string,
		changed: string | null,
		name: string,
		description: string,
		enableEmailNotifications: boolean,
		actionsJson: string,
		entryPoint: string,
		argTypes: TestScenarioArgTypes[],
		argNames: string[])
	{
		this.Guid = guid;
		this.UserGuid = userGuid;
		this.Created = created;
		this.Changed = changed;
		this.name = name;
		this.description = description;
		this.enableEmailNotifications = enableEmailNotifications;
		this.actionsJson = actionsJson;
		this.entryPoint = entryPoint;
		this.argTypes = argTypes;
		this.argNames = argNames;
	}


	public static CreateEmpty = (name: string) =>
	{
		return new CDbTestScenario(
			name, '0', Date.now().toString(), null, name,
			'', false, '{}', '0', [], []);
	}
}