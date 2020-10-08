using UnityEngine;
using System.Collections;

public class NoticeScript : MonoBehaviour {

    IEnumerator NotE;

	public void Open (string txt, float time) {
        gameObject.SetActive(true);
        StopAllCoroutines();
        UnityEngine.UI.Text text = transform.Find("Text").GetComponent<UnityEngine.UI.Text>();
        text.text = txt;
        if (NotE != null)
        {
            StopCoroutine(NotE);
        }
        NotE = OpenF(time);
        StartCoroutine(NotE);
        text.Rebuild(UnityEngine.UI.CanvasUpdate.PreRender);
        GetComponent<RectTransform>().sizeDelta = new Vector2(0, text.preferredHeight + 10);
    }

    IEnumerator OpenF(float time)
    {
        float i = 0;
        float a = 0;
        while (i <=0.5f)
        {
            a += Time.deltaTime;
            i += a;
            GetComponent<RectTransform>().localScale = new Vector3(1, i, 1);
            yield return null;
        }
        while(i <= 1)
        {
            if (a > 0.02f)
            {
                a -= Time.deltaTime;
            }
            else
            {
                a = 0.02f;
            }
            i += a;
            GetComponent<RectTransform>().localScale = new Vector3(1, i, 1);
            yield return null;
        }

        yield return new WaitForSeconds(time);
        while (i >= 0.5f)
        {
            a += Time.deltaTime;
            i -= a;
            GetComponent<RectTransform>().localScale = new Vector3(1, i, 1);
            yield return null;
        }
        while (i >= 0)
        {
            if (a > 0.02f)
            {
                a -= Time.deltaTime;
            }
            else
            {
                a = 0.02f;
            }
            i -= a;
            GetComponent<RectTransform>().localScale = new Vector3(1, i, 1);
            yield return null;
        }
        gameObject.SetActive(false);
        yield break;
    }
}
