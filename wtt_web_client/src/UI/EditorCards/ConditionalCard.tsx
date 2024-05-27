import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { DbConditionalAction, DbEchoAction, DbHttpAction, DbImapAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';
import HtmlHelper from 'src/helpers/Common/HtmlHelper';

interface ConditionalRequestCardArgs
{
	Action: DbConditionalAction;
}

const ConditionalRequestCard: React.FC<ConditionalRequestCardArgs> = (props) =>
{
	const [jsBoolExpr, setJsBoolExpr] = useState(props.Action.JsBoolExpression ?? "");
	const [onTrue, setOnTrue] = useState(props.Action.ActionOnTrue ?? "");
	const [onFalse, setOnFalse] = useState(props.Action.ActionOnFalse ?? "");

	return <span className={cl.actionCard}>
		<span className={cl.editorBlock}>
			<span className={cl.editorPropHeader}>SEARCH REGEX</span>
			<input value={jsBoolExpr} onChange={e => setJsBoolExpr(e.target.value)} placeholder='JS LOGICAL EXPRESSION' />
			<span className={cl.editorPropHeader}>WRITE FOUND TO VARIABLE</span>
			<input value={onTrue} onChange={e => setOnTrue(e.target.value)} placeholder='action guid on TRUE' />
			<span className={cl.editorPropHeader}>WRITE FOUND TO VARIABLE</span>
			<input value={onFalse} onChange={e => setOnFalse(e.target.value)} placeholder='action guid on FALSE' />
		</span>
	</span>
}

export default ConditionalRequestCard;