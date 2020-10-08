using UnityEngine;
using System.Collections;

public class AnimationRotationScript : MonoBehaviour {
    public GameObject Mouth;
    public GameObject Sweat;
    public AudioSource Aus;

    private void Start()
    {
        Aus = Aus.GetComponents<AudioSource>()[2];
    }

    public void rotateMouth(float z)
    {
        Mouth.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, z);
    }

    public void Sound()
    {
        Aus.PlayOneShot(Resources.Load<AudioClip>("Sound/반짝2"));
    }
}
