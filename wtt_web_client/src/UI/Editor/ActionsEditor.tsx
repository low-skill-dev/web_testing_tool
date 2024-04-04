import { useState, useMemo, useReducer, MutableRefObject, useEffect } from 'react';
import cl from "./Editor.module.css";
import clg from "../../App.module.css";
import ActionsColumn from './ActionsColumn';
import { ActionsCollection, ADbAction, DbHttpAction } from "src/csharp/project";
import ScenarioHelper from '../../helpers/Scenario/ScenarioHelper';

type ActionsEditorArgs = {
	Actions: ActionsCollection;
}

const ActionsEditor: React.FC<ActionsEditorArgs> = (props) =>
{
	//const [actions, setActions] = useState<ActionsCollection>();
	const [enumeratedActions, setEnumeratedActions] = useState<ADbAction[]>([]);
	const [columns, setColumns] = useState<number[]>([]);
	const [actionsByColumn, setActionsByColumn] = useState<Map<number, Array<ADbAction>>>();
	const [, forceUpdate] = useReducer(x => x + 1, 0);

	// useLayoutEffect(() =>
	// {
	// 	setActions(actions);
	// 	setEnumeratedActions(actions!.Enumerate());
	// 	setColumns([...new Set(enumeratedActions!.map(x=> x.columnId))]);
	// }, [actions]);

	useMemo(async () =>
	{
		setEnumeratedActions(props.Actions ? ScenarioHelper.EnumerateActions(props.Actions) : []);
		console.info(`Enumeated actions: ${enumeratedActions!.length}`);
		setColumns([...new Set(enumeratedActions!.map(x => x.ColumnId))]);
		setActionsByColumn(new Map<number, Array<ADbAction>>());
		columns.forEach(c =>
		{
			actionsByColumn?.set(c, enumeratedActions!.filter(x => x.ColumnId === c));
		});
	}, []);

	const AddColumn = () =>
	{
		let newId = columns.length > 0 ? Math.max(...columns) + 1 : 0;
		actionsByColumn?.set(newId, []);
		setColumns([...columns, newId]);
		console.info(`Added column[${newId}]. Total: [${Array.from(actionsByColumn?.keys() ?? []).join(',')}].`);
	}

	const initNewAction = (a: ADbAction, column: number, row: number) =>
	{
		let name = `action_${Date.now().toString().substring(4, 10)}`;
		while (enumeratedActions.some(x => x.Name === name))
			name = `action_${Math.floor(Math.random() * (1000 * 1000))}`;

		a.Name = a.Guid = name;
		console.info(`Adding action: '${a.Name}'.`);
	}

	const addNewHttpAction = (column: number, row: number) =>
	{
		console.log("addNewHttpAction()");

		let toAdd = new DbHttpAction();
		initNewAction(toAdd, column, row);

		props.Actions.DbHttpActions?.push(toAdd);
		forceUpdate();
	}

	const addNewEchoAction = (column: number, row: number) =>
	{
		console.log("addNewEchoAction()");

		let toAdd = new DbHttpAction();
		initNewAction(toAdd, column, row);

		props.Actions.DbEchoActions?.push(toAdd);
		forceUpdate();
	}

	return <span className={clg.rootElementsMargin}>
		<span className={cl.columnsList}>
			{columns.map((x, index) => (
				<ActionsColumn
					key={index}
					SelfColumnId={x}
					Actions={enumeratedActions.filter(y => y.ColumnId === x)}
					MoveActionLeftCallback={null!}
					MoveActionRightCallback={null!}
					AddHttpCallback={addNewHttpAction}
					AddEchoCallback={addNewEchoAction}
				/>))}
			<button onClick={AddColumn}>Add column</button>
		</span>
	</span>
}

export default ActionsEditor;