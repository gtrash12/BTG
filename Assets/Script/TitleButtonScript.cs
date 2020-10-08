using UnityEngine;
using System.Collections;

public class TitleButtonScript : MonoBehaviour {

    public CameraFilterPack_Colors_Brightness cb;
    public GameObject Pan;
    public GameObject SaveSlotContainer;
    public GameObject NewGame;
    public bool isTestvers;
    public UnityEngine.UI.Text versiontxt;

    public void Start()
    {
        PlayerPrefs.SetInt("MainEpisode", 1);
        versiontxt.text = "v."+Application.version;
    }

    public void quit()
    {
        Application.Quit();
    }

    public void ChapterReset()
    {
        Singleton.Instance.SelectedChapter = 0;
    }

    public void ClickSaveSlot(int slotindex)
    {
        if (Singleton.Instance.isNewGame)
        {
            Singleton.Instance.currentSaveSlot = slotindex;
            if (Singleton.Instance.SelectedChapter == 0)
            {
                StartCoroutine(StartNewGameE());
            }
            else
            {
                StartCoroutine(StartLoadGameE());
            }
            Pan.SetActive(false);
            SaveSlotContainer.SetActive(false);
        }
        else
        {
            
            var filePath = Application.persistentDataPath + "/Savedata/BtGsave" + slotindex + ".xml";
            if (System.IO.File.Exists(filePath))
            {
                Singleton.Instance.currentSaveSlot = slotindex;
                StartCoroutine(StartLoadGameE());
                Pan.SetActive(false);
                SaveSlotContainer.SetActive(false);
            }
        }
    }

    public void Continue()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/Savedata/BtGsave" + PlayerPrefs.GetInt("ContinueSlot",0) + ".xml")) {
            Singleton.Instance.isNewGame = false;
            Singleton.Instance.currentSaveSlot = PlayerPrefs.GetInt("ContinueSlot");
            StartCoroutine(StartLoadGameE());
            Pan.SetActive(false);
        }
    }


    public void LoadGame()
    {
        Singleton.Instance.isNewGame = false;
        SaveSlotContainer.SetActive(true);
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void StartNewGame()
    {
        if(PlayerPrefs.GetInt("MainEpisode",0) == 1) {
            Singleton.Instance.isNewGame = true;
            SaveSlotContainer.SetActive(true);
        }
        else
        {
            GetComponent<InAppPurchaser>().BuyProductID("mainepisode");
        }
        //StartCoroutine(StartNewGameE());
        //Pan.SetActive(false);

        
    }

    public void StartDemo()
    {
        Singleton.Instance.isNewGame = true;
        SaveSlotContainer.SetActive(true);
    }

    IEnumerator StartNewGameE()
    {
        PlayerPrefs.SetInt("ContinueSlot", Singleton.Instance.currentSaveSlot);
        yield return StartCoroutine(FlashOut());
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        yield break;
    }

    IEnumerator StartLoadGameE()
    {
        PlayerPrefs.SetInt("ContinueSlot", Singleton.Instance.currentSaveSlot);
        yield return StartCoroutine(FlashOut());
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);

        yield break;
    }

    public void TitleBttClick()
    {
        transform.parent.Find("XmlManager").GetComponent<DialogScript>().SaveGame();
        //Singleton.Instance.Initailize();
        //Application.LoadLevel("메인화면");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("메인화면");

    }

    public void gotoTitle()
    {
        //Application.LoadLevel("메인화면");
        Singleton.Instance.Initailize();
        UnityEngine.SceneManagement.SceneManager.LoadScene("메인화면");
    }

    IEnumerator Loading()
    {
        yield break;
    }

    IEnumerator FlashOut()
    {
        AudioSource aus = GetComponent<AudioSource>();
        GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/화악"));
        float bgmvolume = PlayerPrefs.GetFloat("BGMVolume");
        float i = 1;
        while(i < 2)
        {
            aus.volume = bgmvolume - bgmvolume * (i/2);
            i += Time.deltaTime;
            cb._Brightness = i;
            yield return null;
        }


        yield break;
    }

    public void selectChapter(int chap)
    {
        Singleton.Instance.SelectedChapter = chap;
    }
    
}
