using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour {
    public Slider BGMopt;
    public Slider SFXopt;
    public Toggle SkipChk;
    public Toggle ViBEChk;
    public AudioSource Aud;

	void Start () {
        BGMopt.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        SFXopt.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        if (PlayerPrefs.GetInt("SkipChk", 1) == 1)
            SkipChk.isOn = true;
        else
            SkipChk.isOn = false;
        if (PlayerPrefs.GetInt("VibeChk", 1) == 1)
            ViBEChk.isOn = true;
        else
            ViBEChk.isOn = false;

        if (Aud != null)
        {
            Aud.volume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        }

    }

    public void BGMC()
    {
        PlayerPrefs.SetFloat("BGMVolume", BGMopt.value);

        if (Aud != null)
            Aud.volume = BGMopt.value;
    }

    public void SFXC()
    {
        PlayerPrefs.SetFloat("SFXVolume", SFXopt.value);
    }

    public void SkipC()
    {
        if (SkipChk.isOn)
            PlayerPrefs.SetInt("SkipChk", 1);
        else
            PlayerPrefs.SetInt("SkipChk", 0);
    }

    public void VibeC()
    {
        if (ViBEChk.isOn)
            PlayerPrefs.SetInt("VibeChk", 1);
        else
            PlayerPrefs.SetInt("VibeChk", 0);
    }

    public void ChangeLang(string Lang)
    {
        PlayerPrefs.SetString("Language", Lang);
    }
}
