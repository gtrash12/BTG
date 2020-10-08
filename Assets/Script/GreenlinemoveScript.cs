using UnityEngine;
using System.Collections;

public class GreenlinemoveScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<RectTransform> ().Translate (-10 * Time.deltaTime, 0, 0);
		if (GetComponent<RectTransform> ().anchoredPosition.x < -14) {
			GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
		}
	}
}
