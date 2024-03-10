using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Models.Database.Abstract;
using Models.Database.TestScenarios;

namespace Models.Structures;

public sealed class ActionsCollection : IEnumerable<ADbAction>
{
	#pragma warning disable format

	public List<DbGetParametersAction>	DbGetParametersActions	{ get; init; }
	public List<DbCertificateAction>	DbCertificateActions	{ get; init; }
	public List<DbConditionalAction>	DbConditionalActions	{ get; init; }
	public List<DbScenarioAction>		DbScenarioActions		{ get; init; }
	public List<DbDelayAction>			DbDelayActions			{ get; init; }
	public List<DbErrorAction>			DbErrorActions			{ get; init; }
	public List<DbEchoAction>			DbEchoActions			{ get; init; }
	public List<DbHttpAction>			DbHttpActions			{ get; init; }
	public List<DbImapAction>			DbImapActions			{ get; init; }

	#pragma warning restore format

	public ActionsCollection()
	{
		DbGetParametersActions = new();
		DbCertificateActions = new();
		DbConditionalActions = new();
		DbScenarioActions = new();
		DbDelayActions = new();
		DbErrorActions = new();
		DbEchoActions = new();
		DbHttpActions = new();
		DbImapActions = new();
	}

	public ActionsCollection(IEnumerable<ADbAction> source) : this()
	{
		foreach(var a in source)
		{
			if(a is DbImapAction _DbImapAction) DbImapActions.Add(_DbImapAction);
			else if(a is DbHttpAction _DbHttpAction) DbHttpActions.Add(_DbHttpAction);
			else if(a is DbEchoAction _DbEchoAction) DbEchoActions.Add(_DbEchoAction);
			else if(a is DbErrorAction _DbErrorAction) DbErrorActions.Add(_DbErrorAction);
			else if(a is DbDelayAction _DbDelayAction) DbDelayActions.Add(_DbDelayAction);
			else if(a is DbScenarioAction _DbScenarioAction) DbScenarioActions.Add(_DbScenarioAction);
			else if(a is DbConditionalAction _DbConditionalAction) DbConditionalActions.Add(_DbConditionalAction);
			else if(a is DbCertificateAction _DbCertificateAction) DbCertificateActions.Add(_DbCertificateAction);
			else if(a is DbGetParametersAction _DbGetParametersAction) DbGetParametersActions.Add(_DbGetParametersAction);
			else throw new NotImplementedException(a.GetType().ToString());
		}
	}


	public IEnumerator<ADbAction> GetEnumerator()
	{
		var concated = this.GetType().GetFields().Where(x => x.Name.StartsWith("Db") && x.Name.EndsWith("Actions"))
			.Select(x => x.GetValue(this)).SelectMany(x => ((IEnumerable)x!).Cast<ADbAction>());

		foreach(var el in concated) yield return el;
	}

	IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}