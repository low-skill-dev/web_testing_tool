import endpoints from "../../config/endpoints.json"
import EnvHelper from '../Common/EnvHelper';
import { urlJoin } from 'url-join-ts';

/**
 * This class generates URLs for predefined endpoints.
 */
export default class UrlHelper
{
    static get HostUrl()
    {
        return EnvHelper.usePredefinedHost ? endpoints.host
            : window.location.protocol + '//' + window.location.host;
    }
    static Backend = class
    {
        static V1 = class
        {
            static get BasePath() { return endpoints.backend.v1.basePath; }
            static Auth = class
            {
                static get BasePath() { return endpoints.backend.v1.authController.basePath; }
                static get FullPath()
                {
                    return urlJoin(
                        UrlHelper.HostUrl,
                        UrlHelper.Backend.V1.BasePath,
                        UrlHelper.Backend.V1.Auth.BasePath);
                }
                static Get = class
                {
                    static get ValidateToken()
                    {
                        return urlJoin(
                            UrlHelper.Backend.V1.Auth.FullPath,
                            endpoints.backend.v1.authController.get.ValidateToken);
                    }
                    static get GetMySessions()
                    {
                        return urlJoin(
                            UrlHelper.Backend.V1.Auth.FullPath,
                            endpoints.backend.v1.authController.get.GetMySessions);
                    }
                }
                static Post = class
                {
                    static get Login()
                    {
                        return urlJoin(
                            UrlHelper.Backend.V1.Auth.FullPath,
                            endpoints.backend.v1.authController.post.Login);
                    }
                    static get ResetPassword()
                    {
                        return urlJoin(
                            UrlHelper.Backend.V1.Auth.FullPath,
                            endpoints.backend.v1.authController.post.ResetPassword);
                    }
                }
                static Put = class
                {
                    static get Register()
                    {
                        return urlJoin(
                            UrlHelper.Backend.V1.Auth.FullPath,
                            endpoints.backend.v1.authController.put.Register);
                    }
                    static get CreateAndSendLink()
                    {
                        return urlJoin(
                            UrlHelper.Backend.V1.Auth.FullPath,
                            endpoints.backend.v1.authController.put.CreateAndSendLink);
                    }
                }
                static Patch = class
                {
                    static get Refresh()
                    {
                        return urlJoin(
                            UrlHelper.Backend.V1.Auth.FullPath,
                            endpoints.backend.v1.authController.patch.Refresh);
                    }
                    static get ChangePassword()
                    {
                        return urlJoin(
                            UrlHelper.Backend.V1.Auth.FullPath,
                            endpoints.backend.v1.authController.patch.ChangePassword);
                    }
                }
                static Delete = class
                {
                    static get TerminateSession()
                    {
                        return urlJoin(
                            UrlHelper.Backend.V1.Auth.FullPath,
                            endpoints.backend.v1.authController.delete.TerminateSession);
                    }
                }
            }
            static Scenario = class
            {
                static get BasePath() { return endpoints.backend.v1.authController.basePath; }
                static get FullPath()
                {
                    return urlJoin(
                        UrlHelper.HostUrl,
                        UrlHelper.Backend.V1.BasePath,
                        UrlHelper.Backend.V1.Scenario.BasePath);
                }
                static Get = class
                {
                    static get GetScenarios()
                    {
                        return urlJoin(
                            UrlHelper.Backend.V1.Scenario.FullPath,
                            endpoints.backend.v1.scenarioController.get.GetScenarios);
                    }
                }
            }
        }
    }
}