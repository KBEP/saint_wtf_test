using UnityEngine;

[RequireComponent (typeof(MotionController))]

public class ResourceController : MonoBehaviour
{
	public RESOURCE_TYPE Type { get; private set; }
	
	MotionController mc;
	
	public void SetData (RESOURCE_TYPE type)
	{
		Type = type;
	}
	
	void Awake ()
	{
		mc = GetComponent<MotionController>();
	}

	public void CmdMoveTo (Transform destination, Vector3 offset)
	{
		mc.CmdMoveTo(destination, offset);
	}
}
