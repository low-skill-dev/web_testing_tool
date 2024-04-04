import ADbObjectWithRelatedUser from "../Common/ObjectWithUser";
import TestScenarioArgTypes from "./TestScenarioArgTypes";

export default interface DbTestScenario extends ADbObjectWithRelatedUser
{
	name: string;
	description: string;
	enableEmailNotifications: boolean;
	actionsJson: string;
	entryPoint: string;
	argTypes: TestScenarioArgTypes[];
	argNames: string[];
}
