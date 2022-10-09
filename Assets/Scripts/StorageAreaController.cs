using UnityEngine;

//просто контроллирует триггер и обеспечивает доступ к связанному контроллеру здания
public class StorageAreaController : MonoBehaviour
{
	public BuildingController BuildingCtr { get; private set; }

	void Awake ()
	{
		BuildingCtr = transform.parent.GetComponent<BuildingController>();
		if (BuildingCtr == null) Debug.Log("ERROR");
	}
}
