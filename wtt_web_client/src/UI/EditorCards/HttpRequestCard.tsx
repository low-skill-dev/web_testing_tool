import { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { DbHttpAction } from 'src/csharp/project';

interface HttpRequestCardArgs extends EditorCardButtons
{
	Action: DbHttpAction;
}

const HttpRequestCard: React.FC<HttpRequestCardArgs> = (props) =>
{
	return <span className={cl.actionCard}>
		<span>TestHttpCard</span>
		<span className={cl.actionGuid}>{props.Action.Guid}</span>
		<span className={cl.moveRow}>
			<button className={[cl.moveBtn, cl.moveUp].join('')} />
			<button className={[cl.moveBtn, cl.moveDown].join('')} />
			<button className={[cl.moveBtn, cl.moveLeft].join('')} />
			<button className={[cl.moveBtn, cl.moveRight].join('')} />
		</span>
	</span>
}

export default HttpRequestCard;