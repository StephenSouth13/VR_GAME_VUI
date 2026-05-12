using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class HammerHit : MonoBehaviour
{
    [Header("Collision Settings")]
    [SerializeField] private float minVelocityToHit = 0.5f;
    
    [Header("Optional Effects")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private ParticleSystem hitParticles;
    
    [Header("Cupcake Destruction")]
    [SerializeField] private float fadeDuration = 1f;
    
    private AudioSource audioSource;
    private bool canHit = true;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private CupcakeSpawner spawner;
    
    void Start()
    {
        if (hitSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = hitSound;
            audioSource.playOnAwake = false;
        }
        
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }
        
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
        
        spawner = FindObjectOfType<CupcakeSpawner>();
    }
    
    void OnGrabbed(SelectEnterEventArgs args)
    {
        Debug.Log("✊ MARTEAU GRABÉ");
    }
    
    void OnReleased(SelectExitEventArgs args)
    {
        Debug.Log("🖐️ MARTEAU RELÂCHÉ");
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cupcake") && canHit)
        {
            float impactVelocity = collision.relativeVelocity.magnitude;
            
            if (impactVelocity > minVelocityToHit)
            {
                Debug.Log($"✅ CUPCAKE HIT! Velocity={impactVelocity:F2}");
                
                ScoreManagerFirstScene firstScene = FindObjectOfType<ScoreManagerFirstScene>();
                ScoreManager secondScene = FindObjectOfType<ScoreManager>();
                
                if (firstScene != null)
                {
                    firstScene.OnCupcakeHit();
                    Debug.Log("🎯 +10 POINTS! (GameScene 0-50)");
                }
                else if (secondScene != null)
                {
                    secondScene.OnCupcakeHit();
                    Debug.Log("🎯 +10 POINTS! (GameSceneIntermediaire 50-100)");
                }
                else
                {
                    Debug.LogWarning("⚠️ Aucun ScoreManager trouvé!");
                }
                
                // Son
                if (audioSource != null && hitSound != null)
                {
                    audioSource.Play();
                }
                
                // PARTICULES
                if (hitParticles != null && collision.contacts.Length > 0)
                {
                    Vector3 hitPoint = collision.contacts[0].point;
                    GameObject particlesObj = Instantiate(hitParticles.gameObject, hitPoint, Quaternion.identity);
                    ParticleSystem ps = particlesObj.GetComponent<ParticleSystem>();
                    
                    if (ps != null)
                    {
                        var main = ps.main;
                        main.maxParticles = 200;
                        ps.Emit(50);
                        Destroy(particlesObj, 3f);
                    }
                }
                
                // BLOQUER LE CUPCAKE
                Rigidbody cupcakeRb = collision.gameObject.GetComponent<Rigidbody>();
                if (cupcakeRb != null)
                {
                    cupcakeRb.linearVelocity = Vector3.zero;
                    cupcakeRb.angularVelocity = Vector3.zero;
                    cupcakeRb.isKinematic = true;
                }
                
                Collider[] colliders = collision.gameObject.GetComponentsInChildren<Collider>();
                foreach (Collider c in colliders)
                {
                    c.enabled = false;
                }
                
                StartCoroutine(FadeAndDestroy(collision.gameObject));
                StartCoroutine(HitCooldown());
            }
        }
    }
    
    private IEnumerator FadeAndDestroy(GameObject cupcake)
    {
        Renderer[] renderers = cupcake.GetComponentsInChildren<Renderer>();
        
        Material[][] originalMaterials = new Material[renderers.Length][];
        Color[][] originalColors = new Color[renderers.Length][];
        
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
            originalColors[i] = new Color[originalMaterials[i].Length];
            
            for (int j = 0; j < originalMaterials[i].Length; j++)
            {
                originalMaterials[i][j].SetFloat("_Mode", 3);
                originalMaterials[i][j].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                originalMaterials[i][j].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                originalMaterials[i][j].SetInt("_ZWrite", 0);
                originalMaterials[i][j].DisableKeyword("_ALPHATEST_ON");
                originalMaterials[i][j].EnableKeyword("_ALPHABLEND_ON");
                originalMaterials[i][j].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                originalMaterials[i][j].renderQueue = 3000;
                
                originalColors[i][j] = originalMaterials[i][j].color;
            }
        }
        
        float elapsed = 0f;
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / fadeDuration);
            
            for (int i = 0; i < renderers.Length; i++)
            {
                for (int j = 0; j < originalMaterials[i].Length; j++)
                {
                    Color newColor = originalColors[i][j];
                    newColor.a = alpha;
                    originalMaterials[i][j].color = newColor;
                }
            }
            
            yield return null;
        }
        
        // Notify spawner
        if (spawner != null)
        {
            spawner.OnCupcakeDestroyed(cupcake);
        }

        Destroy(cupcake);
    }
    
    private IEnumerator HitCooldown()
    {
        canHit = false;
        yield return new WaitForSeconds(0.3f);
        canHit = true;
    }
}