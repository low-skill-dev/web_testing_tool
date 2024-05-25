import { ReactElement, useEffect, useState } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import HttpRequestCard from './HttpRequestCard';
import { ActionTypes, ADbAction, DbHttpAction } from "src/csharp/project";
import ImagePathHelper from 'src/helpers/Common/ImagePathHelper';
import Editor from 'react-simple-code-editor';
import { highlight, languages } from 'prismjs';
import 'prismjs/components/prism-clike';
import 'prismjs/components/prism-javascript';
import 'prismjs/themes/prism.css'; //Example style, you can use another
import EchoRequestCard from './EchoRequestCard';

interface GenericCardArgs extends EditorCardButtons
{
	Action: ADbAction;
}

const GenericCard: React.FC<GenericCardArgs> = (props) =>
{
	const [name, setName] = useState(props.Action.Name);
	const [script, setScript] = useState(props.Action.AfterRunScript);
	const [next, setNext] = useState(props.Action.Next);

	useEffect(() =>
	{
		props.Action.Next = next;
	}, next)

	const setNameInternal = (name: string) =>
	{
		props.Action.Name = name;
		setName(name);
	}
	const setScriptInternal = (script: string) =>
	{
		var s = script.replaceAll('\n', '\r\n');
		props.Action.AfterRunScript = s;
		setScript(s);
	}

	const getTypeString = (at: ActionTypes) =>
	{
		switch (at)
		{
			case ActionTypes.DbHttpActionType:
				return "HTTP";
			case ActionTypes.DbEchoActionType:
				return "ECHO";
			default:
				return "Request";
		}
	}

	let common =
		<span className={cl.actionHeader}>
			<span className={cl.actionGuid} title='Copy UUID' onClick={() => navigator.clipboard.writeText(props.Action.Guid!)}>{getTypeString(props.Action.Type!)} {props.Action.Guid!.substring(36-12, 36)}</span>
			{/* <span className={cl.actionGuid} title='UUID'>{props.Action.Guid!!!.substring(30, 6)}</span> */}
			<input className={cl.actionName} value={name} onChange={e => setNameInternal(e.target.value)} title='Name' />
			<span className={cl.moveRow}>
				<span title='Column and row ids'>({props.Action.ColumnId},{props.Action.RowId})</span>
				<button className={[cl.moveBtn, "moveUpBackground"].join(' ')} onClick={() => props.MoveUpCallback(props.Action.Guid!!)} style={{ backgroundImage: ImagePathHelper.UpArrow }} />
				<button className={[cl.moveBtn, "moveDownBackground"].join(' ')} onClick={() => props.MoveDownCallback(props.Action.Guid!!)} style={{ backgroundImage: ImagePathHelper.DownArrow }} />
				<button className={[cl.moveBtn, "moveLeftBackground"].join(' ')} onClick={() => props.MoveLeftCallback(props.Action.Guid!!)} style={{ backgroundImage: ImagePathHelper.LeftArrow }} />
				<button className={[cl.moveBtn, "moveRightBackground"].join(' ')} onClick={() => props.MoveRightCallback(props.Action.Guid!!)} style={{ backgroundImage: ImagePathHelper.RightArrow }} />
			</span>
		</span>;

	let specific: ReactElement;
	switch (props.Action.Type)
	{
		case ActionTypes.DbHttpActionType:
			specific = <HttpRequestCard Action={props.Action as DbHttpAction} />;
			break;
		case ActionTypes.DbEchoActionType:
			specific = <EchoRequestCard Action={props.Action as DbHttpAction} />;
			break;
		default:
			specific = <span />;
			break;
	}

	return <span className={cl.actionCardWrapper}>
		{common}
		{specific}
		<span className={cl.editorBlock} >
			<span className={cl.editorPropHeader}>SCRIPT</span>
			<Editor className={cl.codeEditor}
				value={script ?? ""}
				onValueChange={code => setScriptInternal(code)}
				highlight={code => highlight(code, languages.js, "js")}
				padding={3}
				style={{
					fontFamily: '"Fira code", "Fira Mono", monospace',
					fontSize: 12,
				}}
			/>
		</span>
		{
			props.Action.Type !== ActionTypes.DbConditionalActionType ?
				<span className={cl.editorBlock}>
					<span className={cl.editorPropHeader}>NEXT ACTION UUID</span>
					<input value={next} onChange={e => setNext(e.target.value)}></input>
				</span>
				:
				<span />
		}
	</span >
}

export default GenericCard;