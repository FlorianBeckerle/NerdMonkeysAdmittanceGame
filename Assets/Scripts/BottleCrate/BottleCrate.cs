using UnityEngine;

public class BottleCrate : MonoBehaviour
{
    
    [Header("Components")]
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private Transform spawnPoint;
    
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private IngredientSO ingredientSO;

    private bool isInitialSpawn = true;

    // Update is called once per frame
    void Update()
    {
        if (spawnPoint.childCount > 0)
        {
            return;
        }
        else
        {
            if (!isInitialSpawn)
            {
                PlayRustleSound();    
            }
            
            SpawnNewBottle();
            isInitialSpawn = false;
        }
    }

    private void PlayRustleSound()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    private void SpawnNewBottle()
    {
        GameObject newBottle = Instantiate(ingredientPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        Ingredient newIngredient = newBottle.GetComponent<Ingredient>();
        if (newIngredient == null)
        {
            Debug.Log("New bottle has no ingredient");
            return;
        }
        
        Rigidbody rb = newBottle.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearDamping = 10;
            //might disable this later
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        
        newIngredient.ingredientSO = ingredientSO;
        newIngredient.ChangeColor();
    }
}
