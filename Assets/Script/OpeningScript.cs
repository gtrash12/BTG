using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningScript : MonoBehaviour {
    public Animator Anim;
    public AudioSource bgm;

    private void Start()
    {
        StartCoroutine(St());
    }

    IEnumerator St () {
        RuntimeAnimatorController ran = Anim.runtimeAnimatorController;
        Anim.Play("Opening");
        Debug.Log(ran.animationClips[0].length);
        yield return new WaitForSeconds(ran.animationClips[0].length);
        Debug.Log("done");
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
        yield break;
	}

    public void SkipOpening()
    {
        StopAllCoroutines();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
