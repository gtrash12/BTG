using UnityEngine;
using System.Collections;

public class GaugeScript : MonoBehaviour {
    public UnityEngine.UI.Image Gauge;
    public UnityEngine.UI.Image GaugeRE;
    AudioSource aud;
    IEnumerator GuaE;
    void Start()
    {
        Gauge = GetComponent<UnityEngine.UI.Image>();
        GaugeRE = transform.parent.Find("HpRe").GetComponent<UnityEngine.UI.Image>();
        aud = GetComponent<AudioSource>();
    }

    public void Damage(float Dmg)
    {
        if(GuaE != null)
        {
            StopCoroutine(GuaE);
        }
        GuaE = ChangeAni(Dmg);
        StartCoroutine(GuaE);
    }

    public void Heal(float Dmg)
    {
        if (Singleton.Instance.hp > 100)
        {
            Dmg = 1f;
        }
        if (GuaE != null)
        {
            StopCoroutine(GuaE);
        }
        GuaE = HealAni(Dmg);
        StartCoroutine(GuaE);
    }

    IEnumerator ChangeAni(float Dmg)
    {
        float count = 0;
        float ax = 0;
        float Origin = Singleton.Instance.hp*0.01f;
        float dest = Origin - Dmg;
        if(dest< 0)
        {
            dest = 0;
        }
        Debug.Log(Origin);
        //GetComponent<AudioSource>().pitch = 1;
        aud.volume = PlayerPrefs.GetFloat("SFXVolume", 1f)/10;
        aud.Play();
        while (Gauge.fillAmount > dest)
        {
            //GetComponent<AudioSource>().pitch -= 0.1f*Time.deltaTime;
            Gauge.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
            count += ax;
            dest -= ax;
            Gauge.fillAmount = dest;
            ax += 0.01f* Time.deltaTime;
            yield return null;
        }
        aud.Stop();
        Gauge.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Gauge.fillAmount = Origin - Dmg;
        yield return new WaitForSeconds(2);
        while (GaugeRE.fillAmount > dest)
        {
            GaugeRE.fillAmount -= 0.1f*Time.deltaTime;
            yield return null;
        }
        GaugeRE.fillAmount = Origin - Dmg;
    }

    IEnumerator HealAni(float Dmg)
    {
        float count = 0;
        float ax = 0;
        float Origin = Singleton.Instance.hp * 0.01f;
        float dest = Origin + Dmg;
        if(dest > 1)
        {
            dest = 1;
        }
        aud.volume = PlayerPrefs.GetFloat("SFXVolume", 1f)/10;
        aud.Play();
        while (Gauge.fillAmount < dest)
        {
            Gauge.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
            count += ax;
            dest += ax;
            Gauge.fillAmount = dest;
            ax += 0.01f * Time.deltaTime;
            yield return null;
        }
        aud.Stop();
        Gauge.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Gauge.fillAmount = Origin + Dmg;
        GaugeRE.fillAmount = Origin + Dmg;
    }
}
