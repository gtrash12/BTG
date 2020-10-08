using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointlineScript : MonoBehaviour {
    public float currentLine = 0;
    RectTransform rtrs;
	// Use this for initialization
	void Start () {
        rtrs = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        rtrs.anchoredPosition = new Vector2(currentLine * 722,0);
        currentLine = currentLine >= 1.0f ? 0 : currentLine + 0.2f * Time.deltaTime;
        
	}

    private void OnDisable()
    {
        currentLine = 0;
    }
}
