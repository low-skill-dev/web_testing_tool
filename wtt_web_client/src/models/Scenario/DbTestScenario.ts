import ADbObjectWithRelatedUser from "../Common/ADbObjectWithRelatedUser";
import TestScenarioArgTypes from "./TestScenarioArgTypes";

export default interface DbTestScenario extends ADbObjectWithRelatedUser
{
	name: string;
	description: string;
	changeDate: string;
	enableEmailNotifications: boolean;
	actionsJson: string;
	entryPoint: string;
	argTypes: TestScenarioArgTypes[];
	argNames: string[];
	sha512: string;
}
