using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManagerFirstScene : MonoBehaviour
{
    [Header("Score Settings - FIRST SCENE (0-50)")]
    [Tooltip("Score actuel du joueur")]
    public int currentScore = 0;
    
    [Tooltip("Score minimum")]
    public int minScore = 0;
    
    [Tooltip("Score maximum (50 pour aller à GameSceneIntermediaire)")]
    public int maxScore = 50;
    
    [Tooltip("Points gagnés par cupcake")]
    public int pointsPerCupcake = 10;
    
    [Header("UI References")]
    [Tooltip("Barre de progression (UI Slider)")]
    public Slider scoreBar;
    
    [Tooltip("Texte affichant le score (optionnel)")]
    public TextMeshProUGUI scoreText;
    
    [Header("Scene Transition")]
    [Tooltip("Nom de la scène suivante (GameSceneIntermediaire)")]
    public string nextSceneName = "GameSceneIntermédiaire";
    
    [Tooltip("Délai avant transition (secondes)")]
    public float transitionDelay = 2f; 
    
    public static ScoreManagerFirstScene Instance;
    
    void Awake()
    {
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
 
        if (scoreBar != null)
        {
            scoreBar.minValue = minScore;
            scoreBar.maxValue = maxScore;
            scoreBar.value = currentScore;
        }
        
        UpdateScoreDisplay();
    }
    
    public void AddScore(int points)
    {
        currentScore += points;
        currentScore = Mathf.Clamp(currentScore, minScore, maxScore);
        
        UpdateScoreDisplay();
        
        Debug.Log($"💖 Score: {currentScore}/{maxScore}");

        if (currentScore >= maxScore)
        {
            OnMaxScoreReached();
        }
    }
    
    public void OnCupcakeHit()
    {
        AddScore(pointsPerCupcake);
    }
    
    void UpdateScoreDisplay()
    {
        if (scoreBar != null)
        {
            scoreBar.value = currentScore;
        }
        
        if (scoreText != null)
        {
            scoreText.text = $"{currentScore} / {maxScore}";
        }
    }
    
    void OnMaxScoreReached()
    {
        Debug.Log("✨ Score de 50 atteint ! Transition vers scène intermédiaire...");
              
        Invoke(nameof(LoadNextScene), transitionDelay);
    }
    
    void LoadNextScene()
    {
            SceneManager.LoadScene(nextSceneName);

    }
}