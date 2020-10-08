using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;
using System.IO;

public class ScenarioEditor : EditorWindow {
    public UnityEngine.Object ScenarioText;
    XmlDocument xmldoc;
    XmlNodeList nodelist;
    XmlNodeList DiaNodeList;
    string[] Phaselist;
    int selectedPhase = 0;
    int prevPhase = 0;
    int addPhase = 0;
    string addPhasetxt = "";
    string FindTagtxt = "";
    string LockTagtxt = "";
    Vector2 scrollpos;
    Vector2 scrolldiapos;
    Vector2 scrollnamepos;
    Vector2 scrolltypepos;
    Vector2 scrollunitpos;
    List<int> selectedDialog = new List<int>();
    int currentDialog = -1;
    int keystate;
    int UnitNumber = 1;
    string adddiaoption = "";
    string addnameoption = "";
    string addtypeoption = "";
    int SetunitNumber = 50;

    List<string> dialogoption = new List<string>();
    int diaopselec = 0;
    List<string> nameoption = new List<string>();
    List<string> typeoption = new List<string>();


    float widthMax = 100;

    [SerializeField]
    string Name = "";
    string Type = "";
    string Motion = "";
    [SerializeField] string Dialogue = "";

    string currentname;
    string currenttype;
    string currentmotion;

    string format = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\n<root>\n</root>";
    string exportname = "";
    string TagName = "Say";

    bool addEleChk = false;
    string addEleName = "";
    string addEleText = "";
    int addAttributeChk = -1;
    string addAtrName = "";
    string addAtrText = "";

    bool protectname = false;
    bool protectmotion = false;
    bool protecttype = false;
    bool EditChk = false;

    string focustarget = "";

    List<XmlElement> CopyList = new List<XmlElement>();
    List<XmlElement> CopyElement = new List<XmlElement>();

    TextEditor editor;


    [MenuItem("Window/GalTraScenarioEditor")]
	public static void ShowScenarioEditor(){
		ScenarioEditor myWindow = (ScenarioEditor)EditorWindow.GetWindow (typeof(ScenarioEditor));
        myWindow.Show();
    }
	
