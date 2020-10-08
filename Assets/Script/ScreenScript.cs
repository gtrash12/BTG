using UnityEngine;
using System.Collections;

public class ScreenScript : MonoBehaviour {
    public Camera Cam;
    public Camera SecCam;
    public RectTransform SecPanner;
    public RectTransform Border;
    IEnumerator ChangeE;
    IEnumerator SwapEc;
    public float OriginWidth;
    RectTransform rctf;

    private void Awake()
    {
        OriginWidth = transform.parent.GetComponent<RectTransform>().rect.width;
        rctf = GetComponent<RectTransform>();
        Debug.Log("OW : " + OriginWidth);
    }

    void Change(bool Chk = true)
    {
        if (Chk)
        {
            float sz = rctf.sizeDelta.x / OriginWidth;
            if (sz > 0) { Cam.rect = new Rect(1 - sz, 0, sz, 1); } else { Cam.rect = new Rect(0.9999f, 0, sz, 1); }
            if (sz < 0.99f) { SecCam.rect = new Rect(0, 0, 1 - sz, 1);} else { SecCam.rect = new Rect(0, 0, 0.001f, 1); }
            //SecCam.rect = new Rect(0, 0, 1 - sz, 1);
            SecPanner.sizeDelta = new Vector2((1 - sz) * OriginWidth, 0);
            Border.anchoredPosition = new Vector2((1 - sz) * 800, 0);
            if (sz <= 0.5f) { Border.localScale = new Vector3(0.5f + sz, 1, 1); } else { Border.localScale = new Vector3((1.5f - sz), 1, 1); }
        }
        else
        {
            float sz = rctf.sizeDelta.x / OriginWidth;
            if (sz > 0) { Cam.rect = new Rect(0, 0, sz, 1); } else { Cam.rect = new Rect(0.9999f, 0, sz, 1); }
            if(sz < 0.99f) { SecCam.rect = new Rect(sz, 0, 1 - sz, 1);} else { SecCam.rect = new Rect(sz, 0, 0.001f, 1); }
            //SecCam.rect = new Rect(sz, 0, 1 - sz, 1);
            SecPanner.sizeDelta = new Vector2((1 - sz) * OriginWidth, 0);
            Border.anchoredPosition = new Vector2(sz * 800, 0);
            if (sz <= 0.5f) { Border.localScale = new Vector3(0.5f + sz, 1, 1); } else { Border.localScale = new Vector3((1.5f - sz), 1, 1); }
        }
        
    }

    public void Swap(bool Chk)
    {
        if (SwapEc != null)
            StopCoroutine(SwapEc);
        SwapEc = SwapE(Chk,0.6f);
        StartCoroutine(SwapEc);
    }

    IEnumerator SwapE(bool Chk, float spd = 0.3f)
    {
        //float sz = rctf.sizeDelta.x / OriginWidth;
        /*
        float a = 0.7f;
        while (a < 1)
        {
            SecPanner.anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2((1 - sz) * 800, 0), a);
            GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(-sz*800, 0), a);
            a += spd*Time.deltaTime;
            yield return null;
        }
        */
        if (ChangeE != null)
            StopCoroutine(ChangeE);
        yield return StartCoroutine(ChangeSize(0, 3, !Chk,false));
        ChangeImmediatly(0, Chk);
        yield return StartCoroutine(ChangeSize(400 * OriginWidth / 800, 3, Chk,true,true));
    }

    public void ChangeScreen(float size, float spd = 1,bool chk = true)
    {
        if (ChangeE != null)
            StopCoroutine(ChangeE);
        ChangeE = ChangeSize(size * OriginWidth /800, spd, chk);
        StartCoroutine(ChangeE);
    }

    public void ChangeImmediatly(float size, bool Chk = true)
    {
        if (ChangeE != null)
            StopCoroutine(ChangeE);
        rctf.sizeDelta = new Vector2(size* OriginWidth/800, 0);
        Change(Chk);
    }

    IEnumerator ChangeSize(float size, float spd, bool chk, bool axChk = true, bool fastStart = false)
    {
        //if (!SecPanner.gameObject.activeSelf) SecPanner.gameObject.SetActive(true);
        float originalsize = rctf.sizeDelta.x;
        float distance = size - originalsize;
        float ax = 0.01f;
        if (fastStart)
            ax = 0.2f;
        for (float i = 0; i <= 0.5f; i += ax)
        {
            ax += 0.2f * Time.deltaTime*spd;
            rctf.sizeDelta = new Vector2(originalsize + distance * i, 0);
            Change(chk);
            yield return null;
        }
        for (float i = 0.5f; i < 1; i += ax)
        {
            if (axChk)
            {
                if (ax > 0.02f)
                    ax -= 0.1f * Time.deltaTime * spd;
            }
            else
            {
                ax += 0.2f * Time.deltaTime * spd;
            }
            rctf.sizeDelta = new Vector2(originalsize + distance * i, 0);
            Change(chk);
            yield return null;
        }
        /*
        if(size == 800)
        {
            SecPanner.gameObject.SetActive(false);
        }
        */
        ChangeImmediatly(size*800/ OriginWidth, chk);
        yield break;
    }
}
