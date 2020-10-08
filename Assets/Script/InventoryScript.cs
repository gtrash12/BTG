using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryScript : MonoBehaviour {
	public GameObject Slot;
	public Transform Container;
	public GameObject InvenMore;
	public UnityEngine.UI.Text ItemName;
	public UnityEngine.UI.Text ItemInfo;
	public Sprite DetailIcon;
	public GameObject Panel;
	List<GameObject> SlotList = new List<GameObject>();
	public int currentItem = 0;
	public bool ShowChk = false;
    bool HasDetail = false;
    public UnityEngine.UI.Image ItemImage;
    public UnityEngine.UI.Image ItemDetail;
    public GameObject ShowBtt;
    public GameObject Hand;
    public GameObject LeftArrow;
    public GameObject RightArrow;
    public UnityEngine.UI.Image DetailImage;
    public UnityEngine.UI.Image Quit;
    public GameObject ChangeBtt;
    public GameObject Scrollrect;
    public GameObject SlotArrow;
    public GameObject OptionBtt;
    public Sprite cb1;
    public Sprite cb2;




    void Awake()
    {
        //Singleton.Instance.ChaList.Add(new InvenClass("김탐정"));
       /*
        Singleton.Instance.ChaList.Add(new InvenClass("한나정"));
        Singleton.Instance.ChaList.Add(new InvenClass("나미녀",1));
        Singleton.Instance.ChaList.Add(new InvenClass("차임도",3));
        Singleton.Instance.ChaList.Add(new InvenClass("조신혜"));
        Singleton.Instance.Inventory.Add(new InvenClass("딸기모양 머리끈",2));//사용
        Singleton.Instance.Inventory.Add(new InvenClass("끊어진 딸기모양 머리끈"));//사용
        Singleton.Instance.Inventory.Add(new InvenClass("교직원 안내문"));//사용
        Singleton.Instance.Inventory.Add(new InvenClass("피해자 정보"));
        Singleton.Instance.Inventory.Add(new InvenClass("막힌 변기"));
        Singleton.Instance.Inventory.Add(new InvenClass("차임도의 열쇠"));
        Singleton.Instance.Inventory.Add(new InvenClass("구두 주걱"));
        Singleton.Instance.Inventory.Add(new InvenClass("한나정의 휴대폰"));
        Singleton.Instance.Inventory.Add(new InvenClass("한나정의 자물쇠"));
        Singleton.Instance.Inventory.Add(new InvenClass("휴지통"));//사용
        Singleton.Instance.Inventory.Add(new InvenClass("열린 변기 커버"));//사용
        Singleton.Instance.Inventory.Add(new InvenClass("떨어진 슬리퍼"));//사용
        Singleton.Instance.Inventory.Add(new InvenClass("한나정의 시체"));
        Singleton.Instance.Inventory.Add(new InvenClass("치한 사진"));//사용
        Singleton.Instance.Inventory.Add(new InvenClass("진통제"));
        Singleton.Instance.Inventory.Add(new InvenClass("휴지통"));
        Singleton.Instance.Inventory.Add(new InvenClass("차임도의 사진"));//사용
        Singleton.Instance.Inventory.Add(new InvenClass("차임도의 문자"));//사용
        Singleton.Instance.Inventory.Add(new InvenClass("육상부 안내문"));//사용
       */
        FirstInventorySet();
        CloseInventory();
    }

    public void ChangeTypeBtt()
    {
        ChangeInvenType(!Singleton.Instance.InvenTypeChk);
        if (Singleton.Instance.InvenTypeChk)
            ChangeBtt.GetComponent<UnityEngine.UI.Image>().sprite = cb1;
        else
            ChangeBtt.GetComponent<UnityEngine.UI.Image>().sprite = cb2;
    }

    void ChangeInvenType(bool type = true)
    {
        //transform.FindChild("Change").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("")
        Singleton.Instance.InvenTypeChk = type;
        //Singleton.Instance.InvenTypeSet(ItemChk);
        Container.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Container.transform.parent.GetComponent<UnityEngine.UI.ScrollRect>().velocity = Vector2.zero;
        currentItem = 0;
        InvenReset();
        //OpenInventory();
        GetComponent<Animator>().Rebind();
        GetComponent<Animator>().Play("OpenInventory");
        ArrowBttChk();
        SetItemInfo(currentItem);
        ValueChanged();
    }

    void InvenReset()
    {
        Debug.Log(Singleton.Instance.InvenTypeChk);
        for(int i = 0; i < SlotList.Count; i ++)
        {
            if (i < Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk).Count)
            {
                string item = Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk)[i].name;
                if (Resources.Load("item/" + item) != null)
                {
                    SlotList[i].transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("item/" + item);
                }
                else
                {
                    SlotList[i].transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("item/메모");
                }
                SlotList[i].SetActive(true);
            }
            else
            {
                SlotList[i].SetActive(false);
            }
        }
        Container.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk).Count, 80);
    }

	public void ValueChanged(){
        int total = Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk).Count;
		if (total > 6) {
			if (Container.transform.parent.GetComponent<UnityEngine.UI.ScrollRect> ().normalizedPosition.x > 0.95f) {
				InvenMore.SetActive (false);
			} else {
				InvenMore.SetActive (true);
			}
		}
		int Index = Mathf.FloorToInt(-Container.GetComponent<RectTransform> ().anchoredPosition.x/80);
		if (Index < 0) {
			Index = 0;
		}
		if (Index > 0) {
			for (int i = Index -1; i >=0; i --) {
				SlotList [i].SetActive (false);
			}
		}
		if (Index < total - 6) {
			for (int i = Index; i <= Index+6; i ++) {
				SlotList [i].SetActive (true);
			}
		}
		Index += 7;
		if (Index < total) {
			for (int i = Index; i < total; i ++) {
				SlotList [i].SetActive (false);
			}
		}
	}

	void FirstInventorySet(){
        int total = Singleton.Instance.Inventory.Count;
        if (Singleton.Instance.InvenTypeChk)
            ChangeBtt.GetComponent<UnityEngine.UI.Image>().sprite = cb1;
        else
            ChangeBtt.GetComponent<UnityEngine.UI.Image>().sprite = cb2;
        for (int i = 0; i < total; i ++) {
                GameObject it = Instantiate(Slot, Vector3.zero, Quaternion.identity) as GameObject;
                it.transform.SetParent(Container);
                it.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                it.GetComponent<RectTransform>().localPosition = new Vector3(80 * i, 0, 0);
                it.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                it.SetActive(true);
                if (Resources.Load("item/" + Singleton.Instance.Inventory[i].name) != null)
                {
                    it.transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("item/" + Singleton.Instance.Inventory[i].name);
                }
                else
                {
                    it.transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("item/메모");
                }
                SlotList.Add(it);
        }
        if (total > 0)
        {
            SetItemInfo(currentItem);
        }
		
        Container.GetComponent<RectTransform>().sizeDelta = new Vector2(80 * total, 80);
        total = Singleton.Instance.ChaList.Count;
        Debug.Log(total);
        for (int i = Singleton.Instance.Inventory.Count; i < total; i++)
        {
            Debug.Log(SlotList.Count);
            if (SlotList.Count <= i)
            {
                GameObject it = Instantiate(Slot, Vector3.zero, Quaternion.identity) as GameObject;
                it.transform.SetParent(Container);
                it.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                it.GetComponent<RectTransform>().localPosition = new Vector3(80 * i, 0, 0);
                it.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                it.SetActive(false);
                if (Resources.Load("item/" + i) != null)
                {
                    it.transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("item/" + Singleton.Instance.ChaList[i]);
                }
                else
                {
                    it.transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("item/메모");
                }
                SlotList.Add(it);
            }
        }
        Singleton.Instance.InvenTypeChk = Singleton.Instance.ChaList.Count > Singleton.Instance.Inventory.Count;
        ArrowBttChk();
        InvenReset();
        /*
        int itd;
        if (ItChk)
        {
            itd = Singleton.Instance.Inventory.Count;
            Singleton.Instance.Inventory.Add(item);
        }
        else
        {
            itd = Singleton.Instance.ChaList.Count;
            Singleton.Instance.ChaList.Add(item);
        }
		
		if (SlotList.Count > itd) {
			SlotList [itd].SetActive (true);
			if(Resources.Load("item/"+item) != null){
				SlotList [itd].transform.FindChild ("ItemImage").GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> ("item/" + item);
			}else{
				SlotList [itd].transform.FindChild ("ItemImage").GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> ("item/메모");
			}
		} else {
			GameObject it = Instantiate (Slot, Vector3.zero, Quaternion.identity) as GameObject;
			it.transform.SetParent (Container);
			it.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
			it.GetComponent<RectTransform> ().localPosition = new Vector3 ( 80*itd, 0, 0);
			it.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0,0,0);
			it.SetActive(true);
			if(Resources.Load("item/"+item) != null){
				it.transform.FindChild("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("item/"+item);
			}else{
				it.transform.FindChild("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("item/메모");
			}
			SlotList.Add(it);
		}
		Container.GetComponent<RectTransform> ().sizeDelta = new Vector2 (80+ 80*itd, 80);
        */
        OpenInventory();
    }

    void SetItemInfo(int index, bool det = true){
        Singleton.Instance.SFXSource.PlayOneShot(Resources.Load<AudioClip>("Sound/확대"));
        HasDetail = false;
        if (Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk).Count > index) {
			ItemName.text = Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk) [index].name;
            if (Singleton.Instance.InvenTypeChk)
            {
                int itindex = Singleton.Instance.GetItemIndex(Singleton.Instance.Inventory[index].name);
                string info = "";
                int total = Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk)[index].level;
                for (int i = 0; i <= total; i ++)
                {
                    info += "[ "+Singleton.Instance.ItemDB.SelectNodes("root/" + Singleton.Instance.stage + "/Item")[itindex].SelectNodes("text")[i].InnerText + " ]\n";
                }
                ItemInfo.text = info;
                if (Singleton.Instance.ItemDB.SelectNodes("root/" + Singleton.Instance.stage + "/Item")[itindex].SelectNodes("Detail").Count > 0)
                {
                    HasDetail = true;
                }
                //ItemInfo.text = Singleton.Instance.ItemDB.SelectNodes("root/" + Singleton.Instance.stage + "/Item")[Singleton.Instance.GetItemIndex(Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk)[index].name)].SelectSingleNode("text").InnerText;
            }
            else
            {
                int itindex = Singleton.Instance.GetCharacterIndex(Singleton.Instance.ChaList[index].name);
                string info = "";
                int total = Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk)[index].level;
                for (int i = 0; i <= total; i++)
                {
                    info += "[ " + Singleton.Instance.CharacterDB.SelectNodes("root/" + Singleton.Instance.stage + "/Item")[itindex].SelectNodes("text")[i].InnerText + " ]\n";
                }
                ItemInfo.text = info;
                if (Singleton.Instance.CharacterDB.SelectNodes("root/" + Singleton.Instance.stage + "/Item")[itindex].SelectNodes("Age").Count > 0)
                {
                    ItemName.text += "(" + Singleton.Instance.CharacterDB.SelectNodes("root/" + Singleton.Instance.stage + "/Item")[itindex].SelectSingleNode("Age").InnerText + ")";
                }
                //ItemInfo.text = Singleton.Instance.CharacterDB.SelectNodes("root/" + Singleton.Instance.stage + "/Item")[Singleton.Instance.GetCharacterIndex(Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk)[index].name)].SelectSingleNode("text").InnerText;
            }
			
			ItemImage.gameObject.SetActive(true);
			if(Resources.Load("item/"+Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk)[index].name) != null){
                ItemImage.sprite = Resources.Load<Sprite> ("item/" + Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk) [index].name);
			}else{
                ItemImage.sprite = Resources.Load<Sprite> ("item/메모");
			}
            ItemImage.GetComponent<ImageRotateScript> ().Change ();
            if(det)
			    ItemDetail.gameObject.SetActive (true);
		} else {
			ItemName.text = "";
			ItemInfo.text = "";
            ItemImage.gameObject.SetActive(false);
            ItemDetail.gameObject.SetActive (false);
		}
	}

	public void SelectItem(Transform Sl){
		int Index = Mathf.FloorToInt(Sl.GetComponent<RectTransform> ().anchoredPosition.x/80);
		SetItemInfo (Index);
		currentItem = Index;
		ArrowBttChk ();
	}

	public void DetailBtt(){
        if (HasDetail)
        {
            DetailImage.transform.parent.gameObject.SetActive(true);
            int itindex = Singleton.Instance.GetItemIndex(Singleton.Instance.Inventory[currentItem].name);
            DetailImage.sprite =
                Resources.Load<Sprite>("item/" + Singleton.Instance.ItemDB.SelectNodes("root/" + Singleton.Instance.stage + "/Item")[itindex].SelectNodes("Detail")[0].InnerText);
            return;
        }
		if (GetComponent<Animator> ().GetBool ("Detail")) {
			GetComponent<Animator>().SetBool("Detail",false);
            ItemImage.GetComponent<ImageRotateScript> ().enabled = true;
			ItemDetail.sprite = DetailIcon;
            //transform.FindChild ("Item").FindChild ("Frame").gameObject.SetActive(true);
            Singleton.Instance.SFXSource.PlayOneShot(Resources.Load<AudioClip>("Sound/축소"));
        } else {
            ItemImage.GetComponent<ImageRotateScript> ().Change ();
            ItemImage.GetComponent<ImageRotateScript> ().enabled = false;
			ItemDetail.sprite = Quit.sprite;
			//transform.FindChild ("Item").FindChild ("Frame").gameObject.SetActive(false);
			GetComponent<Animator>().SetBool("Detail",true);
            Singleton.Instance.SFXSource.PlayOneShot(Resources.Load<AudioClip>("Sound/확대"));
        }
	}
	public void ArrowBttChk(){
		if (currentItem == 0) {
			LeftArrow.SetActive (false);
		} else {
            LeftArrow.SetActive (true);
		}
		if (currentItem < Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk).Count -1) {
            RightArrow.SetActive (true);
		} else {
            RightArrow.SetActive (false);
		}
		CurrentSlotAnim ();
	}
	public void ArrowBtt(int n){
		currentItem += n;
		SetItemInfo (currentItem);
		ArrowBttChk ();
        Debug.Log(currentItem);
        if(n>0 && Container.GetComponent<RectTransform>().anchoredPosition.x > -80 * (currentItem - 5))
        {
            Container.GetComponent<RectTransform>().anchoredPosition = new Vector2(-80 * (currentItem - 5), 0);
        }
        if (n < 0 && Container.GetComponent<RectTransform>().anchoredPosition.x > -80 * (currentItem - 5))
        {
            Container.GetComponent<RectTransform>().anchoredPosition = new Vector2(-80 * (currentItem - 5), 0);
        }
        if (n < 0 && Container.GetComponent<RectTransform>().anchoredPosition.x < -80 * (currentItem))
        {
            Container.GetComponent<RectTransform>().anchoredPosition = new Vector2(-80 * (currentItem), 0);
        }
        ValueChanged();
    }
	void CurrentSlotAnim(){
		int total = Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk).Count;
        
		for (int i = 0; i < total; i++) {
			if(i == currentItem){
				SlotList [i].transform.Find("Frame").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 0.5f,0.25f);
				SlotList [i].transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 0.5f,0.25f);
			}else{
				SlotList [i].transform.Find("Frame").GetComponent<UnityEngine.UI.Image>().color = Color.white;
				SlotList [i].transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().color = Color.white;
				//SlotList [i].transform.FindChild("Panel").gameObject.SetActive(false);
			}
		}
	}
	void PanelOn(bool chk){
		if (chk == true) {
			Panel.SetActive (true);
			//Panel.GetComponent<UnityEngine.UI.Image> ().color = new Color (0, 0, 0, 0.5f);
		} else {
			Panel.SetActive(false);
		}
	}
	void PuttoInventory(string item){
        /*
        int itd;
        if (ItChk)
        {
            itd = Singleton.Instance.Inventory.Count;
            Singleton.Instance.Inventory.Add(new InvenClass(item));
        }
        else
        {
            itd = Singleton.Instance.ChaList.Count;
            Singleton.Instance.ChaList.Add(new InvenClass(item));
        }
        */
        int itd = Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk).Count;
        Singleton.Instance.CurrentList(Singleton.Instance.InvenTypeChk).Add(new InvenClass(item));
        if (SlotList.Count > itd) {
			SlotList [itd].SetActive (true);
			if(Resources.Load("item/"+item) != null){
				SlotList [itd].transform.Find ("ItemImage").GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> ("item/" + item);
			}else{
				SlotList [itd].transform.Find ("ItemImage").GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> ("item/메모");
			}
		} else {
			GameObject it = Instantiate (Slot, Vector3.zero, Quaternion.identity) as GameObject;
			it.transform.SetParent (Container);
			it.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
			it.GetComponent<RectTransform> ().localPosition = new Vector3 ( 80*SlotList.Count, 0, 0);
			it.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0,0,0);
			it.SetActive(true);
			if(Resources.Load("item/"+item) != null){
				it.transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("item/"+item);
			}else{
				it.transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("item/메모");
			}
			SlotList.Add(it);
		}
		Container.GetComponent<RectTransform> ().sizeDelta = new Vector2 (80+ 80*itd, 80);
	}
	public void GetItem(string item, bool itChk){
        if (Singleton.Instance.InvenTypeChk != itChk)
        {
            Singleton.Instance.InvenTypeChk = itChk;
            //ChangeInvenType(itChk);
        }
        Singleton.Instance.SFXSource.PlayOneShot(Resources.Load<AudioClip>("Sound/아이템획득"));
        gameObject.SetActive (true);
		ShowChk = false;
		ShowBtt.SetActive (false);
        RightArrow.SetActive (false);
		LeftArrow.SetActive (false);
        ItemDetail.gameObject.SetActive (false);
		Hand.SetActive (false);
		Scrollrect.SetActive (false);
		SlotArrow.SetActive (false);
        ChangeBtt.SetActive(false);
        OptionBtt.SetActive(false);
        DetailImage.transform.parent.gameObject.SetActive(false);
        GetComponent<Animator> ().SetBool ("Detail", false);
		GetComponent<Animator> ().Play ("GetItem");
		PanelOn (true);
        Panel.GetComponent<UnityEngine.UI.Button>().interactable = true;

        PuttoInventory (item);
		SetItemInfo (Singleton.Instance.CurrentList(itChk).Count - 1,false);
        InvenReset();
	}

	public void OpenInventory(){
        gameObject.SetActive (true);
        Singleton.Instance.SFXSource.PlayOneShot(Resources.Load<AudioClip>("Sound/인벤오픈"));
        ItemDetail.gameObject.SetActive (true);
		Hand.SetActive (true);
		Scrollrect.SetActive (true);
		ItemDetail.sprite = DetailIcon;
        ChangeBtt.SetActive(true);
        OptionBtt.SetActive(true);
        DetailImage.transform.parent.gameObject.SetActive(false);
        ArrowBttChk ();
		GetComponent<Animator> ().SetBool ("Detail", false);
        GetComponent<Animator> ().Play ("OpenInventory");
        DialogScript diac = transform.parent.Find("XmlManager").GetComponent<DialogScript>();
        if (diac.Node != null && diac.ShowChk())
        {
            ShowBtt.SetActive(true);
        }
        else
        {
            ShowBtt.SetActive(false);
        }
		PanelOn (true);
        SetItemInfo (currentItem);
        Singleton.Instance.BGMSource.volume *= 0.1f;

    }
    public void PanelBttOn()
    {
        //Panel.GetComponent<UnityEngine.UI.Button>().interactable = true;
    }
	public void CloseInventory(){
        Singleton.Instance.SFXSource.PlayOneShot(Resources.Load<AudioClip>("Sound/인벤닫기"));
        gameObject.SetActive (false);
        ShowBtt.SetActive (false);
		Panel.SetActive (false);
        if (ShowChk) {
			transform.parent.Find("XmlManager").GetComponent<DialogScript>().QuitBttFunction(transform.parent.Find("Quit").GetComponent<SelectButtonScript>());
			//ShowChk = false;
		}
        ShowChk = false;
        Singleton.Instance.BGMSource.volume = PlayerPrefs.GetFloat("BGMVolume"); ;
    }

    public void LevelUpItem(string name, int level)
    {
        int i = Singleton.Instance.FindInvenIndex(name);
        if(i == -1)
        {
            i = Singleton.Instance.FindChaIndex(name);
            if(Singleton.Instance.ChaList[i].level < level)
                Singleton.Instance.ChaList[i].level = level;
        }
        else
        {
            if (Singleton.Instance.Inventory[i].level < level)
                Singleton.Instance.Inventory[i].level = level;
        }

    }
}
