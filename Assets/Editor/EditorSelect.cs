using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SearchScript), true)]
public class EditorSelect : Editor
{
    SearchScript mtarget;
    Transform CameraTransform;
    void OnEnable()
    {
        mtarget = target as SearchScript;
        CameraTransform = GameObject.Find("Area").transform;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("등록"))
        {
            mtarget.CPosition = CameraTransform.transform.position;
            mtarget.CRotation = CameraTransform.transform.rotation.eulerAngles;
        }
        /*
        GUILayout.BeginHorizontal();
        GUILayout.Label("AppearList");
        if(GUILayout.Button("Add")){
            mtarget.AppearList.Add(new SearchListClass());
        }
        GUILayout.EndHorizontal();
        bool[] foldchk = new bool[mtarget.AppearList.Count];
        for(int i = 0; i < mtarget.AppearList.Count; i++)
        {
            GUILayout.BeginHorizontal();
            foldchk[i] = EditorGUILayout.Foldout(foldchk[i], i.ToString());
            mtarget.AppearList[i].type = GUILayout.TextField(mtarget.AppearList[i].type);
            mtarget.AppearList[i].Tag = GUILayout.TextField(mtarget.AppearList[i].Tag);
            Debug.Log(i + " : " + mtarget.AppearList[i].type + " , " + mtarget.AppearList[i].Tag);
            if (GUILayout.Button("-",GUILayout.ExpandWidth(false)))
            {
                mtarget.AppearList.RemoveAt(i);
            }
            GUILayout.EndHorizontal();
        }
        */
    }
}
