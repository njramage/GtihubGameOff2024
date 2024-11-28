using UnityEngine;

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

    public void PlayRandomSoundFxClip(AudioClip[] soundFXs, Transform spawntransform)
    {
        AudioSource audioSource = Instantiate(soundFxObject, spawntransform.position, Quaternion.identity);

        int random = Random.Range(0, soundFXs.Length);
        audioSource.clip = soundFXs[random];
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
