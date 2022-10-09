using System.Collections.Generic;
using UnityEngine;

//аналогичен StorageStack, но может отличатся в дальнейшем, например, иным порядком выкладки ресурса
public class InventoryStack : MonoBehaviour
{
	Stack<ResourceController> resCtrs = new Stack<ResourceController>();
	Transform anchor;//куда крепятся ресурсы
	float offset = 0.5f;

	void Awake ()
	{
		anchor = transform;
	}

	public bool Add (ResourceController resCtr)
	{
		if (resCtr == null) return false;
		resCtr.CmdMoveTo(anchor, new Vector3(0.0f, offset * resCtrs.Count, 0.0f));
		resCtrs.Push(resCtr);
		return true;
	}

	public ResourceController Remove () => resCtrs.Count > 0 ? resCtrs.Pop() : null;
}
