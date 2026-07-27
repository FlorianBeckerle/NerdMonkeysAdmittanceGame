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
        
        newIngredient.ingredientSO = ingredientSO;
        newIngredient.ChangeColor();
    }
}
