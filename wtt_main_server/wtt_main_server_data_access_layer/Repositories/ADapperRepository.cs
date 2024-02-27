//using Dapper;
//using Npgsql;
//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace wtt_main_server_data_access_layer.Repositories;
//public abstract class ADapperRepository
//{
//	public required string DatabaseName { get; init; }
//	public required string ConnectionString { get; init; }

//	private NpgsqlConnection _connection;
//	public NpgsqlConnection Connection
//	{
//		get
//		{
//			if(_connection is null) 
//				_connection = new(ConnectionString);

//			return _connection;
//		}
//	}

//	public ADapperRepository(string connectionString, string? databaseName = null)
//	{
//		this.DatabaseName = databaseName ?? connectionString
//			.Split(';').Select(x => new string(x.Where(c => !char.IsWhiteSpace(c)).ToArray()))
//			.Select(x => x.ToLowerInvariant()).FirstOrDefault(x => x.StartsWith("database="))?
//			.Split('=').Last() ?? throw new ArgumentNullException(nameof(databaseName));

//		this.ConnectionString = connectionString;
//	}

//	#region Database methods

//	public async Task<bool> IsDatabaseExists()
//	{
//		const string sql = "SELECT count(*) FROM pg_database WHERE datname = @db";

//		await using var conn = new NpgsqlConnection(this.ConnectionString);
//		var result = await conn.ExecuteScalarAsync<int>(sql, new { db = conn.Database });

//		return result > 0;
//	}

//	public async Task CreateDatabase()
//	{
//		const string sql = "CREATE DATABASE @db";

//		await using var conn = new NpgsqlConnection(this.ConnectionString);
//		await conn.ExecuteAsync(sql, new { db = conn.Database });
//	}
//	public async Task DropDatabase()
//	{
//		await using var conn = new NpgsqlConnection(this.ConnectionString);
//		await conn.ExecuteAsync(_requestTemplates.DropDatabase());
//	}


//	public async Task EnsureDatabaseCreated()
//	{
//		if(!(await IsDatabaseExists())) await CreateDatabase();
//	}
//	public async Task EnsureDatabaseDropped()
//	{
//		if(await IsDatabaseExists()) await DropDatabase();
//	}
//	public async Task EnsureDatabaseRecreated()
//	{
//		await EnsureDatabaseDropped();
//		await CreateDatabase();
//	}
//	#endregion

//	#region Tables methods
//	public async Task<DbDataReader> GetAllTables()
//	{
//		await using var conn = new NpgsqlConnection(this.ConnectionString);
//		var result = await conn.ExecuteReaderAsync(_requestTemplates.GetTables());

//		return result;
//	}
//	public async Task<bool> IsTableExists(string name)
//	{
//		await using var conn = new NpgsqlConnection(this.ConnectionString);
//		await using var result = await conn.ExecuteReaderAsync(_requestTemplates.GetTables(name));

//		return result.HasRows;
//	}
//	public async Task CreateTable(string tableName, Dictionary<string, string> columnToType)
//	{
//		await using var conn = new NpgsqlConnection(this.ConnectionString);
//		await conn.ExecuteAsync(_requestTemplates.CreateTable(tableName, columnToType));
//	}
//	public async Task DropTable(string tableName)
//	{
//		await using var conn = new NpgsqlConnection(this.ConnectionString);
//		await conn.ExecuteAsync(_requestTemplates.DropTable(tableName));
//	}
//	#endregion

//	#region Insert methods
//	public async Task InsertIntoTable(string tableName, IEnumerable<IEnumerable<object>> rows)
//	{
//		await using var conn = new NpgsqlConnection(this.ConnectionString);
//		await conn.ExecuteAsync(_requestTemplates.InsertValues(tableName, rows));
//	}
//	#endregion
//}
