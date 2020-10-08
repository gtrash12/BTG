using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveChkScript : MonoBehaviour {
    public GameObject cont;
    public string item;


	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetInt(item,0) == 1)
        {
            cont.SetActive(false);
        }
        else
        {
            cont.SetActive(true);
        }
	}
	

}
