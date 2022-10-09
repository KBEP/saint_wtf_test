using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
	//только для инициализации
	public int resourceLimit = 5;
	//
	
	InventoryStack[] stacks;
	Inventory inv;
	
	void Awake ()
	{
		stacks = new InventoryStack[ResourceType.Count];
		inv = new Inventory(resourceLimit);
		for (int i = 0; i < stacks.Length; ++i)
		{
			Transform t = transform.Find("ResStack" + i);
			if (t == null)
			{
				Debug.Log("ERROR");
				continue;
			}
			stacks[i] = t.GetComponent<InventoryStack>();
			if (stacks[i] == null) Debug.Log("ERROR");
		}
	}

	public bool HasFreeSpace (RESOURCE_TYPE type) => inv.CountOf(type) < inv.Limit;

	public bool HasRes (RESOURCE_TYPE type) => inv.CountOf(type) > 0;

	public bool AddResource (ResourceController resCtr)
	{
		if (resCtr == null || inv.CountOf(resCtr.Type) >= inv.Limit) return false;
		int idx = (int)resCtr.Type;
		if (stacks[idx] == null) return false;
		if (inv.AddResource(resCtr.Type, 1) != 1) return false;
		else return stacks[idx].Add(resCtr);
	}
	
	public ResourceController RemoveResource (RESOURCE_TYPE type)
	{
		if (inv.CountOf(type) <= 0) return null;
		int idx = (int)type;
		if (stacks[idx] == null) return null;
		if (inv.AddResource(type, -1) != -1) return null;
		return stacks[idx].Remove();
	}
}
