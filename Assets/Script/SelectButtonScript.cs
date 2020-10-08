using UnityEngine;
using System.Collections;

public class SelectButtonScript : MonoBehaviour {
	public string TagName;
	public int Index = 0;
	public Vector2 destination;
	public float Angle;
	public bool SetChk = false;
	public bool SelectChk = true;

	void Update(){
		if (destination != Vector2.zero) {
			Angle += 0.25f * Time.deltaTime;
			destination = new Vector2 ((Mathf.Cos (Angle) - Mathf.Sin (Angle)) * 180, 70 + 0.5f * (Mathf.Cos (Angle) + Mathf.Sin (Angle)) * 180);
			GetComponent<RectTransform>().anchoredPosition = destination;
        }
	}
}
