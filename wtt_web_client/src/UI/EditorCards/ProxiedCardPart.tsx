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
	const [username, setUsername] = useState(props.Action.ProxyUsername);
	const [password, setPassword] = useState(props.Action.ProxyPassword);

	useEffect(() => { props.Action.ProxyUrl = url }, [url]);

	return <span className={cl.actionCard}>

		<span className={cl.editorBlock}>
			{/* <span className={cl.editorPropHeader}>PROXY URL</span> */}
			<input className={cl.editorInput} value={url} onChange={e=> setUrl(e.target.value)} placeholder='PROXY URL' />
			<input className={cl.editorInput} value={username} onChange={e=> setUsername(e.target.value)} placeholder='PROXY USERNAME' />
			<input className={cl.editorInput} value={password} onChange={e=> setPassword(e.target.value)} placeholder='PROXY PASSWORD' />
		</span>
	</span>
}

export default ProxiedCardPart;