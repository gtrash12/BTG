using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SearchListClass
{
    public string type;
    public string Tag;
    public List<InvenClass> NeedList;
    public SearchListClass(string n = "", string t = "")
    {
        type = n;
        Tag = t;
        NeedList = new List<InvenClass>();
    }
}
public class SearchScript : MonoBehaviour {
	public string Tag;
    [SerializeField]
    public List<SearchListClass> AppearList = new List<SearchListClass>();
    public Vector3 CPosition;
    public Vector3 CRotation;

    public void SetState()
    {
        if (AppearList.Count > 0)
        {
            AppearChk();
            
        }
    }
    
    void failhave(string type)
    {
        if (type.Substring(0, 2) == "등장")
        {
            gameObject.SetActive(false);
        }
    }

    void AppearChk()
    {
        for (int i = 0; i < AppearList.Count; i++)
        {
            int total = AppearList[i].NeedList.Count;
            for (int l = 0; l < total; l++)
            {
                if (AppearList[i].NeedList[0].level >= 0)
                {
                    if (!Singleton.Instance.IsHave(AppearList[i].NeedList[l].name, AppearList[i].NeedList[l].level))
                    {
                        failhave(AppearList[i].type);
                        return;
                    }
                }
                else
                {
                    if ((AppearList[i].NeedList[l].level == -2 && Singleton.Instance.phase == AppearList[i].NeedList[l].name)
                        || (AppearList[i].NeedList[l].level == -1 && Singleton.Instance.phase != AppearList[i].NeedList[l].name))
                    {
                        failhave(AppearList[i].type);
                        return;
                    }
                }
            }
            if (AppearList[i].type == "등장대사" || AppearList[i].type == "퇴장대사")
            {
                Singleton.Instance.AutoDia = AppearList[i].Tag;
            }
            if (AppearList[i].type == "퇴장" || AppearList[i].type == "퇴장대사")
            {
                gameObject.SetActive(false);
            }
            else if(AppearList[i].type == "태그")
            {
                Tag = AppearList[i].Tag;
            }
            if (AppearList[i].type == "위치컨테이너")
            {
                Singleton.Instance.PosContainer = transform;
            }
            if (i > 0)
            {
                AppearList.RemoveAt(0);
                i = 0;
            }
            /*
            if(AppearList[0].type != "퇴장" || AppearList[0].type == "퇴장대사")
            {
                AppearList.RemoveAt(0);
            }
            */
            //AppearList.Clear();
        }
    }
}
