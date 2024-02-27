import DbTestScenario from '../../models/Scenario/DbTestScenario';

export default class ScenarioHelper
{
	// https://stackoverflow.com/a/43836580/11325184
	public static GetClone = (s: DbTestScenario): DbTestScenario => Object.assign({}, s);
}