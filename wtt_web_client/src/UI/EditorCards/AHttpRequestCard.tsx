import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { ADbHttpAction, DbHttpAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';
import WebRequestCardPart from './WebRequestCard';
import HtmlHelper from 'src/helpers/Common/HtmlHelper';

interface AHttpRequestCardArgs
{
	Action: ADbHttpAction;
}

const AHttpRequestCard: React.FC<AHttpRequestCardArgs> = (props) =>
{
	const [method, setMethod] = useState(props.Action.Method);
	//const [headers, setHeaders] = useState(props.Action.RequestHeaders);
	//const [cookies, setCookies] = useState(props.Action.RequestCookies);
	//const [script, setScript] = useState(props.Action.AfterRunScript); // ! COMMON

	useEffect(() => { props.Action.Method = method }, [method]);

	return <span className={cl.actionCard}>
		<span className={cl.editorBlock}>
			<span className={cl.editorPropHeader}>METHOD</span>
			<select name='method' onVolumeChange={e => setMethod(parseInt(e.currentTarget.value))}>
				<option value={HttpRequestMethod.Get}>GET</option>
				<option value={HttpRequestMethod.Post}>POST</option>
				<option value={HttpRequestMethod.Put}>PUT</option>
				<option value={HttpRequestMethod.Patch}>PATCH</option>
				<option value={HttpRequestMethod.Delete}>DELETE</option>
			</select>
		</span>
		<WebRequestCardPart Action={props.Action} />
		<ProxiedCardPart Action={props.Action} />
	</span>
}

export default AHttpRequestCard;