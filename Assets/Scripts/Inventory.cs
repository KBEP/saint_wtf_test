using UnityEngine;

public class Inventory
{
	public int Limit { get; private set; }
	int[] resCount;

	public Inventory (int limit)
	{
		resCount = new int[ResourceType.Count];
		Limit = limit >= 0 ? limit : 0;
	}

	public int AddResource (RESOURCE_TYPE type, int count)
	{
		int idx = (int)type;
		int wasCount = resCount[idx];
		resCount[idx] = Mathf.Clamp(wasCount + count, 0, Limit);
		return resCount[idx] - wasCount;
	}

	public int CountOf (RESOURCE_TYPE type)
	{
		return resCount[(int)type];
	}
}
