import ADbObjectWithGuid from "./ObjectWithGuid";

export default interface ADbObjectWithRelatedUser extends ADbObjectWithGuid
{
	UserGuid: string | null;
}