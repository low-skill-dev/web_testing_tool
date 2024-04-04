import { useState, useMemo, useReducer } from 'react';
import cl from "./Editor.module.css";
import clg from "../../App.module.css";
import GenericCard from '../EditorCards/GenericCard';
import { ADbAction } from "src/csharp/project";

type ActionsColumnArgs = {
	SelfColumnId: number;
	Actions: ADbAction[];
	MoveActionLeftCallback: (guid: string) => void;
	MoveActionRightCallback: (guid: string) => void;
	AddHttpCallback: (columnId: number, rowId: number) => void;
	AddEchoCallback: (columnId: number, rowId: number) => void;
}

const ActionsColumn: React.FC<ActionsColumnArgs> = (props) =>
{
	const [actions, setActions] = useState<ADbAction[]>();
	const [enumeratedActions, setEnumeratedActions] = useState<ADbAction[]>();
	const [columns, setColumns] = useState<number[]>([]);
	const [, forceUpdate] = useReducer(x => x + 1, 0);

	// useLayoutEffect(() =>
	// {
	// 	setActions(actions);
	// 	setEnumeratedActions(actions!.Enumerate());
	// 	setColumns([...new Set(enumeratedActions!.map(x=> x.columnId))]);
	// }, [actions]);

	useMemo(async () =>
	{
		console.info(`Creating actions column with '${props.Actions.length}' actions.`);
		if (props.Actions.length === 0) return;

		setActions(props.Actions);
		setEnumeratedActions((props.Actions as any).toSorted((a: ADbAction, b: ADbAction) => a.RowId - b.ColumnId));
		console.info(`Enumerated actions contsins '${enumeratedActions!.length}' elements.`);
		setColumns([...new Set(enumeratedActions!.map(x => x.ColumnId))]);
	}, []);

	const moveUp = (guid: string) =>
	{

	}
	const moveDown = (guid: string) =>
	{

	}
	const getNewRowId = () => Math.max(...actions?.map(x => x.RowId) ?? [0]);

	//
	return <span className={cl.actionsColumns}>
		{enumeratedActions?.map((x, index) =>
			<GenericCard
				key={index}
				Action={x}
				MoveUpCallback={moveUp}
				MoveDownCallback={moveDown}
				MoveLeftCallback={props.MoveActionLeftCallback}
				MoveRightCallback={props.MoveActionRightCallback}
			/>
		)}

		<button className={[cl.columnAddAtionsBtns, cl.addActionBtn, cl.addActionHttp].join(' ')} onClick={() => { props.AddHttpCallback(props.SelfColumnId, getNewRowId()); forceUpdate(); }}>
			+HTTP
		</button>
		<button className={[cl.columnAddAtionsBtns, cl.addActionBtn, cl.addActionIcmp].join(' ')} onClick={() => { props.AddEchoCallback(props.SelfColumnId, getNewRowId()); forceUpdate(); }}>
			+ECHO
		</button>
	</span>
}

export default ActionsColumn;