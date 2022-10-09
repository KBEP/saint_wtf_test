using UnityEngine;

//класс обеспечивает функционал подбора и выгрузки ресурсов
public class ResourcePicker : MonoBehaviour
{
	float transferTime = float.MinValue;//время, после которого можно произвести подбор/выгрузку след. единицы ресурса
	float transferDelay = 0.1f;//задержка между подбором/выгрузкой

	InventoryController invCtr;

	void Awake ()
	{
		invCtr = transform.GetChildComponent<InventoryController>("Inventory");
		if (invCtr == null) Debug.Log("ERROR");
	}
	
	void OnTriggerStay (Collider collider)
	{
		if (Time.time < transferTime || collider.tag != "StorageArea") return;
		BuildingController buildingCtr = collider.transform.GetComponent<StorageAreaController>()?.BuildingCtr;
		if (buildingCtr == null) return;
		//подбор ресурса в инвентарь
		if (invCtr.HasFreeSpace(buildingCtr.OutResType))
		{
			ResourceController outResCtr = buildingCtr.RemoveOutResource();
			invCtr.AddResource(outResCtr);
		}
		//выгрузка ресурса на склад
		foreach (var t in buildingCtr.GetAllInResTypes())
		{
			if (buildingCtr.HasFreeSpace(t) && invCtr.HasRes(t))
			{
				ResourceController inResCtr = invCtr.RemoveResource(t);
				buildingCtr.AddInResource(inResCtr);
			}
		}
		//
		transferTime = Time.time + transferDelay;
	}
}
