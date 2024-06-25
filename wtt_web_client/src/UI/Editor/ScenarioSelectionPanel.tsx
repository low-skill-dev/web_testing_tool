import { useState, useEffect } from 'react';
import cl from "./Editor.module.css";
import { DbTestScenario } from 'src/csharp/project';

type ScenarioSelectionPanelArgs = {
	Scenarios: DbTestScenario[];
	onSelectionChanged: (guid: string) => void;
	onAddNew: () => void;
	onSaveAll: () => void;
}

const ScenarioSelectionPanel: React.FC<ScenarioSelectionPanelArgs> = (props) =>
{
	const [selectedGuid, setSelectedGuid] = useState<string | null>(null);

	const onSelectionInternal = (newSelectedGuid: string) =>
	{
		console.info(`New scenario selected: '${newSelectedGuid}'.`);
		if (!props.Scenarios.some(s => s.Guid! === newSelectedGuid)
			|| newSelectedGuid === selectedGuid) return;

		setSelectedGuid(newSelectedGuid);
		props.onSelectionChanged(newSelectedGuid);
	}

	const printDebugInfo = () =>
	{
		console.log(props.Scenarios);
	}

	return <span className={cl.scenarioSelectionPanel}>

		{
			props.Scenarios.map((x, index) => <button
				key={index}
				className={cl.scenarioSelectionCard}
				onClick={() => onSelectionInternal(x.Guid!)}
			>
				{x.Name}
			</button>)
		}

		<button
			onClick={props.onAddNew}
			className={[cl.scenarioSelectionCard, cl.scenarioSelectionAddNew].join(' ')}
		>+1</button>

		<button
			onClick={props.onSaveAll}
			className={[cl.scenarioSelectionCard, cl.scenarioSelectionSaveAll].join(' ')}
			title='Save all scenarios to the database and realod the page'
		>SAVE</button>

		{/* <button
			onClick={printDebugInfo}
		>debug</button> */}

	</span>
}

export default ScenarioSelectionPanel;