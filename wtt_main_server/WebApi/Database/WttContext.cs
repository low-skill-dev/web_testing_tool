using Microsoft.EntityFrameworkCore;
using System;
using Models.Database.Common;
using Models.Database.Infrastructure;
using Models.Database.Networking;
using Models.Database.TestScenarios;
using CommonLibrary.Models;
using System.Text.Json;
using Models.Structures;
using Models.Database.RunningScenarios;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Database;

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
		//modelBuilder.Entity<ObjectWithGuid>().UseTpcMappingStrategy();
		//modelBuilder.Entity<ObjectWithGuid>(e => { e.HasKey(x => x.Guid); });

		modelBuilder.Entity<RefreshJwt>();
		modelBuilder.Entity<RecoveryJwt>();
		modelBuilder.Entity<DbEmailSendLog>();
		modelBuilder.Entity<DbUserProxy>();
		modelBuilder.Entity<DbProxy>();
		modelBuilder.Entity<DbUserImapAccount>();
		modelBuilder.Entity<DbImapAccount>();

		var serialize = (ActionsCollection ac) => JsonSerializer.Serialize(ac);
		var deserialize = (string ac) => JsonSerializer.Deserialize<ActionsCollection>(ac);

		modelBuilder.Entity<DbTestScenario>()
			.Property(e => e.ActionsJson).HasConversion(
				e => serialize(e),
				e => deserialize(e));
	}

	public virtual void CreateTriggers()
	{
		//	var test1 = this.GetType().GetProperties()
		//		.Where(x => x.PropertyType.IsGenericType).ToList();

		//	var test2 = this.GetType().GetProperties()
		//		.Where(x => x.PropertyType.IsGenericType &&
		//			x.PropertyType.GetGenericTypeDefinition()
		//			.IsAssignableTo(typeof(ObjectWithGuid))).ToList();

		//	var test3 = this.GetType().GetProperties()
		//		.Where(x => x.PropertyType.IsGenericType &&
		//			x.PropertyType.GetGenericTypeDefinition()
		//			.IsAssignableFrom(typeof(ObjectWithGuid))).ToList();

		//	var test4 = this.GetType().GetProperties()
		//.Where(x => x.PropertyType.IsGenericType).Select(x=> 
		//	x.PropertyType.GetGenericArguments().First().IsAssignableTo(typeof(ObjectWithGuid))).ToList();


		var TablesWithGuid = this.GetType().GetProperties()
			.Where(x => x.PropertyType.IsGenericType &&
				x.PropertyType.GetGenericArguments().First()
				.IsAssignableTo(typeof(ObjectWithGuid)))
			.Select(x => x.Name);

		var TablesWithDates = this.GetType().GetProperties()
			.Where(x => x.PropertyType.IsGenericType &&
				x.PropertyType.GetGenericArguments().First()
				.IsAssignableTo(typeof(ObjectWithDates)))
			.Select(x => x.Name);

		var DbSetGuidFunc = "DbSetGuidFn";
		var DbSetCreatedFunc = "DbSetCreatedFn";
		var DbSetChangedFunc = "DbSetChangedFn";

		var createFns = $"""
			
			CREATE OR REPLACE FUNCTION {DbSetGuidFunc}()
				RETURNS TRIGGER LANGUAGE PLPGSQL AS $$
			BEGIN
				NEW."{nameof(ObjectWithGuid.Guid)}" = gen_random_uuid();
				RETURN NEW;
			END; $$;
			
			CREATE OR REPLACE FUNCTION {DbSetCreatedFunc}()
				RETURNS TRIGGER LANGUAGE PLPGSQL AS $$
			BEGIN
				NEW."{nameof(ObjectWithDates.Created)}" := (now() at time zone 'utc');
				NEW."{nameof(ObjectWithDates.Changed)}" := NULL;
				RETURN NEW;
			END; $$;

			CREATE OR REPLACE FUNCTION {DbSetChangedFunc}()
				RETURNS TRIGGER LANGUAGE PLPGSQL AS $$
			BEGIN
				NEW."{nameof(ObjectWithDates.Changed)}" := (now() at time zone 'utc');
				RETURN NEW;
			END; $$;
		
		""";

		var TablesWithGuidTrigger = "TablesWithGuidTrigger";
		var TablesWithCreatedTrigger = "TablesWithCreatedTrigger";
		var TablesWithChangedTrigger = "TablesWithChangedTrigger";

		var createGuidTriggers = string.Join('\n', TablesWithGuid.Select(x => $"""
			CREATE OR REPLACE TRIGGER {TablesWithGuidTrigger} AFTER
				INSERT ON "{x}" FOR EACH ROW EXECUTE PROCEDURE {DbSetGuidFunc}();
		"""));

		var createDatesTriggers = string.Join('\n', TablesWithGuid.Select(x => $"""
			CREATE OR REPLACE TRIGGER {TablesWithCreatedTrigger} AFTER
				INSERT ON "{x}" FOR EACH ROW EXECUTE PROCEDURE {DbSetCreatedFunc}();
			CREATE OR REPLACE TRIGGER {TablesWithChangedTrigger} AFTER
				UPDATE ON "{x}" FOR EACH ROW EXECUTE PROCEDURE {DbSetChangedFunc}();
		"""));

		this.Database.ExecuteSqlRaw(string.Join('\n',
			[createFns, createGuidTriggers, createDatesTriggers]));
	}

	public DbSet<DbUser> Users { get; protected set; }
	public DbSet<RefreshJwt> RefreshJwts { get; protected set; }
	public DbSet<RecoveryJwt> RecoveryJwts { get; protected set; }

	public DbSet<DbEmailSendLog> EmailSendLogs { get; protected set; }


	// Отказ от разработки по причине нехватки времени
	//public DbSet<DbProxy> Proxies { get; protected set; }
	//public DbSet<DbUserProxy> UserProxies { get; protected set; }

	// Отказ от разработки по причине нехватки времени
	//public DbSet<DbImapAccount> ImapAccounts { get; protected set; }
	//public DbSet<DbUserImapAccount> UserImapAccounts { get; protected set; }

	public DbSet<DbTestScenario> TestScenarios { get; protected set; }
	public DbSet<DbScenarioRun> ScenarioRuns { get; protected set; }
}