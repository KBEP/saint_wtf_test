using UnityEngine;

public static class EM_Transform
{
	public static T GetChildComponent<T> (this Transform t, string path) where T : Component
	{
		Transform child = t.Find(path);
		return (child != null) ? child.GetComponent<T>() : null;
	}
}
