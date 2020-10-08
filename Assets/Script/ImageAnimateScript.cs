using UnityEngine;
using System.Collections;

public class ImageAnimateScript : MonoBehaviour {



    public void Show()
    {
        StartCoroutine(ShowE());
    }

    IEnumerator ShowE()
    {
        float i = 0;
        while (i < 1)
        {
            i += 3f * Time.deltaTime;
            GetComponent<RectTransform>().localScale = new Vector3(i,i,1);
            yield return null;
        }
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    public void Close()
    {
        StartCoroutine(CloseE());
    }

    IEnumerator CloseE()
    {
        float i = 1;
        while (i > 0)
        {
            i -= 3f * Time.deltaTime;
            GetComponent<RectTransform>().localScale = new Vector3(i, i, 1);
            yield return null;
        }
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        gameObject.SetActive(false);
    }
}
