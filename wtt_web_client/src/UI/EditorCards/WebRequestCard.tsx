import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { DbHttpAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';
import { ADbWebRequest } from '../../csharp/project';
import HtmlHelper from 'src/helpers/Common/HtmlHelper';

interface WebRequestCardArgs
{
	Action: ADbWebRequest;
}

const WebRequestCardPart: React.FC<WebRequestCardArgs> = (props) =>
{
	const [url, setUrl] = useState(props.Action.RequestUrl);
	useEffect(() => { props.Action.RequestUrl = url }, [url]);

	return <span className={cl.editorBlock}>
		<span className={cl.editorPropHeader}>URL</span>
		<textarea rows={1} className={cl.editorTextBlock} value={url} onChange={e => setUrl(e.target.value)} onInput={e => HtmlHelper.FixTextAreaHeight(e)} />
	</span>
}

export default WebRequestCardPart;