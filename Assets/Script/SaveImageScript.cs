using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class SaveImageScript : MonoBehaviour {
    public int index;
    public Text Numbertxt;
    public Text Timetxt;
    public Image Screen;

	// Use this for initialization
	void Start () {
        Numbertxt.text = "Slot-"+index;
        var filePath = Application.persistentDataPath + "/Savedata/BtGsave" + index+".png";
        Debug.Log(filePath);
        if (System.IO.File.Exists(filePath))
        {
            var bytes = System.IO.File.ReadAllBytes(filePath);
            var tex = new Texture2D(640, 360, TextureFormat.ARGB32, false);
            tex.LoadImage(bytes);
            Screen.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),new Vector2(0.5f,0.5f));
            Timetxt.text = System.IO.File.GetLastWriteTime(filePath).ToString();
            
        }
        else
        {
            Screen.gameObject.SetActive(false);
            
            Timetxt.text = "빈 슬롯";
        }

    }

    // Update is called once per frame
    void Update () {
		
	}

}
