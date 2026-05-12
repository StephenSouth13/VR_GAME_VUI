using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CupcakeSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] cupcakePrefabs;
    [SerializeField] private Transform[] spawnPositions;
    
    [Header("Timing")]
    [SerializeField] private float stayUpDuration = 3f;
    [SerializeField] private float moveUpSpeed = 2f;
    [SerializeField] private float moveDownSpeed = 2f;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float delayAfterHammerGrab = 2f;
    
    [Header("Movement")]
    [SerializeField] private float popUpHeight = 5f;
    
    [Header("Hammer Reference")]
    [SerializeField] private HammerHit hammerScript; 
    
    private List<int> availablePositions = new List<int>();
    private Dictionary<int, GameObject> activeCupcakes = new Dictionary<int, GameObject>();
    private bool canSpawn = false;
    private bool hammerWasGrabbed = false;

    void Start()
    {
        // Trouver le marteau actif dans la scène
        if (hammerScript == null)
        {
            // Chercher par tag
            GameObject hammerObj = GameObject.FindGameObjectWithTag("Hammer");
            if (hammerObj != null)
            {
                hammerScript = hammerObj.GetComponent<HammerHit>();
                Debug.Log($"🔨 Marteau trouvé: {hammerObj.name}");
            }
            else
            {
                Debug.LogError("❌ Aucun marteau trouvé avec le tag 'Hammer'!");
            }
        }
        
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            availablePositions.Add(i);
        }
        
        
        StartCoroutine(WaitForHammerGrab());
    }

    private IEnumerator WaitForHammerGrab()
    {
        Debug.Log("⏳ En attente du grab du marteau...");
        
        
        while (!hammerWasGrabbed)
        {
            if (hammerScript != null)
            {
                var grabInteractable = hammerScript.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
                if (grabInteractable != null && grabInteractable.isSelected)
                {
                    hammerWasGrabbed = true;
                    Debug.Log("✊ MARTEAU GRABÉ ! Attente de 2 secondes avant spawn...");
                    break;
                }
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        yield return new WaitForSeconds(delayAfterHammerGrab);
        
        canSpawn = true;
        Debug.Log("🧁 SPAWNING ACTIVÉ !");
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            
            if (canSpawn && availablePositions.Count > 0)
            {
                SpawnCupcake();
            }
        }
    }

    private void SpawnCupcake()
    {
        int randomIndex = Random.Range(0, availablePositions.Count);
        int positionIndex = availablePositions[randomIndex];
        availablePositions.RemoveAt(randomIndex);
        
        GameObject randomPrefab = cupcakePrefabs[Random.Range(0, cupcakePrefabs.Length)];
        Transform spawnPoint = spawnPositions[positionIndex];
        
        Vector3 downPos = spawnPoint.position;
        float heightOffset = GetHeightOffsetForPrefab(randomPrefab);
        Vector3 upPos = spawnPoint.position + Vector3.up * (popUpHeight + heightOffset);
        
        Quaternion rotation = GetRotationForPrefab(randomPrefab);
        GameObject cupcake = Instantiate(randomPrefab, downPos, rotation);
        
        activeCupcakes[positionIndex] = cupcake;
        
        StartCoroutine(CupcakeLifecycle(cupcake, positionIndex, downPos, upPos));
    }

    private float GetHeightOffsetForPrefab(GameObject prefab)
    {
        string name = prefab.name.ToLower();
        
        if (name.Contains("cherry"))
            return 0.3f;
        else if (name.Contains("strawberry"))
            return -0.6f;
        
        return 0f;
    }

    private Quaternion GetRotationForPrefab(GameObject prefab)
    {
        string name = prefab.name.ToLower();
        
        if (name.Contains("blue"))
            return Quaternion.Euler(-90f, 0f, 0f);
        else if (name.Contains("cherry"))
            return Quaternion.Euler(90f, 0f, 0f);
        else if (name.Contains("strawberry"))
            return Quaternion.Euler(90f, 0f, 0f);
        else if (name.Contains("brown"))
            return Quaternion.Euler(90f, 0f, 0f);
        else if (name.Contains("chocolate"))
            return Quaternion.Euler(35f, 0f, 0f);
        
        return Quaternion.identity;
    }

    private IEnumerator CupcakeLifecycle(GameObject cupcake, int positionIndex, Vector3 downPos, Vector3 upPos)
    {
        Rigidbody rb = cupcake.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        
        
        float elapsed = 0f;
        while (elapsed < moveUpSpeed)
        {
            if (cupcake == null) yield break;
            
            elapsed += Time.deltaTime;
            float t = elapsed / moveUpSpeed;
            cupcake.transform.position = Vector3.Lerp(downPos, upPos, t);
            yield return null;
        }
        
    
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        
        
        yield return new WaitForSeconds(stayUpDuration);
        
        
        if (cupcake != null)
        {
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            
            elapsed = 0f;
            Vector3 currentPos = cupcake.transform.position;
            
            while (elapsed < moveDownSpeed)
            {
                if (cupcake == null) yield break;
                
                elapsed += Time.deltaTime;
                float t = elapsed / moveDownSpeed;
                cupcake.transform.position = Vector3.Lerp(currentPos, downPos, t);
                yield return null;
            }
            
            if (cupcake != null)
            {
                Destroy(cupcake);
            }
        }
        
        activeCupcakes.Remove(positionIndex);
        availablePositions.Add(positionIndex);
    }

    public void OnCupcakeDestroyed(GameObject cupcake)
    {
        foreach (var kvp in activeCupcakes)
        {
            if (kvp.Value == cupcake)
            {
                activeCupcakes.Remove(kvp.Key);
                availablePositions.Add(kvp.Key);
                break;
            }
        }
    }
}