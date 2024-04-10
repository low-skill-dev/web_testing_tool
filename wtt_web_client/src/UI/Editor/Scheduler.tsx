import clg from "../../App.module.css";
import cl from './Editor.module.css';
import ActionsEditor from "./ActionsEditor";
import ScenarioEditor from "./ScenarioEditor";
import { useState, useEffect, useMemo } from 'react';
import ScenarioApi from "src/helpers/Api/ScenarioApi";
import ScenarioSelectionPanel from "./ScenarioSelectionPanel";
import ScenarioHelper from "src/helpers/Scenario/ScenarioHelper";
import { ActionsCollection, DbTestScenario } from "src/csharp/project";

const Scheduler: React.FC = () =>
{
	const [scenarios, setScenarios] = useState<DbTestScenario[]>([]);
	const [selectedScenario, setSelectedScenario] = useState<string>('');
	const [loadCompleted, setLoadCompleted] = useState(false);
	const [failedToLoad, setFailedToLoad] = useState(false);
	const [selectedScenarioActions, setSelectedScenarioActions] = useState<ActionsCollection>();
	const [key, setKey] = useState<string>('');

	useMemo(async () =>
	{
		let api = await ScenarioApi.Create();
		var scens = await api.GetMyScenarios();

		setFailedToLoad(scens === null);
		if (scens === null) return;
		setLoadCompleted(true);

		setScenarios(scens);
	}, []);

	const saveAll = async () =>
	{
		var api = await ScenarioApi.Create();
		api.SaveScenarios(scenarios);
	}


	return <span className={clg.rootElementsMargin}>
		<button
			onClick={saveAll}
			className={[cl.scenarioSelectionCard, cl.scenarioSelectionSaveAll].join(' ')}
			title='Save all scenarios to the database and realod the page'
		>SAVE</button>
		{scenarios.map(s => <span key={s.Guid!} className={cl.schedulerCard}>
			<hr />
			<span>UUID: {s.Guid!}</span>
			<span style={{ fontSize: "1.5rem" }} >NAME: {s.Name}</span>
			<span style={{ fontSize: "1.5rem" }}> Run every <input type="number"></input> minutes </span>
		</span>)}
	</span>
}

export default Scheduler;