using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Database.Abstract;
using Models.Enums;

namespace Models.Application.Abstract;

public abstract class AActionResult
{
	public Guid DbActionGuid { get; set; }
	public Guid? Next { get; set; }

	public DateTime? Started { get; set; }
	public DateTime? Completed { get; set; }

	public List<(LogType typ, string msg)> Logs { get; } = new();
	public List<(string name, string val)> ContextUpdates { get; } = new();


	public Engine BindAll(Engine e) => BindLogging(BindUpdating(e));

	private void LogInfo(string msg) => Log(LogType.Info, msg);
	private void LogWarn(string msg) => Log(LogType.Warn, msg);
	private void LogError(string msg) => Log(LogType.Error, msg);
	private void Log(int typ, string msg) => Log((LogType)typ, msg);
	private void Log(LogType typ, string msg) => Logs.Add((typ, msg));
	public Engine BindLogging(Engine e)
	{
		e.SetValue("logInfo", this.LogInfo);
		e.SetValue("logWarn", this.LogWarn);
		e.SetValue("logError", this.LogError);
		e.SetValue("log", (int typ, string msg) => this.Log(typ, msg));

		return e;
	}

	private void UpdateVariable(string name, string val) => ContextUpdates.Add((name, val));
	public Engine BindUpdating(Engine e)
	{
		e.SetValue("updateVariable", this.UpdateVariable);
		return e;
	}
}
