using UnityEngine;
using System.Collections;

public class SwaetAnimation : MonoBehaviour {
	string chaname;
	string motion;
	int total;
	int i = 0;
	public DialogScript gm;
	// Use this for initialization
	void OnEnable () {
		chaname = gm.Readname ();
		motion = gm.Readname(true);
		if(Resources.Load(chaname + motion + "땀") == null){
			gameObject.SetActive(false);
		}else{
			total = Resources.LoadAll(chaname + motion + "땀").Length -2;
			StartCoroutine (Ani ());
		}
	}

	IEnumerator Ani(){
		while (gameObject.activeSelf == true) {
			GetComponent<UnityEngine.UI.Image>().sprite = Resources.LoadAll<Sprite> (chaname + motion + "땀")[i];
			if (i < total) {
				i ++;
			}else{
				i = 0;
			}
			yield return new WaitForSeconds(0.2f);
		}
	}
}
