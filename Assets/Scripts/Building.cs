using System.Collections.Generic;
using UnityEngine;
using System;

public class Building
{
	public const int MAX_IN_RESOURCES = 2;

	public string Name { get; private set; }
	public float Progress { get; private set; }//прогресс производства текущей единицы ресурса
	public float ProductionTime { get; private set; }//время за которое производится одна единица ресурса
	public RESOURCE_TYPE OutResType => outStorage.ResType;
	public int OutResCount => outStorage.ResCount;

	public event Action<RESOURCE_TYPE, int> ResourcesProduced = (RESOURCE_TYPE t, int count) => {};//ресурс произведён
	public event Action<RESOURCE_TYPE> OutStorageOverfilled   = (RESOURCE_TYPE t) => {};//склад отгрузки переполнен
	public event Action<RESOURCE_TYPE> OutStorageReleased     = (RESOURCE_TYPE t) => {};//склад отгрузки снова может принимать производимый ресурс
	public event Action<RESOURCE_TYPE> InStorageEmptied       = (RESOURCE_TYPE t) => {};//склад ресурсов для производства пуст
	public event Action<RESOURCE_TYPE> InStorageFilled        = (RESOURCE_TYPE t) => {};//склад ресурсов для производства снова заполнен

	//эта пустая коллекция возвращается методом GetAllInResTypes, если здание не использует ресурсов для производства
	static readonly Dictionary<RESOURCE_TYPE, ResourceRack>.KeyCollection empty
	  = new Dictionary<RESOURCE_TYPE, ResourceRack>.KeyCollection(new Dictionary<RESOURCE_TYPE, ResourceRack>());
	
	OutStorage outStorage;//склад отгрузки готовых ресурсов
	InStorage inStorage;//склад ресурсов используемых для производства
	bool hasResources;//флаг - есть ресурсы для производства
	bool hasFreeSpace;//флаг - есть свободное месть на складе отгрузки
	float accTime;//накопительный счётчик времени производства ресурса

	public Building (string name, RESOURCE_TYPE outResource, RESOURCE_TYPE[] inResource, float productionTime, int limit)
	{
		Name = name ?? "NO_NAME";
		outStorage = new OutStorage(outResource, limit);
		outStorage.Overfilled += OnOutStorageOverfilled;
		outStorage.Released += OnOutStorageReleased;
		if (inResource == null || inResource.Length < 1)
		{
			inStorage = null;
		}
		else
		{
			inStorage = new InStorage(inResource, limit);
			inStorage.Emptied += OnInStorageEmptied;
			inStorage.Filled += OnInStorageFilled;
		}
		hasResources = CanProduce() >= 1;
		hasFreeSpace = outStorage.FreeSpace >= 1;
		ProductionTime = productionTime >= 0.0f ? productionTime : 0.0f;//0.0f - мгновенное производство
		Progress = 0.0f;
	}
	
	public int Update (float deltaTime)
	{
		if (deltaTime <= 0.0f || !hasResources || !hasFreeSpace) return 0;
		accTime += deltaTime;
		//сколько ресурса могло быть произведено за время accTime
		float resCount_f = ProductionTime > 0.0f ? accTime / ProductionTime : float.MaxValue;
		Progress = resCount_f - Mathf.Floor(resCount_f);
		if (resCount_f < 1.0f) return 0;//нет изменений по количеству ресурсов
		int resCount = resCount_f.AsPosInt();
		//фактическое количество произведённого ресурса с учётом времени, свободного места и доступных ресурсов
		int resMin = Mathf.Min(resCount, outStorage.FreeSpace, CanProduce());
		if (resMin < 1) return 0;//нет изменений по количеству ресурсов
		ConsumeResources(resMin);
		int resProduced = outStorage.AddResource(resMin);
		accTime -= ProductionTime * resProduced;
		ResourcesProduced(outStorage.ResType, resMin);//вызов события
		return resMin;
	}

	public bool HasFreeSpace (RESOURCE_TYPE type) => inStorage != null && inStorage.HasFreeSpace(type);
	
	public int InResCount (RESOURCE_TYPE type) => inStorage != null ? inStorage.CountOf(type) : 0;

	public int AddOutResource (int count) => outStorage.AddResource(count);
	
	public int AddInResource (RESOURCE_TYPE type, int count)
	  => inStorage != null ? inStorage.AddResource(type, count) : 0;

	public Dictionary<RESOURCE_TYPE, ResourceRack>.KeyCollection GetAllInResTypes ()
	  => inStorage != null ? inStorage.GetAllResTypes() : empty;

	int ConsumeResources (int count) => inStorage != null ? inStorage.ConsumeResources(count) : 0;

	//какое количество ресурсов можно произвести из имеющихся потребляемых ресурсов
	int CanProduce () => inStorage != null ? inStorage.CanProduce() : int.MaxValue;

	//обработчики событий
	
	void OnInStorageEmptied (RESOURCE_TYPE type)
	{
		hasResources = false;
		InStorageEmptied(type);
	}

	void OnInStorageFilled (RESOURCE_TYPE type)
	{
		hasResources = CanProduce() >= 1;
		InStorageFilled(type);
	}

	void OnOutStorageOverfilled (RESOURCE_TYPE type)
	{
		hasFreeSpace = false;
		OutStorageOverfilled(type);
	}

	void OnOutStorageReleased (RESOURCE_TYPE type)
	{
		hasFreeSpace = true;
		OutStorageReleased(type);
	}
}
