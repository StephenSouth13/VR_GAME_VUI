using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class FinalSceneController : MonoBehaviour
{
    [Header("Dialogue Bubble")]
    public GameObject dialogueBubble;
    public TextMeshProUGUI dialogueText;
    
    [Header("Message Final")]
    [TextArea(3, 10)]
    public string[] finalMessages = new string[]
    {
        "Wake up.",
        "Those weren't cupcakes... they were your friends. Real people.",
        "The drugs twisted everything, made you see sweetness where there was only horror.",
        "Every swing of that hammer, every point you earned... you destroyed the people you cared about.",
        "This is what it does—it blinds you to reality until it's too late, and there's no going back."
    };
    
    [Header("Timing")]
    public float typingSpeed = 0.05f;
    public float messageDelay = 3f;
    public float bubblePopSpeed = 0.4f;
    public float initialDelay = 2f;
    
    [Header("Fade to Black")]
    public float fadeToBlackDuration = 3f;
    public float delayBeforeQuit = 2f;
    
    [Header("Audio")]
    public AudioClip darkAmbience;
    public AudioClip heartbeatSound;
    
    private AudioSource ambienceSource;
    private AudioSource heartbeatSource;
    private Vector3 bubbleOriginalScale;
    private Camera mainCamera;
    private GameObject fadePanel;

    void Start()
    {
        mainCamera = Camera.main;
        
        // Créer le panel de fade to black
        CreateFadePanel();
        
        // Ambience audio (loop)
        ambienceSource = gameObject.AddComponent<AudioSource>();
        ambienceSource.loop = true;
        ambienceSource.spatialBlend = 0f;
        ambienceSource.volume = 0.3f;
        
        if (darkAmbience != null)
        {
            ambienceSource.clip = darkAmbience;
            ambienceSource.Play();
        }
        
        // Heartbeat audio (loop)
        heartbeatSource = gameObject.AddComponent<AudioSource>();
        heartbeatSource.loop = true;
        heartbeatSource.spatialBlend = 0f;
        heartbeatSource.volume = 0.5f;
        
        // Cacher la bulle au début
        if (dialogueBubble != null)
        {
            bubbleOriginalScale = dialogueBubble.transform.localScale;
            dialogueBubble.transform.localScale = Vector3.zero;
            dialogueBubble.SetActive(false);
        }
        
        // Démarrer la séquence automatiquement
        StartCoroutine(FinalSequence());
    }
    void CreateFadePanel()
    {
        // Créer un Canvas en Screen Space Overlay
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // Au-dessus de tout
        
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // Créer le panel noir
        fadePanel = new GameObject("FadePanel");
        fadePanel.transform.SetParent(canvasObj.transform, false);
        
        UnityEngine.UI.Image image = fadePanel.AddComponent<UnityEngine.UI.Image>();
        image.color = new Color(0, 0, 0, 0); // Noir transparent au début
        
        RectTransform rect = fadePanel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
    }

    IEnumerator FinalSequence()
    {
        // Attendre un moment dans le silence
        yield return new WaitForSeconds(initialDelay);
        
        // Jouer le son de battement de cœur (en loop)
        if (heartbeatSound != null)
        {
            heartbeatSource.clip = heartbeatSound;
            heartbeatSource.Play();
        }
        
        yield return new WaitForSeconds(1f);
        
        // Bulle apparaît
        if (dialogueBubble != null)
        {
            dialogueBubble.SetActive(true);
            yield return StartCoroutine(PopBubble());
        }
        
        // Afficher tous les messages
        foreach (string msg in finalMessages)
        {
            if (string.IsNullOrEmpty(msg))
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            
            yield return StartCoroutine(TypeMessage(msg));
            yield return new WaitForSeconds(messageDelay);
        }
        
        // Fade out le texte de la bulle (pas la bulle elle-même)
        yield return StartCoroutine(FadeOutText());
        
        // Fade to black (yeux qui se ferment) ET fade out audio ensemble
        yield return StartCoroutine(FadeToBlackAndAudio());
        
        // Attendre un peu dans le noir
        yield return new WaitForSeconds(delayBeforeQuit);
        
        // Quitter le jeu
        QuitGame();
    }

    IEnumerator PopBubble()
    {
        float elapsed = 0f;
        
        while (elapsed < bubblePopSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / bubblePopSpeed;
            
            float bounce = Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - 0.075f) * (2f * Mathf.PI) / 0.3f) + 1f;
            
            dialogueBubble.transform.localScale = bubbleOriginalScale * bounce;
            yield return null;
        }
        
        dialogueBubble.transform.localScale = bubbleOriginalScale;
    }

    IEnumerator TypeMessage(string message)
    {
        if (dialogueText == null) yield break;
        
        dialogueText.richText = true;
        dialogueText.fontStyle = FontStyles.Normal;
        dialogueText.alignment = TextAlignmentOptions.Center;
        
        string styledMessage = message;
        
        // COULEURS DRAMATIQUES
        if (message.Contains("Wake up"))
        {
            styledMessage = $"<size=200%><b><color=#FF0000><u>{message}</u></color></b></size>";
        }
        else if (message.Contains("weren't cupcakes"))
        {
            styledMessage = $"<size=130%><color=#FFFFFF><i>{message}</i></color></size>";
        }
        else if (message.Contains("drugs twisted"))
        {
            styledMessage = $"<color=#FFAAAA>{message}</color>";
        }
        else if (message.Contains("destroyed"))
        {
            styledMessage = message.Replace("destroyed", "<b><size=150%><color=#CC0000>DESTROYED</color></size></b>");
        }
        else if (message.Contains("no going back"))
        {
            styledMessage = $"<b><color=#880000><u>{message}</u></color></b>";
        }
        else
        {
            styledMessage = $"<color=#DDDDDD>{message}</color>";
        }
        
        dialogueText.text = "";
        string visibleText = System.Text.RegularExpressions.Regex.Replace(styledMessage, "<.*?>", string.Empty);
        
        for (int i = 0; i <= visibleText.Length; i++)
        {
            string currentVisible = visibleText.Substring(0, i);
            
            if (message.Contains("Wake up"))
            {
                dialogueText.text = $"<size=200%><b><color=#FF0000><u>{currentVisible}</u></color></b></size>";
            }
            else if (message.Contains("weren't cupcakes"))
            {
                dialogueText.text = $"<size=130%><color=#FFFFFF><i>{currentVisible}</i></color></size>";
            }
            else if (message.Contains("drugs twisted"))
            {
                dialogueText.text = $"<color=#FFAAAA>{currentVisible}</color>";
            }
            else if (message.Contains("destroyed"))
            {
                dialogueText.text = $"<color=#DDDDDD>{currentVisible.Replace("destroyed", "<b><size=150%><color=#CC0000>DESTROYED</color></size></b>")}</color>";
            }
            else if (message.Contains("no going back"))
            {
                dialogueText.text = $"<b><color=#880000><u>{currentVisible}</u></color></b>";
            }
            else
            {
                dialogueText.text = $"<color=#DDDDDD>{currentVisible}</color>";
            }
            
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator FadeOutText()
    {
        if (dialogueText == null) yield break;
        
        float elapsed = 0f;
        float duration = 2f;
        Color originalColor = dialogueText.color;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / duration);
            dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        
        dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    IEnumerator FadeToBlackAndAudio()
    {
        float elapsed = 0f;
        UnityEngine.UI.Image fadePanelImage = fadePanel.GetComponent<UnityEngine.UI.Image>();
        
        float startVolumeAmbience = ambienceSource.volume;
        float startVolumeHeartbeat = heartbeatSource.volume;
        
        while (elapsed < fadeToBlackDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / fadeToBlackDuration;
            
            // Fade to black (yeux qui se ferment)
            fadePanelImage.color = new Color(0, 0, 0, progress);
            
            // Fade out audio
            ambienceSource.volume = Mathf.Lerp(startVolumeAmbience, 0, progress);
            heartbeatSource.volume = Mathf.Lerp(startVolumeHeartbeat, 0, progress);
            
            yield return null;
        }
        
        fadePanelImage.color = Color.black;
        ambienceSource.Stop();
        heartbeatSource.Stop();
    }

    void QuitGame()
    {
        Debug.Log("Fermeture du jeu...");
        
        #if UNITY_EDITOR
            // Si on est dans l'éditeur Unity, on arrête le Play Mode
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Si on est dans un build, on quitte l'application
            Application.Quit();
        #endif
    }
}