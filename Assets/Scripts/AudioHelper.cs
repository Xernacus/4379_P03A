using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    public static AudioSource PlayClip2D(AudioClip clip, float volume)
    {
        GameObject audioObject = new GameObject("2DAudio");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = Random.Range(0.75f, 1.25f);

        audioSource.Play();
        Object.Destroy(audioObject, clip.length);

        return audioSource;
    }
}
