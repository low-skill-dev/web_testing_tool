import React, { useState, useMemo, useReducer, MutableRefObject, useEffect } from 'react';
import cl from "./Editor.module.css";
import clg from "../../App.module.css";
import ActionsColumn from './ActionsColumn';
import { ActionsCollection, ADbAction, DbEchoAction, DbHttpAction } from "src/csharp/project";
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
		setColumns([...new Set([...actions!.map(x => x.ColumnId),0])]);
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

	const showDebugInfo = () =>
	{
		console.log(actions);
		console.log(columns);
	}

	return <span className={clg.rootElementsMargin}>
		<button onClick={showDebugInfo}>debug</button>
		<span className={cl.columnsList}>
			{columns.map((x, index) => (
				<ActionsColumn
					key={1000 * (index + 1) + keyStart * (x + 1)}
					SelfColumnId={x}
					Actions={actions.filter(y => y.ColumnId === x)}
					MoveActionLeftCallback={null!}
					MoveActionRightCallback={null!}
					AddHttpCallback={addNewHttpAction}
					AddEchoCallback={addNewEchoAction}
					UpdateParentLayout={() => setKeyStart(keyStart + 100)}
				/>))}
			<button onClick={AddColumn}>Add column</button>
		</span>
	</span>
}

export default ActionsEditor;