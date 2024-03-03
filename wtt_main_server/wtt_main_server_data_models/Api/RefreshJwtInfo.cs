namespace wtt_main_server_data.Api;

public class RefreshJwtInfo
{
	public required Guid UserGuid { get; init; }
	public required byte[] Jti { get; init; }
}
