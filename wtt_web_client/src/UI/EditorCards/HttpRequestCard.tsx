import React, { useState, useEffect } from 'react';
import cl from './EditorCards.module.css';
import EditorCardButtons from './EditorCardsCommon';
import { DbHttpAction, HttpRequestMethod } from 'src/csharp/project';
import Editor from 'react-simple-code-editor';
import ProxiedCardPart from './ProxiedCardPart';
import WebRequestCardPart from './WebRequestCardPart';
import AHttpRequestCard from './AHttpRequestCard';
import HtmlHelper from 'src/helpers/Common/HtmlHelper';

interface HttpRequestCardArgs
{
	Action: DbHttpAction;
}

const HttpRequestCard: React.FC<HttpRequestCardArgs> = (props) =>
{
	//const [url, setUrl] = useState(props.Action.RequestUrl);
	//const [method, setMethod] = useState(props.Action.Method);
	const [body, setBody] = useState(props.Action.RequestBody);
	const [headersText, setHeadersText] = useState(props.Action.RequestHeaders?.filter(x =>
		x[0]?.length > 0 && x[1]?.length > 0).join('\n'));
	const [cookiesText, setCookiesText] = useState(props.Action.RequestCookies?.filter(x =>
		x[0]?.length > 0 && x[1]?.length > 0).join('\n'));
	const [minCode, setMinCode] = useState(props.Action.MinResponseCode);
	const [maxCode, setMaxCode] = useState(props.Action.MaxResponseCode);
	//const [headers, setHeaders] = useState(props.Action.RequestHeaders);
	//const [cookies, setCookies] = useState(props.Action.RequestCookies);
	//const [script, setScript] = useState(props.Action.AfterRunScript); // ! COMMON

	//useEffect(() => { props.Action.RequestUrl = url }, [url]);
	//useEffect(() => { props.Action.Method = method }, [method]);
	useEffect(() => { props.Action.RequestBody = body }, [body]);
	useEffect(() => { props.Action.MinResponseCode = minCode }, [minCode]);
	useEffect(() => { props.Action.MaxResponseCode = maxCode }, [maxCode]);

	useEffect(() =>
	{
		try
		{
			props.Action.RequestHeaders = headersText?.replaceAll('\r', '').split('\n')
			.filter(x=> x?.length > 3).map(a =>
			{
				let [k, v] = a.split(' ', 2);
				return [k, v];
			})!;
		} catch { }
	}, [headersText]);
	useEffect(() =>
	{
		try
		{
			props.Action.RequestCookies = cookiesText?.replaceAll('\r', '').split('\n')
			.filter(x=> x?.length > 3).map(a =>
			{
				let [k, v] = a.split(' ', 2);
				return [k, v];
			})!;
		} catch { }
	}, [cookiesText]);
	//useEffect(() => { props.Action.AfterRunScript = script}, [script]);

	return <span className={cl.actionCard}>
		<AHttpRequestCard Action={props.Action} />
		<details>
			<summary>BODY, HEADERS, COOKIES, HTTP CODES</summary>
			<span className={cl.editorBlock}>
				{/* <span className={cl.editorPropHeader}>BODY</span> */}
				{/* https://stackoverflow.com/a/48460773/11325184 */}
				<textarea rows={1} className={cl.editorTextBlock} value={body} onChange={e => setBody(e.target.value)} onInput={e => HtmlHelper.FixTextAreaHeight(e)} placeholder='BODY' title='BODY' />
			</span>
			<span className={cl.editorBlock}>
				{/* <span className={cl.editorPropHeader}>HEADERS</span> */}
				{/* https://stackoverflow.com/a/48460773/11325184 */}
				<textarea rows={1} className={cl.editorTextBlock} value={headersText} onChange={e => setHeadersText(e.target.value)} onInput={e => HtmlHelper.FixTextAreaHeight(e)} placeholder='HEADERS' title='HEADERS' />
			</span>
			<span className={cl.editorBlock}>
				{/* <span className={cl.editorPropHeader}>COOKIES</span> */}
				{/* https://stackoverflow.com/a/48460773/11325184 */}
				<textarea rows={1} className={cl.editorTextBlock} value={cookiesText} onChange={e => setCookiesText(e.target.value)} onInput={e => HtmlHelper.FixTextAreaHeight(e)} placeholder='COOKIES' title='COOKIES' />
			</span>
			{/* <span className={cl.editorBlock}>
			<span className={cl.editorPropHeader}>BODY</span>
			<textarea className={cl.editorTextBlock} value={body} onChange={e=> setBody(e.target.value)}/>
		</span> */}
			<span style={{ display: "flex", flexDirection: "row", marginTop:".25rem" }}>
				Allowed response codes:
				<input title='min code' type='number' style={{ width: "3rem",marginLeft:".25rem" }}
					min={0} max={999} value={minCode} onChange={e => setMinCode(parseInt(e.target.value))} />
				{"<->"}
				<input title='max code' type='number' style={{ width: "3rem" }}
					min={0} max={999} value={maxCode} onChange={e => setMaxCode(parseInt(e.target.value))} />
			</span>
		</details>
	</span>
}

export default HttpRequestCard;