import { useState, useMemo, useReducer, useEffect } from 'react';
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
	UpdateParentLayout: () => void;
}

const ActionsColumn: React.FC<ActionsColumnArgs> = (props) =>
{
	const [enumeratedActions, setEnumeratedActions] = useState<ADbAction[]>();

	const UpscaleRowIndexesToZero = () =>
	{
		if (!props.Actions || props.Actions.length < 1) return;

		console.log(`Rows before upscaling: [${props.Actions.map(x => x.RowId).join(',')}].`)

		let minId = Math.min(...props.Actions.map(x => x.RowId));
		if (minId >= 0) return;

		props.Actions.forEach(x => x.RowId -= minId);

		console.log(`Rows after upscaling: [${props.Actions.map(x => x.RowId).join(',')}].`)
	}
	const MinimizeRowIndexes = () =>
	{
		if (!props.Actions || props.Actions.length < 1) return;

		console.log(`Rows before minimizing: [${props.Actions.map(x => x.RowId).join(',')}].`)

		let minId = Math.min(...props.Actions.map(x => x.RowId));

		for (let i = 0; i < props.Actions.length; i++)
		{
			props.Actions[i].RowId -= minId;
			if (i > 1)
			{
				props.Actions[i - 1].RowId -=
					props.Actions[i].RowId - props.Actions[i - 1].RowId - 1;
			}
		}

		console.log(`Rows after minimizing: [${props.Actions.map(x => x.RowId).join(',')}].`)
	}

	const reorderActions = () =>
	{
		UpscaleRowIndexesToZero();
		MinimizeRowIndexes();

		console.log(`Rows before sorting: [${props.Actions.map(x => x.RowId).join(',')}]`)
		let enumerated = [...(props.Actions.sort((a, b) => a.RowId - b.RowId))];
		console.log(`Rows after sorting: [${enumerated.map(x => x.RowId).join(',')}]`)

		console.log(`Settings new enumerated actions: [${enumerated.map(x => x.Guid!).join(',')}].`);
		setEnumeratedActions(enumerated);
		//setKeyStart(keyStart + 1000);
		//props.UpdateParentLayout();
	}

	useEffect(() =>
	{
		// if (!enumeratedActions || enumeratedActions.length === 0) return;
		// console.info(`Enumerated actions contsins '${enumeratedActions!.length}' elements.`);
		// setColumns([...new Set(enumeratedActions!.map(x => x.ColumnId))]);
	}, [enumeratedActions]);

	useMemo(async () =>
	{
		console.info(`Creating actions column with '${props.Actions.length}' actions.`);
		if (props.Actions.length === 0) return;

		//setActions(props.Actions);
		reorderActions();
	}, []);

	const moveUp = (guid: string) =>
	{
		console.log(`moveUp[${props.Actions.length}]`);

		let targetIndex = props.Actions!.findIndex(x => x.Guid! === guid);
		if (targetIndex === 0) return;

		let prev = props.Actions![targetIndex - 1];

		console.log(`Settings new rowId for action '${guid}': '${props.Actions![targetIndex].RowId}' -> '${prev.RowId - 1}'.`);

		props.Actions![targetIndex].RowId = prev.RowId - 1;
		reorderActions();
	}
	const moveDown = (guid: string) =>
	{
		console.log(`moveDown[${props.Actions.length}]`);

		let targetIndex = props.Actions!.findIndex(x => x.Guid! === guid);
		if (targetIndex === props.Actions!.length - 1) return;

		let next = props.Actions![targetIndex + 1];

		console.log(`Settings new rowId for action '${guid}': '${props.Actions![targetIndex].RowId}' -> '${next.RowId - 1}'.`);

		props.Actions![targetIndex].RowId = next.RowId + 1;
		reorderActions();
	}

	const getNewRowId = () =>
	{
		if (props.Actions && props.Actions.length > 0)
			return Math.max(...props.Actions?.map(x => x.RowId)) + 1;
		else
			return 0;
	}

	const printCurrentState = () =>
	{
		console.log(props.Actions);
	}

	return <span className={cl.actionsColumns}>
		<span className={cl.selfColumnId}>{props.SelfColumnId}</span>
		<button onClick={printCurrentState}>debug</button>
		<span style={{ border: "1px solid", borderColor: "inherit", width: "100%", height: "0px" }} />
		{enumeratedActions?.map((x, index) =>
			<GenericCard
				key={x.Guid!}
				Action={x}
				MoveUpCallback={moveUp}
				MoveDownCallback={moveDown}
				MoveLeftCallback={props.MoveActionLeftCallback}
				MoveRightCallback={props.MoveActionRightCallback}
			/>
		)}

		<button className={[cl.columnAddAtionsBtns, cl.addActionBtn, cl.addActionHttp].join(' ')} onClick={() => { props.AddHttpCallback(props.SelfColumnId, getNewRowId()); reorderActions(); }}>
			+HTTP
		</button>
		<button className={[cl.columnAddAtionsBtns, cl.addActionBtn, cl.addActionIcmp].join(' ')} onClick={() => { props.AddEchoCallback(props.SelfColumnId, getNewRowId()); reorderActions(); }}>
			+ECHO
		</button>
	</span>
}

export default ActionsColumn;