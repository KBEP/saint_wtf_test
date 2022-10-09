using UnityEngine;

public class MotionController : MonoBehaviour
{
	float moveSpeed = 10.0f;
	float rotSpeed = 1000.0f;
	Transform destination;
	Vector3 offset;
	
	//отправляет этот объект в destination со смещением позиции на величину offset
	public void CmdMoveTo (Transform destination, Vector3 offset)
	{
		if (destination == null) return;
		this.destination = destination;
		transform.parent = null;
		this.offset = offset;
		enabled = true;
	}
	
	void Update ()
	{
		if (destination == null)
		{
			enabled = false;
			return;
		}
		Vector3 pos = destination.position + offset;
		transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
		transform.rotation
		  = Quaternion.RotateTowards(transform.rotation, destination.rotation, rotSpeed * Time.deltaTime);
		if (transform.position == pos && transform.rotation == destination.rotation)
		{
			transform.parent = destination;
			destination = null;
			offset = Vector3.zero;
			enabled = false;
		}
	}
}
