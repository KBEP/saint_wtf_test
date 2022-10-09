using System;

public class OutStorage//склад, работающий на отгрузку ресурсов
{
	ResourceRack rack;

	public event Action<RESOURCE_TYPE> Overfilled = (RESOURCE_TYPE t) => {};//переполнен
	public event Action<RESOURCE_TYPE> Released = (RESOURCE_TYPE t) => {};//снова готов принимать произведённый ресурс

	public int FreeSpace => rack.FreeSpace;
	public RESOURCE_TYPE ResType => rack.type;
	public int ResCount => rack.Count;
	
	public OutStorage (RESOURCE_TYPE type, int limit)
	{
		rack = new ResourceRack(type, limit);
	}

	public int AddResource (int count)
	{
		bool wasFilled = rack.IsFilled;
		int result = rack.AddResource(count);
		if (wasFilled)//был переполнен
		{
			if (!rack.IsFilled) Released(rack.type);//а теперь не переполнен
		}
		else//не был переполнен
		{
			if (rack.IsFilled) Overfilled(rack.type);//а теперь переполнен
		}
		return result;
	}
}
