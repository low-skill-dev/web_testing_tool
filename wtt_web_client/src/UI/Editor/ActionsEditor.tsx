import React, { useState, useMemo, useReducer, MutableRefObject, useEffect } from 'react';
import cl from "./Editor.module.css";
import clg from "../../App.module.css";
import ActionsColumn from './ActionsColumn';
import { ActionsCollection, ADbAction, DbCertificateAction, DbConditionalAction, DbDelayAction, DbEchoAction, DbErrorAction, DbHttpAction, DbImapAction, DbScenarioAction } from "src/csharp/project";
import ScenarioHelper from '../../helpers/Scenario/ScenarioHelper';
import { randomUUID } from 'crypto';

type ActionsEditorArgs = {
	Actions: ActionsCollection;
}

const ActionsEditor: React.FC<ActionsEditorArgs> = (props) =>
{
	const [actions, setActions] = useState<ADbAction[]>([]);
	const [columns, setColumns] = useState<number[]>([]);
	const [keyStart, setKeyStart] = useState(1000);

	const [, updateState] = React.useState();
	const forceUpdate = React.useCallback(() => updateState({} as any), []);

	useMemo(() =>
	{
		console.log(`Creating ActionsEditor. Actions is ${props.Actions}.`);
		setActions(props.Actions ? ScenarioHelper.EnumerateActions(props.Actions) : []);
		setColumns([...new Set(actions!.map(x => x.ColumnId)), 0]);
	}, []);

	useEffect(() =>
	{
		setColumns([...new Set([...actions!.map(x => x.ColumnId), 0])]);
	}, [actions]);

	const AddColumn = () =>
	{
		let newId = columns.length > 0 ? Math.max(...columns) + 1 : 0;
		setColumns([...columns, newId]);
		console.info(`Added column[${newId}]. Total: [${columns.join(',')}].`);
	}

	const initNewAction = (a: ADbAction, column: number, row: number) =>
	{
		let name = `action_${Date.now().toString().substring(4, 10)}`;
		while (actions.some(x => x.Name === name))
			name = `action_${Math.floor(Math.random() * (1000 * 1000))}`;

		a.Name = name;
		a.Guid! = crypto.randomUUID();
		a.ColumnId = column;
		a.RowId = row;
		console.info(`Adding action: '${a.Name}'. RowId: ${a.RowId}. ColumnId: ${a.ColumnId}.`);
	}

	const addNewHttpAction = (column: number, row: number) =>
	{
		console.log("addNewHttpAction()");

		let toAdd = new DbHttpAction();
		initNewAction(toAdd, column, row);

		//console.warn(props.Actions.DbHttpActions === undefined);
		setActions([...actions, toAdd]);
		//forceUpdate();
		props.Actions.DbHttpActions?.push(toAdd);
		// setColumns([...columns]);
		setKeyStart(keyStart + 100);
	}

	const addNewEchoAction = (column: number, row: number) =>
	{
		console.log("addNewEchoAction()");

		let toAdd = new DbEchoAction();
		initNewAction(toAdd, column, row);

		setActions([...actions, toAdd]);
		props.Actions.DbEchoActions?.push(toAdd);
		setKeyStart(keyStart + 100);
	}

	const addNewX509Action = (column: number, row: number) =>
	{
		console.log("addNewX509Action()");

		let toAdd = new DbCertificateAction();
		initNewAction(toAdd, column, row);

		setActions([...actions, toAdd]);
		props.Actions.DbCertificateActions?.push(toAdd);
		setKeyStart(keyStart + 100);
	}

	const addNewDelayAction = (column: number, row: number) =>
	{
		console.log("addNewDelayAction()");

		let toAdd = new DbDelayAction();
		initNewAction(toAdd, column, row);

		setActions([...actions, toAdd]);
		props.Actions.DbDelayActions?.push(toAdd);
		setKeyStart(keyStart + 100);
	}

	const addNewImapAction = (column: number, row: number) =>
	{
		console.log("addNewImapAction()");

		let toAdd = new DbImapAction();
		initNewAction(toAdd, column, row);

		setActions([...actions, toAdd]);
		props.Actions.DbImapActions?.push(toAdd);
		setKeyStart(keyStart + 100);
	}

	const addNewErrorAction = (column: number, row: number) =>
	{
		console.log("addNewErrorAction()");

		let toAdd = new DbErrorAction();
		initNewAction(toAdd, column, row);

		setActions([...actions, toAdd]);
		props.Actions.DbErrorActions?.push(toAdd);
		setKeyStart(keyStart + 100);
	}

	const addNewConditionalAction = (column: number, row: number) =>
	{
		console.log("addNewConditionalAction()");

		let toAdd = new DbConditionalAction();
		initNewAction(toAdd, column, row);

		setActions([...actions, toAdd]);
		props.Actions.DbConditionalActions?.push(toAdd);
		setKeyStart(keyStart + 100);
	}

	const addNewScenarioAction = (column: number, row: number) =>
	{
		console.log("addNewScenarioAction()");

		let toAdd = new DbScenarioAction();
		initNewAction(toAdd, column, row);

		setActions([...actions, toAdd]);
		props.Actions.DbScenarioActions?.push(toAdd);
		setKeyStart(keyStart + 100);
	}

	const showDebugInfo = () =>
	{
		console.log(actions);
		console.log(columns);
	}

	const deleteAction = (guid: string) =>
	{
		console.log(`deleteAction('${guid.substring(36 - 12)}')`);
		ScenarioHelper.DeleteAction(props.Actions, guid);
		setKeyStart(keyStart + 100);
		return;
	}

	const moveRight = (g: string, left: boolean) =>
	{
		console.log(`moveRight('${g.substring(36 - 12)}', ${left})`);

		if (!g) return;
		if (!actions) return;
		var found = actions.find(x => x.Guid === g);
		if (!found) return;

		console.log("found is not null");

		let minColId = actions.map(x => x.ColumnId).sort()[0];
		let maxColId = actions.map(x => x.ColumnId).sort()[actions.length - 1];

		console.log(`minColId=${minColId}`);
		console.log(`maxColId=${maxColId}`);

		//if (maxColId === minColId) return;
		if (left && found.ColumnId === minColId) return;
		if (!left && found.ColumnId === maxColId && maxColId !== minColId) return;

		let colIds = actions.map(x => x.ColumnId).sort();
		let current = colIds.indexOf(found.ColumnId);
		let targetCol = colIds[current + (left ? -1 : 1)];
		if (targetCol === current) targetCol++;

		console.log(`targetCol=${targetCol}`);

		var maxRowIdInDest = actions.filter(x => x.ColumnId === targetCol)
			.map(x => x.RowId).sort().reverse()[0];

		if (isNaN(maxRowIdInDest)) maxRowIdInDest = 0;

		let prevCol = found.ColumnId, prevRow = found.RowId;

		found.ColumnId = targetCol;
		found.RowId = maxRowIdInDest + 1;

		console.log(`Change action '${found.Guid?.substring(36 - 12)}' position:` +
			`[${prevCol},${prevCol}] -> [${found.ColumnId},${found.RowId}]`);

		setKeyStart(keyStart + 100);
		setColumns([...columns]);
	}

	return <span className={clg.rootElementsMargin}>
		<button onClick={showDebugInfo}>debug</button>
		<span className={cl.columnsList}>
			{columns.map((x, index) => (
				<ActionsColumn
					key={1000 * (index + 1) + keyStart * (x + 1)}
					SelfColumnId={x}
					Actions={actions.filter(y => y.ColumnId === x)}
					MoveActionLeftCallback={g => moveRight(g, true)}
					MoveActionRightCallback={g => moveRight(g, false)}
					AddHttpCallback={addNewHttpAction}
					AddEchoCallback={addNewEchoAction}
					AddConditionalCallback={addNewConditionalAction}
					AddDelayCallback={addNewDelayAction}
					AddX509Callback={addNewX509Action}
					AddImapCallback={addNewImapAction}
					AddErrorCallback={addNewErrorAction}
					AddScenarioCallback={addNewScenarioAction}
					UpdateParentLayout={() => setKeyStart(keyStart + 100)}
					DeleteAction={deleteAction}
				/>))}
			<button onClick={AddColumn}>Add column</button>
		</span>
	</span>
}

export default ActionsEditor;