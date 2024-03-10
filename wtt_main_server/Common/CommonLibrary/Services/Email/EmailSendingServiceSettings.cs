namespace ServicesLayer.Models.Runtime;
public class EmailSendingServiceSettings
{
	public string MicroservicePostEndpoint { get; init; } = null!;
	public string MicroserviceGetLimitsEndpoint { get; init; } = null!;
	public string MicroserviceKey { get; init; } = null!;
}
