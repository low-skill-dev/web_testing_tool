﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Database.Abstract;

namespace wtt_main_server_data.Database.TestScenarios;

#pragma warning disable CS8618

public class DbDelayAction : ADbAction
{
	public override string Type => "Delay";

	public int DelayMs { get; set; }
}
