using UnityEngine;

//камера просто следует за объектом target
public class CameraController : MonoBehaviour
{
	public Transform target;
	Vector3 prevTargetPos;

	void OnEnable ()
	{
		if (target == null)
		{
			Debug.Log("No traget!");
			enabled = false;
		}
		else prevTargetPos = target.position;
	}
	
	void Update ()
	{
		transform.position += target.position - prevTargetPos;
		prevTargetPos = target.position;
	}
}
