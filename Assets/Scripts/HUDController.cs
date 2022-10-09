using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
	public static HUDController Instance { get; private set; }

	Dictionary<Notice, Transform> notices = new Dictionary<Notice, Transform>();
	Transform noticesAnchor;//куда крепить уведомления
	
	void Awake ()
	{
		if (Instance == null) Instance = this;
		noticesAnchor = transform.Find("Notices");
		if (noticesAnchor == null) Debug.Log("ERROR");
	}

	public void AddNotice (string buildingName, NOTICE_TYPE notice, RESOURCE_TYPE resType)
	{
		Notice n = new Notice(buildingName, notice, resType);
		if (!notices.ContainsKey(n)) notices.Add(n, GenNotice(n));
	}

	public void DeleteNotice (string buildingName, NOTICE_TYPE notice, RESOURCE_TYPE resType)
	{
		Notice n = new Notice(buildingName, notice, resType);
		if (notices.TryGetValue(n, out Transform t) && t != null)
		{
			Destroy(t.gameObject);
			notices.Remove(n);
		}
	}
	
	Transform GenNotice (in Notice notice)
	{
		Transform t = Instancer.Instantiate("Prefabs/Notice", noticesAnchor);
		if (t == null) return null;
		Text text = t.GetComponent<Text>();
		if (text == null)
		{
			Destroy(t.gameObject);
			return null;
		}
		text.text = GenNoticeText(notice);
		return t;
	}

	string GenNoticeText (in Notice notice)
	{
		if (notice.type == NOTICE_TYPE.OVERFILLED)
		{
			return "Отгрузочный склад здания " + notice.buildingName + " переполнен!";
		}
		else if (notice.type == NOTICE_TYPE.NO_RES)
		{
			return "На складе " + notice.buildingName + " не хватает ресурса " + ResTypeToName(notice.resType) + "!";
		}
		else
		{
			return "";
		}
	}

	string ResTypeToName (RESOURCE_TYPE type) => "№" + ((int)type + 1);
}
