import ADbObjectWithGuid from "./ADbObjectWithGuid";

export default interface ADbObjectWithRelatedUser extends ADbObjectWithGuid
{
	relatedUserGuid: string | null;
}