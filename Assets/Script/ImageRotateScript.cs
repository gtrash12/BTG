using UnityEngine;
using System.Collections;

public class ImageRotateScript : MonoBehaviour {
	void Update () {
		GetComponent<RectTransform> ().Rotate (0, 0, -10*Time.deltaTime);
	}

	IEnumerator FirstSet(){
		GetComponent<RectTransform> ().localRotation = Quaternion.Euler (0, 0, 0);
		for(float i = 0; i < 1; i += Time.deltaTime*3){
			GetComponent<RectTransform>().rotation = Quaternion.Euler (0,720*i,0);
			yield return null;
		}
		GetComponent<RectTransform> ().localRotation = Quaternion.Euler (0, 0, 0);
	}
	public void Change(){
		StopAllCoroutines ();
		StartCoroutine (FirstSet());
	}

}
