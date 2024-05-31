/*
Есть идея как рисовать дерево. Например, можно
При каждом условном операторе можно смещаться вправо.
Если да - идём ниже, если нет - вправо. Это позволяет...
потратить дохуя времени и получить хуйню. Подумай дважды.
Вторым вариантом является ручное перекидывание через колонки.
Юзер жмет создать колонку. Но тогда туда можно закинуть только одно действие.
Можно дать создавать колонки ниже. В целом это все очень страшно.
Поэтому пока делаем ручные ифы. Но колонки точно надо сделать, хотябы
чтобы полностью вручную можно было накидывать. Для каждого экшена хранится колонка
и индекс в ней. Лишние колонки срезаются сервером при записи.
При рендере просто делаем нужное количетсов спанов. Если индексы в колонке
повторились у двух элементов, то просто записываем подряд.

СОХРАНИЕ
Сценарии редактируются как есть ('as-is'), без создания дублей,
в каждом сценарии есть кнопка сохранить и "сбросить", которая заного
подтягивает его с сервера.
*/

import cl from "./Editor.module.css";
import { CSSTransition } from 'react-transition-group';
import { useState, useEffect, useMemo, useLayoutEffect } from 'react';
//import AuthHelper from '../../helpers/AuthHelper';
//import RegistrationRequest from '../../models/Auth/RegistrationRequest';
import ValidationHelper from '../../helpers/ValidationHelper';
import AuthorizedApiInteractionBase from "src/helpers/Api/AuthorizedApiInteractionBase";
import AuthHelper from "src/helpers/Api/AuthHelper";
import { useNavigate } from "react-router-dom";
import ScenarioApi from "src/helpers/Api/ScenarioApi";
import ScenarioSelectionPanel from "./ScenarioSelectionPanel";
import clg from "../../App.module.css";
import ActionsEditor from "./ActionsEditor";
import { ActionsCollection, DbTestScenario } from "src/csharp/project";

type ScenarioEditorArgs = {
	Scenario: DbTestScenario;
	DeleteScenarioCallback: (guid: string) => void;
	// Actions: ActionsCollection;
}


const ScenarioEditor: React.FC<ScenarioEditorArgs> = (props) =>
{
	const [name, setName] = useState<string | null | undefined>(null);
	const [runEvery, setRunEvery] = useState<number>(1);
	const [deleteConfim, setDeleteConfim] = useState(false);

	useEffect(() => { if (name) props.Scenario.Name = name; }, [name]);
	useEffect(() => { if (name) props.Scenario.RunIntervalMinutes = runEvery; }, [runEvery]);

	useMemo(() =>
	{
		setName(props.Scenario.Name);
		setRunEvery(props.Scenario.RunIntervalMinutes ?? 0);
		setDeleteConfim(false);
	}, [props]);

	const deleteInternal = () =>
	{
		if (deleteConfim) props.DeleteScenarioCallback(props.Scenario.Guid!);
		else setDeleteConfim(true);
	}

	const runManually = async () =>{
		console.log(`runManually(${props.Scenario.Guid?.substring(36-12)})`)
		await (await ScenarioApi.Create()).RunManually(props.Scenario.Guid!);
	}

	return <span className={cl.nameEditor}>
		<span>UUID: {props.Scenario.Guid!}</span>
		<span style={{ fontSize: "1.5rem" }}><span onClick={runManually} >Run</span> every <input type="number" value={runEvery} onChange={e=> setRunEvery(parseInt(e.target.value) ?? runEvery)}></input> minutes </span>
		<span style={{ display: "flex", flexDirection: "row", flexWrap: "nowrap" }}>
			{/* <span style={{ display: "flex", marginRight: ".25rem" }}>NAME: </span> */}
			<input style={{ display: "flex", fontSize: "1.5rem" }} value={name ?? ''} onChange={e => setName(e.target.value)} />
			<button className={[cl.scenarioSelectionCard, cl.scenarioSelectionSaveAll].join(' ')} onClick={deleteInternal}>{!deleteConfim ? "DELETE" : "REALLY?"}</button>
		</span>
	</span>
}

export default ScenarioEditor;