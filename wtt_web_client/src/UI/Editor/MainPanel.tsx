import cl from "./AuthForm.module.css";
import { CSSTransition } from 'react-transition-group';
import { useState, useEffect, useMemo } from 'react';
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

const MainPanel: React.FC = () =>
{
	const [scenarios, setScenarios] = useState<DbTestScenario[]>([]);
	const [selectedScenario, setSelectedScenario] = useState('');
	const [loadCompleted, setLoadCompleted] = useState(false);
	const [failedToLoad, setFailedToLoad] = useState(false);

	const logLoaded = (s: DbTestScenario[] | null) =>
	{
		if (!s)
		{
			console.error("Failed to load scenarios.");
			return;
		}

		const names = s.map(x =>
			`\t'${x.name}':'${x.guid.substring(30, 6)}'\n`)
			.join().trimEnd();

		console.info(`Loaded ${s.length} scenarios:\n${names}.`);
	}

	useEffect(() =>
	{
		ScenarioApi.Create().then(a => a.GetMyScenarios().then(s =>
		{
			logLoaded(s);
			if (s) setScenarios(s);

			setFailedToLoad(!s);
			setLoadCompleted(true);
		}));
	});

	return <span >
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
				<ScenarioSelectionPanel >
		}
	</span>
}