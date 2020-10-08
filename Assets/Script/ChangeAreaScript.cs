using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class ChangeAreaScript : MonoBehaviour {
	XmlDocument SceneXml;
	//public DialogScript GM;

	// Use this for initialization
	void Start () {
		SceneXml = new XmlDocument ();
		SceneXml.LoadXml(Resources.Load<TextAsset>("XmlData/SceneData").text);
	}

	public void BttClick(){
		StartCoroutine (Move ());
		transform.parent.Find ("Quit").gameObject.SetActive (true);
		transform.parent.Find ("Quit").GetComponent<SelectButtonScript> ().TagName = "종료";
		transform.parent.Find ("Quit").GetComponent<SelectButtonScript> ().SelectChk = false;
		transform.parent.Find ("RightArrow").gameObject.SetActive (false);
		transform.parent.Find ("LeftArrow").gameObject.SetActive (false);
		transform.parent.Find ("XmlManager").GetComponent<MouseEventScript> ().enabled = false;
		transform.parent.Find ("XmlManager").GetComponent<MouseEventScript> ().SearchCursor.SetActive (false);
		//gameObject.SetActive (false);
	}

	IEnumerator Move(){
		XmlElement Node = SceneXml.SelectSingleNode ("/root/A" + Singleton.Instance.area) as XmlElement;
		int total = Node.SelectNodes("Move").Count;
		transform.parent.Find ("Quit").GetComponent<SelectButtonScript> ().Index = total;
        List<int> container = new List<int>();
        for (int i = 0; i < total; i++)
        {
            if (Node.SelectNodes("Move")[i].Attributes.GetNamedItem("Item") != null)
            {
                string txt = Node.SelectNodes("Move")[i].Attributes.GetNamedItem("Item").Value;
                int n = Node.SelectNodes("Move")[i].Attributes.GetNamedItem("Item").Value.IndexOf(",");
                if (!Singleton.Instance.IsHave(txt.Substring(0, n), System.Convert.ToInt32(txt.Substring(n + 1))))
                {
                    continue;
                }
            }
            else if (Node.SelectNodes("Move")[i].Attributes.GetNamedItem("Phase") != null)
            {
                string txt = Node.SelectNodes("Move")[i].Attributes.GetNamedItem("Phase").Value;
                if (Singleton.Instance.phase != txt)
                {
                    continue;
                }
            }
            container.Add(i);
        }

        total = container.Count;
		for(int i = 0; i < total; i ++){
            // bool Chk = true;
                GameObject Obj = transform.parent.Find("Button" + i).gameObject;
                Obj.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = Node.SelectNodes("Move")[container[i]].InnerText;
                Obj.SetActive(true);
                Obj.GetComponent<Animator>().enabled = false;
                Obj.transform.Find("Animation").gameObject.SetActive(false);
                Obj.transform.Find("Show").gameObject.SetActive(false);
                Obj.GetComponent<SelectButtonScript>().enabled = false;
                Obj.GetComponent<SelectButtonScript>().SelectChk = false;
                Obj.GetComponent<SelectButtonScript>().TagName = Node.SelectNodes("Move")[container[i]].InnerText;
                Obj.GetComponent<SelectButtonScript>().Index = total;
                Obj.GetComponent<SelectButtonScript>().destination = Vector2.zero;
                Vector2 Dest = new Vector2(-250 + 50 / (total - i), (float)-i / (total + 1) * 450 + 100);
                //Obj.GetComponent<RectTransform>().anchoredPosition
                StartCoroutine(MoveE(Obj, Dest));
                yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator MoveE(GameObject Obj, Vector2 destination){

		float a = 0.01f;
		for(float i = 0.01f; i <=0.5f; i += a){
			Obj.GetComponent<RectTransform>().anchoredPosition = destination*i;
			Obj.GetComponent<RectTransform>().localScale = new Vector3(i,1,1);
			a +=  0.5f * Time.deltaTime;
			yield return null;
		}
		for(float i = 0.5f; i <=1; i += a){
			Obj.GetComponent<RectTransform>().anchoredPosition = destination*i;
			Obj.GetComponent<RectTransform>().localScale = new Vector3(i,1,1);
			if( a > 0.01f){
				a -= 0.5f* Time.deltaTime;
			}else{
				a = 0.01f;
			}
			yield return null;
		}
		Obj.GetComponent<RectTransform> ().anchoredPosition = destination;
		Obj.GetComponent<RectTransform>().localScale = Vector3.one;
	}
}
