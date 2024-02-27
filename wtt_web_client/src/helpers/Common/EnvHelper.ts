export default class EnvHelper
{
    static get usePredefinedHost() { return process.env.REACT_APP_Use_Predefined_Host?.toLowerCase() === "true"; }
    static get isDevelopment() { return process.env.REACT_APP_Environment?.toLowerCase() === "development"; }
    static get isProduction() { return process.env.REACT_APP_Environment?.toLowerCase() === "production"; }
    static get isDebugMode() { return process.env.REACT_APP_DebugMode?.toLowerCase() === "true"; }
}