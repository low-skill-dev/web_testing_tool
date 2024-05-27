import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { DbDelayAction, DbEchoAction, DbHttpAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';
import HtmlHelper from 'src/helpers/Common/HtmlHelper';

interface DelayRequestCardArgs
{
	Action: DbDelayAction;
}

const DelayRequestCard: React.FC<DelayRequestCardArgs> = (props) =>
{
	const [delayS, setDelayS] = useState(props.Action.DelaySeconds);

	useEffect(() => { props.Action.DelaySeconds = delayS }, [delayS]);

	return <span className={cl.actionCard}>
		<span className={cl.editorBlock}>
			<span className={cl.editorPropHeader}>DELAY SECONDS</span>
			<input type="number" max={180} min={0} value={delayS} onChange={e => setDelayS(parseInt(e.target.value))} />
		</span>
	</span>
}

export default DelayRequestCard;