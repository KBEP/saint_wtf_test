using UnityEngine;
using UnityEngine.UI;

public class AppQuitButton : MonoBehaviour
{
	void Awake ()
	{
		GetComponent<Button>()?.onClick.AddListener(() => { Application.Quit(); });
	}
}
