import cl from "./Editor.module.css";
import { CSSTransition } from 'react-transition-group';
import { useState, useEffect, useMemo, useRef } from 'react';
//import AuthHelper from '../../helpers/AuthHelper';
//import RegistrationRequest from '../../models/Auth/RegistrationRequest';
import ValidationHelper from '../../helpers/ValidationHelper';
import AuthorizedApiInteractionBase from "src/helpers/Api/AuthorizedApiInteractionBase";
import AuthHelper from "src/helpers/Api/AuthHelper";
import { useNavigate } from "react-router-dom";
import ScenarioApi from "src/helpers/Api/ScenarioApi";
import ScenarioSelectionPanel from "./ScenarioSelectionPanel";
import clg from "../../App.module.css";
import ScenarioEditor from "./ScenarioEditor";
import ActionsEditor from "./ActionsEditor";
import { ActionsCollection, DbHttpAction, DbTestScenario } from "src/csharp/project";
import { ADbAction } from '../../csharp/project';
import ScenarioHelper from "src/helpers/Scenario/ScenarioHelper";

const MainPanel: React.FC = () =>
{
	const [scenarios, setScenarios] = useState<DbTestScenario[]>([]);
	const [selectedScenario, setSelectedScenario] = useState<string>('');
	const [loadCompleted, setLoadCompleted] = useState(false);
	const [failedToLoad, setFailedToLoad] = useState(false);
	const [actionsOfSelected,setActionsOfSelected] = useState<ActionsCollection>();

	const actionsByScenario = new Map<string, ActionsCollection>();

	const logLoaded = (s: Array<DbTestScenario> | null) =>
	{
		if (s === null)
		{
			console.warn("Failed to load scenarios.");
			return;
		}

		console.warn(s);

		const names = s.map(x =>
			`\t'${x.Name}':'${x.Guid.substring(30, 6)}'\n`)
			.join().trimEnd();

		console.info(`Loaded ${s.length} scenarios${s.length > 0 ? ':\n' : ''}${names}.`);
	}

	useMemo(async() =>
	{
		let api = await ScenarioApi.Create();
		var scens = await api.GetMyScenarios();
		logLoaded(scens);

		setFailedToLoad(scens === null);
		if(scens === null) return;

		setScenarios(scens);

		scens.forEach(s =>
			actionsByScenario.set(s.Name!, JSON.parse(s.ActionsJson!) as ActionsCollection)
		);

		setLoadCompleted(true);
	}, []);

	
	useEffect(()=>{
		setActionsOfSelected(actionsByScenario.get(selectedScenario));
	}, [selectedScenario]);

	const addNewScenario = () =>
	{
		let name = `scenario_${Date.now().toString().substring(4, 10)}`;
		while (scenarios.some(x => x.Name === name))
			name = `scenario_${Math.floor(Math.random() * (1000 * 1000))}`;

		// TODO: create empty scenario
		var toAdd = new DbTestScenario();
		toAdd.Name = toAdd.Guid = name;
		toAdd.ActionsJson = JSON.stringify(ScenarioHelper.CreateNewActionsCollection());

		setScenarios([...scenarios, toAdd]);
		console.info(`Added scenario '${name}'.`);
	}

	return <span className={clg.rootElementsMargin}>
		{
			!loadCompleted ?
				<span>
					Loading...
				</span> :
				failedToLoad ?
					<span>
						Failed to load scenarios.
					</span>
					:
					<ScenarioSelectionPanel onAddNew={addNewScenario} Scenarios={scenarios} onSelectionChanged={setSelectedScenario} />
		}
		{
			(loadCompleted && selectedScenario) ?
				<ScenarioEditor
					Scenario={scenarios.find(s => s.Guid === selectedScenario)!}
					//Actions={actionsByScenario.get(selectedScenario)!} 
					/>
				:
				<span />
		}
		{
			(loadCompleted && selectedScenario) ?
				<ActionsEditor Actions={actionsOfSelected!} />
				:
				<span />
		}
	</span>
}

export default MainPanel;