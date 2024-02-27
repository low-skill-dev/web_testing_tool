import { useState, useEffect } from 'react';
import DbTestScenario from '../../models/Scenario/DbTestScenario';
import cl from "./ScenarioSelectionPanel.module.css";
import ScenarioSelectionCard from './ScenarioSelectionCard';

type ScenarioSelectionPanelArgs = {
	Scenarios: DbTestScenario[];
	onSelectionChanged: (guid: string) => void;
}

const ScenarioSelectionPanel: React.FC<ScenarioSelectionPanelArgs> = (props) =>
{
	const [selectedGuid, setSelectedGuid] = useState<string | null>(null);

	const onSelectionInternal = (newSelectedGuid: string) =>
	{
		if(!props.Scenarios.some(s=> s.guid === newSelectedGuid)
			|| newSelectedGuid === selectedGuid) return;

		setSelectedGuid(newSelectedGuid);
		props.onSelectionChanged(selectedGuid!);
	}

	return <span className={cl.panel}>
		{
			props.Scenarios.map(x => <ScenarioSelectionCard 
				Scenario={x} 
				IsSelected={x.guid === selectedGuid}
				onSelection={(g: string) => onSelectionInternal(g)} 
			 />)
		}
	</span>
}

export default ScenarioSelectionPanel;