import IJwtInfo from "../../Common/IJwtInfo";
import UserRoles from "./UserRoles";

export default interface IDbUserPublicInfo extends IJwtInfo
{
    guid: string;
    role: UserRoles;
    isDisabled: boolean;
    registrationDate: string;

    email: string;
    emailConfirmedAtUtc: string | null;

    passwordLastChanged: string;
}