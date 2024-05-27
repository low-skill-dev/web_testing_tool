import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { DbEchoAction, DbHttpAction, DbImapAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';
import HtmlHelper from 'src/helpers/Common/HtmlHelper';

interface ImapRequestCardArgsns
{
	Action: DbImapAction;
}

const ImapRequestCard: React.FC<ImapRequestCardArgsns> = (props) =>
{
	const [imapAddress, setImapAddress] = useState(props.Action.ImapAddress ?? "");
	const [imapPort, setImapPort] = useState(props.Action.ImapPort ?? "");
	const [imapUser, setImapUser] = useState(props.Action.ImapUsername ?? "");
	const [imapPass, setImapPass] = useState(props.Action.ImapPassword ?? "");
	const [subjMustContain, setSubjMustContain] = useState(props.Action.SubjectMustContain ?? "");
	const [senderMustContain, setSenderMustContain] = useState(props.Action.SenderMustContain ?? "");
	const [bodyMustContain, setBodyMustContain] = useState(props.Action.BodyMustContain ?? "");
	const [bodyResultSearchRegex, setBodyResultSearchRegex] = useState(props.Action.BodySearchRegex ?? "");
	const [writeFoundToVariable, setWriteFoundToVariable] = useState(props.Action.WriteResultToVariable ?? "");

	useEffect(() => { props.Action.ImapAddress = imapAddress }, [imapAddress]);
	useEffect(() => { props.Action.ImapPort = imapPort }, [imapPort]);
	useEffect(() => { props.Action.ImapUsername = imapUser }, [imapUser]);
	useEffect(() => { props.Action.ImapPassword = imapPass }, [imapPass]);
	useEffect(() => { props.Action.SubjectMustContain = subjMustContain }, [subjMustContain]);
	useEffect(() => { props.Action.SenderMustContain = senderMustContain }, [senderMustContain]);
	useEffect(() => { props.Action.BodyMustContain = bodyMustContain }, [bodyMustContain]);
	useEffect(() => { props.Action.BodySearchRegex = bodyResultSearchRegex }, [bodyResultSearchRegex]);
	useEffect(() => { props.Action.WriteResultToVariable = writeFoundToVariable }, [writeFoundToVariable]);


	return <span className={cl.actionCard}>

		<details className={cl.editorBlock}>
			<summary>CREDENTIALS</summary>
			<input value={imapAddress} onChange={e => setImapAddress(e.target.value)} placeholder='IMAP ADDRESS'/>
			<input value={imapPort} onChange={e => setImapPort(e.target.value)} placeholder='IMAP PORT'/>
			<input value={imapUser} onChange={e => setImapUser(e.target.value)} placeholder='IMAP USER'/>
			<input value={imapPass} onChange={e => setImapPass(e.target.value)} placeholder='IMAP PASSWORD'/>
		</details>

		<details className={cl.editorBlock}>
			<summary>FILTERS</summary>
			<input value={subjMustContain} onChange={e => setSubjMustContain(e.target.value)} placeholder='SUBJECT MUST CONTAIN'/>
			<input value={senderMustContain} onChange={e => setSenderMustContain(e.target.value)} placeholder='SENDER MUST CONTAIN'/>
			<input value={bodyMustContain} onChange={e => setBodyMustContain(e.target.value)} placeholder='BODY MUST CONTAIN'/>
		</details>

		<span className={cl.editorBlock}>
					<span className={cl.editorPropHeader}>SEARCH REGEX</span>
					<input value={bodyResultSearchRegex} onChange={e => setBodyResultSearchRegex(e.target.value)} placeholder='SEARCH REGEX' />
					<span className={cl.editorPropHeader}>WRITE FOUND TO VARIABLE</span>
					<input value={writeFoundToVariable} onChange={e => setWriteFoundToVariable(e.target.value)} placeholder='OUTPUT VARIABLE NAME' />
				</span>
	</span>
}

export default ImapRequestCard;