import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { DbEchoAction, DbHttpAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';
import HtmlHelper from 'src/helpers/Common/HtmlHelper';

interface EchoRequestCardArgsns
{
	Action: DbEchoAction;
}

const EchoRequestCard: React.FC<EchoRequestCardArgsns> = (props) =>
{
	const [url, setUrl] = useState(props.Action.RequestUrl);

	useEffect(() => { props.Action.RequestUrl = url }, [url]);
	//useEffect(() => { props.Action.AfterRunScript = script}, [script]);

	return <span className={cl.actionCard}>

		<span className={cl.editorBlock}>
			<span className={cl.editorPropHeader}>URL</span>
			<textarea rows={1} className={cl.editorTextBlock} value={url} onChange={e=> setUrl(e.target.value)} onInput={e=> HtmlHelper.FixTextAreaHeight(e)}/>
		</span>
		<ProxiedCardPart Action={props.Action} />
	</span>
}

export default EchoRequestCard;