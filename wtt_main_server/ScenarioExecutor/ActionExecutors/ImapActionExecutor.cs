using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Models.Application.Abstract;
using Models.Application.TestScenarios.ActionResults;
using Models.Database.Abstract;
using Models.Database.Common;
using Models.Database.TestScenarios;
using ScenarioExecutor.Interfaces;
using Jint;
using System.Text;
using Models.Application.Abstract;
using Models.Application.TestScenarios.ActionResults;
using Models.Database.Abstract;
using Models.Database.Common;
using Models.Database.TestScenarios;
using Models.Database.TestExecutors;
using ScenarioExecutor.Helpers;
using ScenarioExecutor.Interfaces;
using static ScenarioExecutor.Helpers.ContextHelper;
using System.Diagnostics;
using static System.Text.Json.JsonSerializer;
using System;
using Models.Constants;
using CommonLibrary.Helpers;
using Jint.Runtime;
using MailKit.Net.Imap;
using static ScenarioExecutor.Helpers.StringExtensions;
using MailKit.Search;
using System.Text.RegularExpressions;
using MimeKit;
using Org.BouncyCastle.Ocsp;
using System.Xml;


namespace ScenarioExecutor.ActionExecutors;

public sealed class ImapActionExecutor : AActionExecutor<DbImapAction, ImapActionResult>
{
	public ImapActionExecutor(DbImapAction action) : base(action) { }

	public override async Task<Dictionary<string, string>> Execute(IDictionary<string, string> currentContext)
	{
		base.Start();

		if(HasKnownImapPassword(Action.ImapUsername, out var pass))
			Action.ImapPassword = pass;

		var ret = new Dictionary<string, string>(1);
		var found = await MakeRequest(currentContext);

		if(string.IsNullOrWhiteSpace(found))
		{
			Result.IsError = true;
			Result.FoundValue = null;
		}
		else
		{
			Result.FoundValue = found;
			if(!string.IsNullOrWhiteSpace(Action.WriteResultToVariable))
				ret.Add(Action.WriteResultToVariable, found);
		}

		base.Complete();
		return ret;

	}

	private async Task<string?> MakeRequest(IDictionary<string, string> currentContext)
	{
		var addr = CreateStringFromContext(Action.ImapAddress!, currentContext);
		var user = CreateStringFromContext(Action.ImapUsername!, currentContext);
		var pass = CreateStringFromContext(Action.ImapPassword!, currentContext);
		var port = int.Parse(CreateStringFromContext(Action.ImapPort!, currentContext));

		if(IsNullOrWhiteSpaceAny(addr, user, pass) || port < 1) return null;

		using var client = new ImapClient();
		_cpuTimeCounter.Stop();
		await client.ConnectAsync(addr, port, MailKit.Security.SecureSocketOptions.Auto);
		await client.AuthenticateAsync(user, pass);
		_cpuTimeCounter.Start();

		if(!client.IsAuthenticated) return null;

		var inbox = client.Inbox;
		_cpuTimeCounter.Stop();
		await inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);
		_cpuTimeCounter.Start();

		var q = new MailKit.Search.SearchQuery();

		if(!string.IsNullOrWhiteSpace(Action.SubjectMustContain))
			q.And(SearchQuery.SubjectContains(CreateStringFromContext(Action.SubjectMustContain, currentContext)));
		if(!string.IsNullOrWhiteSpace(Action.BodyMustContain))
			q.And(SearchQuery.BodyContains(CreateStringFromContext(Action.BodyMustContain, currentContext)));
		if(!string.IsNullOrWhiteSpace(Action.SenderMustContain))
			q.And(SearchQuery.FromContains(CreateStringFromContext(Action.SenderMustContain, currentContext)));

		q.And(SearchQuery.NotSeen);

		_cpuTimeCounter.Stop();
		var messages = client.Inbox.Supports(MailKit.FolderFeature.Sorting)
			? await inbox.SortAsync(q, [OrderBy.ReverseArrival])
			: await inbox.SearchAsync(q);
		_cpuTimeCounter.Start();

		string? found = null;
		foreach(var uid in messages)
		{
			var text = (await inbox.GetMessageAsync(uid)).GetTextBody(MimeKit.Text.TextFormat.Plain);
			if(string.IsNullOrWhiteSpace(text)) continue;

			var match = Regex.Match(text, Action.BodySearchRegex!);
			if(match.Success)
			{
				found = match.Value;
				break;
			}
		}

		return found;
	}

	/// <summary>
	/// email address to it's imap password
	/// </summary>
	public static Dictionary<string, string> KnownImapPasswords { get; set; } = new()
	{
		{ @"wtt_service_box@mail.ru", @"HF7Rfbvte11e9iXaYVQs" },
	};

	public static bool HasKnownImapPassword(string emailAddress, out string? password)
	{
		return KnownImapPasswords.TryGetValue(emailAddress, out password);
	}
}
