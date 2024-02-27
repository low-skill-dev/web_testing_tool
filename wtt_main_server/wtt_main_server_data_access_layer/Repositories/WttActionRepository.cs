//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using wtt_main_server_api.Database;
//using wtt_main_server_data.Database.Abstract;
//using wtt_main_server_data.Structures;

//namespace wtt_main_server_services;

//public sealed class WttActionRepository
//{
//	private readonly WttContext _context;

//	public WttActionRepository(WttContext context)
//	{
//		this._context = context;
//	}

//	public async Task<ActionsCollection> LoadActions(IEnumerable<Guid> guids)
//	{


//		return new()
//		{
//			DbGetParametersActions =	await _context.GetParametersActions	.Where(x => guids.Contains(x.Guid)).ToListAsync(),
//			DbCertificateActions =		await _context.CertificateActions	.Where(x => guids.Contains(x.Guid)).ToListAsync(),
//			DbConditionalActions =		await _context.ConditionalActions	.Where(x => guids.Contains(x.Guid)).ToListAsync(),
//			DbScenarioActions =			await _context.ScenarioActions		.Where(x => guids.Contains(x.Guid)).ToListAsync(),
//			DbErrorActions =			await _context.ErrorActions			.Where(x => guids.Contains(x.Guid)).ToListAsync(),
//			DbDelayActions =			await _context.DelayActions			.Where(x => guids.Contains(x.Guid)).ToListAsync(),
//			DbEchoActions =				await _context.EchoActions			.Where(x => guids.Contains(x.Guid)).ToListAsync(),
//			DbHttpActions =				await _context.HttpActions			.Where(x => guids.Contains(x.Guid)).ToListAsync(),
//			DbImapActions =				await _context.ImapActions			.Where(x => guids.Contains(x.Guid)).ToListAsync(),
//		};
//	}
//}
