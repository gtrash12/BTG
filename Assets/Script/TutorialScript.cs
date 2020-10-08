using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour {
    public List<GameObject> tuto;

    public void OnDisable()
    {
        foreach(GameObject i in tuto)
            i.SetActive(false);
    }
    public void tutorial(int index)
    {
        tuto[index].SetActive(true);
    }
}
