import { useState, useMemo } from 'react';
import ActionsCollection from "src/models/Actions/ActionsCollection";
import cl from "./Editor.module.css";
import clg from "../../App.module.css";
import ADbAction from "src/models/Actions/Abstract/ADbAction";
import GenericCard from '../EditorCards/GenericCard';

type ActionsColumnArgs = {
	Actions: ADbAction[];
	MoveActionLeftCallback: (guid: string) => void;
	MoveActionRightCallback: (guid: string) => void;
}

const ActionsColumn: React.FC<ActionsColumnArgs> = (props) =>
{
	const [actions, setActions] = useState<ADbAction[]>();
	const [enumeratedActions, setEnumeratedActions] = useState<ADbAction[]>();
	const [columns, setColumns] = useState<number[]>([]);

	// useLayoutEffect(() =>
	// {
	// 	setActions(actions);
	// 	setEnumeratedActions(actions!.Enumerate());
	// 	setColumns([...new Set(enumeratedActions!.map(x=> x.columnId))]);
	// }, [actions]);

	useMemo(async () =>
	{
		console.info(`Creating actions column with '${props.Actions.length}' actions.`);
		setActions(props.Actions);

		if (!actions || actions.length === 0) return;
		setEnumeratedActions((actions).toSorted((a, b) => a.InColumnId - b.InColumnId));
		setColumns([...new Set(enumeratedActions!.map(x => x.ColumnId))]);
	}, []);

	const moveUp = (guid: string) =>
	{

	}
	const moveDown = (guid: string) =>
	{

	}

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
	</span>
}

export default ActionsColumn;