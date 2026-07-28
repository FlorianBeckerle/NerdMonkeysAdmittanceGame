using UnityEngine;

[CreateAssetMenu(fileName = "IngredientSO", menuName = "Scriptable Objects/IngredientSO")]
public class IngredientSO : ScriptableObject
{
    public string name;

    public Color color;

    public int value = 1;
    
    public IngredientSO leftIngredient; 
    public IngredientSO rightIngredient; 
}
