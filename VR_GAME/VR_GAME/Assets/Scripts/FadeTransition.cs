using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Gère les transitions fade in/out entre scènes
/// </summary>
public class FadeTransition : MonoBehaviour
{
    private static FadeTransition instance;
    
    [Header("Fade Settings")]
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;
    
    private Image fadeImage;
    private Canvas fadeCanvas;
    
    void Awake()
    {
        // Singleton pattern - garde le fade entre les scènes
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Créer le canvas de fade
        CreateFadeCanvas();
        
        // Fade in au démarrage
        StartCoroutine(FadeIn());
    }
    
    void CreateFadeCanvas()
    {
        // Créer canvas fullscreen
        GameObject canvasGO = new GameObject("FadeCanvas");
        canvasGO.transform.SetParent(transform);
        
        fadeCanvas = canvasGO.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 9999; // Au-dessus de tout!
        
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        
        canvasGO.AddComponent<GraphicRaycaster>();
        
        // Créer l'image noire fullscreen
        GameObject imageGO = new GameObject("FadeImage");
        imageGO.transform.SetParent(canvasGO.transform);
        
        fadeImage = imageGO.AddComponent<Image>();
        fadeImage.color = fadeColor;
        
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
    }
    
    /// <summary>
    /// Fade in (noir vers transparent)
    /// </summary>
    public IEnumerator FadeIn()
    {
        float elapsed = 0f;
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
            
            yield return null;
        }
        
        // Complètement transparent
        Color finalColor = fadeImage.color;
        finalColor.a = 0f;
        fadeImage.color = finalColor;
    }
    
    /// <summary>
    /// Fade out (transparent vers noir)
    /// </summary>
    public IEnumerator FadeOut()
    {
        float elapsed = 0f;
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
            
            yield return null;
        }
        
        // Complètement opaque
        Color finalColor = fadeImage.color;
        finalColor.a = 1f;
        fadeImage.color = finalColor;
    }
    
    /// <summary>
    /// Charge une scène avec fade transition
    /// </summary>
    public static void LoadSceneWithFade(string sceneName, float fadeTime = 1f)
    {
        if (instance != null)
        {
            instance.fadeDuration = fadeTime;
            instance.StartCoroutine(instance.LoadSceneCoroutine(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    
    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Fade to black
        yield return StartCoroutine(FadeOut());
        
        // Charger la scène
        SceneManager.LoadScene(sceneName);
        
        // Fade in
        yield return StartCoroutine(FadeIn());
    }
}