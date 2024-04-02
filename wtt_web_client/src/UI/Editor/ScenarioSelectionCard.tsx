import { useState, useEffect } from 'react';
import DbTestScenario from '../../models/Scenario/DbTestScenario';
import cl from "./Editor.module.css";

type ScenarioSelectionCardArgs = {
	IsSelected: boolean;
	Scenario: DbTestScenario;
	onSelection: (guid: string) => void;
}

const ScenarioSelectionCard: React.FC<ScenarioSelectionCardArgs> = (props) =>
{
	return <button
		className={cl.scenarioSelectionCard}
		onClick={() => props.onSelection(props.Scenario.Guid)}
	>
		{props.Scenario.name}
	</button>
}

export default ScenarioSelectionCard;