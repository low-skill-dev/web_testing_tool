import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { ADbProxiedAction, DbHttpAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';

interface ProxiedCardPartArgs
{
	Action: ADbProxiedAction;
}

const ProxiedCardPart: React.FC<ProxiedCardPartArgs> = (props) =>
{
	const [url, setUrl] = useState(props.Action.ProxyUrl);

	useEffect(() => { props.Action.ProxyUrl = url }, [url]);

	return <span className={cl.actionCard}>

		<span className={cl.editorBlock}>
			<span className={cl.editorPropHeader}>PROXY URL</span>
			<input className={cl.editorInput} value={url} onChange={e=> setUrl(e.target.value)} />
		</span>
	</span>
}

export default ProxiedCardPart;