using CommonLibrary.Models;
using Models.Api;
using Models.Database.Abstract;
using Models.Database.TestScenarios;
using Models.Enums;
using Reinforced.Typings.Attributes;
using Reinforced.Typings.Fluent;
using System.Diagnostics;
using System.Reflection;
using Reinforced.Typings.Ast;
using Models.Api.Auth.Responses;
using Reinforced.Typings.Ast.TypeNames;
using Models.Structures;
using Models.Database.RunningScenarios;

[assembly: TsGlobal(UseModules = true, DiscardNamespacesWhenUsingModules = true, AutoOptionalProperties = true)]

namespace Models.TsConfig;
public class Configuration
{
	//public static string file = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "111.txt");

	public static void Configure(ConfigurationBuilder builder)
	{
		builder.SubstituteGeneric(typeof(Dictionary<string, string>), (t, tr) =>
		{
			var args = t.GetGenericArguments();
			return new RtSimpleTypeName($"Map<{tr.ResolveTypeName(args[0])},{tr.ResolveTypeName(args[1])}>");
		});

		builder.ExportAsEnum<ActionTypes>().Order(10);
		builder.ExportAsEnum<UserRoles>().Order(10);
		builder.ExportAsEnum<HttpRequestMethod>().Order(10);
		builder.ExportAsEnum<HttpTlsValidationMode>().Order(10);

		builder.ExportAsClass<ObjectWithDates>().WithPublicProperties().Order(100);
		builder.ExportAsClass<ObjectWithGuid>().WithPublicProperties().Order(200);
		builder.ExportAsClass<ObjectWithUser>().WithPublicProperties().Order(300);

		builder.ExportAsClass<ADbAction>().Abstract().WithPublicProperties()
			.WithProperty(x => x.Type, c => c.ForceNullable())
			.WithProperty(x => x.Name, c => c.InitializeWith((_, _, _) => "'Action'"))
			//.WithProperty(x => x.Next, null)
			.WithProperty(x => x.ColumnId, c => c.InitializeWith((_, _, _) => "0"))
			.WithProperty(x => x.RowId, c => c.InitializeWith((_, _, _) => "0"))
			.WithProperty(x => x.Bypass, c => c.InitializeWith((_, _, _) => "false"))
			.WithProperty(x => x.ContinueExecutionInCaseOfCriticalError, c => c.InitializeWith((_, _, _) => "false"))
			//.WithProperty(x => x.AfterRunScript!, null)
			//.WithProperty(x => x.ScriptInTryBlock, c => c.InitializeWith((_, _, _) => "false"))
			.Order(400);

		builder.ExportAsClass<ADbProxiedAction>().Abstract().WithPublicProperties()
			.Order(500);

		builder.ExportAsClass<ADbWebRequest>().Abstract().WithPublicProperties()
			.WithProperty<string>(x => x.RequestUrl, c => c.InitializeWith((_, _, _) => "''")
			.Order(600));

		builder.ExportAsClass<ADbHttpAction>().Abstract().WithPublicProperties()
			.WithConstructor(new(
				$"super();\n" +
				$"this.{nameof(ADbHttpAction.Method)} = {nameof(HttpRequestMethod)}.{Enum.GetName(typeof(HttpRequestMethod), (int)(HttpRequestMethod.Get))};\n" +
				$"this.{nameof(ADbHttpAction.TlsValidationMode)} = {nameof(HttpTlsValidationMode)}.{Enum.GetName(typeof(HttpTlsValidationMode), 0)}"))
			.Order(700);

		var actionTypes = Assembly.GetAssembly(typeof(ADbAction))!.GetTypes()
			.Where(x => x.Name.StartsWith("Db") && x.Name.EndsWith("Action"))
			.ToList();

		foreach(var a in actionTypes)
		{
			ExportAction(builder, Activator.CreateInstance(a) as dynamic); // в рантайме!!!
		}

		builder.ExportAsClass<DbConditionalAction>()
			.WithProperty(x => x.JsBoolExpression, c => c.InitializeWith((_, _, _) => "''"));

		builder.ExportAsClass<DbDelayAction>()
			.WithProperty(x => x.DelayMs, c => c.InitializeWith((_, _, _) => "250"));

		builder.ExportAsClass<DbErrorAction>();
			//.WithProperty(x => x.Message, c => c.InitializeWith((_, _, _) => "''"))
			//.WithProperty(x => x.StopExecution, c => c.InitializeWith((_, _, _) => "false"));

		builder.ExportAsClass<DbImapAction>();
		//.WithProperty(x => x.MinSearchLength, c => c.InitializeWith((_, _, _) => "4"))
		//.WithProperty(x => x.MaxSearchLength, c => c.InitializeWith((_, _, _) => "8"))
		//.WithProperty(x => x.SearchMustContain, c => c.InitializeWith((_, _, _) => "[]"));

		builder.ExportAsClass<DbScenarioAction>();
			//.WithProperty(x => x.Arguments, c => c.InitializeWith((_, _, _) => "new Map<string, string>()"));

		builder.ExportAsClass<DbTestScenario>().WithPublicProperties(x => x.ForceNullable())
			.Order(800);

		builder.ExportAsClass<ActionsCollection>().WithPublicProperties(x => x.ForceNullable())
			.Order(850);

		builder.ExportAsClass<ChangePasswordRequest>().WithPublicProperties()
			.WithProperty(x => x.Password, c => c.InitializeWith((_, _, _) => "''"))
			.Order(900);

		builder.ExportAsClass<RegistrationRequest>().WithPublicProperties()
			.WithProperty(x => x.Email, c => c.InitializeWith((_, _, _) => "''"))
			.Order(1000);

		builder.ExportAsClass<LoginRequest>().WithPublicProperties()
			.Order(1100);

		builder.ExportAsInterface<JwtResponse>().WithPublicProperties()
			.Order(1200);

		builder.ExportAsInterface<DbUserPublicInfo>().WithPublicProperties()
			.Order(1300);

		builder.ExportAsInterface<DbScenarioRun>().WithPublicProperties()
			.Order(1400);
	}

	static void ExportAction<T>(ConfigurationBuilder cb, T e)
	{
		var instance = e as dynamic;
		var enumValueName = Enum.GetName(typeof(ActionTypes), instance!.Type);

		cb.ExportAsClass<T>().WithPublicProperties()
			.WithConstructor(new($"super();\nthis.{nameof(ADbHttpAction.Type)}" +
			$" = {nameof(ActionTypes)}.{enumValueName};"))
			.Order(800);
	}
}