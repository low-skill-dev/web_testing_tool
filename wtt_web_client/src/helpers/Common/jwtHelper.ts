import IJwtInfo from '../../models/Common/IJwtInfo';

export default class JwtHelper 
{
	public static GetLifetime = (decodedJwt: IJwtInfo) => 
	{
		let nbfD = new Date((decodedJwt.nbf - 1) * 1000);
		let expD = new Date(decodedJwt.exp * 1000);
		return { nbf: nbfD, exp: expD };
	}

	public static ValidateLifetime = (decodedJwt: IJwtInfo) => 
	{
		var vals = this.GetLifetime(decodedJwt);
		return this.ValidateParsedLifetime(vals.nbf, vals.exp);
	}

	public static ValidateParsedLifetime = (nbf: Date, exp: Date) => 
	{
		let now = new Date(Date.now());
		return (exp < now) && (now < nbf);
	}
}
