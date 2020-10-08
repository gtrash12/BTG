using UnityEngine;
using System.Collections;

public class SoundManagerScript : MonoBehaviour {
    IEnumerator fader;
    IEnumerator intro;

    public void bgmIntro(string main)
    {
        if (intro != null)
            StopCoroutine(intro);
        intro = BgmIntroE(main);
        StartCoroutine(intro);
    }

    IEnumerator BgmIntroE(string main)
    {
        AudioSource source = GetComponents<AudioSource>()[1];
        source.clip = Resources.Load<AudioClip>("BGM/" + main + "(intro)");
        source.loop = false;
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        source.clip = Resources.Load<AudioClip>("BGM/" + main);
        source.loop = true;
        source.Play();
    }

    public void bgmFaider(bool On)
    {
        if (fader != null)
            StopCoroutine(fader);
        fader = bgmVolumeFaider(On);
        StartCoroutine(fader);
    }

    IEnumerator bgmVolumeFaider(bool On)
    {
        float BGMVolume = PlayerPrefs.GetFloat("BGMVolume");
        Debug.Log(BGMVolume);
        if (On)
        {
            float i = 0;
            while (i <= BGMVolume)
            {
                i += 0.2f * Time.deltaTime;
                GetComponents<AudioSource>()[1].volume = i;
                yield return null;
            }
        }
        else
        {
            float i = BGMVolume;
            while (i >= 0f)
            {
                i -= 0.2f * Time.deltaTime;
                GetComponents<AudioSource>()[1].volume = i;
                yield return null;
            }
            GetComponents<AudioSource>()[1].Stop();
        }
    }

    public void NewStart()
    {
        if (fader != null)
            StopCoroutine(fader);
        if (intro != null)
            StopCoroutine(intro);
    }
}
