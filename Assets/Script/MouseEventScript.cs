using UnityEngine;
using System.Collections;

public class MouseEventScript : MonoBehaviour {
	RaycastHit hit;
	RaycastHit CanvasHit;
	public GameObject SearchCursor;
	bool HitChk = false;
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0) && GetComponent<DialogScript>().Inventory.gameObject.activeSelf == false) {
			SearchCursor.SetActive(true);
			Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out CanvasHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Search"));
			SearchCursor.transform.position = CanvasHit.point;
			Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Selectable"));
			if(hit.collider != null){
				if(!HitChk){
					ParticleSystem par = SearchCursor.GetComponent<ParticleSystem> ();
					par.startSize = 0.3f;
					par.startLifetime = 1;
                    var ems = par.emission;
                    ems.rate = new ParticleSystem.MinMaxCurve(7);
                    HitChk = true;
				}
			}else{
				if(HitChk){
					ParticleSystem par = SearchCursor.GetComponent<ParticleSystem> ();
					par.startSize = 0.2f;
					par.startLifetime = 2.5f;
                    var ems = par.emission;
                    ems.rate = new ParticleSystem.MinMaxCurve(2);
					HitChk = false;
				}
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			SearchCursor.SetActive(false);
			if(hit.collider != null && GetComponent<DialogScript>().Inventory.gameObject.activeSelf == false){
                GetComponent<DialogScript>().AreaCamera.GetComponent<CameraTranslateScript>().FocusOn(hit.collider.GetComponent<SearchScript>().CPosition, hit.collider.GetComponent<SearchScript>().CRotation,false);
                GetComponent<DialogScript>().StartDialog(hit.collider.GetComponent<SearchScript>().Tag);
				this.enabled = false;
			}
		}
	}
}
