import IJwtInfo from "../../Common/IJwtInfo";
import UserRoles from "./UserRoles";

export default interface IDbUserPublicInfo extends IJwtInfo
{
    Guid: string;
    Role: UserRoles;
    IsDisabled: boolean;
    RegistrationDate: string;

    Email: string;
    EmailConfirmedAtUtc: string | null;

    PasswordLastChanged: string;
}