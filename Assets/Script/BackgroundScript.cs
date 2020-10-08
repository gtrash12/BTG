using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {
    public UnityEngine.UI.Image Child;
    public UnityEngine.UI.Image My;
    public Sprite 혼란이미지1;
    public Sprite 혼란이미지2;

    public void 혼란()
    {
        StopAllCoroutines();
        StartCoroutine(혼란E());
    }

    IEnumerator 혼란E()
    {
        My.sprite = 혼란이미지1;
        Child.sprite = 혼란이미지2;
        My.color = new Color(1,1,1,1);
        Child.color = new Color(1, 1, 1, 0);
        bool who = true;
        float alpha = 1;
        while (true)
        {
            if (who)
            {
                alpha -= Time.deltaTime;
                if(alpha < 0)
                {
                    alpha = 0;
                    who = false;
                }
            }
            else
            {
                alpha += Time.deltaTime;
                if (alpha > 1)
                {
                    alpha = 1;
                    who = true;
                }
            }
            My.color = new Color(1, 1, 1, alpha);
            Child.color = new Color(1, 1, 1, 1 - alpha);
            yield return null;
        }
    }

    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(FaideOut());
    }
    
    IEnumerator FaideOut()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            My.color = new Color(1, 1, 1, alpha);
            Child.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
