using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;

public class DialogScript : MonoBehaviour {
	public XmlDocument AreaXmlData;
	XmlDocument xmldoc;
    XmlDocument xmlOrigin;
	public XmlElement Node;
	int NumberDia;
	public string currentMotion;
    string secondMotion = "기본";
    bool RightChk = true;
    public Animator 압박배틀;
    public UnityEngine.UI.Text Keywordtxt;
    AudioSource[] Audios;

	GameObject Sweat;
	public Camera MainCamera;
	public UnityEngine.UI.Image ChaImg;
	public Animator ChaAnim;
	public UnityEngine.UI.Image Mouth;
    Animator SecAnim;
    UnityEngine.UI.Image SecMouth;
    GaugeScript HpGuage;
    public UnityEngine.UI.Image Pannel;
	public UnityEngine.UI.Text Nametxt;
	public UnityEngine.UI.Text Dialoguetxt;
	public UnityEngine.UI.Text Droptxt;
	public Transform AreaCamera;
	public InventoryScript Inventory;
    public ScreenScript DynamicScreen;
	public List<GameObject> AreaContainer;
	string currentType;
	bool SayChk;
	Vector2 FieldSize;
	IEnumerator Sayvar;
	bool MouthMoveChk;
	string TagName = "say";
	bool ReadChk = false;
	IEnumerator FlashEnum;
	// 임시
	public bool FreeSkip = true;
	public int StartIndex = 40;
	IEnumerator CR;
    Vector2 SayBoxSize;
    public string StartPhase;
    public string StartTag;
    int TextIndex;
    string currentname;
    string Remember;
    Animator 난입;
    bool 확신 = false;
    int pressCount = 0;
    uint pressChk = 0;
    public GameObject StunParticle;
    public Animator Invenbtt;
    bool AgreeChk = true;
    bool beginwithdia = true;
    float voicevolume;
    public List<GameObject> SelectButtons;
    string CheckPoint;
    public UnityEngine.UI.Image PressGuage;
    public GameObject 압박에임;
    public Animator 대사화살표;
    public PointlineScript Pointline;
    Vector2 PointRange;
    bool donotsave = false;
    public TutorialScript Tutorialcont;

    public void FreeSkipChange()
    {
        if (PlayerPrefs.GetInt("SkipChk", 1) == 1)
            FreeSkip = false;
        else
            FreeSkip = true;
    }

    public void BGMVolumeChange()
    {
        Audios[1].volume = PlayerPrefs.GetFloat("BGMVolume",0.5f);
    }

    public void BGMVolumeChangeinGame()
    {
        Audios[1].volume = PlayerPrefs.GetFloat("BGMVolume", 0.5f)/20;
    }

    public void SFXVolumeChange()
    {
        Audios[0].volume = PlayerPrefs.GetFloat("SFXVolume",1f);
        voicevolume = PlayerPrefs.GetFloat("SFXVolume",1f)/2;
    }

    public void Reloadsc()
    {
        xmldoc.LoadXml(Resources.Load<TextAsset>("Scenario/Dialogue").text);
    }

    // Use this for initialization
    void Awake () {
        xmldoc = new XmlDocument();
        xmlOrigin = new XmlDocument();
        
        //transform.parent.GetComponent<Canvas> ().renderMode = RenderMode.ScreenSpaceCamera;
        Singleton.Instance.SetDB ();
		AreaXmlData = new XmlDocument ();
		AreaXmlData.LoadXml(Resources.Load<TextAsset>("XmlData/AreaData").text);
        
        Debug.Log(Singleton.Instance.phase+ " , " +TagName+" , " + NumberDia);
        SayBoxSize = Dialoguetxt.transform.parent.GetComponent<RectTransform>().sizeDelta;
        SecAnim = DynamicScreen.SecPanner.Find("Character").GetComponent<Animator>();
        SecMouth = DynamicScreen.SecPanner.Find("Character").Find("Mouth").GetComponent<UnityEngine.UI.Image>();
        HpGuage = DynamicScreen.Border.Find("Hp").GetComponent<GaugeScript>();
        난입 = transform.parent.Find("난입").GetComponent<Animator>();
        MainCamera.GetComponent<ShaderController>().GM = GetComponents<AudioSource>()[2];
        DynamicScreen.SecCam.GetComponent<ShaderController>().GM = GetComponents<AudioSource>()[2];
        Audios = GetComponents<AudioSource>();
        Singleton.Instance.SFXSource = Audios[2];
        Singleton.Instance.BGMSource = Audios[1];
    }
	void Start(){
#if (UNITY_ANDROID || UNITY_IPHONE)
        Application.targetFrameRate = 60;
#else
        Debug.Log("notADROID");
        Application.targetFrameRate = 0;
#endif
        Sweat = ChaAnim.transform.Find ("Sweat").gameObject;
		AreaCamera = MainCamera.transform.parent.transform;

        SFXVolumeChange();
        BGMVolumeChange();
        FreeSkipChange();
		//Say ();
		GetComponent<MouseEventScript> ().enabled = false;
        
        if (Singleton.Instance.isNewGame || !File.Exists(Application.persistentDataPath + "/Savedata/BtGsave" + Singleton.Instance.currentSaveSlot + ".xml"))
        {
            // 데모셋팅[
            /*
            TextAsset textAsset = (TextAsset)Resources.Load("Scenario/DemoXml");
            StartIndex = 0;
            StartPhase = "Demo1";
            StartTag = "인트로";
            */
            //]데모셋팅
            // 객체선언
            
            Singleton.Instance.hp = 100;
            Singleton.Instance.Maxhp = 100;
            switch (Singleton.Instance.SelectedChapter)
            {
                 
                case 0:
                    xmldoc.LoadXml(Resources.Load<TextAsset>("Scenario/Dialogue").text);
                    Singleton.Instance.phase = StartPhase;
                    TagName = StartTag;
                    NumberDia = StartIndex;
                    MoveArea("3학년6반");
                    Singleton.Instance.ChaList.Add(new InvenClass("김탐정"));
                    ChaImg.transform.parent.gameObject.SetActive(false);
                    다이나믹씬이동("ni800");

                    Say();
                    Inventory.gameObject.SetActive(true);
                    break;

                case 1:
                    xmldoc.LoadXml(Resources.Load<TextAsset>("Scenario/Dialogue").text);
                    Singleton.Instance.phase = "HighLight";
                    TagName = "인트로";
                    NumberDia = 0;
                    //TagName = "찬반논의5-5동의도둑질";
                    //NumberDia = 0;
                    MoveArea("3학년6반");
                    Singleton.Instance.ChaList.Add(new InvenClass("김탐정"));
                    Singleton.Instance.ChaList.Add(new InvenClass("한나정"));
                    Singleton.Instance.ChaList.Add(new InvenClass("나미녀", 1));
                    Singleton.Instance.ChaList.Add(new InvenClass("차임도", 3));
                    Singleton.Instance.ChaList.Add(new InvenClass("조신혜",1));
                    Singleton.Instance.Inventory.Add(new InvenClass("딸기모양 머리끈", 2));//사용
                    Singleton.Instance.Inventory.Add(new InvenClass("끊어진 딸기모양 머리끈"));//사용
                    Singleton.Instance.Inventory.Add(new InvenClass("교직원 안내문"));//사용
                    Singleton.Instance.Inventory.Add(new InvenClass("피해자 정보"));
                    Singleton.Instance.Inventory.Add(new InvenClass("막힌 변기"));
                    Singleton.Instance.Inventory.Add(new InvenClass("차임도의 열쇠"));
                    Singleton.Instance.Inventory.Add(new InvenClass("한나정의 자물쇠"));
                    Singleton.Instance.Inventory.Add(new InvenClass("휴지통"));//사용
                    Singleton.Instance.Inventory.Add(new InvenClass("열린 변기 커버"));//사용
                    Singleton.Instance.Inventory.Add(new InvenClass("떨어진 슬리퍼"));//사용
                    Singleton.Instance.Inventory.Add(new InvenClass("치한 사진"));//사용
                    Singleton.Instance.Inventory.Add(new InvenClass("진통제"));
                    Singleton.Instance.Inventory.Add(new InvenClass("차임도의 사진"));//사용
                    Singleton.Instance.Inventory.Add(new InvenClass("차임도의 문자"));//사용
                    Singleton.Instance.Inventory.Add(new InvenClass("육상부 안내문"));//사용
                    Singleton.Instance.Inventory.Add(new InvenClass("시약 구매 내역서"));//사용
                    ChaImg.transform.parent.gameObject.SetActive(false);
                    다이나믹씬이동("ni800");

                    Say();
                    Inventory.gameObject.SetActive(true);
                    break;

                case 2:
                    xmldoc.LoadXml(Resources.Load<TextAsset>("Scenario/DemoXml").text);
                    Singleton.Instance.phase = "HighLight";
                    TagName = "인트로";
                    NumberDia = 0;
                    MoveArea("3학년6반");
                    Singleton.Instance.ChaList.Add(new InvenClass("김탐정"));
                    Singleton.Instance.ChaList.Add(new InvenClass("한나정"));
                    Singleton.Instance.ChaList.Add(new InvenClass("나미녀"));
                    Singleton.Instance.ChaList.Add(new InvenClass("차임도"));
                    Singleton.Instance.ChaList.Add(new InvenClass("조신혜"));
                    Singleton.Instance.Inventory.Add(new InvenClass("차임도의 지갑"));
                    ChaImg.transform.parent.gameObject.SetActive(false);
                    다이나믹씬이동("ni800");

                    Say();
                    Inventory.gameObject.SetActive(true);
                    break;
            }
            
        }
        else
        {
            donotsave = true;
            LoadGame();
            Debug.Log("bg : "+ beginwithdia);
            Invoke("LoadMotionSet", Time.deltaTime);
            //HpGuage.Damage(0);
            if (beginwithdia)
            {
                if (NumberDia > 0)
                {
                    StartDialog(TagName, NumberDia);
                }
                else
                {
                    StartDialog(TagName, NumberDia);
                }
            }
            else {
                StartSearch();
            }
            //if(NumberDia > 0)
            //    StartDialog(TagName, NumberDia-1);
            Inventory.gameObject.SetActive(true);
        }
        //Inventory.gameObject.SetActive(true);
    }

    void DialogueCompare()
    {
        XmlNode comp = xmlOrigin.SelectSingleNode("/root/" + Singleton.Instance.stage + "-" + Singleton.Instance.phase).SelectNodes(TagName)[NumberDia];
        if (Node.InnerXml != comp.InnerXml)
        {
            Node.InnerXml = comp.InnerXml;
        }
    }

    public void LoadMotionSet()
    {
        //Transform pp = AreaCamera.transform;
        //Transform sp = DynamicScreen.SecCam.transform;
        if(Singleton.Instance.SelectedChapter == 0 && Singleton.Instance.phase == "HighLight")
        {
            if(!Singleton.Instance.IsHave("한나정의 자물쇠"))
            {
                Inventory.GetItem("한나정의 자물쇠", true);
            }
        }
        배경("종료");
        배경("!종료");
        WorldUpdate();
        Debug.Log("bg2 : " + beginwithdia);
        AutoChk(true,!beginwithdia);
        if (Resources.Load(ChaAnim.runtimeAnimatorController.name + "애니/" + currentMotion))
            ChangeMotion(currentMotion, true);
        if (Resources.Load(SecAnim.runtimeAnimatorController.name + "애니/" + secondMotion))
            ChangeMotion(secondMotion, false);
        
        HpGuage.Gauge.fillAmount = Singleton.Instance.hp / 100;
        HpGuage.GaugeRE.fillAmount = Singleton.Instance.hp / 100;
    }
    
    public bool ShowChk()
    {
        if (Node.SelectNodes("제시실패").Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator ScreenShotManually()
    {
        yield return new WaitForEndOfFrame();
        byte[] imageByte;   //스크린샷을 Byte로 저장.Texture2D use 

        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        //2d texture객체를 만드는대.. 스크린의 넓이, 높이를 선택하고 텍스쳐 포멧은 스크린샷을 찍기 위해서는 이렇게 해야 한다더군요. 

        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true); 
        //말 그대로입니다. 현제 화면을 픽셀 단위로 읽어 드립니다. 

        tex.Apply(); 
        //읽어 들인것을 적용. 

        imageByte = tex.EncodeToPNG(); 
        //읽어 드린 픽셀을 Byte[] 에 PNG 형식으로 인코딩 해서 넣게 됩니다. 

        DestroyImmediate(tex);
        //Byte[]에 넣었으니 Texture 2D 객체는 바로 파괴시켜줍니다.(메모리관리) 

        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Savedata/BtGsave" + Singleton.Instance.currentSaveSlot + ".png", imageByte); 
        //원하는 경로.. 저같은 경우는 저렇게 했습니다. 

    }

    void ReTry()
    {
        LoadGame(true);
        if (Singleton.Instance.Maxhp > 40)
        {
            Singleton.Instance.Maxhp -= 10;
        }
        Singleton.Instance.hp = Singleton.Instance.Maxhp;

        Invoke("LoadMotionSet", Time.deltaTime);
        if (beginwithdia)
        {
            if (NumberDia > 0)
                StartDialog(TagName, NumberDia - 1);
            else
                StartDialog(TagName, NumberDia);
        }
    }

    public void SaveGame(bool isCheckPoint = false)
    {
        if (donotsave)
        {
            donotsave = false;
            return;
        }

        if (!Directory.Exists(Application.persistentDataPath + "/Savedata"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Savedata");
        }
#if UNITY_ANDROID
        StartCoroutine(ScreenShotManually());
#else
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/Savedata/BtGsave"+Singleton.Instance.currentSaveSlot+".png");
#endif
        string txt = xmldoc.InnerXml;

        //Debug.LogAssertion(txt);
        //string Savingdata;
        
        XmlDocument SaveXml = new XmlDocument();
        if (!isCheckPoint)
        {
            SaveXml.InnerXml = xmldoc.InnerXml;
        }
        else
        {
            SaveXml.InnerXml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\n<root>\n</root>";
        }
            
        {
            XmlElement emptynode = SaveXml.CreateElement("savedata");
            SaveXml.SelectSingleNode("/root").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("Phase");
            emptynode.InnerText = Singleton.Instance.phase;
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("Hp");
            emptynode.InnerText = Singleton.Instance.hp.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("MaxHp");
            emptynode.InnerText = Singleton.Instance.Maxhp.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("Area");
            emptynode.InnerText = Singleton.Instance.area;
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("AutoDia");
            emptynode.InnerText = Singleton.Instance.AutoDia;
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("InvenTypeChk");
            emptynode.InnerText = Singleton.Instance.InvenTypeChk.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            {
                emptynode = SaveXml.CreateElement("Inventory");
                SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);
                int total = Singleton.Instance.Inventory.Count;
                for (int i = 0; i < total; i++)
                {

                    XmlElement Slotnode = SaveXml.CreateElement("slot");
                    SaveXml.SelectSingleNode("/root/savedata/Inventory").AppendChild(Slotnode);

                    emptynode = SaveXml.CreateElement("name");
                    emptynode.InnerText = Singleton.Instance.Inventory[i].name;
                    Slotnode.AppendChild(emptynode);

                    emptynode = SaveXml.CreateElement("level");
                    emptynode.InnerText = Singleton.Instance.Inventory[i].level.ToString();
                    Slotnode.AppendChild(emptynode);
                }
            }

            {
                emptynode = SaveXml.CreateElement("ChaList");
                SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);
                int total = Singleton.Instance.ChaList.Count;
                for (int i = 0; i < total; i++)
                {
                    XmlElement Slotnode = SaveXml.CreateElement("slot");
                    SaveXml.SelectSingleNode("/root/savedata/ChaList").AppendChild(Slotnode);

                    emptynode = SaveXml.CreateElement("name");
                    emptynode.InnerText = Singleton.Instance.ChaList[i].name;
                    Slotnode.AppendChild(emptynode);

                    emptynode = SaveXml.CreateElement("level");
                    emptynode.InnerText = Singleton.Instance.ChaList[i].level.ToString();
                    Slotnode.AppendChild(emptynode);
                }
            }

            {
                emptynode = SaveXml.CreateElement("ShaderList1");
                SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);
                int total = Singleton.Instance.Shaderlist1.Count;
                for (int i = 0; i < total; i++)
                {
                    emptynode = SaveXml.CreateElement("comp");
                    emptynode.InnerText = Singleton.Instance.Shaderlist1[i];
                    SaveXml.SelectSingleNode("/root/savedata/ShaderList1").AppendChild(emptynode);

                }

            }

            {
                emptynode = SaveXml.CreateElement("ShaderList2");
                SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);
                int total = Singleton.Instance.Shaderlist2.Count;
                for (int i = 0; i < total; i++)
                {
                    emptynode = SaveXml.CreateElement("comp");
                    emptynode.InnerText = Singleton.Instance.Shaderlist2[i];
                    SaveXml.SelectSingleNode("/root/savedata/ShaderList2").AppendChild(emptynode);

                }

            }

