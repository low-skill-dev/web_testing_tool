import clg from "../../App.module.css";
import ActionsEditor from "./ActionsEditor";
import ScenarioEditor from "./ScenarioEditor";
import { useState, useEffect, useMemo } from 'react';
import ScenarioApi, { ScenarioGuidToRuns } from "src/helpers/Api/ScenarioApi";
import ScenarioSelectionPanel from "./ScenarioSelectionPanel";
import ScenarioHelper from "src/helpers/Scenario/ScenarioHelper";
import { ActionsCollection, DbTestScenario, IDbScenarioRun } from "src/csharp/project";
import { format, formatDistance, formatRelative, subDays, parseISO } from 'date-fns'
//import { useInView } from "react-intersection-observer";

const MainPanel: React.FC = () =>
{
	const [scenarios, setScenarios] = useState<DbTestScenario[]>([]);
	const [logs, setLogs] = useState<ScenarioGuidToRuns[]>();
	const [selectedLogs, setSelectedLogs] = useState<IDbScenarioRun[]>();
	const [selectedDisplayedLogs, setSelectedDisplayedLogs] = useState<IDbScenarioRun[]>();
	const [showAllLogs, setShowAllLogs] = useState(false);
	const [selectedScenario, setSelectedScenario] = useState<string>('');
	const [loadCompleted, setLoadCompleted] = useState(false);
	const [failedToLoad, setFailedToLoad] = useState(false);
	const [selectedScenarioActions, setSelectedScenarioActions] = useState<ActionsCollection>();
	const [key, setKey] = useState<string>('');

	const logLoaded = (s: Array<DbTestScenario> | null) =>
	{
		if (s === null)
		{
			console.warn("Failed to load scenarios.");
			return;
		}

		const names = s.map(x =>
			`\t'${x.Name}':'${x.Guid!.substring(30, 6)}'\n`)
			.join().trimEnd();

		console.info(`Loaded ${s.length} scenarios${s.length > 0 ? ':\n' : ''}${names}.`);
	}

	useEffect(() =>
	{
		console.log("Set new logs:");
		console.log(logs);
	}, [logs]);

	useMemo(async () =>
	{
		let api = await ScenarioApi.Create();
		var scens = await api.GetMyScenarios();
		logLoaded(scens);

		setFailedToLoad(scens === null);
		if (scens === null) return;

		setScenarios(scens);

		var logs = await api.GetMyLogs();

		if (logs)
		{
			console.log("Loaded logs.");
			console.log(logs);
			setLogs(logs);
		}
		else
		{
			console.warn("Unable to load logs.");
			setLogs([]);
		}

		// scens.forEach(s =>
		// 	actionsByScenario.set(s.Name!, JSON.parse(s.ActionsJson!) as ActionsCollection)
		// );

		setLoadCompleted(true);
	}, []);


	useEffect(() =>
	{
		//if(logs) setSelectedLogs(logs.get(selectedScenario) ?? []);
	}, [selectedScenario]);

	useEffect(() =>
	{
		console.log(`useEffect[scenarios]`);

		scenarios.forEach(s =>
		{
			// if (!actionsByScenario.current.has(s.Guid!))
			// 	actionsByScenario.current.set(s.Guid!!, s.ActionsJson!)
		});
		// console.trace(actionsByScenario);
	}, [scenarios]);

	const setNewSelected = (guid: string) =>
	{
		console.trace("setNewSelected");

		setSelectedScenario(guid);
		console.warn("loiggins")
		console.warn(logs);
		if (logs) setSelectedLogs(logs.find(x => x.g === guid)?.r ?? []);
		else console.log("No logs for this scenario.");
		setSelectedScenarioActions(scenarios.find(x => x.Guid! === guid)!.ActionsJson!);
		setKey(ScenarioHelper.EnumerateActions(scenarios.find(x => x.Guid! === guid)!.ActionsJson!).map(a => a.Guid!).join(','));

		console.log(`New scenario selected: '${guid}'.`);
		console.log(`New actions assigned: [${ScenarioHelper.EnumerateActions(scenarios.find(x => x.Guid! === guid)!.ActionsJson!).map(a => a.Guid!).join(',')}].`);
	}

	useEffect(() =>
	{
		if (!selectedLogs) return;

		if (showAllLogs) setSelectedDisplayedLogs(selectedLogs);
		else setSelectedDisplayedLogs(selectedLogs?.slice(0, 10));
	}, [selectedLogs, showAllLogs]);

	const addNewScenario = () =>
	{
		let name = `scenario_${Date.now().toString().substring(4, 10)}`;
		while (scenarios.some(x => x.Name === name))
			name = `scenario_${Math.floor(Math.random() * (1000 * 1000))}`;

		// TODO: create empty scenario
		var toAdd = new DbTestScenario();
		toAdd.Name = name;
		toAdd.Guid! = crypto.randomUUID();

		toAdd.ActionsJson = ScenarioHelper.CreateNewActionsCollection();

		setScenarios([...scenarios, toAdd]);
		console.info(`Added scenario '${name}'.`);
	}

	const deleteScenario = (guid: string) =>
	{
		setSelectedScenario('');
		setScenarios([...(scenarios.filter(x => x.Guid! !== guid))]);
	}

	const showDebugInfo = () =>
	{
		// console.info(actionsByScenario);
		console.log(selectedLogs);
		//console.log(actionsOfSelected);
	}

	const saveAll = async () =>
	{
		var api = await ScenarioApi.Create();
		api.SaveScenarios(scenarios);
	}

	// const { ref: refLogs, inView, entry } = useInView({
	// 	/* Optional options */
	// 	threshold: 0,
	//   });

	return <span className={clg.rootElementsMargin}>
		{/* <button onClick={showDebugInfo}>debug</button> */}
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
					<ScenarioSelectionPanel onAddNew={addNewScenario} Scenarios={scenarios} onSelectionChanged={setNewSelected} onSaveAll={saveAll} />
		}
		<hr style={{ marginTop: ".25rem", marginBottom: ".25rem" }} />
		{
			(loadCompleted && selectedScenario) ?
				<ScenarioEditor
					Scenario={scenarios.find(s => s.Guid! === selectedScenario)!}
					DeleteScenarioCallback={deleteScenario}
				//Actions={actionsByScenario.get(selectedScenario)!} 
				/>
				:
				<span />
		}
		<hr style={{ marginTop: ".25rem", marginBottom: ".25rem" }} />
		{
			(loadCompleted && selectedScenario) ?
				<ActionsEditor key={key} Actions={selectedScenarioActions!} />
				:
				<span />
		}
		<hr style={{ marginTop: ".25rem", marginBottom: ".25rem" }} />
		<br />
		<input type="checkbox" title="SHOW ALL" content="SHOW ALL" checked={showAllLogs} onClick={()=> setShowAllLogs(!showAllLogs)} />
		RUNS: 
		{/* <button onClick={showDebugInfo}>debug</button>					 */}
		<br />
		<span style={{ width: "100%", height: "fit-content" }} /*ref={refLogs}*/>
			{
				(loadCompleted && selectedScenario && selectedLogs && selectedDisplayedLogs) ?
					selectedDisplayedLogs!.map(x => <span key={x.Guid!}>
						<br />
						{/* <span style={{ color: x.IsSucceeded ? "green" : "red" }} >{x.Completed}</span><br/> */}
						<span style={{ color: x.IsSucceeded ? "green" : "red" }} >{format(parseISO(x.Completed), 'dd-MM-yyyy hh:mm:ss')}</span><br />
						<span>SUCCESS: {x.IsSucceeded ? "true" : "false"}</span><br />
						<span>FAIL REASON: {x.ErrorMessage ?? "no"}</span><br />
					</span>)
					:
					<span />
			}
		</span>
	</span>
}

export default MainPanel;