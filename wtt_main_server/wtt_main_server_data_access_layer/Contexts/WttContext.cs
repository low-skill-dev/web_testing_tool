//using Microsoft.EntityFrameworkCore;
//using System;
////using wtt_main_server_data.Database.Common;
//using wtt_main_server_data.Database.Infrastructure;
//using wtt_main_server_data.Database.Networking;
//using wtt_main_server_data.Database.TestScenarios;

//namespace wtt_main_server_api.Database;

//public class WttContext : DbContext
//{
//	public WttContext(DbContextOptions<WttContext> options)
//		: base(options)
//	{

//	}


//	protected override void OnModelCreating(ModelBuilder modelBuilder)
//	{
//		base.OnModelCreating(modelBuilder);

//		var types = new[]
//		{
//				typeof(DbUser),
//				typeof(DbJwtIdentifier),
//				typeof(DbEmailSendLog),
//				typeof(DbProxy),
//				typeof(DbUserProxy),
//				typeof(DbImapAccount),
//				typeof(DbUserImapAccount),
//				typeof(DbTestScenario),
//				typeof(DbImapAction),
//				typeof(DbHttpAction),
//				typeof(DbEchoAction),
//				typeof(DbDelayAction),
//				typeof(DbConditionalAction),
//				typeof(DbErrorAction),
//		};

//		/* A key cannot be configured on 'XClass' because it is a derived type.
//		 * Means that it was configured for the upper (in deriving hierarchy) type.
//		 * No need to configure it again.
//		 */
//		modelBuilder.Entity<ObjectWithGuid>(e => { e.HasKey(x => x.Id); e.HasAlternateKey(x => x.Guid); });
//		//modelBuilder.Entity<DbUser>(e => { /*e.HasKey(x => x.Id); e.HasAlternateKey(x => x.Guid); */});
//		//modelBuilder.Entity<DbJwtIdentifier>(e => { /*e.HasKey(x => x.Id);*/ e.HasAlternateKey(x => x.Guid); });
//		//modelBuilder.Entity<DbEmailSendLog>(e => { /*e.HasKey(x => x.Id);*/ e.HasAlternateKey(x => x.Guid); });
//		//modelBuilder.Entity<DbUserProxy>(e => { /*e.HasKey(x => x.Id);*/ e.HasAlternateKey(x => x.Guid); });
//		//modelBuilder.Entity<DbProxy>(e => { /*e.HasKey(x => x.Id); e.HasAlternateKey(x => x.Guid); } */});
//		//modelBuilder.Entity<DbImapAccount>(e => { /*e.HasKey(x => x.Id);*/ e.HasAlternateKey(x => x.Guid); });
//		//modelBuilder.Entity<DbUserImapAccount>(e => { /*e.HasKey(x => x.Id); e.HasAlternateKey(x => x.Guid); }*/});
//		//modelBuilder.Entity<DbTestScenario>(e => { /*e.HasKey(x => x.Id);*/ e.HasAlternateKey(x => x.Guid); });
//		////modelBuilder.Entity<DbImapAction>(e => { /*e.HasKey(x => x.Id);*/ e.HasAlternateKey(x => x.Guid); });
//		////modelBuilder.Entity<DbHttpAction>(e => { /*e.HasKey(x => x.Id);*/ e.HasAlternateKey(x => x.Guid); });
//		////modelBuilder.Entity<DbEchoAction>(e => { /*e.HasKey(x => x.Id);*/ e.HasAlternateKey(x => x.Guid); });
//		////modelBuilder.Entity<DbDelayAction>(e => { /*e.HasKey(x => x.Id);*/ e.HasAlternateKey(x => x.Guid); });
//		////modelBuilder.Entity<DbConditionalAction>(e => { /*e.HasKey(x => x.Id);*/ e.HasAlternateKey(x => x.Guid); });
//		////modelBuilder.Entity<DbErrorAction>(e => { /*e.HasKey(x => x.Id);*/ e.HasAlternateKey(x => x.Guid); });

//		//modelBuilder.Entity<DbUser>(e => { e.HasAlternateKey(x => x.Email); });

//		var hashMembers = typeof(DbTestScenario).GetFields().Select(x => x.Name);
//		var hashExclude = new string[] 
//		{ 
//			nameof(DbTestScenario.Sha512), nameof(DbTestScenario.Id), 
//			nameof(DbTestScenario.Guid), nameof(DbTestScenario.UserGuid) 
//		};
//		modelBuilder.Entity<DbTestScenario>(e =>
//		{
//			e.Property(x => x.Sha512).HasComputedColumnSql<byte[]>($"sha512(CONCAT({string.Join(',', hashMembers.Except(hashExclude))}))");
//		});
//	}

//	public DbSet<DbUser> Users { get; protected set; }
//	public DbSet<DbJwtIdentifier> RefreshJtis { get; protected set; }
//	public DbSet<DbJwtIdentifier> RecoveryJtis { get; protected set; }

//	public DbSet<DbEmailSendLog> EmailSendLogs { get; protected set; }

//	public DbSet<DbProxy> Proxies { get; protected set; }
//	public DbSet<DbUserProxy> UserProxies { get; protected set; }

//	public DbSet<DbImapAccount> ImapAccounts { get; protected set; }
//	public DbSet<DbUserImapAccount> UserImapAccounts { get; protected set; }


//	public DbSet<DbTestScenario> TestScenarios { get; protected set; }
//	//public DbSet<DbImapAction> ImapActions { get; protected set; }
//	//public DbSet<DbHttpAction> HttpActions { get; protected set; }
//	//public DbSet<DbEchoAction> EchoActions { get; protected set; }
//	//public DbSet<DbDelayAction> DelayActions { get; protected set; }
//	//public DbSet<DbConditionalAction> ConditionalActions { get; protected set; }
//	//public DbSet<DbErrorAction> ErrorActions { get; protected set; }
//	//public DbSet<DbCertificateAction> CertificateActions { get; protected set; }
//	//public DbSet<DbScenarioAction> ScenarioActions { get; protected set; }
//	//public DbSet<DbGetParametersAction> GetParametersActions { get;protected set; }
//}
