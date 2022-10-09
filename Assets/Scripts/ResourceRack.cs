using UnityEngine;

//хранит ресурс заданного типа, работает с InStorage
public class ResourceRack
{
	public readonly RESOURCE_TYPE type;
	public readonly int limit;
	public int Count { get; private set; }
	public int FreeSpace => limit - Count;
	public bool IsFilled => Count == limit;
	public bool IsEmpty => Count == 0;

	public ResourceRack (RESOURCE_TYPE type, int limit)
	{
		this.type = type;
		this.limit = limit >= 0 ? limit : 0;
	}

	public int AddResource (int count)
	{
		int wasCount = Count;
		Count = Mathf.Clamp(Count + count, 0, limit);
		return Count - wasCount;
	}
}
