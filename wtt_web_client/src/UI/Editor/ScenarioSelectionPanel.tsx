import { useState, useEffect } from 'react';
import DbTestScenario from '../../models/Scenario/DbTestScenario';
import cl from "./Editor.module.css";
import ScenarioSelectionCard from './ScenarioSelectionCard';

type ScenarioSelectionPanelArgs = {
	Scenarios: DbTestScenario[];
	onSelectionChanged: (guid: string) => void;
	onAddNew: () => void;
}

const ScenarioSelectionPanel: React.FC<ScenarioSelectionPanelArgs> = (props) =>
{
	const [selectedGuid, setSelectedGuid] = useState<string | null>(null);

	const onSelectionInternal = (newSelectedGuid: string) =>
	{
		console.info(`New scenario selected: '${newSelectedGuid}'.`);
		if (!props.Scenarios.some(s => s.Guid === newSelectedGuid)
			|| newSelectedGuid === selectedGuid) return;

		setSelectedGuid(newSelectedGuid);
		props.onSelectionChanged(newSelectedGuid);
	}

	return <span className={cl.scenarioSelectionPanel}>
		{
			props.Scenarios.map((x, index) => <button
				key={index}
				className={cl.scenarioSelectionCard}
				onClick={() => onSelectionInternal(x.Guid)}
			>
				{x.name}
			</button>)
		}

		<button
			onClick={props.onAddNew}
			className={[cl.scenarioSelectionCard, cl.scenarioSelectionAddNew].join(' ')}
		>+1</button>
	</span>
}

export default ScenarioSelectionPanel;