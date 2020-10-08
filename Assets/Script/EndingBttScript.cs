using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingBttScript : MonoBehaviour {
    public void gotoTitle()
    {
        Singleton.Instance.Initailize();
        UnityEngine.SceneManagement.SceneManager.LoadScene("메인화면");
    }
}
