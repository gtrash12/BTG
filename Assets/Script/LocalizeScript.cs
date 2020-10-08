using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizeScript : MonoBehaviour {
    public string key;

    void textSet()
    {
        GetComponent<UnityEngine.UI.Text>().text = Singleton.Instance.getLocalText(key);
    }

	// Use this for initialization
	void Start () {
        textSet();
	}
}
