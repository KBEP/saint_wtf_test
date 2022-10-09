public readonly struct Notice
{
	public readonly string buildingName;
	public readonly NOTICE_TYPE type;
	public readonly RESOURCE_TYPE resType;

	public Notice (string buildingName, NOTICE_TYPE type, RESOURCE_TYPE resType)
	{
		this.buildingName = buildingName;
		this.type = type;
		this.resType = resType;
	}
}
