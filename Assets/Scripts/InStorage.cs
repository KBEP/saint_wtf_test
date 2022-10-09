using System.Collections.Generic;
using UnityEngine;
using System;

public class InStorage//склад, работающий на приём ресурсов
{
	Dictionary<RESOURCE_TYPE, ResourceRack> racks = new Dictionary<RESOURCE_TYPE, ResourceRack>();
	
	public event Action<RESOURCE_TYPE> Emptied = (RESOURCE_TYPE t) => {};//склад опустел
	public event Action<RESOURCE_TYPE> Filled = (RESOURCE_TYPE t) => {};//склад снова готов принимать ресурсы
	
	public InStorage (RESOURCE_TYPE[] type, int limit)
	{
		if (type == null || type.Length < 1) throw new ArgumentException("type");
		foreach (var t in type) if (!racks.ContainsKey(t)) racks.Add(t, new ResourceRack(t, limit));
	}

	public int ConsumeResources (int count)
	{
		if (count < 0) return 0;
		count = Mathf.Min(count, CanProduce());
		int result = 0;
		foreach (var rack in racks)
		{
			bool wasEmpty = rack.Value.IsEmpty;
			result = rack.Value.AddResource(-count);
			if (wasEmpty)
			{
				if (!rack.Value.IsEmpty) Filled(rack.Value.type);
			}
			else
			{
				if (rack.Value.IsEmpty) Emptied(rack.Value.type);
			}
		}
		return result;
	}

	public int AddResource (RESOURCE_TYPE type, int count)
	{
		if (racks.TryGetValue(type, out ResourceRack rack))
		{
			bool wasEmpty = rack.IsEmpty;
			int result = rack.AddResource(count);
			if (wasEmpty)
			{
				if (!rack.IsEmpty) Filled(type);
			}
			else
			{
				if (rack.IsEmpty) Emptied(type);
			}
			return result;
		}
		else return 0;
	}

	public int CanProduce ()//сколько максимум можно прозвести ресурса, исходя из количества требуемых ресурсов
	{
		int min = int.MaxValue;
		foreach (var r in racks) if (r.Value.Count < min) min = r.Value.Count;
		return min;
	}

	public Dictionary<RESOURCE_TYPE, ResourceRack>.KeyCollection GetAllResTypes () => racks.Keys;

	public bool HasFreeSpace (RESOURCE_TYPE type) =>
	  racks.TryGetValue(type, out ResourceRack rack) && rack.FreeSpace > 0;

	public int CountOf (RESOURCE_TYPE type)
	{
		return racks.TryGetValue(type, out ResourceRack rack) ? rack.Count : 0;
	}
}
