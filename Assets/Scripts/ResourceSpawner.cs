using UnityEngine;
using System;

public static class ResourceSpawner
{
	static string[] path;
	static string[] name;

	static ResourceSpawner ()
	{
		path = new string[ResourceType.Count];
		name = new string[ResourceType.Count];
		foreach (var e in Enum.GetValues(typeof(RESOURCE_TYPE)))
		{
			int idx = (int)e;
			path[idx] = "Prefabs/Resource" + idx;
			name[idx] = "Resource" + idx;
		}
	}
	
	public static ResourceController Spawn (RESOURCE_TYPE type, Vector3 position)
	{
		int idx = (int)type;
		Transform t = Instancer.Instantiate(path[idx], name[idx], position, null);
		if (t == null) return null;//error
		ResourceController resCtr = t.GetComponent<ResourceController>();
		if (resCtr == null)
		{
			UnityEngine.Object.Destroy(t.gameObject);
			return null;//error
		}
		resCtr.SetData(type);
		return resCtr;
	}
}
