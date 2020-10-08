using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Collections;

[CustomEditor(typeof(AreaCamerScript),true)]
public class EditorMove : Editor {
	public string Areaname;
	string AreaIn;
	AreaCamerScript mtarget;
	Vector2 ScrollPos;
	XmlDocument AreaDoc;
	// Use this for initialization

	void OnEnable(){
		mtarget = target as AreaCamerScript;
		AreaDoc = new XmlDocument ();
		AreaDoc.LoadXml (Resources.Load<TextAsset>("XmlData/AreaData").text);
	}

	public override void OnInspectorGUI(){
		int total = AreaDoc.SelectSingleNode ("/root").ChildNodes.Count;

		//base.DrawDefaultInspector ();
		EditorGUILayout.BeginHorizontal ();
		//EditorGUILayout.LabelField ("장소");
		GUILayout.Label ("Tag",GUILayout.ExpandWidth(false));
		Areaname = EditorGUILayout.TextField (Areaname);
		GUILayout.Label ("Area",GUILayout.ExpandWidth(false));
		AreaIn = EditorGUILayout.TextField (AreaIn);
		if (GUILayout.Button ("추가")) {
			XmlElement Empty = AreaDoc.CreateElement("A"+Areaname);
			XmlElement Ele = AreaDoc.CreateElement("Area");
			Ele.InnerText = AreaIn;
			Empty.AppendChild(Ele);
			Ele = AreaDoc.CreateElement("position");
			Ele.InnerText = mtarget.transform.position.ToString();
			Empty.AppendChild(Ele);
			Ele = AreaDoc.CreateElement("rotation");
			Ele.InnerText = mtarget.transform.rotation.eulerAngles.ToString();
			Empty.AppendChild(Ele);

			AreaDoc.SelectSingleNode("/root").AppendChild(Empty);
			AreaDoc.Save(Application.dataPath +"/Resources/XmlData/AreaData.xml");
			//GM.AreaXmlData;
		}
		EditorGUILayout.EndHorizontal ();
		GUILayout.Label ("Move");
		ScrollPos = EditorGUILayout.BeginScrollView (ScrollPos);
		for (int i = 0; i < total; i ++) {
			GUILayout.BeginHorizontal();
			if (GUILayout.Button (AreaDoc.SelectSingleNode ("/root").ChildNodes[i].Name.Substring(1))) {
				MoveArea(AreaDoc.SelectSingleNode ("/root").ChildNodes[i].Name.Substring(1));
			}
            GUI.color = Color.cyan;
            if (GUILayout.Button("수정", GUILayout.Width(40)))
            {
                AreaDoc.SelectSingleNode("/root").ChildNodes[i].SelectSingleNode("position").InnerText = mtarget.transform.position.ToString();
                AreaDoc.SelectSingleNode("/root").ChildNodes[i].SelectSingleNode("rotation").InnerText = mtarget.transform.rotation.eulerAngles.ToString();
                AreaDoc.Save(Application.dataPath + "/Resources/XmlData/AreaData.xml");
            }
            GUI.color = Color.red;
			if (GUILayout.Button ("-",GUILayout.Width(20))) {
				AreaDoc.SelectSingleNode ("/root").RemoveChild(AreaDoc.SelectSingleNode ("/root").ChildNodes[i]);
				AreaDoc.Save(Application.dataPath +"/Resources/XmlData/AreaData.xml");
			}
			GUI.color = Color.white;
			GUILayout.EndHorizontal();
		}
		EditorGUILayout.EndScrollView ();

	}

	void MoveArea(string area){
		float retx;
		float rety;
		float retz;
		int starti;
		int endi;
		string areatxt;
		areatxt = AreaDoc.SelectSingleNode ("/root/A" + area).SelectSingleNode ("position").InnerText;
		starti = 1;
		endi = areatxt.IndexOf(",");
		retx = (float)XmlConvert.ToDecimal(areatxt.Substring(starti,endi-1));
		starti = endi+1;
		endi = areatxt.IndexOf(",", starti);
		rety = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
		starti = endi+1;
		endi = areatxt.IndexOf(")", starti);
		retz = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
		mtarget.transform.position = new Vector3(retx,rety,retz);
		
		areatxt = AreaDoc.SelectSingleNode ("/root/A" + area).SelectSingleNode ("rotation").InnerText;
		starti = 1;
		endi = areatxt.IndexOf(",");
		retx = (float)XmlConvert.ToDecimal(areatxt.Substring(starti,endi-1));
		starti = endi+1;
		endi = areatxt.IndexOf(",", starti);
		rety = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
		starti = endi+1;
		endi = areatxt.IndexOf(")", starti);
		retz = (float)XmlConvert.ToDecimal(areatxt.Substring(starti+1,endi -starti-1));
		mtarget.transform.rotation = Quaternion.Euler(retx,rety,retz);
		//AreaCamera.localScale = Vector3.one;
	}
}
