using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Database.Abstract;

#pragma warning disable CS8618

public abstract class ADbWebRequest : ADbProxiedAction
{
	/* URL запроса. Может содержать встраиваемые переменные, которые будут
	 * при формировании строки получены из контекста пресета.
	 * Может являться IP-адрессом.
	 */
	public string RequestUrl { get; set; } // у сценария будет контекст, откуда Url будет брать перменные для формирования
}
