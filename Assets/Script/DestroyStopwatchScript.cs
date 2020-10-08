using UnityEngine;
using System.Collections;

public class DestroyStopwatchScript : MonoBehaviour {
    public IEnumerator TimeOver(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    public IEnumerator TimeOverOff(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
