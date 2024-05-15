using UnityEngine;

public class Soundtrack : MonoBehaviour
{
    public AudioClip[] Soundtracks;
    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (source.isPlaying == false) {
            source.clip = Soundtracks[Random.Range(0, Soundtracks.Length - 1)];
            source.Play();
        }
    }
}
