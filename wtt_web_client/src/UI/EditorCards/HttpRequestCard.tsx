import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { DbHttpAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';

interface HttpRequestCardArgs
{
	Action: DbHttpAction;
}

const HttpRequestCard: React.FC<HttpRequestCardArgs> = (props) =>
{
	const [url, setUrl] = useState(props.Action.RequestUrl);
	const [method, setMethod] = useState(props.Action.Method);
	const [body, setBody] = useState(props.Action.RequestBody);
	const [headers, setHeaders] = useState(props.Action.RequestHeaders);
	const [cookies, setCookies] = useState(props.Action.RequestCookies);
	//const [script, setScript] = useState(props.Action.AfterRunScript); // ! COMMON

	useEffect(() => { props.Action.RequestUrl = url }, [url]);
	useEffect(() => { props.Action.Method = method }, [method]);
	useEffect(() => { props.Action.RequestBody = body }, [body]);
	useEffect(() => { props.Action.RequestHeaders = headers }, [headers]);
	useEffect(() => { props.Action.RequestCookies = cookies }, [cookies]);
	//useEffect(() => { props.Action.AfterRunScript = script}, [script]);

	const fixTextAreaHeight = (e: any) =>{
		e.currentTarget.style.height = "";e.currentTarget.style.height = `calc(${e.currentTarget.scrollHeight}px + 5px)`; 
	}

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
		<span className={cl.editorBlock}>
			<span className={cl.editorPropHeader}>URL</span>
			<textarea rows={1} className={cl.editorTextBlock} value={url} onChange={e=> setUrl(e.target.value)} onInput={e=> fixTextAreaHeight(e)}/>
		</span>
		<span className={cl.editorBlock}>
			<span  className={cl.editorPropHeader}>BODY</span>
			{/* https://stackoverflow.com/a/48460773/11325184 */}
			<textarea rows={1}  className={cl.editorTextBlock} value={body} onChange={e=> setBody(e.target.value)} onInput={e=> fixTextAreaHeight(e)} />
		</span>
		<span className={cl.editorBlock}>
			<span  className={cl.editorPropHeader}>HEADERS</span>
			{/* https://stackoverflow.com/a/48460773/11325184 */}
			<textarea rows={1}  className={cl.editorTextBlock} value={body} onChange={e=> setBody(e.target.value)} onInput={e=> fixTextAreaHeight(e)} />
		</span>
		<span className={cl.editorBlock}>
			<span  className={cl.editorPropHeader}>COOKIES</span>
			{/* https://stackoverflow.com/a/48460773/11325184 */}
			<textarea rows={1}  className={cl.editorTextBlock} value={body} onChange={e=> setBody(e.target.value)} onInput={e=> fixTextAreaHeight(e)} />
		</span>
		<ProxiedCardPart Action={props.Action} />
		{/* <span className={cl.editorBlock}>
			<span className={cl.editorPropHeader}>BODY</span>
			<textarea className={cl.editorTextBlock} value={body} onChange={e=> setBody(e.target.value)}/>
		</span> */}
	</span>
}

export default HttpRequestCard;