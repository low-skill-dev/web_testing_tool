using Duende.IdentityServer.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using wtt_main_server_data.Database.Common;
using wtt_main_server_data.Database.Infrastructure;
using wtt_main_server_data.Database.Networking;
using wtt_main_server_data.Database.TestScenarios;

namespace wtt_main_server_api.Database;

public class WttContext : DbContext
{
	public WttContext(DbContextOptions<WttContext> options)
		: base(options)
	{

	}


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		/* A key cannot be configured on 'XClass' because it is a derived type.
		 * Means that it was configured for the upper (in deriving hierarchy) type.
		 * No need to configure it again. Moreover, it must be configured there.
		 */
		modelBuilder.Entity<ObjectWithGuid>().UseTpcMappingStrategy();
		modelBuilder.Entity<ObjectWithGuid>(e => { e.HasKey(x => x.Id); e.HasIndex(x => x.Guid).IsUnique(); });

		modelBuilder.Entity<DbJwtIdentifier>();
		modelBuilder.Entity<DbEmailSendLog>();
		modelBuilder.Entity<DbUserProxy>();
		modelBuilder.Entity<DbProxy>();
		modelBuilder.Entity<DbUserImapAccount>();
		modelBuilder.Entity<DbImapAccount>();
		modelBuilder.Entity<DbTestScenario>();
	}

	public virtual void CreateTriggers()
	{
		var hashMembers = typeof(DbTestScenario).GetProperties().Select(x => x.Name);
		var hashExclude = new string[]
		{
			nameof(DbTestScenario.Guid),
			nameof(DbTestScenario.Sha512), nameof(DbTestScenario.UserGuid)
		};
		var result = this.Database.ExecuteSqlRaw($"""
			CREATE OR REPLACE FUNCTION DbTestScenarioHashTriggerFunction()
				RETURNS TRIGGER LANGUAGE PLPGSQL AS $$
			BEGIN
				NEW.{nameof(DbTestScenario.Sha512)} := sha512(concat({string.Join(',', hashMembers.Except(hashExclude))}));
				NEW.{nameof(DbTestScenario.ChangeDate)} := (now() at time zone 'utc');
				RETURN NEW;
			END; $$;

			CREATE OR REPLACE TRIGGER DbTestScenarioHashTrigger AFTER
				INSERT OR UPDATE ON "{nameof(this.TestScenarios)}"
				FOR EACH ROW EXECUTE PROCEDURE DbTestScenarioHashTriggerFunction();
			""");
	}

	public DbSet<DbUser> Users { get; protected set; }
	public DbSet<DbJwtIdentifier> RefreshJtis { get; protected set; }
	public DbSet<DbJwtIdentifier> RecoveryJtis { get; protected set; }

	public DbSet<DbEmailSendLog> EmailSendLogs { get; protected set; }

	public DbSet<DbProxy> Proxies { get; protected set; }
	public DbSet<DbUserProxy> UserProxies { get; protected set; }

	public DbSet<DbImapAccount> ImapAccounts { get; protected set; }
	public DbSet<DbUserImapAccount> UserImapAccounts { get; protected set; }


	public DbSet<DbTestScenario> TestScenarios { get; protected set; }
}
