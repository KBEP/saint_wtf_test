using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
	//используется только для иницализации
	public string buildingName;
	public float productionSpeed = 1.0f;
	public int productionLimit = 5;
	public RESOURCE_TYPE outResource;
	public RESOURCE_TYPE[] inResource = new RESOURCE_TYPE[Building.MAX_IN_RESOURCES];
	//
	
	Building building;
	Transform resSpawn;//где спаунятся произведённые ресурсы
	StorageStack outStorageStack;//куда отправляются произведённые ресурсы
	Dictionary<RESOURCE_TYPE, StorageStack> inStorageStack;//откуда берутся ресурсы для производства
	
	public RESOURCE_TYPE OutResType => building.OutResType;//тип производимых ресурсов
	
	void Start ()
	{
		building = new Building(buildingName, outResource, inResource, productionSpeed, productionLimit);
		building.ResourcesProduced    += OnResourcesProduced;
		building.OutStorageOverfilled += OnOutStorageOverfilled;
		building.OutStorageReleased   += OnOutStorageReleased;
		building.InStorageEmptied     += OnInStorageEmptied;
		building.InStorageFilled      += OnInStorageFilled;
		//поиск и инициализаци объектов, которые понадобятся позже
		resSpawn = transform.Find("ResSpawn");
		if (resSpawn == null) Debug.Log("ERROR");
		outStorageStack = transform.GetChildComponent<StorageStack>("OutResAnchor");
		if (outStorageStack == null) Debug.Log("ERROR");
		inStorageStack = new Dictionary<RESOURCE_TYPE, StorageStack>();
		int idx = 0;
		foreach (var t in building.GetAllInResTypes())
		{
			OnInStorageEmptied(t);//показываем сообщение, т. к. изначально склад пустой
			StorageStack s = transform.GetChildComponent<StorageStack>("InResAnchor" + idx++);
			if (s != null) inStorageStack.Add(t, s);
			else Debug.Log("ERROR");
		}
	}
	
	void Update ()
	{
		building.Update(Time.deltaTime);
	}

	public ResourceController RemoveOutResource ()
	  => building.AddOutResource(-1) == -1 ? outStorageStack.Remove() : null;

	public Dictionary<RESOURCE_TYPE, ResourceRack>.KeyCollection GetAllInResTypes () => building.GetAllInResTypes();
	
	public bool HasFreeSpace (RESOURCE_TYPE type) => building.HasFreeSpace(type);

	public bool AddInResource (ResourceController resCtr)
	{
		return resCtr != null && building.AddInResource(resCtr.Type, 1) == 1
		  && inStorageStack.TryGetValue(resCtr.Type, out StorageStack stack) && stack.Add(resCtr);
	}

	//обработчики событий - вызов соответствующих уведомлений на экране
	
	//вызывается, когда был произведён ресурс
	void OnResourcesProduced (RESOURCE_TYPE type, int count)
	{
		//визуализация траты и появления новых ресурсов
		while (count-- > 0)
		{
			foreach (var s in inStorageStack)
			{
				ResourceController inResCtr = s.Value.Remove();
				if (inResCtr) Destroy(inResCtr.gameObject);
			}
			ResourceController outResCtr = ResourceSpawner.Spawn(building.OutResType, resSpawn.position);
			outStorageStack.Add(outResCtr);
		}
	}
	
	void OnOutStorageOverfilled (RESOURCE_TYPE type)
	{
		HUDController.Instance?.AddNotice(building.Name, NOTICE_TYPE.OVERFILLED, type);
	}

	void OnOutStorageReleased (RESOURCE_TYPE type)
	{
		HUDController.Instance?.DeleteNotice(building.Name, NOTICE_TYPE.OVERFILLED, type);
	}

	void OnInStorageEmptied (RESOURCE_TYPE type)
	{
		HUDController.Instance?.AddNotice(building.Name, NOTICE_TYPE.NO_RES, type);
	}

	void OnInStorageFilled (RESOURCE_TYPE type)
	{
		HUDController.Instance?.DeleteNotice(building.Name, NOTICE_TYPE.NO_RES, type);
	}
}
