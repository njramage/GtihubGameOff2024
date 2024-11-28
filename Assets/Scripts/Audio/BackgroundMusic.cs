using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;  // AudioSource component to play the audio
    [SerializeField]
    private AudioClip background1;
    [SerializeField]
    private AudioClip background2;
    private AudioClip[] musicClips = new AudioClip[2];  // Array to store music clips
    public static BackgroundMusic Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        musicClips[0] = background1;
        musicClips[1] = background2;
        PlayRandomBackgroundMusic();
    }

    void PlayRandomBackgroundMusic()
    {
        // Select a random clip from the array
        AudioClip selectedClip = musicClips[Random.Range(0, musicClips.Length)];

        // Set the selected clip to the AudioSource and play it
        audioSource.clip = selectedClip;
    
        audioSource.Play();
    }

    public void StopBackgroundMusic() {
        audioSource.Stop();
    }

    public void StartBackgroundMusic() {
        PlayRandomBackgroundMusic();
    }

    public void PauseBackgroundMusic() {
        audioSource.Pause();
    }

    public void UnPauseBackgroundMusic() {
        audioSource.Play();
    }
}
