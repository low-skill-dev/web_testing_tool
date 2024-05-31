import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { ADbHttpAction, DbHttpAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';
import WebRequestCardPart from './WebRequestCardPart';
import HtmlHelper from 'src/helpers/Common/HtmlHelper';

interface AHttpRequestCardArgs
{
	Action: ADbHttpAction;
	ShowMethods?: boolean;
}

const AHttpRequestCard: React.FC<AHttpRequestCardArgs> = (props) =>
{
	const [method, setMethod] = useState(props.Action.Method);
	//const [headers, setHeaders] = useState(props.Action.RequestHeaders);
	//const [cookies, setCookies] = useState(props.Action.RequestCookies);
	//const [script, setScript] = useState(props.Action.AfterRunScript); // ! COMMON

	useEffect(() => { props.Action.Method = method; console.log(`method=${method}`) }, [method]);

	return <span className={cl.actionCard}>
		{(props.ShowMethods ?? true) ?
			<span className={cl.editorBlock}>
				<span className={cl.editorPropHeader}>METHOD</span>
				<select name='method' value={method} onChange={e => setMethod(parseInt(e.currentTarget.value))}>
					<option value={HttpRequestMethod.Get}>GET</option>
					<option value={HttpRequestMethod.Post}>POST</option>
					<option value={HttpRequestMethod.Put}>PUT</option>
					<option value={HttpRequestMethod.Patch}>PATCH</option>
					<option value={HttpRequestMethod.Delete}>DELETE</option>
				</select>
			</span> : <span />}
		<WebRequestCardPart Action={props.Action} />
		<ProxiedCardPart Action={props.Action} />
	</span>
}

export default AHttpRequestCard;