            emptynode = SaveXml.CreateElement("TagName");
            emptynode.InnerText = TagName;
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("NumberDia");
            emptynode.InnerText = NumberDia.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("Remember");
            emptynode.InnerText = Remember;
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("AgreeChk");
            emptynode.InnerText = AgreeChk.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("currentMotion");
            emptynode.InnerText = currentMotion;
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("secondMotion");
            emptynode.InnerText = secondMotion;
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("currentname");
            emptynode.InnerText = currentname;
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("Name1");
            emptynode.InnerText = ChaAnim.runtimeAnimatorController.name;
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("Name2");
            emptynode.InnerText = SecAnim.runtimeAnimatorController.name;
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("currentBGM");
            if(Audios[1].clip != null)
                emptynode.InnerText = Audios[1].clip.name;
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("isBGMplay");
            emptynode.InnerText = Audios[1].isPlaying.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("BGMplaytime");
            emptynode.InnerText = Audios[1].time.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);
        
            
            emptynode = SaveXml.CreateElement("Camera1Position");
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);
            {
                emptynode = SaveXml.CreateElement("x");
                emptynode.InnerText = AreaCamera.transform.position.x.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera1Position").AppendChild(emptynode);

                emptynode = SaveXml.CreateElement("y");
                emptynode.InnerText = AreaCamera.transform.position.y.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera1Position").AppendChild(emptynode);

                emptynode = SaveXml.CreateElement("z");
                emptynode.InnerText = AreaCamera.transform.position.z.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera1Position").AppendChild(emptynode);
            }

            emptynode = SaveXml.CreateElement("Camera1Rotation");
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);
            {
                emptynode = SaveXml.CreateElement("x");
                emptynode.InnerText = AreaCamera.transform.rotation.x.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera1Rotation").AppendChild(emptynode);

                emptynode = SaveXml.CreateElement("y");
                emptynode.InnerText = AreaCamera.transform.rotation.y.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera1Rotation").AppendChild(emptynode);

                emptynode = SaveXml.CreateElement("z");
                emptynode.InnerText = AreaCamera.transform.rotation.z.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera1Rotation").AppendChild(emptynode);

                emptynode = SaveXml.CreateElement("w");
                emptynode.InnerText = AreaCamera.transform.rotation.w.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera1Rotation").AppendChild(emptynode);
            }
            
            emptynode = SaveXml.CreateElement("Camera2Position");
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);
            {
                emptynode = SaveXml.CreateElement("x");
                emptynode.InnerText = DynamicScreen.SecCam.transform.parent.transform.position.x.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera2Position").AppendChild(emptynode);

                emptynode = SaveXml.CreateElement("y");
                emptynode.InnerText = DynamicScreen.SecCam.transform.parent.transform.position.y.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera2Position").AppendChild(emptynode);

                emptynode = SaveXml.CreateElement("z");
                emptynode.InnerText = DynamicScreen.SecCam.transform.parent.transform.position.z.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera2Position").AppendChild(emptynode);
            }

            emptynode = SaveXml.CreateElement("Camera2Rotation");
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);
            {
                emptynode = SaveXml.CreateElement("x");
                emptynode.InnerText = DynamicScreen.SecCam.transform.parent.transform.rotation.x.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera2Rotation").AppendChild(emptynode);

                emptynode = SaveXml.CreateElement("y");
                emptynode.InnerText = DynamicScreen.SecCam.transform.parent.transform.rotation.y.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera2Rotation").AppendChild(emptynode);

                emptynode = SaveXml.CreateElement("z");
                emptynode.InnerText = DynamicScreen.SecCam.transform.parent.transform.rotation.z.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera2Rotation").AppendChild(emptynode);

                emptynode = SaveXml.CreateElement("w");
                emptynode.InnerText = DynamicScreen.SecCam.transform.parent.transform.rotation.w.ToString();
                SaveXml.SelectSingleNode("/root/savedata/Camera2Rotation").AppendChild(emptynode);
            }
            
            emptynode = SaveXml.CreateElement("DynamicScreenSize");
            Debug.Log(DynamicScreen.GetComponent<RectTransform>().rect.width * 800 / DynamicScreen.OriginWidth);
            emptynode.InnerText = (DynamicScreen.GetComponent<RectTransform>().rect.width *800/DynamicScreen.OriginWidth).ToString();
            Debug.Log(emptynode.InnerText);
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("isDark");
            emptynode.InnerText = Pannel.gameObject.activeSelf.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("isChaActive");
            emptynode.InnerText = ChaAnim.gameObject.activeSelf.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("isDialogue");
            emptynode.InnerText = (!GetComponent<MouseEventScript>().enabled).ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("currentItemNum");
            emptynode.InnerText = Inventory.currentItem.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("Confidence");
            emptynode.InnerText = 확신.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

            emptynode = SaveXml.CreateElement("Episode");
            emptynode.InnerText = Singleton.Instance.SelectedChapter.ToString();
            SaveXml.SelectSingleNode("/root/savedata").AppendChild(emptynode);

        }
        if (isCheckPoint)
        {
            SaveXml.Save(Application.persistentDataPath + "/Savedata/BtGChecksave" + Singleton.Instance.currentSaveSlot + ".xml");
        }
        else
        {
            PlayerPrefs.SetInt("LastPlayedSlot", Singleton.Instance.currentSaveSlot);
            SaveXml.Save(Application.persistentDataPath + "/Savedata/BtGsave" + Singleton.Instance.currentSaveSlot + ".xml");
        }
        //PlayerPrefs.SetString("SaveXml", txt);
        
    }

    public void LoadGame(bool isCheckPoint = false)
    {
        Debug.Log("LoadGame");
        var filePath = Application.persistentDataPath + "/Savedata/BtGsave" + Singleton.Instance.currentSaveSlot + ".xml";
        if (isCheckPoint)
        {
            filePath = Application.persistentDataPath + "/Savedata/BtGChecksave" + Singleton.Instance.currentSaveSlot + ".xml";
        }
        if (File.Exists(filePath))
        {
            string txt = System.IO.File.ReadAllText(filePath);
            XmlDocument loaddata = new XmlDocument();
            loaddata.LoadXml(txt);
            if (loaddata.SelectSingleNode("/root/savedata/Episode") != null)
            {
                Singleton.Instance.SelectedChapter = XmlConvert.ToInt16(loaddata.SelectSingleNode("/root/savedata/Episode").InnerText);
            }

            if (Singleton.Instance.SelectedChapter <= 1)
            {
                xmlOrigin.LoadXml(Resources.Load<TextAsset>("Scenario/Dialogue").text);
            }
            else
            {
                xmlOrigin.LoadXml(Resources.Load<TextAsset>("Scenario/DemoXml").text);
            }

            if (!isCheckPoint)
            {
                XmlNode lodv = loaddata.SelectSingleNode("/root/Version");
                XmlNode orgv = xmlOrigin.SelectSingleNode("/root/Version");

                int totallodv = lodv.ChildNodes.Count;
                int totalorgv = orgv.ChildNodes.Count;
                Debug.Log(totallodv + "," + totalorgv);
                for (int i = totallodv; i < totalorgv; i++)
                {

                    int totalve = orgv.ChildNodes[i].ChildNodes.Count;
                    //XmlElement tmpv = orgv.ChildNodes[i] as XmlElement;

                    //lodv.AppendChild(tmpv.OuterXml as XmlElement);
                    XmlElement emptynode = loaddata.CreateElement("version");
                    lodv.AppendChild(emptynode);
                    for (int j = 1; j < totalve; j++)
                    {
                        Debug.Log(j);
                        XmlDocument tmpdoc = new XmlDocument();
                        XmlElement tmpnod = orgv.ChildNodes[i].ChildNodes[j] as XmlElement;
                        string tmptxt = tmpnod.InnerText;

                        while (true)
                        {
                            if (tmptxt.Contains("&lt;"))
                                tmptxt = tmpnod.InnerText.Replace("&lt;", "<").Replace("&gt;", ">");
                            else
                                break;
                        }
                        tmpdoc.LoadXml(tmptxt);
                        XmlElement tmpele = tmpdoc.DocumentElement;
                        Debug.Log(tmpele.InnerXml);
                        //loaddata.SelectSingleNode("/root/" + tmpnod.Name).ReplaceChild(tmpele, loaddata.SelectNodes("/root/" + tmpnod.Name + "/" + tmpele.Name)[XmlConvert.ToInt16(tmpnod.GetAttribute("index"))]);
                        loaddata.SelectNodes("/root/" + tmpnod.Name + "/" + tmpele.Name)[XmlConvert.ToInt16(tmpnod.GetAttribute("index"))].InnerXml = tmpele.InnerXml;
                        //Debug.Log(orgv.ChildNodes[i].ChildNodes[j+1].Name);
                        //Debug.Log(tempele.FirstChild.Name);

                    }

                }
            }

            {
                Singleton.Instance.phase = loaddata.SelectSingleNode("/root/savedata/Phase").InnerText;
                if (!isCheckPoint)
                {
                    Singleton.Instance.hp = (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Hp").InnerText);
                    Singleton.Instance.Maxhp = (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/MaxHp").InnerText);
                }
                Singleton.Instance.area = loaddata.SelectSingleNode("/root/savedata/Area").InnerText;
                Singleton.Instance.AutoDia = loaddata.SelectSingleNode("/root/savedata/AutoDia").InnerText;
                Singleton.Instance.InvenTypeChk = System.Convert.ToBoolean(loaddata.SelectSingleNode("/root/savedata/InvenTypeChk").InnerText);
                TagName = loaddata.SelectSingleNode("/root/savedata/TagName").InnerText;
                NumberDia = System.Convert.ToInt16(loaddata.SelectSingleNode("/root/savedata/NumberDia").InnerText);
                Remember = loaddata.SelectSingleNode("/root/savedata/Remember").InnerText;
                AgreeChk = System.Convert.ToBoolean(loaddata.SelectSingleNode("/root/savedata/AgreeChk").InnerText);
                currentMotion = loaddata.SelectSingleNode("/root/savedata/currentMotion").InnerText;
                secondMotion = loaddata.SelectSingleNode("/root/savedata/secondMotion").InnerText;
                currentname = loaddata.SelectSingleNode("/root/savedata/currentname").InnerText;
                if (loaddata.SelectSingleNode("/root/savedata/Name1").InnerText != "" && Resources.Load("Animator/" + loaddata.SelectSingleNode("/root/savedata/Name1").InnerText) != null)
                {
                    ChaAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animator/" + loaddata.SelectSingleNode("/root/savedata/Name1").InnerText);
                    if(Resources.Load(ChaAnim.runtimeAnimatorController.name + "애니/" + currentMotion))
                        ChangeMotion(currentMotion, true);
                }
                if (loaddata.SelectSingleNode("/root/savedata/Name2").InnerText != "" && Resources.Load("Animator/" + loaddata.SelectSingleNode("/root/savedata/Name2").InnerText) != null)
                {
                    SecAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animator/" + loaddata.SelectSingleNode("/root/savedata/Name2").InnerText);
                    if (Resources.Load(SecAnim.runtimeAnimatorController.name + "애니/" + secondMotion))
                        ChangeMotion(secondMotion, false);
                }
                if (loaddata.SelectSingleNode("/root/savedata/currentBGM").InnerText != "" && Resources.Load("BGM/" + loaddata.SelectSingleNode("/root/savedata/currentBGM").InnerText) != null)
                    Audios[1].clip = Resources.Load<AudioClip>("BGM/" + loaddata.SelectSingleNode("/root/savedata/currentBGM").InnerText);

                if (loaddata.SelectSingleNode("/root/savedata/isBGMplay").InnerText == "True")
                    Audios[1].Play();

                Audios[1].time = (float) System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/BGMplaytime").InnerText);

                다이나믹씬이동("ni"+loaddata.SelectSingleNode("/root/savedata/DynamicScreenSize").InnerText);
                /*
                if (loaddata.SelectSingleNode("/root/savedata/isDark").InnerText == "True")
                {
                    Pannel.color = Color.black;
                    Pannel.gameObject.SetActive(true);
                }
                */

                if (loaddata.SelectSingleNode("/root/savedata/isChaActive").InnerText == "False")
                    ChaAnim.gameObject.SetActive(false);

                beginwithdia = System.Convert.ToBoolean(loaddata.SelectSingleNode("/root/savedata/isDialogue").InnerText);

                Inventory.currentItem = System.Convert.ToInt16(loaddata.SelectSingleNode("/root/savedata/currentItemNum").InnerText);
                확신 = System.Convert.ToBoolean(loaddata.SelectSingleNode("/root/savedata/Confidence").InnerText);


                int total = loaddata.SelectNodes("/root/savedata/Inventory/slot").Count;
                if (!isCheckPoint)
                {
                    for (int i = 0; i < total; i++)
                    {
                        XmlNode slnd = loaddata.SelectNodes("/root/savedata/Inventory/slot")[i];
                        InvenClass df = new InvenClass(slnd.SelectSingleNode("name").InnerText, System.Convert.ToInt16(slnd.SelectSingleNode("level").InnerText));
                        Singleton.Instance.Inventory.Add(df);
                        //Singleton.Instance.Inventory[i].name = slnd.SelectSingleNode("name").InnerText;
                        //Singleton.Instance.Inventory[i].level = System.Convert.ToInt16(slnd.SelectSingleNode("level").InnerText);
                    }

                    total = loaddata.SelectNodes("/root/savedata/ChaList/slot").Count;
                    for (int i = 0; i < total; i++)
                    {
                        XmlNode slnd = loaddata.SelectNodes("/root/savedata/ChaList/slot")[i];
                        InvenClass df = new InvenClass(slnd.SelectSingleNode("name").InnerText, System.Convert.ToInt16(slnd.SelectSingleNode("level").InnerText));
                        Singleton.Instance.ChaList.Add(df);
                        //Singleton.Instance.ChaList[i].name = slnd.SelectSingleNode("name").InnerText;
                        //Singleton.Instance.ChaList[i].level = System.Convert.ToInt16(slnd.SelectSingleNode("level").InnerText);
                    }
                }

                total = loaddata.SelectNodes("/root/savedata/ShaderList1").Count;
                for (int i = 0; i < total; i++)
                {
                    Singleton.Instance.Shaderlist1.Add(loaddata.SelectNodes("/root/savedata/ShaderList1")[i].InnerText);
                }

                total = loaddata.SelectNodes("/root/savedata/ShaderList2").Count;
                for (int i = 0; i < total; i++)
                {
                    Singleton.Instance.Shaderlist2.Add(loaddata.SelectNodes("/root/savedata/ShaderList2")[i].InnerText);
                }

                {
                    Vector3 posit = new Vector3((float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera1Position").SelectSingleNode("x").InnerText)
                        , (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera1Position").SelectSingleNode("y").InnerText)
                        , (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera1Position").SelectSingleNode("z").InnerText));
                    AreaCamera.position = posit;
                }

                {
                    Vector3 posit = new Vector3((float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera2Position").SelectSingleNode("x").InnerText)
                        , (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera2Position").SelectSingleNode("y").InnerText)
                        , (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera2Position").SelectSingleNode("z").InnerText));
                    DynamicScreen.SecCam.transform.parent.transform.position = posit;
                }

                {
                    Quaternion quat = new Quaternion((float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera1Rotation").SelectSingleNode("x").InnerText)
                        , (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera1Rotation").SelectSingleNode("y").InnerText)
                        , (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera1Rotation").SelectSingleNode("z").InnerText)
                        , (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera1Rotation").SelectSingleNode("w").InnerText));
                    AreaCamera.rotation = quat;
                }

                {
                    Quaternion quat = new Quaternion((float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera2Rotation").SelectSingleNode("x").InnerText)
                        , (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera2Rotation").SelectSingleNode("y").InnerText)
                        , (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera2Rotation").SelectSingleNode("z").InnerText)
                        , (float)System.Convert.ToDouble(loaddata.SelectSingleNode("/root/savedata/Camera2Rotation").SelectSingleNode("w").InnerText));
                    DynamicScreen.SecCam.transform.parent.transform.rotation = quat;
                }
                if (!isCheckPoint)
                {
                    if (Singleton.Instance.SelectedChapter <= 1)
                    {
                        loaddata.SelectSingleNode("/root").RemoveChild(loaddata.SelectSingleNode("/root/savedata"));
                        xmldoc.LoadXml(loaddata.InnerXml);
                       // xmldoc.LoadXml(Resources.Load<TextAsset>("Scenario/Dialogue").text);
                    }
                    else
                    {
                        xmldoc.LoadXml(Resources.Load<TextAsset>("Scenario/DemoXml").text);
                    }
                    
                }

            }
        }
    }

    public void GoBackToTitle()
    {
        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/화악"));
        배경("플래시페이드아웃");
        배경("!플래시페이드아웃");
        SetDialogueActivate(false);
        Singleton.Instance.Initailize();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

	// - 에리어이동
	Vector3 AreaPosition(string area){
		float retx;
		float rety;
		float retz;
		int starti;
		int endi;
		string areatxt;
		areatxt = AreaXmlData.SelectSingleNode ("/root/A" + area).SelectSingleNode ("position").InnerText;
		starti = 1;
		endi = areatxt.IndexOf(",");
		retx = (float)XmlConvert.ToDecimal(areatxt.Substring(starti,endi-1));
		starti = endi+1;
		endi = areatxt.IndexOf(",", starti);
		rety = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
		starti = endi+1;
		endi = areatxt.IndexOf(")", starti);
		retz = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
		return new Vector3(retx,rety,retz);
	}
	Vector3 AreaRotation(string area){
		float retx;
		float rety;
		float retz;
		int starti;
		int endi;
		string areatxt;
		areatxt = AreaXmlData.SelectSingleNode ("/root/A" + area).SelectSingleNode ("rotation").InnerText;
		starti = 1;
		endi = areatxt.IndexOf(",");
		retx = (float)XmlConvert.ToDecimal(areatxt.Substring(starti,endi-1));
		starti = endi+1;
		endi = areatxt.IndexOf(",", starti);
		rety = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
		starti = endi+1;
		endi = areatxt.IndexOf(")", starti);
		retz = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
		return new Vector3(retx,rety,retz);
	}
	public void MoveArea(string area, bool BothChk = false){
		float retx;
		float rety;
		float retz;
		int starti;
		int endi;
		string areatxt;

        currentCamera(RightChk).GetComponent<CameraTranslateScript>().StopAll();
        areatxt = AreaXmlData.SelectSingleNode ("/root/A" + area).SelectSingleNode ("position").InnerText;
		starti = 1;
		endi = areatxt.IndexOf(",");
		retx = (float)XmlConvert.ToDecimal(areatxt.Substring(starti,endi-1));
		starti = endi+1;
		endi = areatxt.IndexOf(",", starti);
		rety = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
		starti = endi+1;
		endi = areatxt.IndexOf(")", starti);
		retz = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
        AreaCamera.transform.position = new Vector3 (retx, rety, retz);
        

        if (BothChk)
        {
            DynamicScreen.SecCam.transform.parent.position = new Vector3(retx, rety, retz);
        }

        areatxt = AreaXmlData.SelectSingleNode ("/root/A" + area).SelectSingleNode ("rotation").InnerText;
		starti = 1;
		endi = areatxt.IndexOf(",");
		retx = (float)XmlConvert.ToDecimal(areatxt.Substring(starti,endi-1));
		starti = endi+1;
		endi = areatxt.IndexOf(",", starti);
		rety = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
		starti = endi+1;
		endi = areatxt.IndexOf(")", starti);
		retz = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
		AreaCamera.transform.rotation = Quaternion.Euler(retx,rety,retz);
        //AreaCamera.localScale = Vector3.one;

        if (BothChk)
        {
            DynamicScreen.SecCam.transform.parent.rotation = Quaternion.Euler(retx, rety, retz);
        }
		Singleton.Instance.area = AreaXmlData.SelectSingleNode ("/root/A" + area).SelectSingleNode ("Area").InnerText;
        WorldUpdate();

	}

    public void WorldUpdate(bool chk = true)
    {
        foreach (GameObject i in AreaContainer)
        {
            if (i.name == Singleton.Instance.area)
            {
                if (chk) { i.SetActive(true); }
                int total = i.transform.Find("Clickable").childCount;
                Debug.Log(total);
                Singleton.Instance.PosContainer = null;
                for (int x = 0; x < total; x++)
                {
                    i.transform.Find("Clickable").GetChild(x).gameObject.SetActive(true);
                    i.transform.Find("Clickable").GetChild(x).gameObject.GetComponent<SearchScript>().SetState();
                }
            }
            else if(chk)
            {
                i.SetActive(false);
            }
        }
    }
    /*
    public IEnumerator FocusOn(Vector3 fposition, Vector3 frotation)
    {

        Vector3 position = AreaCamera.transform.position;
        Vector3 rotation = AreaCamera.transform.rotation.eulerAngles;
        Vector3 posdif = fposition - AreaCamera.transform.position;
        Vector3 rotdif = frotation - AreaCamera.transform.rotation.eulerAngles;
        if (rotdif.x > 180)rotdif.x -= 360;else if(rotdif.x < -180) rotdif.x += 360;
        if (rotdif.y > 180) rotdif.y -= 360; else if (rotdif.y < -180) rotdif.y += 360;
        if (rotdif.z > 180) rotdif.z -= 360; else if (rotdif.z < -180) rotdif.z += 360;
        float ax = 0.01f;
        for (float i = 0; i <= 0.5f; i += ax)
        {
            ax += 0.2f * Time.deltaTime;
            AreaCamera.position = position + posdif*i;
            AreaCamera.rotation = Quaternion.Euler(rotation + rotdif * i);
            yield return null;
        }
        for (float i = 0.5f; i <= 1f; i += ax)
        {
            if(ax > 0.02f)
            {
                ax -= 0.2f * Time.deltaTime;
            }
            AreaCamera.position = position + posdif * i;
            AreaCamera.rotation = Quaternion.Euler(rotation + rotdif * i);
            yield return null;
        }
        AreaCamera.position = position + posdif;
        AreaCamera.rotation = Quaternion.Euler(rotation + rotdif);
        yield break;
    }
    */

    IEnumerator FocusOn(bool Chk ,Vector3 fposition, Vector3 frotation)
    {
        SetDialogueActivate(false);
        //currentAnim(Chk).gameObject.SetActive(false);
        currentCamera(Chk).GetComponent<CameraTranslateScript>().cname = currentname;
        currentCamera(Chk).GetComponent<CameraTranslateScript>().motion = currentMotionFind(Chk);
        Debug.Log(currentMotionFind(Chk));
        yield return StartCoroutine(currentCamera(Chk).GetComponent<CameraTranslateScript>().FocusOnE(fposition, frotation));
        SetDialogueActivate(true);
        if (currentType != "이미지오프")
        {
            currentAnim(Chk).gameObject.SetActive(true);
            currentAnim(Chk).Play(currentMotionFind(Chk));
        }
        yield break;
    }
	// 색상 커맨드 출력
	string Colordic(string c){
		if (c == "red") {
			c = "#F15F5F";
		} else if (c == "blue") {
			c = "#B2CCFF";
		} else if (c == "pupple"){
			c = "#A566FF";
		} else if (c == "yellow"){
            c = "#FFE400";
        } else {
			c = "#FFFFFF";
		}
		return "<color=" + c + ">";
	}

	// - 대사 출력과 동시에 발동 하는 특수타입대사
	IEnumerator StartType(){
        if (currentType == "시스템")
        {
            Dialoguetxt.alignment = TextAnchor.UpperCenter;
        }
        else if (currentType == "등장")
        {
            Dialoguetxt.text = "";
            ChaAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animator/" + currentname);
            ChangeMotion(Node.Attributes.GetNamedItem("mot").Value);
            SetDialogueActivate(false);
            StopCoroutine(Sayvar);
            yield return StartCoroutine(ChaFaidIn());
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Sayvar);
            //Say();
        }
        else if (currentType == "카메라오프")
        {
            Pannel.gameObject.SetActive(true);
            Pannel.color = new Color(0.1f, 0.1f, 0.1f, 1);
            //MainCamera.gameObject.SetActive (false);
        }
        else if (currentType == "이미지오프")
        {
            ChaAnim.gameObject.SetActive(false);
        }
        else if (currentType == "이미지오프자동")
        {
            ChaAnim.gameObject.SetActive(false);
        }
        
        else if (currentType == "페이드전환")
        {
            SetDialogueActivate(false);
            yield return StartCoroutine(FaidIn(Pannel));
            yield return new WaitForSeconds(0.5f);
        }
        else if (currentType.Length > 1 && currentType.Substring(0, 1) == "S")
        {
            if (currentType.Substring(2) == "삭제")
            {
                RemoveImage();
            }
            else
            {
                if (currentType.Substring(1, 1) == "-")
                {
                    ShowImage(currentType.Substring(2));
                }
                else
                {
                    ShowImage(currentType.Substring(2), System.Convert.ToInt32(currentType.Substring(1, 1)));
                }
            }
        }
        else if (currentType == "페이드아웃" || currentType =="페이드아웃자동")
        {
            SetDialogueActivate(false);
            Pannel.color = Color.black;
            yield return StartCoroutine(FaidIn(Pannel));
            yield return new WaitForSeconds(0.5f);
            SetDialogueActivate(true);
        }
        else if (currentType == "페이드인" || currentType == "페이드인자동")
        {
            SetDialogueActivate(false);
            Pannel.color = Color.black;
            yield return StartCoroutine(FaidOut(Pannel));
            yield return new WaitForSeconds(0.5f);
            SetDialogueActivate(true);
        }
        else if (currentType == "난입")
        {
            currentType = "";
            Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/" + "임펙트"));
            SetDialogueActivate(false);
            Pannel.color = Color.black;
            StartCoroutine(FaidIn(Pannel, 0.6f));
            난입.gameObject.SetActive(true);
            난입.SetBool("확신", false);
            난입.transform.Find("난입이펙트").Find("Cha").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Item/" + currentname);
            if (RightChk)
            {
                난입.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                난입.transform.localScale = new Vector3(-1, 1, 1);
            }
            if (확신)
            {
                yield return new WaitForSeconds(난입.GetCurrentAnimatorStateInfo(0).length/2);
                난입.SetBool("확신", true);
                Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/" + "확신효과음"));
                yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(난입.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                yield return new WaitForSeconds(난입.GetCurrentAnimatorStateInfo(0).length);
            }
            난입.gameObject.SetActive(false);
            StartCoroutine(FaidOut(Pannel, 0.6f));
            SetDialogueActivate(true);
        }
	}

	// - 대사 출력이 끝남과 동시에 발동하는 특수타입대사
	IEnumerator EndType(){
		if (currentType == "카메라온") {
			Pannel.gameObject.SetActive (false);
			SetDialogueActivate (false);
			MainCamera.gameObject.SetActive (true);
			StopCoroutine (Sayvar);
			yield return StartCoroutine (LoadScene ());
			Say ();
		} else if (currentType == "퇴장") {
			SetDialogueActivate (false);
			StopCoroutine (Sayvar);
			yield return StartCoroutine (ChaFaidOut ());
			yield return new WaitForSeconds (1);
			Say ();
		}
        else if (currentType == "후등장")
        {
            SetDialogueActivate(false);
            StopCoroutine(Sayvar);
            yield return StartCoroutine(ChaFaidIn());
            yield return new WaitForSeconds(0.5f);
            Say();
        }
        else if (currentType == "페이드전환") {
			SetDialogueActivate (false);
			yield return StartCoroutine (FaidOut (Pannel));
			yield return new WaitForSeconds (0.5f);
			Say ();

		} else if (currentType == "자동" || currentType == "이미지오프자동" || currentType == "페이드인자동" || currentType == "페이드아웃자동") {
            Say();
        } else if(currentType == "제시"){
			Inventory.OpenInventory();
			Inventory.transform.Find("Show").gameObject.SetActive(true);
            if(Node.SelectNodes("종료").Count == 0)
            {
                Inventory.transform.Find("Quit").gameObject.SetActive(false);
            }
            Inventory.Panel.GetComponent<UnityEngine.UI.Button>().interactable = false;
			Inventory.ShowChk = true;
		}else if(!ReadChk && currentType.Length > 1 && currentType.Substring(0,1) == "I") {
            if (currentType.Substring(1, 1) == "C")
            {
                Inventory.GetItem(currentType.Substring(2),false);
            }
            else
            {
                Inventory.GetItem(currentType.Substring(2),true);
            }
		}
	}

	// - 클릭과 동시에 발동하는 특수타입대사
	IEnumerator ClickType(){
		if (currentType == "시스템") {
			Dialoguetxt.alignment = TextAnchor.UpperLeft;
		}else if(currentType.Length > 1 && currentType.Substring(0,1) == "P") {
			Singleton.Instance.phase = currentType.Substring(2);
            Debug.Log(Singleton.Instance.phase);
            if (currentType.Substring(2) == "Ending")
            {
                
                Invenbtt.gameObject.SetActive(false);
                DynamicScreen.Border.gameObject.SetActive(false);

            }
            //NumberDia = 0;
            //WorldUpdate(false);
		}
        else if (currentType == "월드업데이트")
        {
            WorldUpdate();
        }
        yield break;
	}

	// - 드롭텍스트 효과
	IEnumerator dropText(string txt,string color = ""){
		Droptxt.rectTransform.anchoredPosition = new Vector2 (Dialoguetxt.rectTransform.anchoredPosition.x + FieldSize.x, Dialoguetxt.rectTransform.anchoredPosition.y - Dialoguetxt.preferredHeight);
		if (txt == " ") {
			Droptxt.text = "j";
			FieldSize.x += Droptxt.preferredWidth;
			Droptxt.text = " ";
		} else {
			Droptxt.text = txt;
			FieldSize.x += Droptxt.preferredWidth;
		}
		if (color != "") {
			Droptxt.text = color + txt + "</color>";
		}
		for(float i = 2; i >=1; i -= 0.5f){
			Droptxt.rectTransform.localScale = new Vector3 (i, i, 1);
			yield return null;
		}
		Droptxt.rectTransform.localScale = new Vector3 (1, 1, 1);
	//	Droptxt.rectTransform.Translate (10000, 0, 0);
	}

	IEnumerator ChaFaidOut(){
		ChaImg.color = new Color (1, 1, 1, 1);
		for(float i = 1; i >= 0; i -= 0.05f) {
			yield return new WaitForSeconds (0.02f);
			ChaImg.color = new Color (1, 1, 1, i);
			Mouth.color = new Color (1, 1, 1, i);
		}
		ChaImg.color = new Color (1, 1, 1, 0);
		Mouth.color = new Color (1, 1, 1, 0);
		ChaAnim.gameObject.SetActive (false);
	}

	IEnumerator ChaFaidIn(){
		ChaAnim.gameObject.SetActive (true);
		ChaAnim.Play (currentMotion);
		ChaImg.color = new Color (1, 1, 1, 0);
		for(float i = 0; i <= 1; i += 0.05f) {
			yield return new WaitForSeconds (0.02f);
			ChaImg.color = new Color (1, 1, 1, i);
			Mouth.color = new Color (1, 1, 1, i);
		}
		ChaImg.color = new Color (1, 1, 1, 1);
		Mouth.color = new Color (1, 1, 1, 1);
	}
	

	IEnumerator FaidIn(UnityEngine.UI.Image target, float per = 1){
		target.gameObject.SetActive (true);
		target.color = new Color (target.color.r, target.color.b, target.color.g, 0);
		for(float i = 0; i < per; i += 0.03f) {
			target.color = new Color (target.color.r, target.color.b, target.color.g, i);
			yield return new WaitForSeconds (0.02f);
		}
	}


	IEnumerator FaidOut(UnityEngine.UI.Image target, float per = 1)
    {
        target.gameObject.SetActive(true);
        target.color = new Color (target.color.r, target.color.b, target.color.g, 1);
		for(float i = per; i >= 0; i -= 0.03f) {
			target.color = new Color (target.color.r, target.color.b, target.color.g, i);
			yield return new WaitForSeconds (0.02f);
		}
		target.gameObject.SetActive (false);
	}

	void SetDialogueActivate(bool chk,bool anim = true){
		Dialoguetxt.transform.parent.gameObject.SetActive (chk);
		Nametxt.transform.parent.gameObject.SetActive (chk);
        if (chk && anim)
        {
            StartCoroutine(SetDialogueActivateEnum(!ChaAnim.gameObject.activeSelf));
        }
    }

    IEnumerator SetDialogueActivateEnum(bool Chaact = false)
    {
        UnityEngine.UI.Image ChaImgimg = ChaImg.GetComponent<UnityEngine.UI.Image>();
        RectTransform ChaAnimrect = ChaAnim.GetComponent<RectTransform>();
        UnityEngine.UI.Image Mouthimg = Mouth.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image Dialoguetxtimg = Dialoguetxt.transform.parent.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image Nametxtimg = Nametxt.transform.parent.GetComponent<UnityEngine.UI.Image>();
        RectTransform Dialoguetxtrect = Dialoguetxtimg.GetComponent<RectTransform>();


        for (float i = 0; i < 1; i += 0.1f)
        {
            if (Chaact) {
                ChaImgimg.color = new Color(1, 1, 1, i);
                ChaAnimrect.anchoredPosition = new Vector2(0, 40 * i - 40);
                Mouthimg.color = new Color(1, 1, 1, i);
            }
            Dialoguetxt.color = new Color(1, 1, 1, i);
            Dialoguetxtimg.color = new Color(1, 1, 1, i);
            Nametxt.color = new Color(Nametxt.color.r, Nametxt.color.b, Nametxt.color.g, i);
            Nametxtimg.color = new Color(1, 1, 1, i);
            Dialoguetxtrect.anchoredPosition = new Vector2(0, 20 * i - 10);
            yield return new WaitForSeconds(0.02f);
        }
        if (Chaact)
        {
            ChaImgimg.color = new Color(1, 1, 1, 1);
            Mouthimg.color = new Color(1, 1, 1, 1);
            ChaAnimrect.anchoredPosition = new Vector2(0, 0);
        }
        Dialoguetxt.color = new Color(1, 1, 1, 1);
        Dialoguetxtimg.color = new Color(1, 1, 1,1);
        Nametxt.color = new Color(Nametxt.color.r, Nametxt.color.b, Nametxt.color.g, 1);
        Nametxtimg.color = new Color(1, 1, 1, 1);
        Dialoguetxtrect.anchoredPosition = new Vector2(0, 10);
    }

    // - 드롭텍스트 위치 설정
    void Sizeset(string txt){
		Droptxt.text = txt.Replace (" ", "j");
		FieldSize.x += Droptxt.preferredWidth;
	}

	IEnumerator CameraRotate(float Angle,bool ActiveChk = true){
		SetDialogueActivate (false);
        ChaAnim.gameObject.SetActive(false);
		for(float i = 0; i < 1; i += Time.deltaTime*2) {
            AreaCamera.transform.localRotation = Quaternion.Euler(AreaCamera.transform.localRotation.eulerAngles.x, AreaCamera.transform.localRotation.eulerAngles.y + Angle*Time.deltaTime*2, AreaCamera.transform.localRotation.eulerAngles.z);
            //AreaCamera.transform.Rotate(0,Angle/15,0);
            yield return null;
		}
        if (ActiveChk)
        {
            SetDialogueActivate(true);
            if (currentType != "이미지오프")
            {
                ChaAnim.gameObject.SetActive(true);
                ChaAnim.Play(currentMotionFind(RightChk));
            }
        }
    }

	public void CameraRotateBttFunction(float Angle){
        if (CR != null)
        {
            StopCoroutine(CR);
        }
        CR = CameraRotateEnumerator (Angle);
		GetComponent<MouseEventScript> ().enabled = false;
		GetComponent<MouseEventScript> ().SearchCursor.SetActive (false);
        StartCoroutine (CR);
	}

	public void CameraRotateBttUP(){
        if (CR != null)
        {
            GetComponent<MouseEventScript>().enabled = true;
            StopCoroutine(CR);
        }
	}

	IEnumerator CameraRotateEnumerator(float Angle){
		while (true) {
            AreaCamera.transform.localRotation = Quaternion.Euler(AreaCamera.transform.localRotation.eulerAngles.x, AreaCamera.transform.localRotation.eulerAngles.y + Angle*Time.deltaTime, AreaCamera.transform.localRotation.eulerAngles.z);
            //AreaCamera.transform.Rotate (0, Angle, 0);
			yield return null;
		}
	}

	IEnumerator Shake(float power, bool Chk = true){
        if (Chk)
        {
            RectTransform Chaanimrect = ChaAnim.GetComponent<RectTransform>();
            for (int i = 0; i < 5; i++)
            {
                MainCamera.transform.Translate(new Vector3(Random.Range(-power / 40, power / 40), Random.Range(-power / 40, power / 40), Random.Range(-power / 40, power / 40)));
                Chaanimrect.anchoredPosition = new Vector3(Random.Range(-power / 2, power / 2), Random.Range(-power / 2, power / 2), 0);
                yield return new WaitForSeconds(0.02f);
                MainCamera.transform.localPosition = Vector3.zero;
                Chaanimrect.anchoredPosition = Vector2.zero;
                yield return new WaitForSeconds(0.02f);
            }
        }
        else
        {
            Transform Trs = DynamicScreen.SecCam.transform;
            Vector3 Origin = Trs.localPosition;
            RectTransform SecAnimrect = SecAnim.GetComponent<RectTransform>();
            for (int i = 0; i < 5; i++)
            {
                Trs.Translate(new Vector3(Random.Range(-power / 40, power / 40), Random.Range(-power / 40, power / 40), Random.Range(-power / 40, power / 40)));
                SecAnimrect.anchoredPosition = Origin + new Vector3(Random.Range(-power / 2, power / 2), Random.Range(-power / 2, power / 2), 0);
                yield return new WaitForSeconds(0.02f);
                Trs.localPosition = Origin;
                SecAnimrect.anchoredPosition = Vector2.zero;
                yield return new WaitForSeconds(0.02f);
            }
        }
	}
    void Flash(float time)
    {
        배경("빠른플래시전환", RightChk);
    }
    /*
	void Flash(float time){
		if (FlashEnum != null) {
			StopCoroutine (FlashEnum);
		}
		FlashEnum = FlashFunction (time);
		StartCoroutine (FlashEnum);
	}

	IEnumerator FlashFunction(float time){
		Pannel.gameObject.SetActive (true);
		Pannel.color = Color.white;
		float minus = 1 / time;
		for(float i = 1; i >= 0; i -= minus){
			Pannel.color = new Color(1,1,1,i);
			yield return new WaitForSeconds(0.02f);
		}
		Pannel.gameObject.SetActive (false);
	}
    */
	void Say(){
		Sayvar = SayIEnumerator ();
		StartCoroutine (Sayvar);
	}

    public string Readname(bool mot = false){
        if (mot)
        {
            return currentMotionFind(RightChk);
        }else if (currentname != null) {
			return currentname;
		} else {
			return "";
		}
	}

	void ShowImage(string itname, int n = 0){

        if (n == 0)
        {
            UnityEngine.UI.Image Img = transform.parent.Find("Show0").GetComponent<UnityEngine.UI.Image>();
            if (itname == "제시")
            {
                Img.sprite = Resources.Load<Sprite>("Item/" + Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk)[Inventory.currentItem].name);
            }
            else
            {
                Img.sprite = Resources.Load<Sprite>("Item/" + itname);
            }
            Img.gameObject.SetActive(true);
            Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/확대"));
            Img.GetComponent<ImageAnimateScript>().Show();
        }
        else if (n == 3)
        {
            UnityEngine.UI.Image Img = transform.parent.Find("Show3").GetComponent<UnityEngine.UI.Image>();
            Img.sprite = Resources.Load<Sprite>("Item/" + itname);
            Img.gameObject.SetActive(true);
        }
        else
        {
            Transform Img = transform.parent.Find("Show"+n);
            if (itname == "제시")
            {
                Img.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Item/" + Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk)[Inventory.currentItem].name);
            }
            else
            {
                Img.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Item/" + itname);
            }
            Img.gameObject.SetActive(true);
            Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/확대"));
            Img.GetComponent<ImageAnimateScript>().Show();
        }
	}

	void RemoveImage(int n = -1){
        if (n == -1)
        {
            if (transform.parent.Find("Show0").gameObject.activeSelf) { transform.parent.Find("Show0").GetComponent<ImageAnimateScript>().Close(); }
            if (transform.parent.Find("Show1").gameObject.activeSelf) { transform.parent.Find("Show1").GetComponent<ImageAnimateScript>().Close(); }
            if (transform.parent.Find("Show2").gameObject.activeSelf) { transform.parent.Find("Show2").GetComponent<ImageAnimateScript>().Close(); }
            if (transform.parent.Find("Show3").gameObject.activeSelf) { transform.parent.Find("Show2").GetComponent<ImageAnimateScript>().Close(); }
        }
        else
        {
            transform.parent.Find("Show" + n).GetComponent<ImageAnimateScript>().Close();
        }
	}

    public void ChangeMotion(string Motion, bool RChk = true)
    {
        if (RChk)
        {
            
            Mouth.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            Sweat.SetActive(false);
            currentMotion = Motion;
            ChaAnim.Rebind();
            ChaAnim.Play(Motion);
            ChaAnim.transform.Find("SubImg").gameObject.SetActive(false);
            Debug.Log(currentMotion);
            Mouth.sprite = Resources.LoadAll<Sprite>(ChaAnim.runtimeAnimatorController.name + "입/" + ChaAnim.runtimeAnimatorController.name + currentMotion + "입")[0];
            Mouth.SetNativeSize();
            ChaAnim.SetBool("Stop", true);
        }
        else
        {
            Debug.Log(Motion);
            SecAnim.StopPlayback();
            SecMouth.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            SecAnim.transform.Find("SubImg").gameObject.SetActive(false);
            SecAnim.transform.Find("Sweat").gameObject.SetActive(false);
            secondMotion = Motion;
            SecAnim.Rebind();
            SecAnim.Play(Motion);
            SecMouth.sprite = Resources.LoadAll<Sprite>(SecAnim.runtimeAnimatorController.name + "입/" + SecAnim.runtimeAnimatorController.name + secondMotion + "입")[0];
            SecMouth.SetNativeSize();
            SecAnim.SetBool("Stop", true);
        }
    }

    Animator currentAnim(bool Chk)
    {
        if (Chk)
        {
            return ChaAnim;
        }
        else
        {
            return SecAnim;
        }
    }
    UnityEngine.UI.Image currentMouth(bool Chk)
    {
        if (Chk)
        {
            return Mouth;
        }
        else
        {
            return SecMouth;
        }
    }
    public string currentMotionFind(bool Chk, string mot=null)
    {
        if (Chk)
        {
            if(mot != null)
            {
                currentMotion = mot;
            }
            return currentMotion;
        }
        else
        {
            if (mot != null)
            {
                secondMotion = mot;
            }
            return secondMotion;
        }
    }
    Transform currentCamera(bool Chk)
    {
        if (Chk)
        {
            return AreaCamera;
        }
        else
        {
            return DynamicScreen.SecCam.transform.parent;
        }
    }
    
    void Notice(string txt, float t = 5f)
    {
        transform.parent.Find("Notice").GetComponent<NoticeScript>().Open(txt,t);
    }

    // 연출부

    IEnumerator 압박토크개시()
    {
        CameraTranslateScript currentCameraTranslatescript = currentCamera(RightChk).GetComponent<CameraTranslateScript>();
        UnityEngine.UI.Text atxt = 압박배틀.transform.Find("압박토크").GetComponent<UnityEngine.UI.Text>();
        atxt.text = "압박심문";
        atxt.color = new Color(0.227f, 0.317f, 0.741f, 1);
        압박배틀.transform.Find("마스크").Find("압박토크주제").GetComponent<UnityEngine.UI.Text>().text = Node.SelectSingleNode("연출").Attributes.GetNamedItem("주제").InnerText;
        배경("페이드인");
        currentCameraTranslatescript.FarAway();
        yield return new WaitForSeconds(0.3f);
        RuntimeAnimatorController RC = 압박배틀.runtimeAnimatorController;
        SetDialogueActivate(false);
        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/화악"));
        압박배틀.gameObject.SetActive(true);
        압박배틀.Play("압박토크");
        yield return new WaitForSeconds(RC.animationClips[0].length);
        압박배틀.Play("압박토크닫기");
        yield return new WaitForSeconds(RC.animationClips[1].length);
        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/둥"));
        압박배틀.Play("압박토크개시");
        currentCameraTranslatescript.StopAll();
        currentCameraTranslatescript.ZoomEffect();
        배경("플래시전환");
        yield return new WaitForSeconds(RC.animationClips[2].length);
        배경("종료");
        Singleton.Instance.CurrentShaderlist(RightChk).Clear();
        압박배틀.gameObject.SetActive(false);
        배경("!플래시전환");
        yield break;
    }
    IEnumerator 동의()
    {
        UnityEngine.UI.Text atxt = 압박배틀.transform.Find("압박토크").GetComponent<UnityEngine.UI.Text>();
        atxt.text = "동의";
        atxt.color = new Color(0.925f, 0.219f, 0.219f, 1);
        RuntimeAnimatorController RC = 압박배틀.runtimeAnimatorController;
        SetDialogueActivate(false);
        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/둥"));
        배경("플래시전환");
        배경("!플래시전환");
        압박배틀.gameObject.SetActive(true);
        압박배틀.Play("찬성반대");
        yield return new WaitForSeconds(RC.animationClips[3].length);
        압박배틀.gameObject.SetActive(false);
        SetDialogueActivate(true);
        yield break;
    }
    IEnumerator 반대()
    {
        UnityEngine.UI.Text atxt = 압박배틀.transform.Find("압박토크").GetComponent<UnityEngine.UI.Text>();
        atxt.text = "반대";
        atxt.color = new Color(0.227f, 0.317f, 0.741f, 1);
        RuntimeAnimatorController RC = 압박배틀.runtimeAnimatorController;
        SetDialogueActivate(false);
        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/둥"));
        배경("플래시전환");
        배경("!플래시전환");
        압박배틀.gameObject.SetActive(true);
        압박배틀.Play("찬성반대");
        Singleton.Instance.CurrentShaderlist(RightChk).Clear();
        yield return new WaitForSeconds(RC.animationClips[3].length);
        압박배틀.gameObject.SetActive(false);
        SetDialogueActivate(true);
        yield break;
    }

    IEnumerator 찬반시작()
    {
        CameraTranslateScript currentCameraTranslatescript = currentCamera(true).GetComponent<CameraTranslateScript>();
        CameraTranslateScript secondCameraTranslatescript = currentCamera(false).GetComponent<CameraTranslateScript>();

        SetDialogueActivate(false);
        UnityEngine.UI.Text atxt = 압박배틀.transform.Find("압박토크").GetComponent<UnityEngine.UI.Text>();
        atxt.text = "찬반논의";
        압박배틀.transform.Find("마스크").Find("압박토크주제").GetComponent<UnityEngine.UI.Text>().text = Node.SelectSingleNode("연출").Attributes.GetNamedItem("주제").InnerText;
        atxt.color = new Color(0.227f, 0.741f, 0.380f, 1);
        배경("페이드인");
        currentCameraTranslatescript.FarAway();
        secondCameraTranslatescript.FarAway();

        yield return new WaitForSeconds(0.3f);
        RuntimeAnimatorController RC = 압박배틀.runtimeAnimatorController;
        
        압박배틀.gameObject.SetActive(true);
        압박배틀.Play("압박토크");
        yield return new WaitForSeconds(RC.animationClips[0].length);
        압박배틀.Play("압박토크닫기");
        yield return new WaitForSeconds(RC.animationClips[1].length);
        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/둥"));
        배경("플래시전환");
        배경("!플래시전환");
        압박배틀.Play("압박토크개시");
        currentCameraTranslatescript.StopAll();
        currentCameraTranslatescript.ZoomEffect();
        secondCameraTranslatescript.StopAll();
        secondCameraTranslatescript.ZoomEffect();
        yield return new WaitForSeconds(RC.animationClips[2].length - 0.5f);
        {
            //yield return new WaitForSeconds(1f);
            //다이나믹씬이동("n-400");
            DynamicScreen.ChangeImmediatly(100);
            yield return new WaitForSeconds(0.05f);
            DynamicScreen.ChangeImmediatly(700, false);
            yield return new WaitForSeconds(0.05f);
            DynamicScreen.ChangeImmediatly(200);
            yield return new WaitForSeconds(0.05f);
            currentCameraTranslatescript.Cam.GetComponent<ShaderController>().찬반시작(ConvertColor(191, 191, 255, 255), ConvertColor(0, 128, 255, 255));
            secondCameraTranslatescript.Cam.GetComponent<ShaderController>().찬반시작(ConvertColor(255, 191, 191, 255), ConvertColor(255, 50, 0, 255));
            DynamicScreen.ChangeImmediatly(600, false);
            yield return new WaitForSeconds(0.05f);
            DynamicScreen.ChangeImmediatly(300);
            yield return new WaitForSeconds(0.05f);
            DynamicScreen.ChangeImmediatly(500, false);
            yield return new WaitForSeconds(0.05f);
            DynamicScreen.ChangeScreen(400, 5);
            yield return new WaitForSeconds(0.2f);
            Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/화악"));
            yield return new WaitForSeconds(1);
            배경("플래시전환");
            배경("!플래시전환");
        }
        압박배틀.gameObject.SetActive(false);
    }

    IEnumerator 회상()
    {
        SetDialogueActivate(false);
        int i = 0;
        float sec = 0.5f;
        int totalclip = Resources.LoadAll(currentname + "애니").Length;
        int totalarea = Resources.LoadAll<Sprite>("재고용배경").Length;
        Sprite[] bgil = Resources.LoadAll<Sprite>("재고용배경");
        UnityEngine.UI.Image bgi = DynamicScreen.transform.Find("배경이펙트").GetComponent<UnityEngine.UI.Image>();
        bgi.gameObject.SetActive(true);
        bgi.color = new Color(1, 1, 1, 1);
        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/화악"));
        yield return StartCoroutine(DynamicScreen.SecCam.GetComponent<ShaderController>().플래시페이드아웃E(1));
        DynamicScreen.ChangeImmediatly(800);
        DynamicScreen.SecCam.GetComponent<ShaderController>().재고상태(false);
        배경("지지직");
        배경("떠올리기");
        float v20 = voicevolume / 15;
        while (i < 20)
        {
            배경("빠른플래시전환");
            Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/화악"), v20*i);
            ChaAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animator/"+currentname);
            string ani = Resources.LoadAll<AnimationClip>(currentname + "애니")[Random.Range(0, totalclip)].name;
            ChaAnim.Play(ani);
            Debug.Log(currentname + ani + "입");
            Mouth.sprite = Resources.LoadAll<Sprite>(currentname + "입/" + currentname +ani+"입")[0];
            Mouth.SetNativeSize();
            bgi.sprite = bgil[Random.Range(0, totalarea)];
            i++;
            if (sec > 0.1f)
            {
                sec -= 0.05f;
            }
            else
            {
                sec = 0.1f;
            }
            yield return new WaitForSeconds(sec);
        }
        배경("종료");
        배경("플래시전환");
        배경("지지직");
        bgi.sprite = null;
        bgi.color = new Color(0,0,0,1);
        배경("쇼크웨이브");
    }

    IEnumerator 재고로()
    {
        SetDialogueActivate(false);
        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/화악"));
        배경("플래시페이드아웃");
        yield return StartCoroutine(MainCamera.GetComponent<ShaderController>().볼록렌즈E());
        배경("종료");
        DynamicScreen.ChangeImmediatly(0);
        DynamicScreen.transform.Find("배경이펙트").gameObject.SetActive(false);
        배경("플래시전환");
        배경("!플래시전환");
        DynamicScreen.SecCam.GetComponent<ShaderController>().재고상태(true);
        yield break;
    }

    IEnumerator 강조()
    {
#if UNITY_ANDROID
        if(PlayerPrefs.GetInt("VibeChk",1) == 1)
            Handheld.Vibrate();
#endif
        currentCamera(RightChk).GetComponent<CameraTranslateScript>().ZoomEffect();
        배경("블러포커스",RightChk);
        yield break;
    }
    IEnumerator 회상끝()
    {
        SetDialogueActivate(false);
        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/화악"));
        배경("플래시페이드아웃");
        yield return StartCoroutine(MainCamera.GetComponent<ShaderController>().볼록렌즈E());
        배경("종료");
        DynamicScreen.ChangeImmediatly(0);
        배경("!플래시전환");
        배경("플래시전환");
        Singleton.Instance.CurrentShaderlist(RightChk).Clear();
        yield break;
    }
    public static Color ConvertColor(int r, int g, int b, int a)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
    }
    

    void 배경(string Command, bool Chk = true)
    {
        ShaderController BgObj;
        string Bgt = Command;
        if (Bgt.Substring(0, 1) == "!")
        {
            Bgt = Bgt.Substring(1);
            //BgObj = DynamicScreen.SecPanner.transform.FindChild("배경이펙트").gameObject;
            //BgObj = currentCamera(false).GetComponent<CameraTranslateScript>().Cam.GetComponent<ShaderController>();
            BgObj = currentCamera(!Chk).GetComponent<CameraTranslateScript>().Cam.GetComponent<ShaderController>();
            Chk = !Chk;
        }
        else
        {
            //BgObj = DynamicScreen.transform.FindChild("배경이펙트").gameObject;
            //BgObj = MainCamera.GetComponent<ShaderController>();
            BgObj = currentCamera(Chk).GetComponent<CameraTranslateScript>().Cam.GetComponent<ShaderController>();
        }
        if (Bgt == "종료")
        {
            BgObj.Close();
            Singleton.Instance.CurrentShaderlist(RightChk).Clear();
        }
        else if (Bgt == "엔딩크레딧")
        {
            EndingCredit();
        }
        else
        {
            //BgObj.gameObject.SetActive(true);
            BgObj.SendMessage(Bgt);
        }
    }

    void 다이나믹씬이동(string ft)
    {
        if (!SecAnim.transform.parent.gameObject.activeSelf)
        {
            SecAnim.transform.parent.gameObject.SetActive(true);
            //ChangeMotion(secondMotion, false);
        }
        if (ft.Substring(1, 1) == "i")//즉시
        {
            DynamicScreen.ChangeImmediatly((float)System.Convert.ToDouble(ft.Substring(2)));
        }
        else if (ft.Substring(1, 1) == "q")//빠르게
        {
            DynamicScreen.ChangeScreen((float)System.Convert.ToDouble(ft.Substring(2)), 3);
        }
        else if (ft.Substring(1, 1) == "c")
        {
            DynamicScreen.ChangeScreen((float)System.Convert.ToDouble(ft.Substring(2)), 3);
            if (RightChk)
            {
                AreaCamera.GetComponent<CameraTranslateScript>().FocusOnImmediatly(Singleton.Instance.PosContainer.Find(currentname).GetComponent<SearchScript>().CPosition
                    , Singleton.Instance.PosContainer.Find(currentname).GetComponent<SearchScript>().CRotation);
            }
            else
            {
                DynamicScreen.SecCam.transform.parent.GetComponent<CameraTranslateScript>().FocusOnImmediatly(Singleton.Instance.PosContainer.Find(currentname).GetComponent<SearchScript>().CPosition
                    , Singleton.Instance.PosContainer.Find(currentname).GetComponent<SearchScript>().CRotation);
            }
        }
        else
        {
            DynamicScreen.ChangeScreen((float)System.Convert.ToDouble(ft.Substring(2)));
        }
    }

    IEnumerator SayIEnumerator(){
        bool diaActiveChk = true;
        if (Dialoguetxt.transform.parent.gameObject.activeSelf == true)
        {
            SetDialogueActivate(false);
            diaActiveChk = false;
        }
        Invenbtt.SetBool("SChk", false);
        SayChk = true;
        //Node = xmldoc.SelectSingleNode("/root/"+Singleton.Instance.stage+"_"+Singleton.Instance.phase).ChildNodes[NumberDia];
        Debug.Log (Singleton.Instance.stage + "-"+Singleton.Instance.phase + "," +TagName + "," + NumberDia);
		Node = xmldoc.SelectSingleNode("/root/"+Singleton.Instance.stage+"-"+Singleton.Instance.phase).SelectNodes(TagName)[NumberDia] as XmlElement;
        //if (!Singleton.Instance.isNewGame) DialogueCompare(); //업데이트시 대화비교
        if (Node.Attributes.GetNamedItem("name") != null)
        {
            currentname = Node.Attributes.GetNamedItem("name").Value;
            if (currentname.Substring(0, 1) == ".")
            {
                RightChk = false;
                currentname = currentname.Substring(1);
            }
            else if(currentname == "김탐정")
            {
                RightChk = false;
                //if (DynamicScreen.SecCam.gameObject.activeSelf || Node.SelectSingleNode("text").InnerText.Contains("/n"))
                
                if (DynamicScreen.SecPanner.sizeDelta.x > 0.001f || Node.SelectSingleNode("text").InnerText.Contains("/n"))
                {
                    RightChk = false;
                }
                else
                {
                    RightChk = true;
                }
                
            }
            else
            {
                RightChk = true;
            }
        }
        else
        {
            currentname = null;
            RightChk = true;
        }
        Debug.Log("RGHTCHK = " + RightChk);
        Debug.Log("rc = " + RightChk);
        UnityEngine.UI.Image fmouth = currentMouth(RightChk);
        Animator fanimator = currentAnim(RightChk);
        string fmotion = currentMotionFind(RightChk);
        if (Node.SelectNodes("조건").Count > 0)
        {
            int total = Node.SelectNodes("조건").Count;
            for (int i = 0; i < total; i++)
            {
                string needtxt = Node.SelectNodes("조건")[i].InnerText;
                int n = needtxt.IndexOf(",");
                if (!Singleton.Instance.IsHave(needtxt.Substring(0, n), System.Convert.ToInt32(needtxt.Substring(n + 1))))
                {
                    needtxt = Node.SelectNodes("조건")[0].Attributes.GetNamedItem("이동").Value;
                    ConvertChange(needtxt);
                    Say();
                    yield break;
                }
            }
        }
        if (Node.SelectNodes("연출").Count > 0)
        {
            string needtxt = Node.SelectNodes("연출")[0].InnerText;
            yield return StartCoroutine(needtxt);
            /*
            if(needtxt == "회상")
            {
                yield return StartCoroutine(회상());
            }
            else
            {
                yield return StartCoroutine(회상());
            }
            */
        }
        if (Node.SelectNodes("읽음조건").Count > 0)
        {
            int total = Node.SelectNodes("읽음조건").Count;
            for (int i = 0; i < total; i++)
            {
                string needtxt = Node.SelectNodes("읽음조건")[i].InnerText;
                if (needtxt.Substring(0, 1) == "!")
                {
                    needtxt = needtxt.Substring(1);
                    if (Node.ParentNode.SelectNodes(needtxt)[0].Attributes.GetNamedItem("read") == null)
                    {
                        needtxt = Node.SelectNodes("읽음조건")[0].Attributes.GetNamedItem("이동").Value;
                        ConvertChange(needtxt);
                        Say();
                        yield break;
                    }
                }
                else
                {
                    if (Node.ParentNode.SelectNodes(needtxt)[0].Attributes.GetNamedItem("read") != null)
                    {
                        needtxt = Node.SelectNodes("읽음조건")[0].Attributes.GetNamedItem("이동").Value;
                        ConvertChange(needtxt);
                        Say();
                        yield break;
                    }
                }
            }
        }
        if (Node.SelectNodes("이미지").Count > 0)
        {
            int total = Node.SelectNodes("이미지").Count;
            for (int i = 0; i < total; i++)
            {
                string needtxt = Node.SelectNodes("이미지")[i].InnerText;
                int n = needtxt.IndexOf(",");
                if (needtxt.Length >= 2 &&  needtxt.Substring(0,2) == "삭제")
                {
                    Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/축소"));
                    if (n >= 0)
                    {
                        RemoveImage(System.Convert.ToInt32(needtxt.Substring(n + 1)));
                    }
                    else
                    {
                        RemoveImage();
                    }
                }
                else
                {
                    if (n >= 0)
                    {
                        ShowImage(needtxt.Substring(0, n), System.Convert.ToInt32(needtxt.Substring(n + 1)));
                    }
                    else
                    {
                        ShowImage(needtxt);
                    }
                }
            }
        }
        if (Node.SelectNodes("선택").Count > 0)
        {
            string mtxt = Node.SelectSingleNode("선택").InnerText;
            GameObject obj = transform.parent.Find("Select").gameObject;
            obj.SetActive(true);
            obj.GetComponent<StringContainer>().str = mtxt;
        }
        else
        {
            transform.parent.Find("Select").gameObject.SetActive(false);
        }
        if (Node.Attributes.GetNamedItem ("read") == null) {
			ReadChk = false;
		} else {
			ReadChk = true;
		}
		//- 대화타입 세팅
		if (Node.Attributes.GetNamedItem ("type") != null) {
			currentType = Node.Attributes.GetNamedItem ("type").Value;
			yield return StartCoroutine(StartType());
		} else {
			currentType = "";
		}
        if (Node.SelectNodes("브금").Count > 0)
        {
            string bgt = Node.SelectSingleNode("브금").InnerText;
            if (Node.SelectSingleNode("브금").Attributes.GetNamedItem("인트로") == null)
            {
                if (bgt == "종료")
                {
                    GetComponent<SoundManagerScript>().bgmFaider(false);
                    //StartCoroutine(bgmVolumeFaider(false));
                }
                else if (bgt == "피치업")
                {
                    Audios[1].pitch += 0.1f;
                }
                else if (bgt == "피치")
                {
                    Audios[1].pitch = 1;
                }
                else
                {
                    Audios[1].clip = Resources.Load<AudioClip>("BGM/" + bgt);
                    Audios[1].time = 0;
                    Audios[1].Play();
                    GetComponent<SoundManagerScript>().bgmFaider(true);
                }
            }
            else
            {
                GetComponent<SoundManagerScript>().NewStart();
                GetComponent<SoundManagerScript>().bgmIntro(bgt);
                GetComponent<SoundManagerScript>().bgmFaider(true);
            }
            //StartCoroutine(bgmVolumeFaider(true));
        }
        if (Node.SelectNodes("퀵브금").Count > 0)
        {
            string bgt = Node.SelectSingleNode("퀵브금").InnerText;
            GetComponent<SoundManagerScript>().NewStart();
            if (bgt == "종료")
            {
                GetComponent<SoundManagerScript>().bgmFaider(false);
                //StartCoroutine(bgmVolumeFaider(false));
            }
            else
            {
                Audios[1].time = 0;
                Audios[1].clip = Resources.Load<AudioClip>("BGM/" + bgt);
                Audios[1].Play();
                Audios[1].volume = 0.6f;
            }
        }
        if (Node.SelectNodes("배경").Count > 0)
        {
            int total = Node.SelectNodes("배경").Count;
            for (int i = 0; i < total; i++)
            {
                string Bgt = Node.SelectNodes("배경")[i].InnerText;
                배경(Bgt, RightChk);
            }
            
        }
        if (Node.SelectNodes("포커스").Count > 0)
        {
            SearchScript target = Singleton.Instance.PosContainer.Find(Node.SelectSingleNode("포커스").InnerText).GetComponent<SearchScript>();
            StartCoroutine(currentCamera(RightChk).GetComponent<CameraTranslateScript>().FocusOnE(target.CPosition, target.CRotation));
           // FocusOn(RightChk, target.CPosition, target.CRotation);
            Debug.Log("포커스 : " + target.CPosition);
        }
        if (Node.SelectNodes("키워드").Count > 0)
        {
            string kt = Node.SelectSingleNode("키워드").InnerText;
            if (kt == "종료")
            {
                DeleteKeyword();
            }
            else
            {
                getKeyword(kt);
            }
        }
        if (Node.SelectNodes("튜토리얼").Count > 0 && Node.GetAttribute("read") != "true")
        {
            int kt = System.Convert.ToInt16(Node.SelectSingleNode("튜토리얼").InnerText);
            Tutorialcont.gameObject.SetActive(true);
            Tutorialcont.tutorial(kt);
        }
        /*
        if (Dialoguetxt.transform.parent.gameObject.activeSelf == false)
        {
            SetDialogueActivate(true);
        }
        */
        //- 이름의 존재 여부 판별
        string txt = Node.SelectSingleNode("text").InnerText;


        if (currentname == null) {
			Nametxt.transform.parent.gameObject.SetActive (false);
			MouthMoveChk = false;
		} else {
			Nametxt.transform.parent.gameObject.SetActive (true);
			if(currentType == "???"){
				Nametxt.text = "???";
			}else{
				Nametxt.text = currentname;
			}

            //-이름에 따른 캐릭터이미지표시
            Debug.Log(currentname);
            if (Resources.Load("Animator/"+ currentname) == null || currentType == "목소리") {
				//ChaImg.transform.parent.gameObject.SetActive (false);
				MouthMoveChk = false;
			} else {
				//if(currentType != "등장"){
				//}
				//ChaImg.sprite = Resources.Load<Sprite> (Node.Attributes.GetNamedItem ("name").Value);
				//ChaImg.SetNativeSize ();
				if(Node.Attributes.GetNamedItem("mot") != null){
                    ChaAnim.gameObject.SetActive(true);
                    fmotion = currentMotionFind(RightChk, Node.Attributes.GetNamedItem("mot").Value);
                    if (fanimator.runtimeAnimatorController.name != currentname && Singleton.Instance.PosContainer != null && Singleton.Instance.PosContainer.Find(currentname) != null)
                    {
                        if(txt.Length > 3 && txt.Substring(0, 2) == "/n")
                        {

                            int endsh = txt.IndexOf("/", 1);
                            Debug.Log(txt.Substring(1, endsh-1));
                            다이나믹씬이동(txt.Substring(1, endsh-1));
                            txt = txt.Remove(0, endsh+1);
                            Debug.Log(txt);
                        }
                        if (txt.Length == 0)
                        {
                            SearchScript target = Singleton.Instance.PosContainer.Find(currentname).GetComponent<SearchScript>();
                            StartCoroutine(FocusOn(RightChk, target.CPosition, target.CRotation));
                        }
                        else
                        {
                            SearchScript target = Singleton.Instance.PosContainer.Find(currentname).GetComponent<SearchScript>();
                            yield return StartCoroutine(FocusOn(RightChk, target.CPosition, target.CRotation));
                        }
                    }
                    else
                    {
                        if (fanimator.runtimeAnimatorController.name != currentname)
                        {
                            
                            fanimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animator/" + currentname);
                        }
                        ChangeMotion(Node.Attributes.GetNamedItem("mot").Value, RightChk);
                        
                    }
                }
                /*
				if(fmotion == null || fmotion == ""){
					for(int i = NumberDia-1; i > 0 ; i--){
						if(Node.ParentNode.ChildNodes[i].Attributes.GetNamedItem("mot") != null){
							fmotion = currentMotionFind(RightChk, Node.ParentNode.ChildNodes[i].Attributes.GetNamedItem("mot").Value);
							fanimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animator/" + currentname);
                            fanimator.Play(currentMotion);
							break;
						}
					}
				}
                */
                if (RightChk && ChaAnim.runtimeAnimatorController.name != currentname)
                {
                    MouthMoveChk = false;
                }
                else
                {
                    MouthMoveChk = true;
                }
			}
		}
        //- 타이핑 전 초기 셋팅
        fmotion = currentMotionFind(RightChk);
		int pr = 0;
		List<string> Colorlist = new List<string> ();
		List<int> Colornavi = new List<int> ();
		Dialoguetxt.text = "";
		FieldSize = new Vector2(0,Droptxt.preferredHeight + 10);
		int lw = 0;
		string type = "";
		int FlashChk = 0;
		int ShakeChk = 0;
        bool MouthReal = MouthMoveChk;
        Debug.Log(MouthReal);
        if (currentType == "시스템")
        {
            Audios[0].clip = Resources.Load<AudioClip>("Sound/시스템메세지");
        }else if (currentname != null && Resources.Load("Sound/" + currentname + "목소리") != null)
        {
            Audios[0].clip = Resources.Load<AudioClip>("Sound/" + currentname + "목소리");
        }
        else
        {
            Audios[0].clip = Resources.Load<AudioClip>("Sound/대화점여자");
        }
        
        if (txt.Length == 0)
        {
            SetDialogueActivate(false);
        }
        
        if (MouthMoveChk) { fanimator.SetBool("Talk", true); }
        Dialoguetxt.transform.parent.GetComponent<RectTransform>().sizeDelta = SayBoxSize;
        //- 대사 타이핑
        
        for (int i = 0; i < txt.Length; i ++) {
            if (fanimator.enabled && fanimator.GetBool("Stop"))
            {
                yield return new WaitForSeconds(0.02f);
                fanimator.SetBool("Stop", false);
                while (true)
                {
                    if (fanimator.GetCurrentAnimatorStateInfo(0).IsTag("정지"))
                    {
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // - 명령어 실행(d-딜레이, c-색, t-텍스트효과, s-효과음, r-카메라회전, m-모션변경, a-에리어변경, n-다이나믹스크린, f - 플래쉬)
            if (txt.Substring(i,1) == "/"){
				int endsh = txt.IndexOf("/",i+1);
				string ft = txt.Substring(i+1,endsh-i-1);
				string command = ft.Substring(0,1);
                if (command == "d") {
                    if (MouthReal == true)
                        fmouth.sprite = Resources.LoadAll<Sprite>(currentname + "입/" + currentname + fmotion + "입")[0];
                    yield return new WaitForSeconds((float)(System.Convert.ToDouble(ft.Substring(2)) * 0.1));
                } else if (command == "c") {
                    Colorlist.Add(Colordic(ft.Substring(2)));
                    if (MouthMoveChk)
                    {
                        if (ft.Substring(2) == "blue")
                        {
                            MouthReal = false;
                        }
                        else
                        {
                            MouthReal = true;
                        }
                    }
                    Colornavi.Add(i);
                }
                else if (command == "f")
                {
                    배경("플래시전환", RightChk);
                }
                else if (command == "t") {
                    type = ft.Substring(2);
                    if (ft.Substring(1, 1) == "f")
                    {
                        FlashChk = 5;
                        ShakeChk = 0;
                    }
                    else if (ft.Substring(1, 1) == "s")
                    {
                        ShakeChk = 10;
                        FlashChk = 0;
                    }
                    else if (ft.Substring(1, 1) == "b")
                    {
                        StartCoroutine(강조());
                        FlashChk = 5;
                        ShakeChk = 10;
                    }
                    else if (ft.Substring(1, 1) == "o")
                    {
                        FlashChk = 5;
                        ShakeChk = 10;
                    }
                    else if (ft.Substring(1, 1) == "ㄱ")
                    {
                        StartCoroutine(강조());
                    }
                    else
                    {
                        FlashChk = 0;
                        ShakeChk = 0;
                    }
                    if (type == "drop") {
                        Sizeset(txt.Substring(lw, i - lw));
                        lw = i;
                    }
                } else if (command == "s") {
                    //Audios.volume = 1;
                    //Audios.pitch = 1;
                    //Audios.clip = Resources.Load<AudioClip>("Sound/" + ft.Substring(2));
                    //Audios.Play();
                    Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/" + ft.Substring(2)));
                } else if (command == "r") {
                    bool activechk = true;
                    if (txt.Length > endsh + 1)
                    {
                        //Debug.Log(txt.Substring(endsh +1, 2));
                        //Debug.Log(txt.Length +"+" + (endsh+6));
                        if (txt.Length > endsh + 7 && (txt.Substring(endsh + 1, 2) == "/d"  || txt.Substring(endsh + 1, 2) == "/r"))
                        {
                            activechk = false;
                        }
                    }
                    yield return StartCoroutine(CameraRotate((float)System.Convert.ToDouble(ft.Substring(2)),activechk));
                } else if (command == "m") {
                    if (ft.Substring(1, 1) == "!")
                    {
                        ChangeMotion(ft.Substring(2),!RightChk);
                        currentMotionFind(!RightChk, ft.Substring(2));
                    }
                    else
                    {
                        ChangeMotion(ft.Substring(2), RightChk);
                        fmotion = currentMotionFind(RightChk);
                    }
                } else if (command == "a") {
                    if (ft.Substring(1, 1) == "q") {
                        MoveArea(ft.Substring(2));
                    }
                    else if (ft.Substring(1, 1) == "b")
                    {
                        MoveArea(ft.Substring(2), true);
                    }
                    else {
                        MoveArea(ft.Substring(2));
                        yield return StartCoroutine(LoadScene());
                        NumberDia++;
                        Say();
                        yield break;
                    }
                } else if (command == "n")
                {
                    다이나믹씬이동(ft);
                }
                txt = txt.Remove(i, endsh - i + 1);
                i--;
                continue;
			}
            // 임시
            if(i == 0)
            {
                if (txt.Length == 0)
                {
                    SetDialogueActivate(false);
                }
                else if (Dialoguetxt.transform.parent.gameObject.activeSelf == false)
                {
                    SetDialogueActivate(true,diaActiveChk);
                    
                }
            }

            TextIndex = i;
            if (FieldSize.y < Dialoguetxt.preferredHeight){
                if (Dialoguetxt.cachedTextGenerator.lineCount > 3)
                {
                    Dialoguetxt.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, SayBoxSize.y + 35* (Dialoguetxt.cachedTextGenerator.lineCount-3));
                }
                FieldSize.y = Dialoguetxt.preferredHeight +1;
				FieldSize.x = 0;
				lw = Mathf.Max(txt.LastIndexOf("\n",i),txt.LastIndexOf(" ",i));
				Droptxt.rectTransform.Translate(0,10000,0);
				if(i-lw > 1){
					Sizeset(txt.Substring(lw+1,i-lw-1));
				}
				lw = i;
			}
			if(type == "drop"){
				if(Colornavi.Count >0){
					StartCoroutine(dropText(txt.Substring(i,1),Colorlist[Colorlist.Count-1]));
				}else{
					StartCoroutine(dropText(txt.Substring(i,1)));
				}
				if(i == txt.Length -1){
					Dialoguetxt.text = txt.Substring(0,i+1);
				}else{
					Dialoguetxt.text = txt.Substring(0,i);
				}
			}else{
				Dialoguetxt.text = txt.Substring(0,i+1);
            }

            // - 대사타이핑
            if (Colornavi.Count >0){
				for(int c = 0; c < Colornavi.Count; c++){

					Dialoguetxt.text = Dialoguetxt.text.Insert(Colornavi[c]+c*15,Colorlist[c]);
					
					Dialoguetxt.text = Dialoguetxt.text.Insert(Dialoguetxt.text.Length,"</color>");
				}
			}
            // - 입모양 변화와 타이핑소리
            
            float s = (float)i/3;
            if (txt.Substring(i, 1) != " ")
            {
                
                if (type == "drop")
                {
                    Audios[0].volume = voicevolume*1.5f;
                    //Audios.pitch = Random.Range(1, 1.02f);
                    //Audios.pitch = (1 + (s - Mathf.FloorToInt(s)) * 0.2f);
                }
                
                else
                {
                    //Audios.volume = Random.Range(0.04f, 0.3f);
                    Audios[0].volume = voicevolume;
                    // Audios.pitch = Random.Range(1, 1.02f);
                    //Audios.pitch = (1 + (s - Mathf.FloorToInt(s)) * 0.1f);
                }
                
                Audios[0].Play();
            }
            if (s == Mathf.FloorToInt(s)){
                
                if (ShakeChk > 0)
                {
                    StartCoroutine(Shake(ShakeChk,RightChk));
                }
                if (FlashChk > 0)
                {
                    Flash(FlashChk);
                }
                if (MouthReal == true /*&& txt.Substring(i, 1) != " "*/) {
					int r = Random.Range(0,3);
					if(r == pr){
						if(r != 2){
							r ++;
						}else{
							r = 0;
						}
					}
					pr = r;
					fmouth.sprite = Resources.LoadAll<Sprite> (currentname + "입/" + currentname + fmotion + "입")[r];
				}
			}
			yield return new WaitForSeconds(0.04f);
		}

        // - 종료
		EndSay ();
	}

	// - 대사창 클릭
	public void NextDia(){
        if (SayChk == false && Singleton.Instance.phase != "Ending") {
			StartCoroutine(ClickType());
			if (NumberDia < xmldoc.SelectSingleNode ("/root/"+Singleton.Instance.stage+"-"+Singleton.Instance.phase).SelectNodes(TagName).Count) {
				SayChk = true;
				Say();
                
			}else{
				StartSearch();
			}
		} else if((ReadChk || FreeSkip) && Node.SelectNodes("선택지").Count == 0 && Singleton.Instance.phase != "Ending"){
			StopCoroutine(Sayvar);
            SayChk = false;
            SkipDia();
		}
	}

	public void StartDialog(string cover, int num = 0){
		SayChk = true;
		TagName = cover;
		NumberDia = num;
		transform.parent.Find ("RightArrow").gameObject.SetActive (false);
		transform.parent.Find ("LeftArrow").gameObject.SetActive (false);
		transform.parent.Find ("Move").gameObject.SetActive (false);
		GetComponent<MouseEventScript> ().enabled = false;
        Say();
        //SetDialogueActivate(true);
    }
	
	void StartSearch(){
		ChaAnim.gameObject.SetActive(false);
		SetDialogueActivate(false);
        AreaCamera.GetComponent<CameraTranslateScript>().Cam.transform.localPosition = Vector3.zero;
		transform.parent.Find ("RightArrow").gameObject.SetActive (true);
		transform.parent.Find ("LeftArrow").gameObject.SetActive (true);
		transform.parent.Find ("Move").gameObject.SetActive (true);
		GetComponent<MouseEventScript> ().enabled = true;
        AreaCamera.GetComponent<CameraTranslateScript>().FocusOn(AreaPosition(Singleton.Instance.area), AreaRotation(Singleton.Instance.area), false);
        SaveGame();
	}

	// - 타이핑 스킵
	void SkipDia(){
		string txt = Node.SelectSingleNode("text").InnerText;
		int cc = 0;
        Debug.Log(txt);
        while (true) {
			int startsh = txt.IndexOf("/");
			int endsh = txt.IndexOf("/",startsh+1);
            if (endsh >=0){
				string ft = txt.Substring (startsh + 1, endsh - startsh - 1);
				txt = txt.Remove(startsh,endsh-startsh+1);
                string command = ft.Substring(0, 1);
                if (command == "c")
                {
                    cc++;
                    txt = txt.Insert(startsh, Colordic(ft.Substring(2)));
                } else if (startsh > TextIndex) {
                    if (command == "a")
                    {
                        if (ft.Substring(1, 1) == "b")
                        {
                            MoveArea(ft.Substring(2),true);
                        }
                        else
                        {
                            MoveArea(ft.Substring(2));
                        }
                    }
                    else if (command == "m")
                    {
                        if (ft.Substring(1, 1) == "!")
                        {
                            ChangeMotion(ft.Substring(2), !RightChk);
                            currentAnim(!RightChk).SetBool("Talk", true);
                        }
                        else
                        {
                            ChangeMotion(ft.Substring(2), RightChk);
                            currentAnim(RightChk).SetBool("Talk", true);
                        }
                    }
                    else if (command == "n")
                    {
                        if (!SecAnim.transform.parent.gameObject.activeSelf)
                        {
                            SecAnim.transform.parent.gameObject.SetActive(true);
                            ChangeMotion(secondMotion, false);
                        }
                        DynamicScreen.ChangeImmediatly((float)System.Convert.ToDouble(ft.Substring(2)));
                    }
                    else if (command == "r")
                    {
                        float Angle = (float)System.Convert.ToDouble(ft.Substring(2));
                        currentCamera(RightChk).transform.localRotation = Quaternion.Euler(AreaCamera.transform.localRotation.eulerAngles.x, AreaCamera.transform.localRotation.eulerAngles.y + Angle, AreaCamera.transform.localRotation.eulerAngles.z);
                    }
                }
            }
            else{
				if(cc >0){
					for(int i = 0; i < cc; i ++){
					txt = txt.Insert (txt.Length,"</color>");
					}
				}
				break;
			}
		}
		Dialoguetxt.text = txt;
        Dialoguetxt.Rebuild(UnityEngine.UI.CanvasUpdate.PreRender);
        if (Dialoguetxt.cachedTextGenerator.lineCount > 3)
        {
            Dialoguetxt.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, SayBoxSize.y + 35 * (Dialoguetxt.cachedTextGenerator.lineCount - 3));
        }
        EndSay ();
	}

	void EndSay(){
        if (Node.GetAttribute("read") != "false") { Node.SetAttribute("read", "true"); }
        if (MouthMoveChk == true)
        {
            currentMouth(RightChk).sprite = Resources.LoadAll<Sprite>(currentname + "입/" + currentname + currentMotionFind(RightChk) + "입")[0];
            currentAnim(RightChk).SetBool("Talk", false);
        }
        // - 종료
        if (Node.SelectNodes("종료").Count == 1)
        {
            transform.parent.Find("Quit").GetComponent<SelectButtonScript>().SelectChk = true;
            if (Node.SelectSingleNode("종료").InnerText == "종료")
            {
                transform.parent.Find("Quit").GetComponent<SelectButtonScript>().TagName = "종료";
                transform.parent.Find("Quit").gameObject.SetActive(true);
            }
            else
            {
                string txt = Node.SelectSingleNode("종료").InnerText;
                ConvertChange(txt);
                int n = Node.SelectSingleNode("종료").InnerText.IndexOf(",");
                if (n > 0)
                {
                    transform.parent.Find("Quit").GetComponent<SelectButtonScript>().TagName = txt.Substring(0, n);
                    transform.parent.Find("Quit").GetComponent<SelectButtonScript>().Index = System.Convert.ToInt32(txt.Substring(n + 1));
                }
                else
                {
                    transform.parent.Find("Quit").GetComponent<SelectButtonScript>().TagName = txt;
                    transform.parent.Find("Quit").GetComponent<SelectButtonScript>().Index = 0;
                }
                transform.parent.Find("Quit").gameObject.SetActive(true);
            }
        }
        if (Node.SelectNodes("정보추가").Count > 0)
        {
            string txt = Node.SelectSingleNode("정보추가").InnerText;
            int n = Node.SelectSingleNode("정보추가").InnerText.IndexOf(",");
            Inventory.LevelUpItem(txt.Substring(0, n), System.Convert.ToInt32(txt.Substring(n + 1)));
        }
        if (Node.SelectNodes("알림").Count > 0)
        {
            XmlElement ele = Node.SelectSingleNode("알림") as XmlElement;
            if (ele.GetAttribute("read") != "true")
            {
                if(ele.Attributes.GetNamedItem("시간") != null)
                {
                    Notice(Node.SelectSingleNode("알림").InnerText, (float)System.Convert.ToDouble(ele.GetAttribute("시간")));
                    if (ele.GetAttribute("read") != "false")
                    {
                        ele.SetAttribute("read", "true");
                    }
                }
                else
                {
                    Notice(Node.SelectSingleNode("알림").InnerText);
                    if (ele.GetAttribute("read") != "false")
                    {
                        ele.SetAttribute("read", "true");
                    }
                }
            }
        }
        if (Node.SelectNodes("데미지").Count > 0)
        {
            float dmg = (float)System.Convert.ToDouble(Node.SelectSingleNode("데미지").InnerText);
            if (확신) { dmg *= 2; 확신 = false; }
            Shake(20,false);
            Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/공격"));
            HpGuage.Damage(dmg*0.01f);
            Singleton.Instance.hp -= dmg;
            if(Singleton.Instance.hp <= 0)
            {
                StartDialog("사망");
            }
        }
        if (Node.SelectNodes("회복").Count > 0)
        {
            if (Node.SelectSingleNode("회복").InnerText == "확신")
            {
                if (확신 == true)
                {
                    Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/반짝"));
                    HpGuage.Heal(0.1f);
                    Singleton.Instance.hp += 0.1f;
                    확신 = false;
                }
            }
            else
            {
                float dmg = (float)System.Convert.ToDouble(Node.SelectSingleNode("회복").InnerText);
                HpGuage.Damage(dmg * 0.01f);
                Singleton.Instance.hp += dmg;
            }
        }
        if (Node.SelectNodes("기억").Count > 0)
        {
            if (Node.SelectSingleNode("기억").InnerText != "")
            {
                SaveGame(true);
            }
            else
            {
                Remember = TagName + "," + NumberDia;
            }
        }
        if (Node.SelectNodes("제시실패").Count > 0)
        {
            Invenbtt.SetBool("SChk", true);
        }
        // - 종료
        if (Node.SelectNodes("선택지").Count == 0) {
			SayChk = false;
			NumberDia ++;
			Droptxt.rectTransform.anchoredPosition = new Vector3 (0, 10000, 0);
			if (Node.SelectNodes("이동").Count >0){
				string txt = Node.SelectSingleNode("이동").InnerText;
                ConvertChange(txt);
            }
            if (Node.SelectNodes("액션").Count > 0)
            {
                string txt = Node.SelectSingleNode("액션").InnerText;
                if (txt == "압박")
                {
                    /* 구버전
                    txt = Node.SelectSingleNode("액션").Attributes.GetNamedItem("횟수").InnerText;
                    if (txt == "")
                    {
                        pressCount = 99999;
                    }
                    else
                    {
                        pressCount = System.Convert.ToInt32(txt);
                    }
                    압박에임.SetActive(true);
                    PressGuage.fillAmount = 1;
                    */
                    if (Node.SelectSingleNode("액션").Attributes.GetNamedItem("범위") == null)
                    {
                        PointRange = new Vector2(-1,-1);
                    }
                    else
                    {
                        txt = Node.SelectSingleNode("액션").Attributes.GetNamedItem("범위").InnerText;
                        PointRange = new Vector2((float)System.Convert.ToDouble(txt.Substring(0, 5)), (float)System.Convert.ToDouble(txt.Substring(6, 5)));
                    }
                    압박에임.SetActive(true);
                    Pointline.gameObject.SetActive(true);
                    pressChk = 1;
                }
                else if (txt == "찬반")
                {
                    if (Node.SelectSingleNode("액션").Attributes.GetNamedItem("동의") == null)
                    {
                        if (pressChk != 2)
                        {
                            AgreeChk = true;
                            currentCamera(false).GetComponent<CameraTranslateScript>().Cam.GetComponent<ShaderController>().찬반(ConvertColor(255, 191, 191, 255), ConvertColor(255, 50, 0, 255));
                        }
                        pressChk = 2;
                    }
                    else
                    {
                        if (AgreeChk)
                        {
                            txt = Node.SelectSingleNode("액션").Attributes.GetNamedItem("동의").InnerText;
                            ConvertChange(txt);
                            //Notice("<color=#FF0000>동의</color>", 1);
                        }
                        else
                        {
                            txt = Node.SelectSingleNode("액션").Attributes.GetNamedItem("반대").InnerText;
                            ConvertChange(txt);
                            DynamicScreen.Swap(true);
                            //Notice("<color=#0054FF>반대</color>", 1);
                        }
                        pressChk = 0;
                    }
                }
                else
                {
                    pressChk = 0;
                }
            }
            else
            {
                if (압박에임.activeSelf)
                {
                    Pointline.gameObject.SetActive(false);
                    압박에임.SetActive(false);
                    PressGuage.fillAmount = 0;
                    pressChk = 0;
                }
            }
            if (currentType != "")
            {
                StartCoroutine(EndType());
                return;
            }
        } else {
			int total = Node.SelectNodes("선택지").Count;
            if (!Pannel.gameObject.activeSelf) { Pannel.color = Color.black; StartCoroutine(FaidIn(Pannel, 0.5f)); }
			for(int i = total-1; i >= 0; i --){
                XmlElement SelectedNode = Node.SelectNodes("선택지")[i] as XmlElement;
                if (SelectedNode.Attributes.GetNamedItem("조건") != null)
                {
                    string txtn = SelectedNode.GetAttribute("조건");
                    int nn = SelectedNode.GetAttribute("조건").IndexOf(",");
                    if(!Singleton.Instance.IsHave(txtn.Substring(0, nn), System.Convert.ToInt32(txtn.Substring(nn + 1))))
                    {
                        total--;
                        continue;
                    }
                }
				string txt = SelectedNode.GetAttribute("이동");
				int n = SelectedNode.GetAttribute("이동").IndexOf (",");
				GameObject Obj = SelectButtons[i];
                SelectButtonScript ObjCp = Obj.GetComponent<SelectButtonScript>();
				Obj.SetActive(true);
				Obj.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = Node.SelectNodes("선택지")[i].InnerText;
                if (n >= 0)
                {
                    ObjCp.TagName = txt.Substring(0, n);
                    ObjCp.Index = System.Convert.ToInt32(txt.Substring(n + 1));
                }
                else if(txt == "")
                {
                    ObjCp.TagName = TagName;
                    ObjCp.Index = NumberDia+1;
                }
                else
                {
                    ObjCp.TagName = txt;
                    ObjCp.Index = 0;
                }
                ObjCp.SelectChk = true;
                Obj.GetComponent<Animator>().SetInteger("잠금", 0);
                Obj.transform.GetChild(1).gameObject.SetActive(false);
                Obj.transform.GetChild(2).gameObject.SetActive(false);
                Obj.GetComponent<Animator>().enabled = false;
                if (SelectedNode.Attributes.GetNamedItem("read") == null){
					Obj.GetComponent<UnityEngine.UI.Image>().color = Color.white;
				}else{
					if(SelectedNode.GetAttribute("read") == "true"){
						Obj.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
                        if (SelectedNode.Attributes.GetNamedItem("잠금") != null)
                        {
                            string txts = SelectedNode.GetAttribute("잠금해제");
                            int ns = SelectedNode.GetAttribute("잠금해제").IndexOf(",");
                            if (Node.ParentNode.SelectNodes(txts.Substring(0, ns))[0].Attributes.GetNamedItem("read") != null)
                            {
                                SelectedNode.Attributes.GetNamedItem("이동").Value = txts.Substring(ns + 1);
                                SelectedNode.Attributes.RemoveNamedItem("잠금");
                                SelectedNode.Attributes.RemoveNamedItem("잠금해제");
                                SelectedNode.Attributes.RemoveNamedItem("read");
                                txt = SelectedNode.GetAttribute("이동");
                                n = SelectedNode.GetAttribute("이동").IndexOf(",");
                                if (n >= 0)
                                {
                                    ObjCp.TagName = txt.Substring(0, n);
                                    ObjCp.Index = System.Convert.ToInt32(txt.Substring(n + 1));
                                }
                                else
                                {
                                    ObjCp.TagName = txt;
                                    ObjCp.Index = 0;
                                }
                                Obj.GetComponent<UnityEngine.UI.Image>().color = Color.white;
                                Obj.GetComponent<Animator>().SetInteger("잠금", 2);
                            }
                            else
                            {
                                txts = SelectedNode.GetAttribute("잠금");
                                ns = SelectedNode.GetAttribute("잠금").IndexOf(",");
                                if (ns >= 0)
                                {
                                    Obj.transform.Find("Show").GetComponent<SelectButtonScript>().TagName = txts.Substring(0, ns);
                                    Obj.transform.Find("Show").GetComponent<SelectButtonScript>().Index = System.Convert.ToInt32(txts.Substring(ns + 1));
                                }
                                else
                                {
                                    Obj.transform.Find("Show").GetComponent<SelectButtonScript>().TagName = txts;
                                    Obj.transform.Find("Show").GetComponent<SelectButtonScript>().Index = 0;
                                }
                                Obj.GetComponent<UnityEngine.UI.Image>().color = Color.red;
                                Obj.GetComponent<Animator>().SetInteger("잠금", 1);
                            }
                        }
                    }
                    else{
						Obj.GetComponent<UnityEngine.UI.Image>().color = Color.white;
					}
				}
				StartCoroutine(SelectBttSetting((360/total*(1+i)-45*Mathf.CeilToInt(total/4 - Mathf.FloorToInt(total/4))-90*(1+Mathf.Pow(-1,total+1))/2)*Mathf.PI/180,Obj));
			}
		}
        if (Node.SelectNodes("저장").Count > 0)
        {
            SaveGame();
        }
        대사화살표.SetTrigger("완독");
    }

	IEnumerator SelectBttSetting(float Angle, GameObject Obj){
		Vector2 destination;
        RectTransform ObjRtf = Obj.GetComponent<RectTransform>();
        SelectButtonScript Objsbs = Obj.GetComponent<SelectButtonScript>();
		destination = new Vector2 ((Mathf.Cos (Angle) - Mathf.Sin (Angle))*180,70+ 0.5f * (Mathf.Cos (Angle) + Mathf.Sin (Angle))*180);
        ObjRtf.anchoredPosition = Vector2.zero;
        ObjRtf.localScale = Vector3.zero;
        Objsbs.destination = Vector2.zero;
        Objsbs.Angle = 0;
		float a = 0.01f;
		for(float i = 0.01f; i <=0.5f; i += a){
            ObjRtf.anchoredPosition = destination*i;
            ObjRtf.localScale = Vector3.one*i;
			a += 0.02f * Time.deltaTime;
			Angle += 5f* Time.deltaTime;
			destination = new Vector2 ((Mathf.Cos (Angle) - Mathf.Sin (Angle))*180,70+ 0.5f * (Mathf.Cos (Angle) + Mathf.Sin (Angle))*180);
            yield return null;
		}
		for(float i = 0.5f; i <=1; i += a){
            ObjRtf.anchoredPosition = destination*i;
            ObjRtf.localScale = Vector3.one*i;
			if( a >= 0.02f){
				a -= 0.02f * Time.deltaTime;
			}
			Angle += 4f * Time.deltaTime;
			destination = new Vector2 ((Mathf.Cos (Angle) - Mathf.Sin (Angle))*180,70+ 0.5f * (Mathf.Cos (Angle) + Mathf.Sin (Angle))*180);
            yield return null;
		}
        /*
		for(float i = 1; i >=1; i += a){
			Obj.GetComponent<RectTransform>().anchoredPosition = destination*i;
			Obj.GetComponent<RectTransform>().localScale = Vector3.one*i;
			a -= 0.02f;
			yield return new WaitForSeconds(0.02f);
		}
		*/
        Obj.GetComponent<Animator>().enabled = true;
        Objsbs.enabled = true;
        Objsbs.destination = destination;
        Objsbs.Angle = Angle;
        ObjRtf.anchoredPosition = destination;
        ObjRtf.localScale = Vector3.one;

	}

	IEnumerator LoadScene(bool IsSearch = false){
		int i = 179;
		for (int c=0; c < 8; c ++) {
			i-=c;
			MainCamera.fieldOfView = i-c;
			yield return null;
		}
		while(i > 60) {
			i -= Mathf.CeilToInt((float)(i-60)/10) ;
			MainCamera.fieldOfView = i;
			yield return null;
		}
		MainCamera.fieldOfView = 60;
		if(IsSearch){
            Debug.Log(Singleton.Instance.AutoDia);
            AutoChk(true,true);
		}
	}

    void AutoChk(bool type = true, bool isSearch = false)
    {
        Debug.Log(isSearch);
        Debug.LogAssertion(this);
        if (isSearch)
        {
            if (type)
            {
                if (Singleton.Instance.AutoDia != "")
                {
                    if (xmldoc.SelectSingleNode("/root/" + Singleton.Instance.stage + "-" + Singleton.Instance.phase).SelectNodes(Singleton.Instance.AutoDia).Count > 0
                    && xmldoc.SelectSingleNode("/root/" + Singleton.Instance.stage + "-" + Singleton.Instance.phase).SelectNodes(Singleton.Instance.AutoDia)[0].Attributes.GetNamedItem("read") == null)
                    {
                        StartDialog(Singleton.Instance.AutoDia, 0);
                        Singleton.Instance.AutoDia = "";
                    }
                    else
                    {
                        Singleton.Instance.AutoDia = "";
                        AutoChk(false, isSearch);
                    }
                }
                else
                {
                    AutoChk(false,isSearch);
                }
            }
            if (!type)
            {
                if (xmldoc.SelectSingleNode("/root/" + Singleton.Instance.stage + "-" + Singleton.Instance.phase).SelectNodes("A" + Singleton.Instance.area + "입장").Count > 0)
                {
                    if (xmldoc.SelectSingleNode("/root/" + Singleton.Instance.stage + "-" + Singleton.Instance.phase).SelectNodes("A" + Singleton.Instance.area + "입장")[0].Attributes.GetNamedItem("read") == null)
                    {
                        StartDialog("A" + Singleton.Instance.area + "입장", 0);
                    }
                    else if (xmldoc.SelectSingleNode("/root/" + Singleton.Instance.stage + "-" + Singleton.Instance.phase).SelectNodes("A" + Singleton.Instance.area + "입장")[0].Attributes.GetNamedItem("read").Value != "true")
                    {
                        StartDialog("A" + Singleton.Instance.area + "입장", 0);
                    }
                    else
                    {
                        StartSearch();
                    }
                }
                else
                {
                    StartSearch();
                }
            }
        }
    }

	public void SelectBttFunction(SelectButtonScript btt){
        if (btt.SelectChk) {
			int total = Node.SelectNodes ("선택지").Count;
			for (int i = 0; i < total; i ++) {
                SelectButtons[i].SetActive(false);
			}
            if (btt.transform.parent.name == "UICanvas")
            {
                int btindex = System.Convert.ToInt32(btt.gameObject.name.Substring(6));
                XmlElement nd = Node.SelectNodes("선택지")[btindex] as XmlElement;
                
                if (nd.GetAttribute("read") != "false")
                {
                    if(nd.GetAttribute("read") == "")
                    nd.SetAttribute("read", "true");
                    
                    //if (!Singleton.Instance.IsExistCurrentElement(TagName, NumberDia, btindex))
                }
            }
			transform.parent.Find ("Quit").gameObject.SetActive (false);
            if (btt.TagName == "재도전")
            {
                ReTry();
            }
            else if(btt.TagName == "타이틀")
            {
                GoBackToTitle();
            }
            else
            {
                TagName = btt.TagName;
                NumberDia = btt.Index;
                Say();
            }
            if(Node.SelectSingleNode("text").InnerText.Length > 0 || Node.SelectNodes("선택지").Count == 0)
            {
                if (Pannel.gameObject.activeSelf) StartCoroutine(FaidOut(Pannel, 0.5f));
            }
		} else {
			int total = btt.Index;
			for (int i = 0; i < total; i ++) {
				SelectButtons[i].SetActive(false);
			}
            transform.parent.Find("Move").gameObject.SetActive(false);
            transform.parent.Find("Quit").gameObject.SetActive(false);
			MoveArea(btt.TagName,true);
			StartCoroutine(LoadScene(true));
		}
	}

	public void QuitBttFunction(SelectButtonScript btt){
        if (btt.SelectChk) {
			int total = Node.SelectNodes ("선택지").Count;
			for (int i = 0; i < total; i ++) {
                SelectButtons[i].SetActive (false);
			}
		} else {
			int total = btt.Index;
			for (int i = 0; i < total; i ++) {
                SelectButtons[i].SetActive (false);
			}
		}
		if (btt.TagName == "종료") {
            if (Pannel.gameObject.activeSelf) StartCoroutine(FaidOut(Pannel, 0.5f));
            StartSearch();
		} else {
			TagName = btt.TagName;
			NumberDia = btt.Index;
			Say ();
            if (Node.SelectSingleNode("text").InnerText.Length > 0)
            {
                if (Pannel.gameObject.activeSelf) StartCoroutine(FaidOut(Pannel, 0.5f));
            }
        }
	}

	public void ChangeDialog(string Tg = "say", int Index = 0){
		TagName = Tg;
		NumberDia = Index;
	}

	public void ShowItem(){
        int totals = Node.SelectNodes("선택지").Count;
        if (totals > 0)
        {
            for (int i = 0; i < totals; i++)
            {
                SelectButtons[i].SetActive(false);
            }
        }
        Inventory.ShowBtt.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        Inventory.transform.Find("Quit").gameObject.SetActive(true);
        int total = Node.SelectNodes ("제시").Count;
		string currentitem = Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk)[Inventory.currentItem].name;
		Inventory.ShowChk = false;
		Inventory.CloseInventory ();
        Pannel.gameObject.SetActive(false);
		transform.parent.Find ("Quit").gameObject.SetActive (false);
		for (int i = 0; i < total; i ++) {
			if(Node.SelectNodes("제시")[i].InnerText == currentitem){
				string txt = Node.SelectNodes("제시")[i].Attributes.GetNamedItem("이동").InnerText;
                ConvertChange(txt);
                Say ();
				return;
			}
		}
		string txtx = Node.SelectSingleNode("제시실패").InnerText;
        ConvertChange(txtx);
        Say ();
	}
    public void Change확신()
    {
        if(Input.GetMouseButton(0) == true)
        {
            if (확신)
            {
                확신 = false;
                Inventory.ShowBtt.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
            else
            {
                확신 = true;
                Inventory.ShowBtt.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
        }
    }

    public void ConvertChange(string txt)
    {
        int n = txt.IndexOf(",");

        if (n >= 0)
        {
            ChangeDialog(txt.Substring(0, n), System.Convert.ToInt32(txt.Substring(n + 1)));
        }
        else if (txt == "")
        {
            ChangeDialog(TagName, NumberDia);
        }
        else if (txt == "기억")
        {
            n = Remember.IndexOf(",");
            txt = Remember;
            ChangeDialog(Remember.Substring(0, n), System.Convert.ToInt32(Remember.Substring(n + 1)));
        }
        else if (txt == "랜덤")
        {
            int rnd = Random.Range(0, Node.ParentNode.SelectNodes(TagName).Count);
            if (rnd == NumberDia - 1)
            {
                if (rnd < Node.ParentNode.SelectNodes(TagName).Count - 1)
                {
                    rnd += 1;
                }
                else
                {
                    rnd = 0;
                }
            }
            ChangeDialog(TagName, rnd);
        }
        else
        {
            ChangeDialog(txt, 0);
        }
    }

    public void PressTouch()
    {
        Debug.Log(pressChk);
        switch (pressChk)
        {
            case 1:
                /*
                배경("쇼크웨이브", RightChk);
                currentCamera(RightChk).GetComponent<CameraTranslateScript>().ZoomEffect();
                pressCount--;
                Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/둥2"));
                PressGuage.fillAmount -= 0.067f;
                if (PressGuage.fillAmount == 0)
                {
                    if (pressCount <= 0)
                    {
                        배경("플래시전환", RightChk);
                        ChangeDialog(TagName + "스턴", 0);
                        Say();
                        PressGuage.fillAmount = 0;
                        pressChk = 0;
                    }
                    else
                    {
                        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/의문소리"));
                        Notice("압박에 반응하지 않는다.",2);
                        Notice("압박에 반응하지 않는다.", 2);
                    }
                }
                */
                배경("쇼크웨이브", RightChk);
                currentCamera(RightChk).GetComponent<CameraTranslateScript>().ZoomEffect();
                Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/둥2"));
                if(Pointline.currentLine > PointRange.x && Pointline.currentLine < PointRange.y)
                {
                    배경("플래시전환", RightChk);
                    ChangeDialog(TagName + "스턴", 0);
                    Say();
                }
                else
                {
                    Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/의문소리"));
                    Notice("압박에 반응하지 않는다.", 2);
                }
                return;
            case 2:
                AgreeChk = !AgreeChk;
                Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/화악"));
                if (AgreeChk)
                {
                    currentCamera(false).GetComponent<CameraTranslateScript>().Cam.GetComponent<ShaderController>().찬반(ConvertColor(255, 191, 191, 255), ConvertColor(255, 50, 0, 255));
                }
                else
                {
                    currentCamera(false).GetComponent<CameraTranslateScript>().Cam.GetComponent<ShaderController>().찬반(ConvertColor(191, 191, 255, 255), ConvertColor(0, 128, 255, 255));
                }
                DynamicScreen.Swap(AgreeChk);
                return;
        
        }
    }

    void getKeyword(string txt)
    {
        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/반짝"));
        Keywordtxt.gameObject.SetActive(true);
        Keywordtxt.GetComponent<Animator>().Play("키워드획득");
        Keywordtxt.text = txt;
    }

    void DeleteKeyword()
    {
        Keywordtxt.GetComponent<Animator>().Play("키워드삭제");
        Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/반짝2"));
        Keywordtxt.GetComponent<DestroyStopwatchScript>().StartCoroutine(Keywordtxt.GetComponent<DestroyStopwatchScript>().TimeOverOff(3f));
    }

    void EndingCredit()
    {
        //UnityEngine.SceneManagement.SceneManager.MergeScenes(UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(2), UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        UnityEngine.SceneManagement.SceneManager.LoadScene("엔딩크레딧", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void 채팅클릭()
    {
        if (SayChk == false && Singleton.Instance.phase != "Ending")
        {
            Audios[2].PlayOneShot(Resources.Load<AudioClip>("Sound/" + "딸깍"));
        }
    }
}
