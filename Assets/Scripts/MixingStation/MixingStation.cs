using UnityEngine;

public class MixingStation : MonoBehaviour
{
    [SerializeField] private Collider _ingredientA = null;
    [SerializeField] private Collider _ingredientB = null;


    [SerializeField] private Transform resultSpawnLocation;

    [SerializeField] private IngredientCollectionSO _recipies; 

    public void OnIngredientEnter(TriggerRelay.TriggerSlot slot, Collider other)
    {
        //Optional: filter by tag/layer so you only react to actual ingredient objects
        if (!other.CompareTag("Ingredient")) return;

        if (slot == TriggerRelay.TriggerSlot.A)
        {
            _ingredientA = other;
            Debug.Log("Ingredient A entered: " + other.name);
        }
        else
        {
            _ingredientB = other;
            Debug.Log("Ingredient B entered: " + other.name);
        }

        TryCombine();
    }

    public void OnIngredientExit(TriggerRelay.TriggerSlot slot, Collider other)
    {
        if (slot == TriggerRelay.TriggerSlot.A && _ingredientA == other)
        {
            _ingredientA = null;
        }
        else if (slot == TriggerRelay.TriggerSlot.B && _ingredientB == other)
        {
            _ingredientB = null;
        }
    }

    private void TryCombine()
    {
        if (_ingredientA != null && _ingredientB != null)
        {
            IngredientSO ingredientA = _ingredientA.gameObject.GetComponent<Ingredient>().ingredientSO;
            IngredientSO ingredientB = _ingredientB.gameObject.GetComponent<Ingredient>().ingredientSO;

            if (_ingredientA == null || _ingredientB == null)
            {
                Debug.Log("Ingredient Error: ");
                Debug.Log("Ingredient A entered: " + _ingredientA.name);
                Debug.Log("Ingredient B entered: " + _ingredientB.name);
                return;
            }
            
            Debug.Log("Both slots filled — attempting combine");
            IngredientSO result = _recipies.FindResult(ingredientA, ingredientB);

            if (result.displayName != _recipies.GetFailedIngredient().displayName)
            {
                GameManager.instance.TryAddingRecipes(result);
            }

            SpawnResultBottle(result);
        }
    }

    private void SpawnResultBottle(IngredientSO result)
    {
        //Destroy old bottles
        Destroy(_ingredientA.gameObject);
        Destroy(_ingredientB.gameObject);
        
        //Spawn new bottle
        GameObject go = Instantiate(_recipies.itemPrefab, resultSpawnLocation.position, Quaternion.identity);
        go.transform.SetParent(resultSpawnLocation);
        Ingredient i = go.GetComponent<Ingredient>();
        if (i != null)
        {
            i.ingredientSO = result;
        }
        go.name = result.displayName;
    }
}
