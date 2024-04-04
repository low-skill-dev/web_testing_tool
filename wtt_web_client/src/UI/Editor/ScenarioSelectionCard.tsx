import { useState, useEffect } from 'react';
import cl from "./Editor.module.css";
import { DbTestScenario } from "src/csharp/project";

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
		{props.Scenario.Name}
	</button>
}

export default ScenarioSelectionCard;