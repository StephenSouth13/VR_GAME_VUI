using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    [Tooltip("Score actuel du joueur")]
    public int currentScore = 50; // Commence à 50 pour la scène intermédiaire
    
    [Tooltip("Score minimum (début)")]
    public int minScore = 50;
    
    [Tooltip("Score maximum (fin du jeu)")]
    public int maxScore = 100;
    
    [Tooltip("Points gagnés par cupcake")]
    public int pointsPerCupcake = 10;
    
    [Header("UI References")]
    [Tooltip("Barre de progression (UI Slider)")]
    public Slider scoreBar;
    
    [Tooltip("Texte affichant le score (optionnel)")]
    public TextMeshProUGUI scoreText;
    
    [Header("Scene Transition")]
    [Tooltip("Nom de la scène suivante quand score = 100")]
    public string nextSceneName = "EndingScene";
    
    [Tooltip("Délai avant transition (secondes)")]
    public float transitionDelay = 2f;
    
    [Tooltip("Durée du fade out de la musique")]
    public float musicFadeOutDuration = 1.5f;
    
    // Singleton pour accès facile depuis d'autres scripts
    public static ScoreManager Instance;
    
    void Awake()
    {
        // Pattern Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        // Initialiser la barre de score
        if (scoreBar != null)
        {
            scoreBar.minValue = minScore;
            scoreBar.maxValue = maxScore;
            scoreBar.value = currentScore;
        }
        
        // Afficher le score initial
        UpdateScoreDisplay();
    }
    
    /// <summary>
    /// Ajoute des points au score
    /// </summary>
    public void AddScore(int points)
    {
        currentScore += points;
        
        // Limiter le score entre min et max
        currentScore = Mathf.Clamp(currentScore, minScore, maxScore);
        
        // Mettre à jour l'affichage
        UpdateScoreDisplay();
        
        Debug.Log($"Score: {currentScore}/{maxScore}");
        
        // Vérifier si le joueur a atteint 100 points
        if (currentScore >= maxScore)
        {
            OnMaxScoreReached();
        }
    }
    
    /// <summary>
    /// Appelée quand un cupcake est frappé
    /// </summary>
    public void OnCupcakeHit()
    {
        AddScore(pointsPerCupcake);
    }
    
    /// <summary>
    /// Met à jour l'affichage du score
    /// </summary>
    void UpdateScoreDisplay()
    {
        // Mettre à jour la barre
        if (scoreBar != null)
        {
            scoreBar.value = currentScore;
        }
        
        // Mettre à jour le texte
        if (scoreText != null)
        {
            scoreText.text = $"{currentScore} / {maxScore}";
        }
    }
    
    /// <summary>
    /// Appelée quand le score atteint 100
    /// </summary>
    void OnMaxScoreReached()
    {
        Debug.Log("🎉 Score de 100 atteint ! FIN DU JEU !");
        
        // Fade out de la musique via MusicManager
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.FadeOut(musicFadeOutDuration);
            Debug.Log("🎵 Fade out de la musique...");
        }
        
        // Charger la scène suivante après le délai
        Invoke(nameof(LoadNextScene), transitionDelay);
    }
    
    /// <summary>
    /// Charge la scène suivante
    /// </summary>
    void LoadNextScene()
    {
            SceneManager.LoadScene(nextSceneName);

    }
    
    /// <summary>
    /// Réinitialiser le score (pour tester)
    /// </summary>
    public void ResetScore()
    {
        currentScore = 50; // Retour à 50 pour scène intermédiaire
        UpdateScoreDisplay();
    }
}