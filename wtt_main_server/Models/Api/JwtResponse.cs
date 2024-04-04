using Reinforced.Typings.Attributes;

namespace Models.Api.Auth.Responses;

#pragma warning disable CS8618

//[TsInterface]
public class JwtResponse
{
    public string Access { get; set; }
    public string Refresh { get; set; }
}
