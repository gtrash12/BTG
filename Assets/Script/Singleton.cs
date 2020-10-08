using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;


[Serializable]
public class InvenClass
{
    public string name;
    public int level;
    public InvenClass(string n, int l = 0)
    {
        name = n;
        level = l;
    }
}

/*
[Serializable]
public class ClearClass
{
    public string Tag;
    public int Index;
    public List<ClearElement> element;
    
    public ClearClass(string T, int l = 0)
    {
        Tag = T;
        Index = l;
        element = new List<ClearElement>();
    }
}

[Serializable]
public struct ClearElement
{
    public string Type;
    public int Index;
    public int ParentIndex;
    List<ElementAttribute> att;
};

[Serializable]
public struct ElementAttribute
{
    public string name;
    public string text;
};

*/
public class Singleton {
	private static Singleton instance = null;
	public static Singleton Instance
	{
		get
		{
			if(instance==null)
			{
				instance = new Singleton();
			}
			return instance;
		}
	}
	
	private Singleton(){
        Debug.Log(Application.systemLanguage.ToString());
        string syslang = Application.systemLanguage.ToString();
        PlayerPrefs.SetString("Language", "Korean");
        if (PlayerPrefs.GetString("Language", "First") == "First")
        {
            if (syslang == "English" || syslang == "Korean" || syslang == "Japanese")
            {

                PlayerPrefs.SetString("Language", syslang);
            }
            else
            {
                PlayerPrefs.SetString("Language", "English");
            }
            
        }
        LocalizeFile.LoadXml(Resources.Load<TextAsset>("XmlData/LocalizeData").text);
        
    }

    public int currentSaveSlot = 5;
	public string stage = "A";
	public string area;
	public string phase = "Intro";
    public string AutoDia = "";
    public float hp = 100;
    public float Maxhp = 100;
    public bool InvenTypeChk = true;
    public AudioSource SFXSource;
    public AudioSource BGMSource;
    public Transform PosContainer;
	//public List<ClearClass> Clear = new List<ClearClass>();
	public List<InvenClass> Inventory = new List<InvenClass>();
    public List<InvenClass> ChaList = new List<InvenClass>();
    //public List<InvenClass> CurrentList = new List<InvenClass>();
    public XmlDocument ItemDB = new XmlDocument ();
    public XmlDocument CharacterDB = new XmlDocument();
    public XmlDocument LocalizeFile = new XmlDocument();
    public List<string> Shaderlist1 = new List<string>();
    public List<string> Shaderlist2 = new List<string>();
    public bool isNewGame = true;
    public int SelectedChapter = 0;

    public void Initailize()
    {
        hp = 100;
        Maxhp = 100;
        Inventory.Clear();
        ChaList.Clear();
        Shaderlist1.Clear();
        Shaderlist2.Clear();
        //Clear.Clear();
    }

    /*
    public List<ClearElement> CurrentElements(string tagname ,int dianum)
    {
        List<ClearElement> tmp = new List<ClearElement>();
        foreach(ClearClass i in Clear)
        {
            if(i.Tag == tagname)
            {
                foreach(ClearElement j in i.element)
                {
                    if(j.ParentIndex == dianum)
                    {
                        tmp.Add(j);
                    }
                }
                return tmp;
            }
        }
        return null;
    }

    public ClearElement CurrentElement(string tagname, int dianum,int btindex)
    {
        foreach (ClearClass i in Clear)
        {
            if (i.Tag == tagname)
            {
                foreach (ClearElement j in i.element)
                {
                    if (j.ParentIndex == dianum)
                    {
                        if(j.Index == btindex)
                        {
                            return j;
                        }
                    }
                }
            }
        }
        ClearElement tmp = new ClearElement();
        return tmp;
    }

    public bool IsExistCurrentElement(string tagname, int dianum, int btindex)
    {
        foreach (ClearClass i in Clear)
        {
            if (i.Tag == tagname)
            {
                foreach (ClearElement j in i.element)
                {
                    if (j.ParentIndex == dianum)
                    {
                        if (j.Index == btindex)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    */

    public string getLocalText(string key)
    {
        Debug.Log("Root/Text[@key='" + key + "']/C" + PlayerPrefs.GetInt("Country", 10).ToString());
        return LocalizeFile.SelectSingleNode("Root/Text[@key='" + key + "']/"+PlayerPrefs.GetString("Language")).InnerText;
    }

    public List<string> CurrentShaderlist(bool Chk)
    {
        if (Chk)
        {
            return Shaderlist1;
        }
        else
        {
            return Shaderlist2;
        }
    }
    

    public List<InvenClass> CurrentList(bool type)
    {
        if (type)
        {
            return Inventory;
        }
        else
        {
            return ChaList;
        }
    }

    public void SetDB(){
		ItemDB.LoadXml (Resources.Load<TextAsset> ("XmlData/ItemData").text);
        CharacterDB.LoadXml(Resources.Load<TextAsset>("XmlData/CharacterData").text);
    }



	public int GetItemIndex(string name){
		XmlNodeList nodelist = ItemDB.SelectNodes ("root/"+stage+"/Item");
		int total = nodelist.Count;
		for (int i=0; i<total;i++) {
			if(nodelist[i].SelectSingleNode("name").InnerText == name){
				return i;
			}
		}
		return -1;
		//ItemDB.SelectNodes("root/Item")
	}
    public int GetCharacterIndex(string name)
    {
        XmlNodeList nodelist = CharacterDB.SelectNodes("root/" + stage + "/Item");
        int total = nodelist.Count;
        for (int i = 0; i < total; i++)
        {
            if (nodelist[i].SelectSingleNode("name").InnerText == name)
            {
                return i;
            }
        }
        return -1;
        //ItemDB.SelectNodes("root/Item")
    }

    public bool IsHave(string name, int level = 0)
    {
        int inventotal = Singleton.Instance.Inventory.Count;
        for (int i = 0; i < inventotal; i++)
        {
            if (name == Singleton.Instance.Inventory[i].name && level <= Singleton.Instance.Inventory[i].level)
            {
                return true;
            }
        }
        inventotal = Singleton.Instance.ChaList.Count;
        for (int i = 0; i < inventotal; i++)
        {
            if (name == Singleton.Instance.ChaList[i].name && level <= Singleton.Instance.ChaList[i].level)
            {
                return true;
            }
        }
        return false;
    }

    public int FindInvenIndex(string name)
    {
        int inventotal = Singleton.Instance.Inventory.Count;
        for (int i = 0; i < inventotal; i++)
        {
            if (name == Singleton.Instance.Inventory[i].name)
            {
                return i;
            }
        }
        /*
        inventotal = Singleton.Instance.ChaList.Count;
        for (int i = 0; i < inventotal; i++)
        {
            if (name == Singleton.Instance.ChaList[i].name)
            {
                return i;
            }
        }
        */
        return -1;
    }
    public int FindChaIndex(string name)
    {
        int inventotal = Singleton.Instance.ChaList.Count;
        for (int i = 0; i < inventotal; i++)
        {
            if (name == Singleton.Instance.ChaList[i].name)
            {
                return i;
            }
        }
        return -1;
    }
}
