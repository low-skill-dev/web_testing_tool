import { useState, useEffect } from 'react';
import DbTestScenario from '../../models/Scenario/DbTestScenario';
import cl from './EditorCards.module.css';
import DbHttpAction from 'src/models/Actions/DbHttpAction';
import EditorCardButtons from './EditorCardsCommon';

interface HttpRequestCardArgs extends EditorCardButtons
{
	Action: DbHttpAction;
}

const HttpRequestCard: React.FC<HttpRequestCardArgs> = (props) =>
{
	return <span className={cl.actionCard}>
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