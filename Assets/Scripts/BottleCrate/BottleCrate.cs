using UnityEngine;

public class BottleCrate : MonoBehaviour
{
    
    [Header("Components")]
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private Transform spawnPoint;
    
    
    [SerializeField] private IngredientSO ingredientSO;

    // Update is called once per frame
    void Update()
    {
        if (spawnPoint.childCount > 0)
        {
            return;
        }
        else
        {
            SpawnNewBottle();
        }
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
