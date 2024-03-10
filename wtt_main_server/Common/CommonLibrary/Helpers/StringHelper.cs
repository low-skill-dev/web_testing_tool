using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Helpers;

public static class StringHelper
{
	public static unsafe void RemoveNewLines(string s)
	{
		fixed(char* p = s)
			for(var i = 0; i < s.Length; i++)
				if(p[i] == '\n' || p[i] == '\r')
					p[i] = ' ';
	}
}
