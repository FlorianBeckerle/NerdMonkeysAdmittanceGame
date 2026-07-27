using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientCollectionSO", menuName = "Scriptable Objects/IngredientCollectionSO")]
public class IngredientCollectionSO : ScriptableObject
{
    /*
     *
     * Works like a dictionary where all the ingredients are stored
     * 
     */
    [SerializeField] private List<IngredientSO> ingredients;
    
    [SerializeField] private IngredientSO failedIngredient;
    
    [SerializeField] public GameObject itemPrefab;

    public IngredientSO FindResult(IngredientSO a, IngredientSO b)
    {
        foreach (var ingredient in ingredients)
        {
            bool matchesForward = ingredient.leftIngredient == a && ingredient.rightIngredient == b;
            bool matchesReversed = ingredient.leftIngredient == b && ingredient.rightIngredient == a;

            if (matchesForward || matchesReversed)
            {
                return ingredient;
            }
        }

        return failedIngredient; // no combination found
    }
    
}