	void OnGUI(){
        if (xmldoc == null) {
			selectedPhase = 0;
			if (Resources.Load ("ScenarioEditor") == null) {
				xmldoc = new XmlDocument ();  // 객체선언
				xmldoc.LoadXml (format);
				if (Directory.Exists (Application.dataPath + "/GalTraScenarioEditor/Resources")) {
					if (Directory.Exists (Application.dataPath + "/GalTraScenarioEditor/Resources/Scenario") == false) {
						Directory.CreateDirectory(Application.dataPath + "/GalTraScenarioEditor/Resources/Scenario");
					}
				} else {
					Directory.CreateDirectory(Application.dataPath + "/GalTraScenarioEditor/Resources");
				}
				xmldoc.Save (Application.dataPath + "/GalTraScenarioEditor/Resources/ScenarioEditor.xml");
			}
			ScenarioText = Resources.Load<TextAsset> ("ScenarioEditor");
			SetData ();
		}

		if (prevPhase != selectedPhase) {
			selectedDialog.Clear();
			prevPhase = selectedPhase;
			currentDialog = -1;
		}
		editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
        if (focustarget.Length > 0) {
			if(GUI.GetNameOfFocusedControl() == focustarget && !Event.current.isKey){
				GUI.FocusControl(focustarget);
				focustarget = "";
			}else{
				GUI.FocusControl(focustarget);
			}
		}
		// - 키보드 입력
		if (Event.current.type == EventType.keyDown) {
			if(Event.current.keyCode == KeyCode.LeftControl){
				keystate = 1;
				Event.current.Use();
			}else if(Event.current.keyCode == KeyCode.LeftAlt){
				keystate = 2;
				Event.current.Use();
			}
			// - 방향키
            /*
			if(keystate == 0 && Event.current.keyCode == KeyCode.DownArrow){
				if(GUI.GetNameOfFocusedControl() != "dialog" && GUI.GetNameOfFocusedControl() != "selecteddialog"){
					if(currentDialog < DiaNodeList.Count-1){
						currentDialog ++;
						selectedDialog.Clear();
						selectedDialog.Add(currentDialog);
						scrollpos = new Vector2(0, scrollpos.y + 55);
						Event.current.Use();
					}
				}
			}else if(keystate == 0 && Event.current.keyCode == KeyCode.UpArrow){
				if(GUI.GetNameOfFocusedControl() != "dialog" && GUI.GetNameOfFocusedControl() != "selecteddialog"){
					if(currentDialog != 0){
						currentDialog --;
						selectedDialog.Clear();
						selectedDialog.Add(currentDialog);
						scrollpos = new Vector2(0, scrollpos.y - 55);
						Event.current.Use();

					}
				}
			}
            */


			// - 컨트롤 방향키
			if(keystate == 1 && Event.current.keyCode == KeyCode.DownArrow){
				if(diaopselec < dialogoption.Count-1){ 
					diaopselec ++;
					Event.current.Use();
					scrolldiapos = new Vector2(0, diaopselec*21-(Screen.height-280)/2);
				}
				Event.current.Use();
			}

            if (keystate == 1 && Event.current.keyCode == KeyCode.UpArrow){
				if(diaopselec >0){ 
					diaopselec --;
					Event.current.Use();
					scrolldiapos = new Vector2(0, diaopselec*21-(Screen.height-280)/2);
				}
				Event.current.Use();
			}

            // - 알트 방향키
            if (keystate == 2 && Event.current.keyCode == KeyCode.DownArrow){
				PuttoDown();
				scrollpos = new Vector2(0, scrollpos.y + 55);
				Event.current.Use();
                return;
			}
			
			if(keystate == 2 && Event.current.keyCode == KeyCode.UpArrow){
				PuttoUp();
				Event.current.Use();
                return;
			}


			// - 알트 +엔터
			if(keystate == 2 && Event.current.keyCode == KeyCode.Return){
				//if(GUI.GetNameOfFocusedControl() == "dialog"){
                if(GUI.GetNameOfFocusedControl() != "addeletext") {
					//PuttoEnd(false);
                    PuttoEnd(true);
                    Event.current.Use();
                }
                else
                {
                    addEleText = addEleText.Insert(editor.cursorIndex, "\n");
                    editor.MoveRight();
                }
            }
            // - 엔터
            if (Event.current.keyCode == KeyCode.Return){
				if(GUI.GetNameOfFocusedControl() == "addtypeoption"){
					if(addtypeoption != ""){
						typeoption.Add(addtypeoption);
						PlayerPrefs.SetString("ScenarioEditortypeoption",PlayerPrefs.GetString("ScenarioEditortypeoption") + ","+addtypeoption);
						addtypeoption = "";
						Event.current.Use();
					}
				}else if(GUI.GetNameOfFocusedControl() == "addnameoption"){
					if(addnameoption != ""){
						nameoption.Add(addnameoption);
						PlayerPrefs.SetString("ScenarioEditornameoption",PlayerPrefs.GetString("ScenarioEditornameoption") + ","+addnameoption);
						addnameoption = "";
						Event.current.Use();
					}
				}else if(GUI.GetNameOfFocusedControl() == "adddiaoption"){
					if(adddiaoption != ""){
						dialogoption.Add(adddiaoption);
						PlayerPrefs.SetString("ScenarioEditordialogoption",PlayerPrefs.GetString("ScenarioEditordialogoption") + ","+adddiaoption);
						adddiaoption = "";
						Event.current.Use();
					}
				}else if(keystate != 2 && (GUI.GetNameOfFocusedControl() == "addatrname" || GUI.GetNameOfFocusedControl() == "addatrtext" || GUI.GetNameOfFocusedControl() == "addelename" || GUI.GetNameOfFocusedControl() == "addeletext")){
                    XmlElement emptynode = xmldoc.CreateElement(addEleName);
					emptynode.InnerText = addEleText;
					if(addAttributeChk == 0 && addAtrName != ""){
						emptynode.SetAttribute(addAtrName, addAtrText);
					}
					DiaNodeList[currentDialog].AppendChild(emptynode);
					addEleChk = false;
                    GUI.FocusControl("");
					Save();
                    Event.current.Use();
					return;
				}
			}
			if(keystate == 1 && Event.current.keyCode == KeyCode.Return){
				Event.current.Use();
			}
			if(Event.current.keyCode == KeyCode.Tab){
				if(GUI.GetNameOfFocusedControl() == "dialog" || GUI.GetNameOfFocusedControl() == "selecteddialog"){
					focustarget = "name";
					//Event.current.Use();
				}
			}
			// - 숫자 커맨드
			if(keystate == 2 && Event.current.keyCode == KeyCode.Alpha1){
				if(nameoption.Count >= 1){
					Name = nameoption[0];
					GUI.FocusControl("dialog");
				}
			}
			if(keystate == 2 && Event.current.keyCode == KeyCode.Alpha2){
				if(nameoption.Count >= 2){
					Name = nameoption[1];
					GUI.FocusControl("dialog");
				}
			}
			if(keystate == 2 && Event.current.keyCode == KeyCode.Alpha3){
				if(nameoption.Count >= 3){
					Name = nameoption[2];
					GUI.FocusControl("dialog");
				}
			}
			if(keystate == 2 && Event.current.keyCode == KeyCode.Alpha4){
				if(nameoption.Count >= 4){
					Name = nameoption[3];
					GUI.FocusControl("dialog");
				}
			}
			if(keystate == 2 && Event.current.keyCode == KeyCode.Alpha5){
				if(nameoption.Count >= 5){
					Name = nameoption[4];
					GUI.FocusControl("dialog");
				}
			}
			if(keystate == 2 && Event.current.keyCode == KeyCode.Alpha6){
				if(nameoption.Count >= 6){
					Name = nameoption[5];
					GUI.FocusControl("dialog");
				}
			}
			if(keystate == 2 && Event.current.keyCode == KeyCode.Alpha7){
				if(nameoption.Count >= 7){
					Name = nameoption[6];
					GUI.FocusControl("dialog");
				}
			}
			if(keystate == 2 && Event.current.keyCode == KeyCode.Alpha8){
				if(nameoption.Count >= 8){
					Name = nameoption[7];
					GUI.FocusControl("dialog");
				}
			}
			if(keystate == 2 && Event.current.keyCode == KeyCode.Alpha9){
				if(nameoption.Count >= 9){
					Name = nameoption[8];
					GUI.FocusControl("dialog");
				}
			}
        }

		if (Event.current.type == EventType.keyUp){
			if(Event.current.keyCode == KeyCode.LeftAlt || Event.current.keyCode == KeyCode.LeftControl){
				keystate = 0;
				Event.current.Use();
			}
            // - 컨트롤 +엔터
            if (keystate == 1 && Event.current.keyCode == KeyCode.Return){
					Dialogue = Dialogue.Remove(editor.cursorIndex-1,1);
					PuttoEnd();
					Event.current.Use();
			}
            // - 컨트롤 + 스페이스
            if (keystate == 1 && Event.current.keyCode == KeyCode.Space)
            {
                if (GUI.GetNameOfFocusedControl() == "dialog")
                {
                    Dialogue = Dialogue.Remove(editor.cursorIndex - 1, 1);
                    Dialogue = Dialogue.Insert(editor.cursorIndex - 1, dialogoption[diaopselec]);
                    editor.text = Dialogue;
                }
                else if (GUI.GetNameOfFocusedControl() == "selecteddialog")
                {
                    DiaNodeList[currentDialog].SelectSingleNode("text").InnerText = DiaNodeList[currentDialog].SelectSingleNode("text").InnerText.Remove(editor.cursorIndex - 1, 1);
                    DiaNodeList[currentDialog].SelectSingleNode("text").InnerText = DiaNodeList[currentDialog].SelectSingleNode("text").InnerText.Insert(editor.cursorIndex - 1, dialogoption[diaopselec]);
                    editor.text = DiaNodeList[currentDialog].SelectSingleNode("text").InnerText;
                }
                editor.cursorIndex = editor.cursorIndex + dialogoption[diaopselec].Length - 1;
                editor.SelectNone();
                Event.current.Use();
            }
        }
		// - 옵션 세팅
		if(PlayerPrefs.GetString("ScenarioEditordialogoption").Length >1 && dialogoption.Count == 0){
			string txt = PlayerPrefs.GetString("ScenarioEditordialogoption");
			for(int s = 0; s < 10; s ++){
				int i = txt.IndexOf(",");
				if(i >= 0){
					txt = txt.Remove(0,1);
					i = txt.IndexOf(",");
					if(i == -1){
						dialogoption.Add(txt);
						txt = txt.Remove(0, txt.Length);
					}else{
					dialogoption.Add(txt.Substring(0,i));
						txt = txt.Remove(0,i);
					}
				}else{
					break;
				}
			}

		}
		if(PlayerPrefs.GetString("ScenarioEditornameoption").Length >1 && nameoption.Count == 0){
			string txt = PlayerPrefs.GetString("ScenarioEditornameoption");
			for(int s = 0; s < 10; s ++){
				int i = txt.IndexOf(",");
				if(i >= 0){
					txt = txt.Remove(0,1);
					i = txt.IndexOf(",");
					if(i == -1){
						nameoption.Add(txt);
						txt = txt.Remove(0, txt.Length);
					}else{
						nameoption.Add(txt.Substring(0,i));
						txt = txt.Remove(0,i);
					}
				}else{
					break;
				}
			}
			
		}
		if(PlayerPrefs.GetString("ScenarioEditortypeoption").Length >1 && typeoption.Count == 0){
			string txt = PlayerPrefs.GetString("ScenarioEditortypeoption");
			for(int s = 0; s < 10; s ++){
				int i = txt.IndexOf(",");
				if(i >= 0){
					txt = txt.Remove(0,1);
					i = txt.IndexOf(",");
					if(i == -1){
						typeoption.Add(txt);
						txt = txt.Remove(0, txt.Length);
					}else{
						typeoption.Add(txt.Substring(0,i));
						txt = txt.Remove(0,i);
					}
				}else{
					break;
				}
			}
			
		}

		//
		GUIStyle richtext = new GUIStyle();
		richtext.richText = true;
		richtext.fontSize = 17;
		GUIStyle diatxtstyle = new GUIStyle ();
		diatxtstyle.richText = true;
		diatxtstyle.fontSize = 13;
		diatxtstyle.wordWrap = true;
		GUIStyle selectedstyle = new GUIStyle (GUI.skin.textField);
		selectedstyle.fontSize = 17;
		selectedstyle.wordWrap = true;
		GUIStyle selecteddiastyle = new GUIStyle (GUI.skin.textField);
		selecteddiastyle.fontSize = 13;
		selecteddiastyle.wordWrap = true;


		EditorGUILayout.BeginHorizontal ();
		GUILayout.BeginVertical ();
		GUI.color = Color.green;
		keystate = GUILayout.SelectionGrid (keystate,new string[]{"Nomal","Ctrl","Alt"},1,new GUILayoutOption[]{GUILayout.Width(80),GUILayout.Height(200)});
		if (GUILayout.Button ("release",new GUILayoutOption[]{GUILayout.Width(80),GUILayout.Height(50)})) {
			currentDialog = -1;
			selectedDialog.Clear();
            CopyElement.Clear();
            CopyList.Clear();
		}
        GUI.color = Color.white;
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        if (Phaselist.Length > 0)
        {
            if (LockTagtxt != "")
            {
                if (nodelist[selectedPhase].SelectNodes(LockTagtxt).Count == 0)
                {
                    DiaNodeList = nodelist[selectedPhase].ChildNodes;
                    LockTagtxt = "";
                    return;
                }
                else
                {
                    DiaNodeList = nodelist[selectedPhase].SelectNodes(LockTagtxt);
                }
            }
            else
            {
                DiaNodeList = nodelist[selectedPhase].ChildNodes;
            }
            if (DiaNodeList != null)
            {
                string[] UnitArray = new string[Mathf.CeilToInt((float)DiaNodeList.Count / SetunitNumber) + 1];
                UnitArray[0] = "All";
                for (int i = 1; i < UnitArray.Length; i++)
                {
                    UnitArray[i] = (i - 1) * SetunitNumber + "~" + ((i - 1) * SetunitNumber + SetunitNumber - 1);
                }
                if (UnitNumber >= UnitArray.Length)
                {
                    UnitNumber = UnitArray.Length - 1;
                }
                if (UnitArray.Length > 1)
                {
                    scrollunitpos = GUILayout.BeginScrollView(scrollunitpos);
                    UnitNumber = GUILayout.SelectionGrid(UnitNumber, UnitArray, 1, new GUILayoutOption[] { GUILayout.Width(65) });
                    GUILayout.EndScrollView();
                }
            }
        }
        GUILayout.EndVertical ();
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandHeight(true), GUILayout.Width(1)});
        if (Phaselist.Length > 0) {
			EditorGUILayout.BeginVertical();
			scrollpos = EditorGUILayout.BeginScrollView(scrollpos,GUIStyle.none/*,GUILayout.Width(Screen.width*0.7f)*/);
            int total;
            int startn = 0;
            if (LockTagtxt != "")
            {
                if(nodelist[selectedPhase].SelectNodes(LockTagtxt).Count == 0)
                {
                    DiaNodeList = nodelist[selectedPhase].ChildNodes;
                    LockTagtxt = "";
                    return;
                }
                else
                {
                    DiaNodeList = nodelist[selectedPhase].SelectNodes(LockTagtxt);
                    total = nodelist[selectedPhase].SelectNodes(LockTagtxt).Count;
                }
            }
            else
            {
                DiaNodeList = nodelist[selectedPhase].ChildNodes;
                total = nodelist[selectedPhase].ChildNodes.Count;
            }
            selectedDialog.Sort();
            if(UnitNumber > 0)
            {
                startn = (UnitNumber - 1)* SetunitNumber;
                if(total > (UnitNumber - 1) * SetunitNumber + SetunitNumber)
                {
                    total = (UnitNumber - 1) * SetunitNumber + SetunitNumber;
                }
            }
			for(int i = startn; i < total; i++){
				GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
				XmlElement node = DiaNodeList[i] as XmlElement;
				if(node.SelectNodes("text").Count == 0){
					Debug.LogError("Scenario Editor Error (node n." + i + ") : There is no <text> element! retry after add <text> element inside parrent element.");
					this.Close();
				}
				if(selectedDialog.Count == 0 || i != selectedDialog[0]){
                    EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal();
					string txt = i+". ";
					if(node.Attributes.GetNamedItem("name") != null){
						txt = txt + "<color=#CC723D>  "+node.Attributes.GetNamedItem("name").Value+"</color>";
					}
					if(node.Attributes.GetNamedItem("mot") != null){
						txt = txt +"<color=#008299>  ( "+node.Attributes.GetNamedItem("mot").Value + " )</color>";
					}
					if(node.Attributes.GetNamedItem("type") != null){
						txt = txt + "<color=#980000>  ( "+node.Attributes.GetNamedItem("type").Value + " )</color>";
					}


					if(GUILayout.Button(txt,richtext)){
						UnselectednodeClick(i);
					}

					if(GUILayout.Button("<color=#22741C>< "+node.Name+" ></color>",richtext,GUILayout.ExpandWidth(false))){
						UnselectednodeClick(i);
					}

					EditorGUILayout.EndHorizontal();
					if(GUILayout.Button(node.SelectSingleNode("text").InnerText, diatxtstyle)){
						UnselectednodeClick(i);
					}
					if(node.ChildNodes.Count >= 2){
						int totalele = node.ChildNodes.Count;
						string eletxt = "<color=#3F0099>";
						for (int e = 1; e < totalele; e ++){
							eletxt = eletxt + "( "+node.ChildNodes[e].Name+" ) ";
						}
						eletxt = eletxt + "</color>";
						if(GUILayout.Button(eletxt, diatxtstyle)){
							UnselectednodeClick(i);
						}
					}
					EditorGUILayout.Space();
				}else if(currentDialog == i){
					// - 선택된 대사

					GUI.color = Color.red;
					if(GUILayout.Button("▲",GUILayout.Height(25))){
						PuttoUp();
						total ++;
						break;
					}
					GUILayout.BeginHorizontal();
					//GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(10)});
					GUILayout.Label("<color=#22741C>< "+node.Name+" ></color>",richtext,GUILayout.ExpandWidth(false));
					GUI.color = Color.red;
					if(GUILayout.Button("remove",GUILayout.MaxWidth(100))){
						xmldoc.SelectSingleNode("/root").ChildNodes[selectedPhase].RemoveChild(node);
						Save();
						total -=1;
						if(i == total){
							//currentDialog --;
							selectedDialog[selectedDialog.IndexOf(i)] = i-1;
                            ChangeDialog(currentDialog - 1);
                        }
                        else
                        {
                            ChangeDialog(currentDialog);
                        }
                        return;
                    }
					GUI.color = Color.green;
					if(GUILayout.Button("Save",GUILayout.MaxWidth(100))){
						Save();
					}
					
					if(addEleChk){
						GUI.color = Color.red;
						if(addAttributeChk != 0 && GUILayout.Button("Add Attribute",GUILayout.MaxWidth(100))){
							focustarget = "addatrname";
							addAttributeChk = 0;
						}
                        GUI.color = Color.white;
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.BeginHorizontal();
						GUILayout.Label("Element",EditorStyles.boldLabel, GUILayout.MaxWidth(70));
						GUILayout.Label("Tag",GUILayout.MaxWidth(30));
						GUI.SetNextControlName("addelename");
						addEleName = EditorGUILayout.TextField(addEleName);
						GUILayout.Label("Text",GUILayout.MaxWidth(30));
						GUI.SetNextControlName("addeletext");
						addEleText = EditorGUILayout.TextField(addEleText);
						if(GUILayout.Button("Add",GUILayout.MaxWidth(40))){
							XmlElement emptynode = xmldoc.CreateElement(addEleName);
							emptynode.InnerText = addEleText;
							if(addAttributeChk == 0 && addAtrName != ""){
								emptynode.SetAttribute(addAtrName, addAtrText);
							}
							node.AppendChild(emptynode);
							addEleChk = false;
							Save();
						}
						if(GUILayout.Button("X",GUILayout.MaxWidth(25))){
							addEleChk = false;
						}
						if(addAttributeChk == 0){
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.BeginHorizontal();
							GUILayout.Label("Attribute",EditorStyles.boldLabel, GUILayout.MaxWidth(70));
							GUILayout.Label("Tag",GUILayout.MaxWidth(30));
							GUI.SetNextControlName("addatrname");
							addAtrName = EditorGUILayout.TextField(addAtrName);
							GUILayout.Label("Text",GUILayout.MaxWidth(30));
							GUI.SetNextControlName("addatrtext");
							addAtrText = EditorGUILayout.TextField(addAtrText);
							if(GUILayout.Button("X",GUILayout.MaxWidth(25))){
								addAttributeChk = -1;
							}
						}
					}else{
						GUI.color = Color.cyan;
						if(GUILayout.Button("Add Element",GUILayout.MaxWidth(100))){
							addEleText = "";
							addAtrText = "";
							addEleChk = true;
							focustarget = "addelename";
						}
                        GUI.color = Color.yellow;
                        if (GUILayout.Button("CopyChild", GUILayout.MaxWidth(100)))
                        {
                            int childcount = node.ChildNodes.Count;
                            CopyElement.Clear();
                            for (int c = 0; c < childcount; c++)
                            {
                                if (node.ChildNodes[c].Name != "text")
                                {
                                    CopyElement.Add(node.ChildNodes[c] as XmlElement);
                                }
                            }
                        }
                        if (CopyElement.Count > 0 && GUILayout.Button("PasteChild", GUILayout.MaxWidth(100)))
                        {
                            foreach (XmlElement ele in CopyElement)
                            {
                                node.AppendChild(ele.Clone());
                            }
                            Save();
                        }
                    }
                    GUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					GUI.color = Color.cyan;

					GUILayout.Label(i.ToString() + ".", richtext, GUILayout.ExpandWidth(false));
                    /*
                    if (node.Attributes.GetNamedItem("name") != null && node.Attributes.GetNamedItem("name").Value.Length > 0)
                    {
                        node.Attributes.GetNamedItem("name").Value = EditorGUILayout.TextField(node.Attributes.GetNamedItem("name").Value, selectedstyle, GUILayout.Height(25));
                    }
                    else
                    {
                        currentname = EditorGUILayout.TextField(currentname, selectedstyle, GUILayout.Height(25));
                    }

                    if (node.Attributes.GetNamedItem("mot") != null && node.Attributes.GetNamedItem("mot").Value.Length > 0)
                    {
                        node.Attributes.GetNamedItem("mot").Value = EditorGUILayout.TextField(node.Attributes.GetNamedItem("mot").Value, selectedstyle, GUILayout.Height(25));
                    }
                    else
                    {
                        currentmotion = EditorGUILayout.TextField(currentmotion, selectedstyle, GUILayout.Height(25));
                    }

                    if (node.Attributes.GetNamedItem("type") != null && node.Attributes.GetNamedItem("type").Value.Length > 0)
                    {
                        node.Attributes.GetNamedItem("type").Value = EditorGUILayout.TextField(node.Attributes.GetNamedItem("type").Value, selectedstyle, GUILayout.Height(25));
                    }
                    else
                    {
                        if (node.Attributes.GetNamedItem("type") != null && node.Attributes.GetNamedItem("type").Value.Length == 0) { node.RemoveAttribute("type"); }
                        if (currenttype.Length > 0) { node.SetAttribute("type", currenttype); }
                        currenttype = EditorGUILayout.TextField(currenttype, selectedstyle, GUILayout.Height(25));
                    }
                    */
                    if(currentname == null)
                    {
                        if (node.Attributes.GetNamedItem("name") != null)
                        {
                            currentname = node.GetAttribute("name");
                        }
                        else
                        {
                            currentname = "";
                        }
                    }
                    if (currentmotion == null)
                    {
                        if (node.Attributes.GetNamedItem("mot") != null)
                        {
                            currentmotion = node.GetAttribute("mot");
                        }
                        else
                        {
                            currentmotion = "";
                        }
                    }
                    if (currenttype == null)
                    {
                        if (node.Attributes.GetNamedItem("type") != null)
                        {
                            currenttype = node.GetAttribute("type");
                        }
                        else
                        {
                            currenttype = "";
                        }
                    }

                    currentname = EditorGUILayout.TextField(currentname, selectedstyle ,GUILayout.Height(25));
                    currentmotion = EditorGUILayout.TextField(currentmotion, selectedstyle ,GUILayout.Height(25));
                    currenttype = EditorGUILayout.TextField(currenttype,selectedstyle ,GUILayout.ExpandWidth(true),GUILayout.Height(25));

                    if (currentname == ""){
						if(node.Attributes.GetNamedItem("name") != null)
							node.Attributes.RemoveNamedItem("name");
					}else{
						node.SetAttribute("name", currentname);
					}
					if(currentmotion == ""){
						if(node.Attributes.GetNamedItem("mot") != null)
							node.Attributes.RemoveNamedItem("mot");
					}else{
						node.SetAttribute("mot", currentmotion);
					}
					if(currenttype == ""){
						if(node.Attributes.GetNamedItem("type") != null)
							node.Attributes.RemoveNamedItem("type");
					}else{
						node.SetAttribute("type", currenttype);
					}
                    
                    EditorGUILayout.EndHorizontal();
					GUI.color = Color.white;
					GUI.SetNextControlName("selecteddialog");
                    if (EditChk)
                    {
                        node.SelectSingleNode("text").InnerText = EditorGUILayout.TextArea(node.SelectSingleNode("text").InnerText, selecteddiastyle);
                    }
                    else
                    {
                        node.SelectSingleNode("text").InnerText = GUILayout.TextArea(node.SelectSingleNode("text").InnerText, selecteddiastyle);
                    }
                    if (node.ChildNodes.Count >= 2){
						int totalele = node.ChildNodes.Count;
						for (int e = 1; e < totalele; e ++){
							EditorGUILayout.BeginHorizontal();
							GUILayout.Label("<color=#3F0099>"+node.ChildNodes[e].Name + " : " + node.ChildNodes[e].InnerText + "</color>",diatxtstyle);
							string attxt = "<color=#CC3D3D>";
							for(int a = 0; a < node.ChildNodes[e].Attributes.Count; a++){
								attxt += "( "+node.ChildNodes[e].Attributes.Item(a).Name + " : " + node.ChildNodes[e].Attributes.Item(a).Value + " ) ";
							}
							attxt += "</color>";
							GUILayout.Label(attxt,diatxtstyle);
							if(addAttributeChk == e && !addEleChk){
                                if (GUILayout.Button("Element Change", GUILayout.ExpandWidth(false)))
                                {
                                    node.ChildNodes[e].InnerText = addAtrText;
                                    addAtrText = "";
                                    addAttributeChk = -1;
                                }
                                if (GUILayout.Button("CopyText", GUILayout.ExpandWidth(false)))
                                {
                                    addAtrText = node.ChildNodes[e].InnerText;
                                }
                                EditorGUILayout.EndHorizontal();
								EditorGUILayout.BeginHorizontal();
								GUILayout.Label("Tag",GUILayout.MaxWidth(30));
								//GUI.SetNextControlName("addatrname");
								addAtrName = EditorGUILayout.TextField(addAtrName);
								GUILayout.Label("Text",GUILayout.MaxWidth(30));
								addAtrText = EditorGUILayout.TextArea(addAtrText);
								if(GUILayout.Button("Add",GUILayout.Width(40))){
									if(addAtrName != ""){
										XmlElement nodeele = node.ChildNodes[e] as XmlElement;
										nodeele.SetAttribute(addAtrName, addAtrText);
									}
									Save();
									addAttributeChk = -1;
								}
                                if (GUILayout.Button("Del", GUILayout.MaxWidth(30)))
                                {
                                    if (addAtrName != "" && node.ChildNodes[e].Attributes.GetNamedItem(addAtrName) != null)
                                    {
                                        XmlElement nodeele = node.ChildNodes[e] as XmlElement;
                                        nodeele.RemoveAttribute(addAtrName);
                                        addAttributeChk = -1;
                                    }
                                }
                                if (GUILayout.Button("X",GUILayout.MaxWidth(25))){
									addAttributeChk = -1;
								}
							}else{
								if(GUILayout.Button("+",GUILayout.Width(25))){
									addEleChk = false;
									addAttributeChk = e;
								}
								if(GUILayout.Button("-",GUILayout.Width(25))){
									node.RemoveChild(node.ChildNodes[e]);
									Save();
									totalele -=1;
								}
							}
							EditorGUILayout.EndHorizontal();
						}
					}
					GUI.color = Color.red;
					EditorGUILayout.Space();
					//GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
					if(GUILayout.Button("▼",GUILayout.Height(25))){
						PuttoDown();
						total ++;
					}
					GUI.color = Color.white;

					selectedDialog.Add(selectedDialog[0]);
					selectedDialog.RemoveAt(0);
				}else{
					GUI.color = Color.blue;
					GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2)});
					EditorGUILayout.BeginHorizontal();
					string txt = i+". ";
					if(node.Attributes.GetNamedItem("name") != null){
						txt = txt + "<color=#FF0000>"+node.Attributes.GetNamedItem("name").Value+ "</color>";
					}
					if(node.Attributes.GetNamedItem("mot") != null){
						txt = txt +"<color=#008299>  ( "+node.Attributes.GetNamedItem("mot").Value + " )</color>";
					}
					if(node.Attributes.GetNamedItem("type") != null){
						txt = txt + "<color=#980000>  ( "+node.Attributes.GetNamedItem("type").Value + " )</color>";
					}
					if(GUILayout.Button(txt, richtext)){
						SelectednodeClick(i);
					}
					if(GUILayout.Button("<color=#22741C>< "+node.Name+" ></color>",richtext,GUILayout.ExpandWidth(false))){
						SelectednodeClick(i);
					}
					EditorGUILayout.EndHorizontal();
					if(GUILayout.Button("<color=#FF0000>"+node.SelectSingleNode("text").InnerText+ "</color>", diatxtstyle)){
						SelectednodeClick(i);
					}
					if(node.ChildNodes.Count >= 2){
						int totalele = node.ChildNodes.Count;
						string eletxt = "<color=#3F0099>";
						for (int e = 1; e < totalele; e ++){
							eletxt = eletxt + "( "+node.ChildNodes[e].Name+" ) ";
						}
						eletxt = eletxt + "</color>";
						if(GUILayout.Button(eletxt, diatxtstyle)){
							SelectednodeClick(i);
						}
					}
					GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2)});
					GUI.color = Color.white;
					selectedDialog.Add(selectedDialog[0]);
					selectedDialog.RemoveAt(0);
				}
			}
			GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
		GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandHeight(true), GUILayout.Width(1)});

		// - 입력기
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.LabelField ("TextAsset", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
		EditorGUILayout.BeginHorizontal ();
		ScenarioText = EditorGUILayout.ObjectField (ScenarioText , typeof(TextAsset), false, GUILayout.MaxWidth(widthMax));
		if (GUILayout.Button ("Read", GUILayout.MaxWidth(70))) {
			if(ScenarioText != null){
				SetData();
			}
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.LabelField ("Export", EditorStyles.boldLabel, GUILayout.MaxWidth(70));
		EditorGUILayout.BeginHorizontal ();
		if(addPhase != 3){
			exportname = EditorGUILayout.TextField(exportname, GUILayout.MaxWidth (100));
			if (GUILayout.Button ("Export", GUILayout.MaxWidth(70))) {
				if(exportname != ""){
					if(Resources.Load("Scenario/"+exportname) == null){
						ExportXml();
					}else{
						addPhase = 3;
					}
				}
			}
		} else {
			EditorGUILayout.LabelField("'"+exportname + "' is existing filename");
			if (GUILayout.Button ("Overwrite", GUILayout.MaxWidth(70))) {
				ExportXml();
				addPhase = 0;
			}
			if (GUILayout.Button ("Cancel", GUILayout.MaxWidth(70))) {
				addPhase = 0;
			}
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
        GUILayout.Label("LockTag", GUILayout.Width(60));
        FindTagtxt = GUILayout.TextField(FindTagtxt);
        if (GUILayout.Button("Lock",GUILayout.ExpandWidth(false)))
        {
            if(nodelist[selectedPhase].SelectNodes(FindTagtxt).Count > 0)
            {
                LockTagtxt = FindTagtxt;
                TagName = LockTagtxt;
                currentDialog = -1;
                selectedDialog.Clear();
            }
            else
            {
                Debug.LogError("Can't find any node with this tag");
            }
        }
        if (LockTagtxt != "" && GUILayout.Button("Cancel", GUILayout.ExpandWidth(false)))
        {
            LockTagtxt = "";
            currentDialog = -1;
            selectedDialog.Clear();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Phase",GUILayout.Width(60));
		if (addPhase == 1) {
			addPhasetxt = EditorGUILayout.TextField (addPhasetxt, GUILayout.MaxWidth (100));
			if (GUILayout.Button ("+", GUILayout.Width (25))) {
				AddPhase ();
			}
			if (GUILayout.Button ("c", GUILayout.Width (25))) {
				addPhase = 0;
			}

		}else if(addPhase == 2){
			EditorGUILayout.LabelField ("Really remove this phase?", GUILayout.MaxWidth (200));
			if (GUILayout.Button ("Yes", GUILayout.Width (70))) {
				RemovePhase();
			}
			if (GUILayout.Button ("No", GUILayout.Width (70))) {
				addPhase = 0;
			}
		}else {
			selectedPhase = EditorGUILayout.Popup (selectedPhase, Phaselist, GUILayout.MaxWidth (100));
			if(GUILayout.Button("+",GUILayout.Width(25))){
				addPhase = 1;
				addPhasetxt = "";
			}
			if(Phaselist.Length > 0){
				if(GUILayout.Button("-",GUILayout.Width(25))){
					addPhase = 2;
					addPhasetxt = "";
				}
			}

		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();
		//EditorGUILayout.LabelField ("TagName", GUILayout.MaxWidth (60));
		TagName = EditorGUILayout.TextField("TagName",TagName);
		if(selectedDialog.Count >=1){
			GUI.color = Color.cyan;
			if(GUILayout.Button("C", GUILayout.ExpandWidth(false))){
				ChangeMany("Tag");
			}
			GUI.color = Color.white;
            if (GUILayout.Button("Copy", GUILayout.ExpandWidth(false)))
            {
                TagName = DiaNodeList[currentDialog].Name;
            }
        }
        EditorGUILayout.EndHorizontal ();
		if (Phaselist.Length > 0) {
			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical();
			protectname = GUILayout.Toggle(protectname, "Name", GUILayout.MaxWidth(150));
			GUI.SetNextControlName("name");
			Name = EditorGUILayout.TextField (Name , GUILayout.MaxWidth(150));
			if(selectedDialog.Count >1){
				GUI.color = Color.cyan;
				if(GUILayout.Button("Change", GUILayout.MaxWidth(widthMax))){
					ChangeMany("name");
				}
				GUI.color = Color.white;
			}
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			protectmotion = GUILayout.Toggle(protectmotion, "Motion", GUILayout.MaxWidth(150));
			GUI.SetNextControlName("motion");
			Motion = EditorGUILayout.TextField (Motion , GUILayout.MaxWidth(150));
			if(selectedDialog.Count >1){
				GUI.color = Color.cyan;
				if(GUILayout.Button("Change", GUILayout.MaxWidth(widthMax))){
					ChangeMany("motion");
				}
				GUI.color = Color.white;
			}
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			protecttype = GUILayout.Toggle(protecttype, "Type", GUILayout.MaxWidth(150));
			GUI.SetNextControlName("type");
			Type = EditorGUILayout.TextField (Type , GUILayout.MaxWidth(150));
			if(selectedDialog.Count >1){
				GUI.color = Color.cyan;
				if(GUILayout.Button("Change", GUILayout.MaxWidth(widthMax))){
					ChangeMany("type");
				}
				GUI.color = Color.white;
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal ();

			// 일괄삭제
			if(selectedDialog.Count >1){
				GUILayout.BeginHorizontal();
				GUI.color = Color.red;
				if(GUILayout.Button("Remove Selected", GUILayout.MaxWidth(200))){
					ChangeMany("remove");
				}
				GUI.color = Color.white;
				GUILayout.EndHorizontal();
			}

			GUI.SetNextControlName("dialog");
            EditorGUI.BeginChangeCheck();
            var tempdialogue = Dialogue;
            if (EditChk)
            {
                tempdialogue = EditorGUILayout.TextArea(Dialogue);
            }
            else
            {
                tempdialogue = GUILayout.TextArea(Dialogue);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "undo");
                Dialogue = tempdialogue;
            }
            GUILayout.BeginHorizontal();
            EditChk = EditorGUILayout.Toggle(EditChk,GUILayout.Width(10));
			if(GUILayout.Button("Put", GUILayout.MaxWidth(widthMax))){
				PuttoEnd();
			}
			if(GUILayout.Button("ProtectPut", GUILayout.MaxWidth(widthMax))){
				PuttoEnd(false);
			}
			if(GUILayout.Button("Clear", GUILayout.MaxWidth(widthMax))){
				Name = "";
				Type = "";
				Motion = "";
				Dialogue = "";
			}
            if (selectedDialog.Count > 0 && GUILayout.Button("Copy", GUILayout.MaxWidth(widthMax)))
            {
                Copy();
            }
            if (CopyList.Count > 0&& GUILayout.Button("Paste", GUILayout.MaxWidth(widthMax)))
            {
                if(currentDialog >= 0)Paste();
            }
            GUILayout.EndHorizontal();
			GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
			GUILayout.Label("Quick Macro", EditorStyles.boldLabel);
			GUILayout.BeginHorizontal();

			// 퀵옵션대사
			GUILayout.BeginVertical();
			GUILayout.Label("Dialog Option", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
			if(GUILayout.Button("Reset Dialog", GUILayout.MaxWidth(100))){
				PlayerPrefs.SetString("ScenarioEditordialogoption","");
				dialogoption.Clear();
			}
			GUILayout.BeginHorizontal();
			GUI.SetNextControlName("adddiaoption");
			adddiaoption = GUILayout.TextField(adddiaoption,GUILayout.MinWidth(30));
			if(GUILayout.Button("+",GUILayout.Width(25))){
				if(adddiaoption != ""){
					dialogoption.Add(adddiaoption);
					PlayerPrefs.SetString("ScenarioEditordialogoption",PlayerPrefs.GetString("ScenarioEditordialogoption") + ","+adddiaoption);
					adddiaoption = "";
				}
			}
			GUILayout.EndHorizontal();
			scrolldiapos = GUILayout.BeginScrollView(scrolldiapos);
			for (int i = 0;i< dialogoption.Count;i ++){
				if(i == diaopselec)
					GUI.color = Color.cyan;
				if(GUILayout.Button(dialogoption[i], GUILayout.MaxWidth(120))){
					diaopselec = i;
				}
				if(i == diaopselec)
					GUI.color = Color.white;
			}
			EditorGUILayout.EndScrollView();
			GUILayout.EndVertical();

			// - 이름 퀵옵션
			GUILayout.BeginVertical();
			GUILayout.Label("Name Option", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
			if(GUILayout.Button("Reset Name", GUILayout.MaxWidth(100))){
				PlayerPrefs.SetString("ScenarioEditornameoption","");
				nameoption.Clear();
			}
			GUILayout.BeginHorizontal();
			GUI.SetNextControlName("addnameoption");
			addnameoption = GUILayout.TextField(addnameoption,GUILayout.MinWidth(30));
			if(GUILayout.Button("+",GUILayout.Width(25))){
				if(addnameoption != ""){
					nameoption.Add(addnameoption);
					PlayerPrefs.SetString("ScenarioEditornameoption",PlayerPrefs.GetString("ScenarioEditornameoption") + ","+addnameoption);
					addnameoption = "";
				}
			}
			GUILayout.EndHorizontal();
			scrollnamepos = GUILayout.BeginScrollView(scrollnamepos);
			for (int i = 0;i< nameoption.Count;i ++){
				GUILayout.BeginHorizontal();
				if(i < 9){
					GUILayout.Label(i+1+".",GUILayout.Width(10));
				}
				if(GUILayout.Button(nameoption[i], GUILayout.MaxWidth(120))){
					Name = nameoption[i];
					GUI.FocusControl("dialog");
				}
				GUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();
			GUILayout.EndVertical();

			// - 타입 퀵옵션
			GUILayout.BeginVertical();
			GUILayout.Label("Type Option", EditorStyles.boldLabel, GUILayout.MaxWidth(100));
			if(GUILayout.Button("Reset Type", GUILayout.MaxWidth(100))){
				PlayerPrefs.SetString("ScenarioEditortypeoption","");
				typeoption.Clear();
			}
			GUILayout.BeginHorizontal();
			GUI.SetNextControlName("addtypeoption");
			addtypeoption = GUILayout.TextField(addtypeoption,GUILayout.MinWidth(30));
			if(GUILayout.Button("+",GUILayout.Width(25))){
				if(addtypeoption != ""){
					typeoption.Add(addtypeoption);
					PlayerPrefs.SetString("ScenarioEditortypeoption",PlayerPrefs.GetString("ScenarioEditortypeoption") + ","+addtypeoption);
					addtypeoption = "";
				}
			}
			GUILayout.EndHorizontal();
			scrolltypepos = GUILayout.BeginScrollView(scrolltypepos);
			for (int i = 0;i< typeoption.Count;i ++){
				if(GUILayout.Button(typeoption[i], GUILayout.MaxWidth(120))){
					Type = typeoption[i];
					GUI.FocusControl("dialog");
				}
			}
			EditorGUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();


			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndHorizontal ();
    }

	void ChangeMany(string ctarget){
		if (ctarget == "name") {
			currentname = Name;
			foreach (int i in selectedDialog) {
				XmlElement ele = DiaNodeList[i] as XmlElement;
				if (Name == "") {
					if (ele.Attributes.GetNamedItem ("name") != null)
						ele.Attributes.RemoveNamedItem ("name");
				} else {
					ele.SetAttribute ("name", Name);
				}
			}
		} else if (ctarget == "motion") {
			currentmotion = Motion;
			foreach (int i in selectedDialog) {
				XmlElement ele = DiaNodeList[i] as XmlElement;
				if (Motion == "") {
					if (ele.Attributes.GetNamedItem ("mot") != null)
						ele.Attributes.RemoveNamedItem ("mot");
				} else {
					ele.SetAttribute ("mot", Motion);
				}
			}
		} else if (ctarget == "type") {
			currenttype = Type;
			foreach (int i in selectedDialog) {
				XmlElement ele = DiaNodeList[i] as XmlElement;
				if (Type == "") {
					if (ele.Attributes.GetNamedItem ("type") != null)
						ele.Attributes.RemoveNamedItem ("type");
				} else {
					ele.SetAttribute ("type", Type);
				}
			}
		} else if (ctarget == "remove") {
			selectedDialog.Reverse ();
			foreach (int i in selectedDialog) {
				nodelist [selectedPhase].RemoveChild (DiaNodeList[i]);
			}
			selectedDialog.Clear ();
			currentDialog = -1;
		} else if (ctarget == "Tag") {
			foreach (int i in selectedDialog) {
				if (TagName != "") {
					XmlElement ele = DiaNodeList[i] as XmlElement;
					XmlElement emptynode = xmldoc.CreateElement (TagName);
					emptynode.InnerXml = ele.InnerXml;
					if(ele.Attributes.GetNamedItem("name") != null){
						emptynode.SetAttribute("name",ele.GetAttribute("name"));
					}
					if(ele.Attributes.GetNamedItem("mot") != null){
						emptynode.SetAttribute("mot",ele.GetAttribute("mot"));
					}
					if(ele.Attributes.GetNamedItem("type") != null){
						emptynode.SetAttribute("type",ele.GetAttribute("type"));
					}
					nodelist[selectedPhase].InsertAfter(emptynode , ele);
					ele.ParentNode.RemoveChild(ele);
				}
			}
		}
		Save ();
	}
	
	void ChangeDialog(int i){
		GUI.FocusControl ("");
		currentDialog = i;
        /*
        XmlNode node = DiaNodeList[i];
		if (node.Attributes.GetNamedItem ("name") != null) {
			currentname = node.Attributes.GetNamedItem ("name").Value;
		} else {
			currentname = "";
		}
		if (node.Attributes.GetNamedItem ("mot") != null) {
			currentmotion = node.Attributes.GetNamedItem ("mot").Value;
		} else {
			currentmotion = "";
		}
		if (node.Attributes.GetNamedItem ("type") != null) {
			currenttype = node.Attributes.GetNamedItem ("type").Value;
		} else {
			currenttype = "";
		}
        */
        currentname = null;
        currentmotion = null;
        currenttype = null;
        if(UnitNumber >0 && i >= (UnitNumber)* SetunitNumber)
        {
            UnitNumber++;
        }
	}

	void SetData(){
		if (ScenarioText != null) {
			xmldoc = new XmlDocument();  // 객체선언
			xmldoc.LoadXml((ScenarioText as TextAsset).text);
		}

		nodelist = xmldoc.SelectSingleNode("/root").ChildNodes;
		int total = nodelist.Count;
		Phaselist = new string[total];
		for(int i = 0; i < total; i ++){
			Phaselist[i] = nodelist[i].Name;
		}
		selectedPhase = PlayerPrefs.GetInt ("ScenarioEditorselectedPhase");
		if (selectedPhase >= Phaselist.Length) {
			selectedPhase = 0;
		}
		PlayerPrefs.SetInt ("ScenarioEditorselectedPhase", selectedPhase);
		exportname = PlayerPrefs.GetString ("ScenarioEditorexportname");
		selectedDialog.Clear ();
		currentDialog = 0;
		TagName = PlayerPrefs.GetString ("ScenarioEditorTagName");
		if (TagName == "") {
			TagName = "say";
		}
	}
	void PuttoEnd(bool removeChk = true){
		XmlElement emptynode = xmldoc.CreateElement (TagName);
		if (Name != "") {
			emptynode.SetAttribute ("name", Name);
		}
		if (Motion != "") {
			emptynode.SetAttribute ("mot", Motion);
		}
		if (Type != "") {
			emptynode.SetAttribute ("type", Type);
		}
		emptynode.AppendChild (xmldoc.CreateElement ("text"));
		emptynode.SelectSingleNode ("text").InnerText = Dialogue;
        nodelist[selectedPhase].InsertAfter(emptynode, DiaNodeList[DiaNodeList.Count - 1]);
        nodelist = xmldoc.SelectSingleNode ("/root").ChildNodes;
		Save ();
		selectedDialog.Clear ();
		selectedDialog.Add (DiaNodeList.Count - 1);
		ChangeDialog (DiaNodeList.Count - 1);
		Dialogue = "";
		if (removeChk || !protectmotion) {
			Motion = "";
		}
		if (removeChk || !protecttype) {
			Type = "";
		}
		scrollpos = new Vector2 (0, Mathf.Infinity);
		if (removeChk || !protectname) {
			Name = "";
		}
		focustarget = "dialog";
	}
	
	void PuttoUp(){
		XmlElement emptynode = xmldoc.CreateElement (TagName);
		if(Name != ""){
			emptynode.SetAttribute("name",Name);
		}
		if(Motion != ""){
			emptynode.SetAttribute("mot",Motion);
		}
		if(Type != ""){
			emptynode.SetAttribute("type", Type);
		}
		emptynode.AppendChild (xmldoc.CreateElement("text"));
		emptynode.SelectSingleNode("text").InnerText = Dialogue;
        nodelist[selectedPhase].InsertBefore(emptynode, DiaNodeList[currentDialog]);
        selectedDialog.Clear();
		selectedDialog.Add(currentDialog);
		ChangeDialog(currentDialog);
        Dialogue = "";
        if (!protectmotion)
        {
            Motion = "";
        }
        if (!protecttype)
        {
            Type = "";
        }
        if (!protectname)
        {
            Name = "";
        }
        Save();
		focustarget = "dialog";
		Event.current.Use ();
	}
	
	void PuttoDown(){
		XmlElement emptynode = xmldoc.CreateElement (TagName);
		if(Name != ""){
			emptynode.SetAttribute("name",Name);
		}
		if(Motion != ""){
			emptynode.SetAttribute("mot",Motion);
		}
		if(Type != ""){
			emptynode.SetAttribute("type", Type);
		}
		emptynode.AppendChild (xmldoc.CreateElement("text"));
		emptynode.SelectSingleNode("text").InnerText = Dialogue;
        nodelist[selectedPhase].InsertAfter(emptynode, DiaNodeList[currentDialog]);
        selectedDialog.Clear();
		selectedDialog.Add(currentDialog+1);
		ChangeDialog(currentDialog+1);
        Dialogue = "";
        if (!protectmotion)
        {
            Motion = "";
        }
        if (!protecttype)
        {
            Type = "";
        }
        if (!protectname)
        {
            Name = "";
        }
        Save();
		focustarget = "dialog";
	}
	
	void AddPhase(){
		if (addPhasetxt != "") {
			addPhase = 0;
			XmlElement emptynode = xmldoc.CreateElement (addPhasetxt);
			xmldoc.SelectSingleNode ("/root").AppendChild (emptynode);
			int total = nodelist.Count;
			Phaselist = new string[total];
			for(int i = 0; i < total; i ++){
				Phaselist[i] = nodelist[i].Name;
			}
			selectedPhase = Phaselist.Length-1;
			addPhasetxt = "";
		}
		Save ();
	}

	void RemovePhase(){
		xmldoc.SelectSingleNode ("/root").RemoveChild (nodelist [selectedPhase]);
		int total = nodelist.Count;
		Phaselist = new string[total];
		for (int i = 0; i < total; i ++) {
			Phaselist [i] = nodelist [i].Name;
		}
		if (Phaselist.Length > 0) {
			selectedPhase -= 1;
		}
		addPhase = 0;
		Save ();
	}

	void UnselectednodeClick(int i){
		if(keystate == 0){
			selectedDialog.Clear();
		}
		if(keystate == 2){
			int st = Mathf.Min(i,currentDialog);
			int t = st + Mathf.Abs(i - currentDialog);
			for(int s = st; s < t; s ++){
				if(selectedDialog.IndexOf(s) == -1){
					selectedDialog.Add(s);
				}
			}
		}
		if(selectedDialog.IndexOf(i) == -1)selectedDialog.Add(i);
		ChangeDialog(i);
	}

	void SelectednodeClick(int i){
		if(keystate == 1){
			selectedDialog.Remove(i);
		}else if(keystate == 2){
			int st = Mathf.Min(i,currentDialog);
			int t = st + Mathf.Abs(i - currentDialog);
			for(int s = st; s < t; s ++){
				if(selectedDialog.IndexOf(s) == -1){
					selectedDialog.Add(s);
				}
			}
			if(selectedDialog.IndexOf(i) == -1)selectedDialog.Add(i);
			ChangeDialog(i);
		}else{
			selectedDialog.Clear();
			selectedDialog.Add(i);
			ChangeDialog(i);
		}
	}

	void Save(){
		PlayerPrefs.SetInt ("ScenarioEditorselectedPhase", selectedPhase);
		PlayerPrefs.SetString ("ScenarioEditorTagName", TagName);
		xmldoc.Save (Application.dataPath + "/GalTraScenarioEditor/Resources/ScenarioEditor.xml");
	}

	void ExportXml(){
		if (Directory.Exists (Application.dataPath + "/GalTraScenarioEditor/Resources")) {
			if (Directory.Exists (Application.dataPath + "/GalTraScenarioEditor/Resources/Scenario") == false) {
				Directory.CreateDirectory(Application.dataPath + "/GalTraScenarioEditor/Resources/Scenario");
			}
		} else {
			Directory.CreateDirectory(Application.dataPath + "/GalTraScenarioEditor/Resources");
			Directory.CreateDirectory(Application.dataPath + "/GalTraScenarioEditor/Resources/Scenario");
		}
		xmldoc.Save (Application.dataPath + "/GalTraScenarioEditor/Resources/Scenario/" + exportname+".xml");
		PlayerPrefs.SetString ("ScenarioEditorexportname", exportname);
	}

    void Copy()
    {
        CopyList.Clear();
        foreach (int i in selectedDialog)
        {
            CopyList.Add(DiaNodeList[i] as XmlElement);
        }
    }

    void Paste(bool downchk = true)
    {
        int total = CopyList.Count;
        for(int i = 0; i < total; i++)
        {
            XmlElement ele;
            /*
            if(CopyList[i] > currentDialog)
            {
                ele = DiaNodeList[CopyList[i]+i].Clone() as XmlElement;
            }
            else
            {
                ele = DiaNodeList[CopyList[i]].Clone() as XmlElement;
            }
            */
            ele = CopyList[i].Clone() as XmlElement;
            DiaNodeList[0].ParentNode.InsertAfter(ele, DiaNodeList[currentDialog+i]);
        }
        Save();
        //Debug.Log(currentDialog);
    }
}
