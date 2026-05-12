using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class MascotIntro : MonoBehaviour
{
    [Header("Animation")]
    [Tooltip("Nom de l'animation Wave")]
    public string waveAnimationName = "Wave";
    
    [Tooltip("Durée de la rotation (secondes)")]
    public float turnDuration = 1f;
    
    [Header("Dialogue Bubble")]
    [Tooltip("Canvas avec la bulle de dialogue")]
    public GameObject dialogueBubble;
    
    [Tooltip("TextMeshPro pour afficher le texte")]
    public TextMeshProUGUI dialogueText;
    
    [Header("Messages Courts et Mignons")]
    [TextArea(2, 5)]
    public string[] messages = new string[]
    {
        "Hi sweetie!",
        "Collect ALL the cupcakes you can!",
        "Hit them to catch them!",
        "",
        "How to play:",
        "Hold the button... grab the hammer... SMASH!",
        "",
        "Ready? Let's go!"
    };
    
    [Header("Timing")]
    public float typingSpeed = 0.04f;
    public float messageDelay = 1.5f;
    public float bubblePopSpeed = 0.3f;
    
    [Header("Scene Transition")]
    public string gameSceneName = "GameScene";
    public float delayBeforeGameStart = 1f;
    
    [Header("Audio (Optionnel)")]
    public AudioClip helloSound;
    public AudioClip textSound;
    
    private bool hasStarted = false;
    private AudioSource audioSource;
    private Vector3 bubbleOriginalScale;
    private Animation mascotAnimation;
    private Camera mainCamera;

    void Start()
    {
        // Trouver la caméra
        mainCamera = Camera.main;
        
        // Trouver automatiquement l'Animation component
        mascotAnimation = GetComponentInChildren<Animation>();
        if (mascotAnimation == null)
        {
            mascotAnimation = GetComponent<Animation>();
        }
        
        if (mascotAnimation != null)
        {
            Debug.Log("✅ Animation component trouvé!");
            mascotAnimation.playAutomatically = false;
        }
        else
        {
            Debug.LogWarning("⚠️ Aucun Animation component trouvé sur la mascotte!");
        }
        
        // Audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0.5f;
        
        // Cacher la bulle au début
        if (dialogueBubble != null)
        {
            bubbleOriginalScale = dialogueBubble.transform.localScale;
            dialogueBubble.transform.localScale = Vector3.zero;
            dialogueBubble.SetActive(false);
        }
        
        // Setup interactions VR et souris
        SetupInteraction();
        
        // Start text wobble effect
        if (dialogueText != null)
        {
            StartCoroutine(WobbleText());
        }
    }

    void Update()
    {
        // Faire en sorte que la bulle regarde toujours la caméra
        if (dialogueBubble != null && dialogueBubble.activeSelf && mainCamera != null)
        {
            dialogueBubble.transform.LookAt(mainCamera.transform);
            dialogueBubble.transform.Rotate(0, 180, 0); // Retourner pour que le texte soit dans le bon sens
        }
    }

    void SetupInteraction()
    {
        // XR pour VR controllers - utiliser celui qui existe déjà
        XRSimpleInteractable xr = GetComponent<XRSimpleInteractable>();
        
        if (xr != null)
        {
            // Nettoyer les anciens listeners
            xr.hoverEntered.RemoveAllListeners();
            xr.selectEntered.RemoveAllListeners();
            
            // Ajouter les nouveaux
            xr.hoverEntered.AddListener(OnInteract);
            xr.selectEntered.AddListener(OnInteract);
            
            Debug.Log("✅ XRSimpleInteractable configuré!");
        }
        else
        {
            Debug.LogError("❌ XRSimpleInteractable manquant sur la mascotte!");
        }
        
        // Vérifier le collider
        BoxCollider col = GetComponent<BoxCollider>();
        if (col != null)
        {
            Debug.Log("✅ BoxCollider trouvé: Size = " + col.size);
        }
        else
        {
            Debug.LogError("❌ BoxCollider manquant!");
        }
    }

    void OnInteract(BaseInteractionEventArgs args)
    {
        Debug.Log("🎯 VR Interaction détectée!");
        if (!hasStarted) StartIntroSequence();
    }

    void OnMouseEnter()
    {
        Debug.Log("🖱️ Mouse hover détecté!");
        if (!hasStarted) StartIntroSequence();
    }
    
    void OnMouseDown()
    {
        Debug.Log("🖱️ Mouse click détecté!");
        if (!hasStarted) StartIntroSequence();
    }

    void StartIntroSequence()
    {
        if (hasStarted) return;
        hasStarted = true;
        
        Debug.Log("🧁 Intro started!");
                
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // 1. Rotation vers le joueur
        yield return StartCoroutine(TurnAround());
        
        // 2. Jouer Wave animation + Hello sound!
        if (mascotAnimation != null)
        {
            mascotAnimation.wrapMode = WrapMode.Once;
            mascotAnimation.Play(waveAnimationName);
            
            Debug.Log("👋 Wave animation playing...");
            
            // PLAY HELLO SOUND WHILE WAVING
            if (helloSound != null)
            {
                audioSource.PlayOneShot(helloSound);
            }
            
            yield return new WaitForSeconds(mascotAnimation[waveAnimationName].length);
            
            Debug.Log("✅ Wave animation finished!");
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // 3. Bulle pop!
        if (dialogueBubble != null)
        {
            dialogueBubble.SetActive(true);
            yield return StartCoroutine(PopBubble());
        }
        
        // 4. Afficher tous les messages avec effet typing
        foreach (string msg in messages)
        {
            if (string.IsNullOrEmpty(msg))
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            
            yield return StartCoroutine(TypeMessage(msg));
            yield return new WaitForSeconds(messageDelay);
        }
        
        // 5. Fade out bulle
        yield return StartCoroutine(FadeOutBubble());
        
        // 6. Wave goodbye!
        Debug.Log("👋 Waving goodbye...");
        if (mascotAnimation != null)
        {
            mascotAnimation.wrapMode = WrapMode.Once;
            mascotAnimation.Play(waveAnimationName);
            yield return new WaitForSeconds(mascotAnimation[waveAnimationName].length);
        }
        
        // 7. Turn back around (180° reverse)
        Debug.Log("🔄 Turning back...");
        yield return StartCoroutine(TurnAround());
        
        // 8. Démarrer le jeu!
        yield return new WaitForSeconds(delayBeforeGameStart);
        LoadGameScene();
    }

    IEnumerator TurnAround()
    {
        float elapsed = 0f;
        Quaternion start = transform.rotation;
        Quaternion end = start * Quaternion.Euler(0, 180, 0);
        
        while (elapsed < turnDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / turnDuration);
            transform.rotation = Quaternion.Lerp(start, end, t);
            yield return null;
        }
        
        transform.rotation = end;
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
        
        // PRETTIER CUPCAKE-THEMED COLORS!
        if (message.Contains("Hi sweetie"))
        {
            styledMessage = $"<size=150%><color=#FFB6D9>{message}</color></size>";
        }
        else if (message.Contains("How to play"))
        {
            styledMessage = $"<size=140%><b><color=#FFF4A3>{message}</color></b></size>";
        }
        else if (message.Contains("SMASH"))
        {
            styledMessage = $"<b><color=#FF6B9D><size=110%>{message}</size></color></b>";
        }
        else if (message.Contains("Let's go"))
        {
            styledMessage = $"<size=160%><b><color=#B4E7CE>{message}</color></b></size>";
        }
        else if (message.Contains("ALL"))
        {
            styledMessage = message.Replace("ALL", "<b><color=#D4A5FF>ALL</color></b>");
        }
        else
        {
            styledMessage = $"<color=#FFF8E7>{message}</color>";
        }
        
        dialogueText.text = "";
        string visibleText = System.Text.RegularExpressions.Regex.Replace(styledMessage, "<.*?>", string.Empty);
        
        for (int i = 0; i <= visibleText.Length; i++)
        {
            string currentVisible = visibleText.Substring(0, i);
            
            if (message.Contains("Hi sweetie"))
            {
                dialogueText.text = $"<size=150%><color=#FFB6D9>{currentVisible}</color></size>";
            }
            else if (message.Contains("How to play"))
            {
                dialogueText.text = $"<size=140%><b><color=#FFF4A3>{currentVisible}</color></b></size>";
            }
            else if (message.Contains("SMASH"))
            {
                dialogueText.text = $"<b><color=#FF6B9D><size=110%>{currentVisible}</size></color></b>";
            }
            else if (message.Contains("Let's go"))
            {
                dialogueText.text = $"<size=160%><b><color=#B4E7CE>{currentVisible}</color></b></size>";
            }
            else if (message.Contains("ALL"))
            {
                dialogueText.text = $"<color=#FFF8E7>{currentVisible.Replace("ALL", "<b><color=#D4A5FF>ALL</color></b>")}</color>";
            }
            else
            {
                dialogueText.text = $"<color=#FFF8E7>{currentVisible}</color>";
            }
            
            // Play typing sound ONLY for letters/digits
            if (i < visibleText.Length && textSound != null && char.IsLetterOrDigit(visibleText[i]))
            {
                audioSource.PlayOneShot(textSound, 0.1f);
            }
            
            yield return new WaitForSeconds(typingSpeed);
        }
        
        // STOP typing sound when message finishes
        audioSource.Stop();
    }

    IEnumerator FadeOutBubble()
    {
        float elapsed = 0f;
        float duration = 0.5f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            dialogueBubble.transform.localScale = Vector3.Lerp(bubbleOriginalScale, Vector3.zero, t);
            yield return null;
        }
        
        dialogueBubble.SetActive(false);
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void OnDestroy()
    {
        XRSimpleInteractable xr = GetComponent<XRSimpleInteractable>();
        if (xr != null)
        {
            xr.hoverEntered.RemoveAllListeners();
            xr.selectEntered.RemoveAllListeners();
        }
    }

    IEnumerator WobbleText()
    {
        while (true)
        {
            if (dialogueBubble != null && dialogueBubble.activeSelf)
            {
                // Wobble seulement le RectTransform, pas la localScale directement
                RectTransform rectTransform = dialogueBubble.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    float wobble = 1f + Mathf.Sin(Time.time * 3f) * 0.05f;
                    rectTransform.localScale = bubbleOriginalScale * wobble;
                }
            }
            yield return new WaitForEndOfFrame(); // Au lieu de "yield return null"
        }
    }
}