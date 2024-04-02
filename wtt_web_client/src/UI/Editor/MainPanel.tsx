import cl from "./Editor.module.css";
import { CSSTransition } from 'react-transition-group';
import { useState, useEffect, useMemo, useRef } from 'react';
//import AuthHelper from '../../helpers/AuthHelper';
//import RegistrationRequest from '../../models/Auth/RegistrationRequest';
import ValidationHelper from '../../helpers/ValidationHelper';
import AuthorizedApiInteractionBase from "src/helpers/Api/AuthorizedApiInteractionBase";
import AuthHelper from "src/helpers/Api/AuthHelper";
import { useNavigate } from "react-router-dom";
import DbTestScenario from "src/models/Scenario/DbTestScenario";
import ActionsCollection from "src/models/Actions/ActionsCollection";
import ScenarioApi from "src/helpers/Api/ScenarioApi";
import ScenarioSelectionPanel from "./ScenarioSelectionPanel";
import CDbTestScenario from "src/models/Scenario/CDbTestScenario";
import clg from "../../App.module.css";
import ScenarioEditor from "./ScenarioEditor";
import ActionsEditor from "./ActionsEditor";

const MainPanel: React.FC = () =>
{
	const [scenarios, setScenarios] = useState<DbTestScenario[]>([]);
	const actions = useRef<Map<string, ActionsCollection>>(new Map<string, ActionsCollection>());
	const [selectedScenario, setSelectedScenario] = useState('');
	const [loadCompleted, setLoadCompleted] = useState(false);
	const [failedToLoad, setFailedToLoad] = useState(false);

	const logLoaded = (s: Array<DbTestScenario> | null) =>
	{
		if (s === null)
		{
			console.error("Failed to load scenarios.");
			return;
		}

		console.warn(s);

		const names = s.map(x =>
			`\t'${x.name}':'${x.Guid.substring(30, 6)}'\n`)
			.join().trimEnd();

		console.info(`Loaded ${s.length} scenarios${s.length > 0 ? ':\n' : ''}${names}.`);
	}

	useMemo(() =>
	{
		ScenarioApi.Create().then(a => a.GetMyScenarios().then(s =>
		{
			logLoaded(s);
			if (s !== null)
			{
				setScenarios(s);
				s.forEach(s =>
					actions.current.set(s.name, JSON.parse(s.actionsJson) as ActionsCollection)
				);
			}
			setFailedToLoad(s === null);
			setLoadCompleted(true);
		}));
	}, []);

	const addNewScenario = () =>
	{
		let name = `scenario_${Date.now().toString().substring(4, 10)}`;
		while (scenarios.some(x => x.name === name))
			name = `scenario_${Math.floor(Math.random() * (1000 * 1000))}`;

		setScenarios([...scenarios, CDbTestScenario.CreateEmpty(name)]);
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
					Actions={actions.current.get(selectedScenario)!} />
				:
				<span />
		}
		{
			(loadCompleted && selectedScenario) ?
				<ActionsEditor Actions={actions.current.get(selectedScenario)!} />
				:
				<span />
		}
	</span>
}

export default MainPanel;