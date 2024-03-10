namespace Models.ServicesSettings;

#pragma warning disable CS8618

public class JwtServiceSettings
{
	public int AccessTokenLifespanSeconds { get; set; } = 60 * 5; // 5 minutes
	public int RefreshTokenLifespanSeconds { get; set; } = 60 * (60 * 24 * 180); // 180 days
	public int RecoveryTokenLifespanSeconds { get; set; } = 60 * (60 * 24 * 7); // 7 days

	public string RecoveryEmailSendFromAddress { get; set; } // i.e. 'no-reply@vdb.lowskill.dev'
	public string RecoveryEndpointPath { get; set; } // i.e. '/api/v1/auth/recovery'
}
