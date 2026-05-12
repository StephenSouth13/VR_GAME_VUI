using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private AudioClip backgroundMusic;
    
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.5f;
    
    [SerializeField] private bool spatialAudio = true;
    
    private AudioSource audioSource;
    private static MusicManager instance;
    
    void Awake()
    {
        // Singleton - empêcher les duplicatas
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject); // Persiste entre les scènes
        
        SetupMusic();
    }
    
    void SetupMusic()
    {
        // Créer ou récupérer AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configuration
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = musicVolume;
        
        if (spatialAudio)
        {
            audioSource.spatialBlend = 1f; // 100% 3D
            audioSource.minDistance = 5f;
            audioSource.maxDistance = 50f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
        }
        else
        {
            audioSource.spatialBlend = 0f; // 100% 2D
        }
        
        // Démarrer la musique si pas déjà en train de jouer
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("🎵 Musique démarrée et persistera entre les scènes!");
        }
    }
    
    // Optionnel : Changer le volume depuis d'autres scripts
    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(volume);
        }
    }
    
    // Optionnel : Fade out pour la fin
    public void FadeOut(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }
    
    private System.Collections.IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }
        
        audioSource.Stop();
    }
}