using UnityEngine;
using System.Collections;

public class StringContainer : MonoBehaviour {
    public string str;

    public void Select()
    {
        DialogScript xmlmanager = transform.parent.Find("XmlManager").GetComponent<DialogScript>();
        xmlmanager.ChangeDialog(str);
        xmlmanager.SendMessage("Say");
    }
}
