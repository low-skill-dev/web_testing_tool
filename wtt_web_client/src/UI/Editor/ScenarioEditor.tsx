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
import DbTestScenario from "src/models/Scenario/DbTestScenario";
import ActionsCollection from "src/models/Actions/ActionsCollection";
import ScenarioApi from "src/helpers/Api/ScenarioApi";
import ScenarioSelectionPanel from "./ScenarioSelectionPanel";
import CDbTestScenario from "src/models/Scenario/CDbTestScenario";
import clg from "../../App.module.css";
import ADbAction from "src/models/Actions/Abstract/ADbAction";
import ActionsEditor from "./ActionsEditor";

type ScenarioEditorArgs = {
	Scenario: DbTestScenario;
	Actions: ActionsCollection;
}


const ScenarioEditor: React.FC<ScenarioEditorArgs> = (props) =>
{
	const [name, setName] = useState<string|null>(null);

	useLayoutEffect(() =>
	{
		setName(props.Scenario.name);
	}, [props.Scenario]);


	const setNameInternal = (name: string) =>
	{
		setName(name);
		props.Scenario.name = name;
	}

	return <span>
		<span>{props.Scenario.Guid}</span>
		<input value={name ?? ''} onChange={e => setNameInternal(e.target.value)} />
	</span>
}

export default ScenarioEditor;