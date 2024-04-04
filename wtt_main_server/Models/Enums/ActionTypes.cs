using Reinforced.Typings.Attributes;

namespace Models.Enums;

[TsEnum(IncludeNamespace = false)]
public enum ActionTypes
{
	DbCertificateActionType,
	DbConditionalActionType,
	DbDelayActionType,
	DbEchoActionType,
	DbErrorActionType,
	DbGetParametersActionType,
	DbHttpActionType,
	DbImapActionType,
	DbScenarioActionType,
	DbTestScenarioType,
}
