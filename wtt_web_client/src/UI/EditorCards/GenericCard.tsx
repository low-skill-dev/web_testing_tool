import { useState, useEffect } from 'react';
import DbTestScenario from '../../models/Scenario/DbTestScenario';
import cl from './EditorCards.module.css';
import DbHttpAction from 'src/models/Actions/DbHttpAction';
import EditorCardButtons from './EditorCardsCommon';
import ADbAction from 'src/models/Actions/Abstract/ADbAction';
import tn from '../../config/TypeNames.json';
import HttpRequestCard from './HttpRequestCard';

interface GenericCardArgs extends EditorCardButtons
{
	Action: ADbAction;
}

const GenericCard: React.FC<GenericCardArgs> = (props) =>
{
	if (props.Action.Type === tn.DbHttpAction)
		return <HttpRequestCard Action={props.Action as DbHttpAction} {...(props as EditorCardButtons)} />;
	if (props.Action.Type === tn.DbHttpAction)
		return <HttpRequestCard Action={props.Action as DbHttpAction} {...(props as EditorCardButtons)} />;

	 return <span/>
}

export default GenericCard;