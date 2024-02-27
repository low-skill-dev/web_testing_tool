import { useState, useEffect } from 'react';
import DbTestScenario from '../../models/Scenario/DbTestScenario';
import cl from "./ScenarioSelectionPanel.module.css";

type ScenarioSelectionCardArgs = {
	IsSelected: boolean;
	Scenario: DbTestScenario;
	onSelection: (guid: string) => void;
}

const ScenarioSelectionCard: React.FC<ScenarioSelectionCardArgs> = (props) =>
{
	return <span
		className={cl.card}
		onClick={() => props.onSelection(props.Scenario.guid)}
	>
		{props.Scenario.name}
	</span>
}

export default ScenarioSelectionCard;