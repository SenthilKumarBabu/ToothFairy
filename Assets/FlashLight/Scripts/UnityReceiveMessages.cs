using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

public class UnityReceiveMessages : MonoBehaviour {
	public static UnityReceiveMessages Instance;
	public string directoryBasePath;
	private string songUrl;

	public bool IsFlashLightOn()
	{
		if(string.Equals(GetComponent<TextMesh>().text.ToUpper(), "ON"))
		{
			return true;
		}
		
		return false;
	}

	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
	public void statusMessage(string message) {
		GetComponent<TextMesh>().text = message;
	}

	public void clearMessage() {
		GetComponent<TextMesh>().text = "";
	}
}
