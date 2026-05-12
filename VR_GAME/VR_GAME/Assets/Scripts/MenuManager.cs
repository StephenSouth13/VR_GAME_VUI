using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    // ========== BUTTON REFERENCES ==========
    [Header("Button GameObjects")]
    public GameObject playButtonObject;
    public GameObject quitButtonObject;

    [Header("Menu Animation Objects")]
    public GameObject titleObject;
    public GameObject cupcakeObject;

    [Header("Game Settings")]
    public string gameSceneName = "IntroScene";

    // ========== BUTTON MATERIALS ==========
    [Header("Play Button Materials")]
    public Material playIdleMaterial;
    public Material playHoverMaterial;
    public Material playClickMaterial;

    [Header("Quit Button Materials")]
    public Material quitIdleMaterial;
    public Material quitHoverMaterial;
    public Material quitClickMaterial;

    // ========== AUDIO ==========
    [Header("Audio")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    // ========== ANIMATION SETTINGS ==========
    [Header("Button Animation Settings")]
    public float hoverScale = 1.15f;
    public float clickScale = 0.85f;
    public float animationSpeed = 0.2f;
    public float delayAfterClick = 0.5f;

    [Header("Glow Effect")]
    public float glowIntensity = 2f;
    public float pulseSpeed = 2f;

    // ========== MENU INTRO ANIMATIONS ==========
    [Header("Menu Intro Animation Settings")]
    public float titleFadeDuration = 1.5f;
    public float delayAfterTitle = 0.3f;
    public float cupcakeAnimDuration = 1.5f;
    public float delayBeforeButtons = 0.2f;
    public float buttonPopDuration = 0.4f;

    [Header("Animation Clips")]
    public string cupcakeAnimationName = "Scene";
    public string playButtonAnimationName = "Scene";
    public string quitButtonAnimationName = "Scene";

    // ========== PRIVATE VARIABLES ==========
    private XRSimpleInteractable playXR;
    private XRSimpleInteractable quitXR;
    
    private Renderer playRenderer;
    private Renderer quitRenderer;
    
    private Material playCurrentMaterial;
    private Material quitCurrentMaterial;
    
    private AudioSource playAudio;
    private AudioSource quitAudio;
    
    private Vector3 playOriginalScale;
    private Vector3 quitOriginalScale;
    
    private bool playIsHovering = false;
    private bool quitIsHovering = false;
    
    private bool isProcessing = false;

    private Animator cupcakeAnimator;
    private Animation cupcakeAnimation;
    
    private Animator playButtonAnimator;
    private Animation playButtonAnimation;
    
    private Animator quitButtonAnimator;
    private Animation quitButtonAnimation;
    
    private Renderer titleRenderer;
    private Material titleMaterial;
    private Vector3 titleOriginalScale;

    // ========== INITIALIZATION ==========
    void Start()
    {
        SaveOriginalButtonScales();
        HideButtonsInitially();
        SetupTitleForFade();
        SetupAnimations();
        InitializePlayButton();
        InitializeQuitButton();
        StartCoroutine(DelayedMenuIntro());
    }
    IEnumerator DelayedMenuIntro()
    {
        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(PlayMenuIntroSequence());
    }
    void SaveOriginalButtonScales()
    {
        if (playButtonObject != null)
        {
            playOriginalScale = playButtonObject.transform.localScale;
        }
        if (quitButtonObject != null)
        {
            quitOriginalScale = quitButtonObject.transform.localScale;
        }
    }

    void HideButtonsInitially()
    {
        if (playButtonObject != null)
        {
            playButtonObject.transform.localScale = Vector3.zero;
        }
        if (quitButtonObject != null)
        {
            quitButtonObject.transform.localScale = Vector3.zero;
        }
    }

    void SetupTitleForFade()
    {
        if (titleObject == null) return;

        titleRenderer = titleObject.GetComponent<Renderer>();
        if (titleRenderer != null && titleRenderer.material != null)
        {
            titleMaterial = new Material(titleRenderer.material);
            titleRenderer.material = titleMaterial;
            
            titleOriginalScale = titleObject.transform.localScale;
            titleObject.transform.localScale = Vector3.zero;
        }
    }

    void SetupAnimations()
    {
        if (cupcakeObject != null)
        {
            cupcakeAnimator = cupcakeObject.GetComponent<Animator>();
            cupcakeAnimation = cupcakeObject.GetComponent<Animation>();
        }

        if (playButtonObject != null)
        {
            playButtonAnimator = playButtonObject.GetComponent<Animator>();
            playButtonAnimation = playButtonObject.GetComponent<Animation>();
        }

        if (quitButtonObject != null)
        {
            quitButtonAnimator = quitButtonObject.GetComponent<Animator>();
            quitButtonAnimation = quitButtonObject.GetComponent<Animation>();
        }
    }

    IEnumerator PlayMenuIntroSequence()
    {
        Debug.Log("✨ Starting menu intro sequence...");

        yield return StartCoroutine(FadeInTitle());
        
        StartCoroutine(PlayCupcakeAnimation());
        
        yield return StartCoroutine(PopUpButtons());

        Debug.Log("✅ Menu sequence complete!");
    }

    IEnumerator FadeInTitle()
    {
        Debug.Log("✨ Title fading in (gold metallic)...");
        
        if (titleObject != null)
        {
            float elapsed = 0f;
            
            while (elapsed < titleFadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / titleFadeDuration;
                
                float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f);
                
                titleObject.transform.localScale = Vector3.Lerp(Vector3.zero, titleOriginalScale, smoothT);
                
                if (titleMaterial != null)
                {
                    if (titleMaterial.HasProperty("_EmissionColor"))
                    {
                        Color emissionColor = titleMaterial.GetColor("_EmissionColor");
                        titleMaterial.SetColor("_EmissionColor", emissionColor * smoothT * 2f);
                    }
                }
                
                yield return null;
            }
            
            titleObject.transform.localScale = titleOriginalScale;
            
            if (titleMaterial != null && titleMaterial.HasProperty("_EmissionColor"))
            {
                Color originalEmission = titleMaterial.GetColor("_EmissionColor");
                titleMaterial.SetColor("_EmissionColor", originalEmission * 2f);
            }
        }
        else
        {
            yield return new WaitForSeconds(titleFadeDuration);
        }
    }

    IEnumerator PlayCupcakeAnimation()
    {
        Debug.Log("🧁 Cupcake animation starting...");
        
        if (cupcakeObject == null)
        {
            Debug.LogError("Cupcake object is NULL!");
            yield break;
        }
        
        if (cupcakeAnimator != null)
        {
            Debug.Log("Found Animator component on cupcake");
            
            if (cupcakeAnimator.runtimeAnimatorController == null)
            {
                Debug.LogError("Cupcake Animator has no AnimatorController assigned!");
                yield break;
            }
            
            Debug.Log("Playing animation: " + cupcakeAnimationName);
            cupcakeAnimator.Play(cupcakeAnimationName, 0, 0f);
            cupcakeAnimator.enabled = true;
        }
        else if (cupcakeAnimation != null)
        {
            Debug.Log("Found Animation component, playing: " + cupcakeAnimationName);
            
            // Set to loop and play
            cupcakeAnimation.wrapMode = WrapMode.Loop;
            cupcakeAnimation[cupcakeAnimationName].speed = 1.5f; // Adjust speed (1.5x faster)
            cupcakeAnimation.Play(cupcakeAnimationName);
        }
        else
        {
            Debug.LogError("No Animator or Animation component found on cupcake object: " + cupcakeObject.name);
        }
        
        yield return null;
    }

    IEnumerator PopUpButtons()
    {
        Debug.Log("🎮 Buttons popping up...");
        
        if (playButtonObject != null)
        {
            if (playButtonAnimator != null)
            {
                playButtonAnimator.Play(playButtonAnimationName);
            }
            else if (playButtonAnimation != null)
            {
                playButtonAnimation.Play(playButtonAnimationName);
            }
            else
            {
                StartCoroutine(PopUpButton(playButtonObject, playOriginalScale));
            }
        }

        yield return new WaitForSeconds(0.1f);

        if (quitButtonObject != null)
        {
            if (quitButtonAnimator != null)
            {
                quitButtonAnimator.Play(quitButtonAnimationName);
            }
            else if (quitButtonAnimation != null)
            {
                quitButtonAnimation.Play(quitButtonAnimationName);
            }
            else
            {
                StartCoroutine(PopUpButton(quitButtonObject, quitOriginalScale));
            }
        }

        yield return new WaitForSeconds(buttonPopDuration);
    }

    IEnumerator PopUpButton(GameObject button, Vector3 originalScale)
    {
        button.transform.localScale = Vector3.zero;

        float elapsed = 0f;
        while (elapsed < buttonPopDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / buttonPopDuration;
            t = ElasticEaseOut(t);
            button.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);
            yield return null;
        }

        button.transform.localScale = originalScale;
    }

    float ElasticEaseOut(float t)
    {
        if (t == 0f || t == 1f) return t;
        float p = 0.3f;
        return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - p / 4f) * (2f * Mathf.PI) / p) + 1f;
    }

    void InitializePlayButton()
    {
        if (playButtonObject == null) return;

        playRenderer = playButtonObject.GetComponent<Renderer>();

        playAudio = playButtonObject.GetComponent<AudioSource>();
        if (playAudio == null)
        {
            playAudio = playButtonObject.AddComponent<AudioSource>();
            playAudio.playOnAwake = false;
            playAudio.spatialBlend = 0;
        }

        playXR = playButtonObject.GetComponent<XRSimpleInteractable>();
        if (playXR != null)
        {
            playXR.hoverEntered.AddListener(OnPlayHoverEnter);
            playXR.hoverExited.AddListener(OnPlayHoverExit);
            playXR.selectEntered.AddListener(OnPlayClicked);
        }

        SetPlayMaterial(playIdleMaterial);

        ButtonMouseHandler mouseHandler = playButtonObject.GetComponent<ButtonMouseHandler>();
        if (mouseHandler == null)
        {
            mouseHandler = playButtonObject.AddComponent<ButtonMouseHandler>();
        }
        mouseHandler.menuManager = this;
        mouseHandler.isPlayButton = true;
    }

    void InitializeQuitButton()
    {
        if (quitButtonObject == null) return;

        quitRenderer = quitButtonObject.GetComponent<Renderer>();

        quitAudio = quitButtonObject.GetComponent<AudioSource>();
        if (quitAudio == null)
        {
            quitAudio = quitButtonObject.AddComponent<AudioSource>();
            quitAudio.playOnAwake = false;
            quitAudio.spatialBlend = 0;
        }

        quitXR = quitButtonObject.GetComponent<XRSimpleInteractable>();
        if (quitXR != null)
        {
            quitXR.hoverEntered.AddListener(OnQuitHoverEnter);
            quitXR.hoverExited.AddListener(OnQuitHoverExit);
            quitXR.selectEntered.AddListener(OnQuitClicked);
        }

        SetQuitMaterial(quitIdleMaterial);

        ButtonMouseHandler mouseHandler = quitButtonObject.GetComponent<ButtonMouseHandler>();
        if (mouseHandler == null)
        {
            mouseHandler = quitButtonObject.AddComponent<ButtonMouseHandler>();
        }
        mouseHandler.menuManager = this;
        mouseHandler.isPlayButton = false;
    }

    void Update()
    {
        if (!isProcessing)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                TriggerPlayButton();
            }

            if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Escape))
            {
                TriggerQuitButton();
            }
        }
    }

    // ========== PLAY BUTTON INTERACTIONS ==========
    void OnPlayHoverEnter(HoverEnterEventArgs args)
    {
        Debug.Log("Hovering over: " + playButtonObject.name);
        playIsHovering = true;

        if (hoverSound != null && playAudio != null)
        {
            playAudio.PlayOneShot(hoverSound, 0.3f);
        }

        SetPlayMaterial(playHoverMaterial);
        StartCoroutine(ScaleAnimation(playButtonObject.transform, playOriginalScale * hoverScale));
        StartCoroutine(PlayPulseGlow());
    }

    void OnPlayHoverExit(HoverExitEventArgs args)
    {
        Debug.Log("Exit hover: " + playButtonObject.name);
        playIsHovering = false;

        SetPlayMaterial(playIdleMaterial);
        StartCoroutine(ScaleAnimation(playButtonObject.transform, playOriginalScale));
    }

    void OnPlayClicked(SelectEnterEventArgs args)
    {
        if (!isProcessing)
        {
            Debug.Log("Clicked: " + playButtonObject.name);

            if (clickSound != null && playAudio != null)
            {
                playAudio.PlayOneShot(clickSound, 0.5f);
            }

            StartCoroutine(PlayClickAnimation());
        }
    }

    // ========== QUIT BUTTON INTERACTIONS ==========
    void OnQuitHoverEnter(HoverEnterEventArgs args)
    {
        Debug.Log("Hovering over: " + quitButtonObject.name);
        quitIsHovering = true;

        if (hoverSound != null && quitAudio != null)
        {
            quitAudio.PlayOneShot(hoverSound, 0.3f);
        }

        SetQuitMaterial(quitHoverMaterial);
        StartCoroutine(ScaleAnimation(quitButtonObject.transform, quitOriginalScale * hoverScale));
        StartCoroutine(QuitPulseGlow());
    }

    void OnQuitHoverExit(HoverExitEventArgs args)
    {
        Debug.Log("Exit hover: " + quitButtonObject.name);
        quitIsHovering = false;

        SetQuitMaterial(quitIdleMaterial);
        StartCoroutine(ScaleAnimation(quitButtonObject.transform, quitOriginalScale));
    }

    void OnQuitClicked(SelectEnterEventArgs args)
    {
        if (!isProcessing)
        {
            Debug.Log("Clicked: " + quitButtonObject.name);

            if (clickSound != null && quitAudio != null)
            {
                quitAudio.PlayOneShot(clickSound, 0.5f);
            }

            StartCoroutine(QuitClickAnimation());
        }
    }

    // ========== ANIMATIONS ==========
    IEnumerator ScaleAnimation(Transform target, Vector3 targetScale)
    {
        Vector3 startScale = target.localScale;
        float elapsed = 0f;

        while (elapsed < animationSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationSpeed;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            target.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        target.localScale = targetScale;
    }

    IEnumerator PlayClickAnimation()
    {
        SetPlayMaterial(playClickMaterial);
        yield return StartCoroutine(ScaleAnimation(playButtonObject.transform, playOriginalScale * clickScale));
        yield return new WaitForSeconds(0.1f);

        if (playIsHovering)
        {
            SetPlayMaterial(playHoverMaterial);
            yield return StartCoroutine(ScaleAnimation(playButtonObject.transform, playOriginalScale * hoverScale));
        }
        else
        {
            SetPlayMaterial(playIdleMaterial);
            yield return StartCoroutine(ScaleAnimation(playButtonObject.transform, playOriginalScale));
        }

        yield return new WaitForSeconds(delayAfterClick);
        LoadGameScene();
    }

    IEnumerator QuitClickAnimation()
    {
        SetQuitMaterial(quitClickMaterial);
        yield return StartCoroutine(ScaleAnimation(quitButtonObject.transform, quitOriginalScale * clickScale));
        yield return new WaitForSeconds(0.1f);

        if (quitIsHovering)
        {
            SetQuitMaterial(quitHoverMaterial);
            yield return StartCoroutine(ScaleAnimation(quitButtonObject.transform, quitOriginalScale * hoverScale));
        }
        else
        {
            SetQuitMaterial(quitIdleMaterial);
            yield return StartCoroutine(ScaleAnimation(quitButtonObject.transform, quitOriginalScale));
        }

        yield return new WaitForSeconds(delayAfterClick);
        QuitGame();
    }

    IEnumerator PlayPulseGlow()
    {
        while (playIsHovering && playRenderer != null && playCurrentMaterial != null)
        {
            float emission = Mathf.PingPong(Time.time * pulseSpeed, 1f) * glowIntensity;

            if (playCurrentMaterial.HasProperty("_EmissionColor"))
            {
                Color baseEmission = playHoverMaterial.GetColor("_EmissionColor");
                playCurrentMaterial.SetColor("_EmissionColor", baseEmission * emission);
            }

            yield return null;
        }
    }

    IEnumerator QuitPulseGlow()
    {
        while (quitIsHovering && quitRenderer != null && quitCurrentMaterial != null)
        {
            float emission = Mathf.PingPong(Time.time * pulseSpeed, 1f) * glowIntensity;

            if (quitCurrentMaterial.HasProperty("_EmissionColor"))
            {
                Color baseEmission = quitHoverMaterial.GetColor("_EmissionColor");
                quitCurrentMaterial.SetColor("_EmissionColor", baseEmission * emission);
            }

            yield return null;
        }
    }

    // ========== MATERIAL MANAGEMENT ==========
    void SetPlayMaterial(Material mat)
    {
        if (playRenderer != null && mat != null)
        {
            playCurrentMaterial = new Material(mat);
            playRenderer.material = playCurrentMaterial;
            playCurrentMaterial.EnableKeyword("_EMISSION");
        }
    }

    void SetQuitMaterial(Material mat)
    {
        if (quitRenderer != null && mat != null)
        {
            quitCurrentMaterial = new Material(mat);
            quitRenderer.material = quitCurrentMaterial;
            quitCurrentMaterial.EnableKeyword("_EMISSION");
        }
    }

    // ========== PUBLIC TRIGGER METHODS ==========
    public void TriggerPlayButton()
    {
        if (!isProcessing && playXR != null)
        {
            playXR.selectEntered.Invoke(new SelectEnterEventArgs());
        }
    }

    public void TriggerQuitButton()
    {
        if (!isProcessing && quitXR != null)
        {
            quitXR.selectEntered.Invoke(new SelectEnterEventArgs());
        }
    }

    public void OnMouseClickPlay()
    {
        TriggerPlayButton();
    }

    public void OnMouseClickQuit()
    {
        TriggerQuitButton();
    }

    public void OnMouseEnterPlay()
    {
        OnPlayHoverEnter(new HoverEnterEventArgs());
    }

    public void OnMouseExitPlay()
    {
        OnPlayHoverExit(new HoverExitEventArgs());
    }

    public void OnMouseEnterQuit()
    {
        OnQuitHoverEnter(new HoverEnterEventArgs());
    }

    public void OnMouseExitQuit()
    {
        OnQuitHoverExit(new HoverExitEventArgs());
    }

    // ========== SCENE/QUIT ACTIONS ==========
    void LoadGameScene()
    {
        isProcessing = true;
        Debug.Log("Loading game scene: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    void QuitGame()
    {
        isProcessing = true;
        Debug.Log("Quitting game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ========== CLEANUP ==========
    void OnDestroy()
    {
        if (playXR != null)
        {
            playXR.hoverEntered.RemoveAllListeners();
            playXR.hoverExited.RemoveAllListeners();
            playXR.selectEntered.RemoveAllListeners();
        }

        if (quitXR != null)
        {
            quitXR.hoverEntered.RemoveAllListeners();
            quitXR.hoverExited.RemoveAllListeners();
            quitXR.selectEntered.RemoveAllListeners();
        }
    }
}

// ========== HELPER CLASS FOR MOUSE INTERACTIONS ==========
public class ButtonMouseHandler : MonoBehaviour
{
    public MenuManager menuManager;
    public bool isPlayButton;

    void OnMouseEnter()
    {
        if (menuManager != null)
        {
            if (isPlayButton)
                menuManager.OnMouseEnterPlay();
            else
                menuManager.OnMouseEnterQuit();
        }
    }

    void OnMouseExit()
    {
        if (menuManager != null)
        {
            if (isPlayButton)
                menuManager.OnMouseExitPlay();
            else
                menuManager.OnMouseExitQuit();
        }
    }

    void OnMouseDown()
    {
        if (menuManager != null)
        {
            if (isPlayButton)
                menuManager.OnMouseClickPlay();
            else
                menuManager.OnMouseClickQuit();
        }
    }
}