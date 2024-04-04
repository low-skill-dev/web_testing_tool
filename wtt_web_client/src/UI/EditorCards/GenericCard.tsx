import { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import tn from '../../config/TypeNames.json';
import HttpRequestCard from './HttpRequestCard';
import { ActionTypes, ADbAction, DbHttpAction } from "src/csharp/project";

interface GenericCardArgs extends EditorCardButtons
{
	Action: ADbAction;
}

const GenericCard: React.FC<GenericCardArgs> = (props) =>
{
	if (props.Action.Type === ActionTypes.DbHttpActionType)
		return <HttpRequestCard Action={props.Action as DbHttpAction} {...(props as EditorCardButtons)} />;
	if (props.Action.Type === ActionTypes.DbImapActionType)
		return <HttpRequestCard Action={props.Action as DbHttpAction} {...(props as EditorCardButtons)} />;

	 return <span/>
}

export default GenericCard;