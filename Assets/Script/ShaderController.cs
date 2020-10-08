using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShaderController : MonoBehaviour {
    public bool RightChk;
    List<Component> Shaders = new List<Component>();
    public AudioSource GM;
    IEnumerator 볼록렌즈Ec;
    IEnumerator 플래시전환Ec;
    IEnumerator 플래시페이드아웃Ec;
    IEnumerator 페이드인Ec;
    IEnumerator 쇼크웨이브Ec;
    IEnumerator 스턴Ec;
    IEnumerator 블러포커스Ec;
    IEnumerator blurAniEc;
    IEnumerator 찬반시작Ec;
    

    public void 깨짐()
    {
        CameraFilterPack_TV_BrokenGlass a;
        a = gameObject.AddComponent<CameraFilterPack_TV_BrokenGlass>();
        a.Broken_High = 50;
        Shaders.Add(a);
        Singleton.Instance.CurrentShaderlist(RightChk).Add("깨짐");
    }

    public void 찬반시작(Color32 color, Color32 color2)
    {
        if (찬반시작Ec != null)
            StopCoroutine(찬반시작Ec);
        찬반시작Ec = 찬반시작E(color, color2);
        StartCoroutine(찬반시작Ec);
    }

    IEnumerator 찬반시작E(Color32 color, Color32 color2)
    {
        CameraFilterPack_AAA_SuperHexagon a;
        a = gameObject.AddComponent<CameraFilterPack_AAA_SuperHexagon>();
        a._BorderColor = color;
        a._HexaColor = color2;
        float xc = -3;
        while(xc < 30)
        {
            a._SpotSize = xc;
            xc += 20 * Time.deltaTime;
            yield return null;
        }
        Destroy(a);
    }

    public void 찬반(Color32 color, Color32 color2)
    {
        if (찬반시작Ec != null)
        {
            StopCoroutine(찬반시작Ec);
            Shaders.Remove(GetComponent<CameraFilterPack_AAA_SuperHexagon>());
            Destroy(GetComponent<CameraFilterPack_AAA_SuperHexagon>());
        }
        찬반시작Ec = 찬반E(color, color2);
        StartCoroutine(찬반시작Ec);
    }

    IEnumerator 찬반E(Color32 color, Color32 color2)
    {
        CameraFilterPack_AAA_SuperHexagon a;
        a = gameObject.AddComponent<CameraFilterPack_AAA_SuperHexagon>();
        a._BorderColor = color;
        a._HexaColor = color2;
        Shaders.Add(a);
        float xc = -3;
        while (xc < 2.5)
        {
            a._SpotSize = xc;
            xc += 10*Time.deltaTime;
            yield return null;
        }
    }

    public void 그레이스케일()
    {
        CameraFilterPack_Color_GrayScale a;
        a = gameObject.AddComponent<CameraFilterPack_Color_GrayScale>();
        Shaders.Add(a);
        Singleton.Instance.CurrentShaderlist(RightChk).Add("그레이스케일");
    }

    public void 블루스케일()
    {
        CameraFilterPack_Color_RGB a;
        a = gameObject.AddComponent<CameraFilterPack_Color_RGB>();
        a.ColorRGB = new Color(0.65f, 0.75f, 0.874f, 1);
        Shaders.Add(a);
        CameraFilterPack_Color_BrightContrastSaturation b;
        b = gameObject.AddComponent<CameraFilterPack_Color_BrightContrastSaturation>();
        b.Contrast = 1.41f;
        b.Brightness = 1.46f;
        Shaders.Add(b);
        Singleton.Instance.CurrentShaderlist(RightChk).Add("블루스케일");
    }

    public void 플래시페이드아웃()
    {
        if(플래시페이드아웃Ec != null)
            StopCoroutine(플래시페이드아웃Ec);
        플래시페이드아웃Ec = 플래시페이드아웃E(1);
        StartCoroutine(플래시페이드아웃Ec);
        Singleton.Instance.CurrentShaderlist(RightChk).Add("플래시페이드아웃");
    }

    public void 페이드인()
    {
        if (페이드인Ec != null)
            StopCoroutine(페이드인Ec);
        페이드인Ec = 페이드인E(1);
        StartCoroutine(페이드인Ec);
    }

    public void 플래시전환()
    {
        if (플래시전환Ec != null)
            StopCoroutine(플래시전환Ec);
        플래시전환Ec = 플래시전환E(1);
        StartCoroutine(플래시전환Ec);
    }

    public void 빠른플래시전환()
    {
        if (플래시전환Ec != null)
            StopCoroutine(플래시전환Ec);
        플래시전환Ec = 플래시전환E(5);
        StartCoroutine(플래시전환Ec);
    }

    public void 볼록렌즈()
    {
        볼록렌즈Ec = 볼록렌즈E();
        StartCoroutine(볼록렌즈Ec);
    }

    public IEnumerator 볼록렌즈E()
    {
        CameraFilterPack_Distortion_Half_Sphere a;
        a = gameObject.AddComponent<CameraFilterPack_Distortion_Half_Sphere>();
        float x = 1;
        a.SphereSize = 5f;
        while (x < 8)
        {
            x += Time.deltaTime * 7f;
            a.SphereSize = x;
            yield return null;
        }
        Destroy(a);
    }

    public void 지지직()
    {
        Component a;
        a = gameObject.AddComponent<CameraFilterPack_Real_VHS>();
        Shaders.Add(a);
        a = gameObject.AddComponent<CameraFilterPack_VHS_Tracking>();
        Shaders.Add(a);
        Singleton.Instance.CurrentShaderlist(RightChk).Add("지지직");
    }

    IEnumerator 플래시전환E(float spd)
    {
        CameraFilterPack_Colors_Brightness b = GetComponent<CameraFilterPack_Colors_Brightness>();
        b.enabled = true;
        float f = 2;
        while(f > 1f)
        {
            f -= Time.deltaTime*spd;
            b._Brightness = f;
            yield return null;
        }
        b._Brightness = 1;
        b.enabled = false;
        Singleton.Instance.CurrentShaderlist(RightChk).Remove("플래시페이드아웃");
    }
    public IEnumerator 플래시페이드아웃E(float spd)
    {
        CameraFilterPack_Colors_Brightness b = GetComponent<CameraFilterPack_Colors_Brightness>();
        b.enabled = true;
        float f = 1;
        while (f < 2f)
        {
            f += Time.deltaTime * spd;
            b._Brightness = f;
            yield return null;
        }
        b._Brightness = 2;
        //b.enabled = false;
    }
    public IEnumerator 페이드인E(float spd)
    {
        CameraFilterPack_Colors_Brightness b = GetComponent<CameraFilterPack_Colors_Brightness>();
        b.enabled = true;
        float f = 0;
        while (f < 1f)
        {
            f += Time.deltaTime * spd;
            b._Brightness = f;
            yield return null;
        }
        b._Brightness = 1;
        //b.enabled = false;
    }
    public void 블러포커스()
    {
        if (블러포커스Ec != null)
        {
            StopCoroutine(블러포커스Ec);
            if(GetComponents<CameraFilterPack_Blur_Focus>() != null)
                Destroy(GetComponent<CameraFilterPack_Blur_Focus>());
        }
        블러포커스Ec = 블러포커스E();
        StartCoroutine(블러포커스Ec);
    }
    IEnumerator 블러포커스E()
    {
        CameraFilterPack_Blur_Focus a;
        a = gameObject.AddComponent<CameraFilterPack_Blur_Focus>();
        yield return new WaitForSeconds(0.2f);
        Destroy(a);
    }


    public void 쇼크웨이브()
    {
        쇼크웨이브Ec = ShokeWave();
        StartCoroutine(쇼크웨이브Ec);
    }

    IEnumerator ShokeWave()
    {
        CameraFilterPack_Distortion_ShockWave a;
        a = gameObject.AddComponent<CameraFilterPack_Distortion_ShockWave>();
        a.Speed = 1;
        a.PosY = 0.5f;
        Shaders.Add(a);
        yield return new WaitForSeconds(0.5f);
        Shaders.Remove(a);
        Destroy(a);
    }

    public void 혼란()
    {
        GM.GetComponents<AudioSource>()[0].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("Sound/Echo");
        GM.GetComponents<AudioSource>()[1].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("Sound/Echo");
        GM.GetComponents<AudioSource>()[2].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("Sound/Echo");
        Component a;
        a = gameObject.AddComponent<CameraFilterPack_FX_Drunk>();
        Shaders.Add(a);
        Singleton.Instance.CurrentShaderlist(RightChk).Add("혼란");
        /*
        Component a;
        CameraFilterPack_Distortion_Dream2 b;
        a = gameObject.AddComponent<CameraFilterPack_FX_Drunk2>();
        Shaders.Add(a);
        b = gameObject.AddComponent<CameraFilterPack_Distortion_Dream2>();
        b.Distortion = 5;
        b.Speed = 1;
        Shaders.Add(b);
        */
    }

    public void 스턴()
    {
        if (스턴Ec != null)
            StopCoroutine(스턴Ec);
        스턴Ec = 스턴E();
        StartCoroutine(스턴Ec);
    }

    IEnumerator 스턴E()
    {
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 0.2f;
        //집중();
        혼란();
        GM.GetComponents<AudioSource>()[0].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("Sound/Echo");
        GM.GetComponents<AudioSource>()[1].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("Sound/Echo");
        GM.GetComponents<AudioSource>()[2].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("Sound/Echo");
        GameObject eff = Instantiate(GM.GetComponent<DialogScript>().StunParticle, Vector3.zero, Quaternion.identity) as GameObject;
        eff.transform.SetParent(GM.GetComponent<DialogScript>().ChaAnim.transform.parent);
        GM.GetComponents<AudioSource>()[2].PlayOneShot(Resources.Load<AudioClip>("Sound/확신효과음"));
        eff.GetComponent<DestroyStopwatchScript>().StartCoroutine("TimeOver", 1);
        eff.transform.localPosition = new Vector3(-400, 100, 0);
        eff.transform.localRotation = Quaternion.Euler(270, 90, 0);
        eff.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(0.2f);
        //Shaders.Remove(GetComponent<CameraFilterPack_Blur_Focus>());
        //Destroy(GetComponent<CameraFilterPack_Blur_Focus>());
        Time.timeScale = 1;
    }

    public void 집중()
    {
        Component a;
        a = gameObject.AddComponent<CameraFilterPack_Blur_Focus>();
        Shaders.Add(a);
        a.GetComponent<CameraFilterPack_Blur_Focus>()._Size = 3f;
        if (blurAniEc != null)
            StopCoroutine(blurAniEc);
        blurAniEc = BlurAni();
        StartCoroutine(blurAniEc);
        Singleton.Instance.CurrentShaderlist(RightChk).Add("집중");
    }

    public void 회상()
    {
        GM.GetComponents<AudioSource>()[2].PlayOneShot(Resources.Load<AudioClip>("Sound/" + "화악"));
        GM.GetComponents<AudioSource>()[0].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("Sound/Echo");
        GM.GetComponents<AudioSource>()[1].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("Sound/Echo");
        /*
        GM.GetComponents<AudioSource>()[1].clip = Resources.Load<AudioClip>("BGM/회상");
        GM.GetComponents<AudioSource>()[1].Play();
        GM.GetComponents<AudioSource>()[1].volume = 0.6f;
        */
        StartCoroutine(회상E());
        //GetComponent<CameraFilterPack_Distortion_ShockWave>().enabled = true;
        Singleton.Instance.CurrentShaderlist(RightChk).Add("회상");
    }

    IEnumerator 회상E()
    {
        StartCoroutine(ShokeWave());
        CameraFilterPack_Drawing_Paper3 sh = GetComponent<CameraFilterPack_Drawing_Paper3>();
        sh.enabled = true;
        sh.Fade_With_Original = 0;
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime;
            sh.Fade_With_Original = i;
            yield return null;
        }
        GetComponent<CameraFilterPack_Distortion_ShockWave>().enabled = true;
    }

    public void 재고상태(bool Chk)
    {
        GetComponent<CameraFilterPack_Drawing_Paper3>().enabled = Chk;
        GetComponent<CameraFilterPack_Distortion_ShockWave>().enabled = Chk;
    }

    public void 재고끝()
    {
        GM.PlayOneShot(Resources.Load<AudioClip>("Sound/" + "화악"));
        GM.GetComponents<AudioSource>()[0].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("Sound/Echo");
        GM.GetComponents<AudioSource>()[1].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("Sound/Echo");
        GM.GetComponents<AudioSource>()[2].outputAudioMixerGroup = Resources.Load<UnityEngine.Audio.AudioMixerGroup>("Sound/Echo");
        /*
        GM.GetComponents<AudioSource>()[1].clip = Resources.Load<AudioClip>("BGM/회상");
        GM.GetComponents<AudioSource>()[1].Play();
        GM.GetComponents<AudioSource>()[1].volume = 0.6f;
        */
        StartCoroutine(재고끝E());
        //GetComponent<CameraFilterPack_Distortion_ShockWave>().enabled = true;
    }

    IEnumerator 재고끝E()
    {
        StartCoroutine(ShokeWave());
        CameraFilterPack_Drawing_Paper3 sh = GetComponent<CameraFilterPack_Drawing_Paper3>();
        float i = 1;
        while (i > 0)
        {
            sh.Fade_With_Original = i;
            yield return null;
            i -= Time.deltaTime;
        }
        GM.GetComponents<AudioSource>()[0].outputAudioMixerGroup = null;
        GM.GetComponents<AudioSource>()[1].outputAudioMixerGroup = null;
        GM.GetComponents<AudioSource>()[2].outputAudioMixerGroup = null;
        GetComponent<CameraFilterPack_Distortion_ShockWave>().enabled = false;
        Singleton.Instance.CurrentShaderlist(RightChk).Clear();
    }

    IEnumerator LifeTimer(Component target,float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(target);
    }

    void 떠올리기()
    {
        Component a;
        a = gameObject.AddComponent<CameraFilterPack_TV_Videoflip>();
        Shaders.Add(a);
        a = gameObject.AddComponent<CameraFilterPack_TV_Vcr>();
        Shaders.Add(a);

    }

    IEnumerator BlurAni()
    {
        CameraFilterPack_Blur_Focus blur = GetComponent<CameraFilterPack_Blur_Focus>();
        blur._Size = 2;
        float a = 0.2f;
        while (a <= 2f)
        {
            Debug.Log(a);
            a += Time.deltaTime;
            
            blur._Size = a;
            yield return null;
        }
        blur._Size = 2f;
        /*
        while (true)
        {
            blur._Size = 2f;
            yield return null;
        }
        */
    }
    IEnumerator BlurAㄷni()
    {
        CameraFilterPack_Blur_GaussianBlur blur = GetComponent<CameraFilterPack_Blur_GaussianBlur>();
        blur.Size = 2;
        float a = 0.2f;
        while (a <= 2f)
        {
            a += Time.deltaTime;

            blur.Size = a;
            yield return null;
        }
    }

    public void Close(bool EchoOff = true)
    {
        StopAllCoroutines();
        foreach (Component i in Shaders)
        {
            
            Destroy(i);
        }
        Shaders.Clear();
        if (GM.GetComponents<AudioSource>()[0].outputAudioMixerGroup != null && EchoOff)
        {
            GM.GetComponents<AudioSource>()[0].outputAudioMixerGroup = null;
            GM.GetComponents<AudioSource>()[1].outputAudioMixerGroup = null;
            GM.GetComponents<AudioSource>()[2].outputAudioMixerGroup = null;
        }
        if (GetComponent<CameraFilterPack_Drawing_Paper3>().enabled)
        {
            GetComponent<CameraFilterPack_Drawing_Paper3>().enabled = false;
            GetComponent<CameraFilterPack_Distortion_ShockWave>().enabled = false;
        }
        Singleton.Instance.CurrentShaderlist(RightChk).Clear();
    }
}
