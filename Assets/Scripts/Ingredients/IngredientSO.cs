using UnityEngine;

[CreateAssetMenu(fileName = "IngredientSO", menuName = "Scriptable Objects/IngredientSO")]
public class IngredientSO : ScriptableObject
{
    public string Name;

    public Color color;
    
    public IngredientSO leftIngredient; 
    public IngredientSO rightIngredient; 
}
