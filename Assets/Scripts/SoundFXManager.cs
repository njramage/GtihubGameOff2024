using UnityEngine;
using UnityEngine.Rendering;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField]
    private AudioSource soundFxObject;
    [SerializeField, Range(0f, 1f)]
    private float volume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayRandomSoundFxClip(AudioClip[] soundFXs, Transform spawntransform, float playVolume = 0)
    {
        AudioSource audioSource = Instantiate(soundFxObject, spawntransform.position, Quaternion.identity);

        int random = Random.Range(0, soundFXs.Length);
        audioSource.clip = soundFXs[random];
        audioSource.volume = playVolume == 0 ? volume : playVolume;
        audioSource.Play();

        Debug.Log("Played sound effect: " + audioSource.clip.name);

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
