using UnityEngine;

public static class Instancer
{
	public static Transform Instantiate (string prefabPath, string name, Vector3 position,
	                                     Quaternion rotation, Transform parent)
	{
		if (string.IsNullOrEmpty(prefabPath)) return null;
		GameObject prefab = Resources.Load<GameObject>(prefabPath);
		if (prefab == null) return null;
		GameObject go = GameObject.Instantiate<GameObject>(prefab, position, rotation, parent);
		if (go == null) return null;
		if (name != null) go.name = name;
		return go.transform;
	}

	public static Transform Instantiate (string prefabPath, string name, Vector3 position, Transform parent)
	{
		if (string.IsNullOrEmpty(prefabPath)) return null;
		GameObject prefab = Resources.Load<GameObject>(prefabPath);
		if (prefab == null) return null;
		GameObject go = GameObject.Instantiate<GameObject>(prefab, position, prefab.transform.rotation, parent);
		if (go == null) return null;
		if (name != null) go.name = name;
		return go.transform;
	}

	public static Transform Instantiate (string prefabPath)
	{
		if (string.IsNullOrEmpty(prefabPath)) return null;
		GameObject prefab = Resources.Load<GameObject>(prefabPath);
		if (prefab == null) return null;
		GameObject go = GameObject.Instantiate<GameObject>(prefab);
		return (go != null) ? go.transform : null;
	}

	public static Transform Instantiate (string prefabPath, string name)
	{
		if (string.IsNullOrEmpty(prefabPath)) return null;
		GameObject prefab = Resources.Load<GameObject>(prefabPath);
		if (prefab == null) return null;
		GameObject go = GameObject.Instantiate<GameObject>(prefab);
		if (go == null) return null;
		if (name != null) go.name = name;
		return go.transform;
	}

	public static Transform Instantiate (string prefabPath, string name, Transform parent,
	                                     bool worldPositionStays = false)
	{
		if (string.IsNullOrEmpty(prefabPath)) return null;
		GameObject prefab = Resources.Load<GameObject>(prefabPath);
		if (prefab == null) return null;
		GameObject go = GameObject.Instantiate<GameObject>(prefab);
		if (go == null) return null;
		//go.transform.parent = parent;
		go.transform.SetParent(parent, worldPositionStays);
		if (name != null) go.name = name;
		return go.transform;
	}

	public static Transform Instantiate (string prefabPath, Transform parent, bool worldPositionStays = false)
	{
		if (string.IsNullOrEmpty(prefabPath)) return null;
		GameObject prefab = Resources.Load<GameObject>(prefabPath);
		if (prefab == null) return null;
		GameObject go = GameObject.Instantiate<GameObject>(prefab);
		if (go == null) return null;
		//go.transform.parent = parent;
		go.transform.SetParent(parent, worldPositionStays);
		return go.transform;
	}

	public static Transform Instantiate (GameObject prefab, string name, Transform parent,
	                                     bool worldPositionStays = false)
	{
		if (prefab == null) return null;
		GameObject go = GameObject.Instantiate<GameObject>(prefab);
		if (go == null) return null;
		//go.transform.parent = parent;
		go.transform.SetParent(parent, worldPositionStays);
		if (name != null) go.name = name;
		return go.transform;
	}
}
