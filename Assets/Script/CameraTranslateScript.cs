using UnityEngine;
using System.Collections;

public class CameraTranslateScript : MonoBehaviour {
    public RectTransform Cha;
    public DialogScript XmlManager;
    public string cname;
    public string motion;
    public bool RightChk;
    bool stopchk = false;
    public Transform Cam;
    //Vector3 OriginPosition;
    //Vector3 OriginRotation;
    IEnumerator ZoE;
    IEnumerator FocE;

    public void ZoomEffect()
    {
        if (ZoE != null)
            StopCoroutine(ZoE);
        ZoE = ZoomEffectE(0.6f);
        StartCoroutine(ZoE);
    }

    public void StopAll()
    {
        stopchk = true;
        if (FocE != null)
            StopCoroutine(FocE);
        if (ZoE != null)
            StopCoroutine(ZoE);
    }

    public void FarAway()
    {
        StopAllCoroutines();
        if (FocE != null)
            StopCoroutine(FocE);
        FocE = farAway();
        StartCoroutine(FocE);
    }

    IEnumerator ZoomEffectE(float spd = 0.2f)
    {
        float ax = 0.01f;
        //Cha.pivot = new Vector2(0.5f,0.5f);
        for (float i = 0; i <= 0.5f; i += ax)
        {
            ax += spd * Time.deltaTime;
            Cam.localPosition = new Vector3(0, 0, i*2);
            Cha.localScale = Vector3.one * (1 + i/10);
            yield return null;
        }
        for (float i = 0.5f; i <= 1f; i += ax)
        {
            if (ax > 0.02f)
            {
                ax -= spd * Time.deltaTime;
            }
            else
            {
                ax = 0.02f;
            }
            Cam.localPosition = new Vector3(0, 0, i*2);
            Cha.localScale = Vector3.one * (1 + i / 10);
            yield return null;
        }
        //yield return new WaitForSeconds(1);
        ax = 1;
        while(ax > 0)
        {
            Cam.localPosition = new Vector3(0,0,ax*2);
            Cha.localScale = Vector3.one * (1 + ax /10);
            ax -= Time.deltaTime/3;
            yield return null;
        }
        Cam.localPosition = Vector3.zero;
        Cha.localScale = Vector3.one;
        yield break;
    }

    public void FocusOn(Vector3 fposition, Vector3 frotation, bool ChaMoveChk = true)
    {
        //OriginPosition = fposition;
        //OriginRotation = frotation;
        StopAllCoroutines();
        if (FocE != null)
            StopCoroutine(FocE);
        FocE = FocusOnE(fposition, frotation, 0.2f, ChaMoveChk);
        StartCoroutine(FocE);
    }

    public IEnumerator FocusOnE(Vector3 fposition, Vector3 frotation, float spd = 0.2f, bool ChaMoveChk = true)
    {
        stopchk = false;
        if (ZoE != null)
        {
            StopCoroutine(ZoE);
            Cha.anchoredPosition = Vector2.zero;
            Cha.localScale = Vector3.one;
        }
       // OriginPosition = fposition;
        //OriginRotation = frotation;
        Vector3 position = transform.position;
        Vector3 rotation = transform.rotation.eulerAngles;
        Vector3 posdif = fposition - transform.position;
        Vector3 rotdif = frotation - transform.rotation.eulerAngles;
        if (rotdif.x > 180) rotdif.x -= 360; else if (rotdif.x < -180) rotdif.x += 360;
        if (rotdif.y > 180) rotdif.y -= 360; else if (rotdif.y < -180) rotdif.y += 360;
        if (rotdif.z > 180) rotdif.z -= 360; else if (rotdif.z < -180) rotdif.z += 360;
        float ax = 0.01f;
        //float disz;
        //Cha.pivot = new Vector2(0.5f,0.5f);
        for (float i = 0; i <= 0.5f; i += ax)
        {
            if (stopchk)
            {
                stopchk = false;
                yield break;
            }
            ax += spd * Time.deltaTime;
            transform.position = position + posdif * i;
            transform.rotation = Quaternion.Euler(rotation + rotdif * i);
            //disz = transform.position.z - position.z;
            //if (disz == 0) disz = 1f;
            float disx = position.x - transform.position.x;
            if (disx == 0) disx = 0.1f;
            if (ChaMoveChk)
            {
                Cha.anchoredPosition = new Vector2(100 * (transform.rotation.eulerAngles.y - rotation.y) * (disx), 0);
            }
            else
            {
                Cha.anchoredPosition = new Vector2(200000, 0);
            }
            //if (disz / 5 > 1.5f) disz = 7.5f;
            //if (disz / 5 < 0.5f) disz = 0.5f;
            //Cha.localScale = Vector3.one * disz / 5;
            yield return null;
        }
        if (cname != "")
        {
            Cha.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animator/" + cname);
            XmlManager.ChangeMotion(motion, RightChk);
        }
        for (float i = 0.5f; i <= 1f; i += ax)
        {
            if (stopchk)
            {
                stopchk = false;
                yield break;
            }
            if (ax > 0.02f)
            {
                ax -= spd * Time.deltaTime;
            }
            else
            {
                ax = 0.02f;
            }
            transform.position = position + posdif * i;
            transform.rotation = Quaternion.Euler(rotation + rotdif * i);
            //disz = 1- Vector3.Distance(fposition, transform.position);
            //if (disz == 0) disz = 1f;
            float disx = transform.position.x - fposition.x;
            if (disx == 0) disx = 0.1f;
            //if (ChaMoveChk)
                Cha.anchoredPosition = new Vector2(100*((transform.rotation.eulerAngles.y -frotation.y)*(disx)), 0);
            //Cha.localScale = Vector3.one*disz;
            yield return null;
        }
        transform.position = position + posdif;
        transform.rotation = Quaternion.Euler(rotation + rotdif);
        //disz = 1 - (transform.position.z - fposition.z);
        //if (disz == 0) disz = 1f;
        //Cha.localScale = Vector3.one * disz;
        Cha.anchoredPosition = Vector2.zero;
        yield break;
    }

    public void FocusOnImmediatly(Vector3 fposition, Vector3 frotation)
    {
        transform.position = fposition;
        transform.rotation = Quaternion.Euler(frotation);
    }

    public IEnumerator farAway(float spd = 0.2f)
    {
        float ax = 0.01f;
        stopchk = false;
        //Cha.pivot = new Vector2(0.5f,0.5f);
        while (true)
        {
            if (stopchk)
            {
                stopchk = false;
                yield break;
            }
            ax += spd * Time.deltaTime;
            Cam.localPosition = new Vector3(0, 0, -ax);
            Cha.localScale = Vector3.one * (1 - ax / 20);
            yield return null;
        }
    }
}
