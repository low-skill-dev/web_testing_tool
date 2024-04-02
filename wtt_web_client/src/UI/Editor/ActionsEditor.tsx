import { useState, useMemo, useReducer } from 'react';
import ActionsCollection from "src/models/Actions/ActionsCollection";
import cl from "./Editor.module.css";
import clg from "../../App.module.css";
import ADbAction from "src/models/Actions/Abstract/ADbAction";
import ActionsColumn from './ActionsColumn';

type ActionsEditorArgs = {
	Actions: ActionsCollection;
}

const ActionsEditor: React.FC<ActionsEditorArgs> = (props) =>
{
	const [actions, setActions] = useState<ActionsCollection>();
	const [enumeratedActions, setEnumeratedActions] = useState<ADbAction[]>([]);
	const [columns, setColumns] = useState<number[]>([]);
	const [actionsByColumn, setActionsByColumn] = useState<Map<number, Array<ADbAction>>>();

	// useLayoutEffect(() =>
	// {
	// 	setActions(actions);
	// 	setEnumeratedActions(actions!.Enumerate());
	// 	setColumns([...new Set(enumeratedActions!.map(x=> x.columnId))]);
	// }, [actions]);

	useMemo(async () =>
	{
		setActions(actions);
		setEnumeratedActions(actions ? actions.Enumerate() : []);
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

	return <span className={clg.rootElementsMargin}>
		<span className={cl.columnsList}>
			{columns.map((x, index) => (
				<ActionsColumn
					key={index}
					Actions={actionsByColumn?.get(x)!}
					MoveActionLeftCallback={null!}
					MoveActionRightCallback={null!}
				/>))}
			<button onClick={AddColumn}>Add column</button>
		</span>
	</span>
}

export default ActionsEditor;