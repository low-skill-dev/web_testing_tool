import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { DbCertificateAction, DbHttpAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';
import WebRequestCardPart from './WebRequestCardPart';
import AHttpRequestCard from './AHttpRequestCard';

interface CertificateRequestCardArgs
{
	Action: DbCertificateAction;
}

const CertificateRequestCard: React.FC<CertificateRequestCardArgs> = (props) =>
{
	// const [url, setUrl] = useState(props.Action.RequestUrl);
	// const [method, setMethod] = useState(props.Action.Method);
	// useEffect(() => { props.Action.RequestUrl = url }, [url]);
	// useEffect(() => { props.Action.Method = method }, [method]);

	return <span className={cl.actionCard}>
		{/* <span className={cl.editorBlock}>
			<span className={cl.editorPropHeader}>METHOD</span>
			<select name='method' onVolumeChange={e => setMethod(parseInt(e.currentTarget.value))}>
				<option value={HttpRequestMethod.Get}>GET</option>
				<option value={HttpRequestMethod.Post}>POST</option>
				<option value={HttpRequestMethod.Put}>PUT</option>
				<option value={HttpRequestMethod.Patch}>PATCH</option>
				<option value={HttpRequestMethod.Delete}>DELETE</option>
			</select>
		</span> */}
		<AHttpRequestCard Action={props.Action} />
		<WebRequestCardPart Action={props.Action} />
		<ProxiedCardPart Action={props.Action} />
	</span>
}

export default CertificateRequestCard;