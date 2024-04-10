import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { DbEchoAction, DbHttpAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';

interface EchoRequestCardArgsns
{
	Action: DbEchoAction;
}

const EchoRequestCard: React.FC<EchoRequestCardArgsns> = (props) =>
{
	const [url, setUrl] = useState(props.Action.RequestUrl);

	useEffect(() => { props.Action.RequestUrl = url }, [url]);
	//useEffect(() => { props.Action.AfterRunScript = script}, [script]);

	const fixTextAreaHeight = (e: any) =>{
		e.currentTarget.style.height = "";e.currentTarget.style.height = `calc(${e.currentTarget.scrollHeight}px + 5px)`; 
	}

	return <span className={cl.actionCard}>

		<span className={cl.editorBlock}>
			<span className={cl.editorPropHeader}>URL</span>
			<textarea rows={1} className={cl.editorTextBlock} value={url} onChange={e=> setUrl(e.target.value)} onInput={e=> fixTextAreaHeight(e)}/>
		</span>
		<ProxiedCardPart Action={props.Action} />
	</span>
}

export default EchoRequestCard;