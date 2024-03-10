using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Database.Abstract;

namespace Models.Database.TestScenarios;

#pragma warning disable CS8618

public class DbEchoAction : ADbWebRequest
{
	public override string Type => "Echo";
}
