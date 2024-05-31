import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { DbConditionalAction, DbEchoAction, DbHttpAction, DbImapAction, DbScenarioAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';
import HtmlHelper from 'src/helpers/Common/HtmlHelper';

interface ScenarioRequestCardArgs
{
	Action: DbScenarioAction;
}

const ScenarioRequestCard: React.FC<ScenarioRequestCardArgs> = (props) =>
{
	const [calledGuid, setCalledGuid] = useState(props.Action.CalledScenarioGuid ?? "");
	const [writeOutContextToVar, setWriteOutContextToVar] = useState(props.Action.WriteAllResultToVariable ?? "");
	const [useCurrentContext, setUseCurrentContext] = useState(props.Action.UseParentContextAsInitial ?? true);

	useEffect(() => { props.Action.CalledScenarioGuid = calledGuid }, [calledGuid]);
	useEffect(() => { props.Action.WriteAllResultToVariable = writeOutContextToVar }, [writeOutContextToVar]);
	useEffect(() => { props.Action.UseParentContextAsInitial = useCurrentContext }, [useCurrentContext]);

	return <span className={cl.actionCard}>
		<span className={cl.editorBlock}>
			<span className={cl.editorPropHeader}>SCENARIO GUID</span>
			<input value={calledGuid} onChange={e => setCalledGuid(e.target.value)} placeholder='SCENARIO GUID' />
			<span className={cl.editorPropHeader}>INTERNAL CONTEXT VAR</span>
			<input value={writeOutContextToVar} onChange={e => setWriteOutContextToVar(e.target.value)} placeholder='WILL BE WRITTEN AFTER COMPLETION' />
			<span className={cl.editorPropHeader}>USE CURRENT CONTEXT AS INITIAL</span>
			<input style={{ width: "fit-content" }} type="checkbox" checked={useCurrentContext} onChange={e => setUseCurrentContext(e.target.checked)} />
		</span>
	</span>
}

export default ScenarioRequestCard;