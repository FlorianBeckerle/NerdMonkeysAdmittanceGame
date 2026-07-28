using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "IngredientSO", menuName = "Scriptable Objects/IngredientSO")]
public class IngredientSO : ScriptableObject
{
    [FormerlySerializedAs("name")] public string displayName;

    public Color color;

    public int value = 1;
    
    public IngredientSO leftIngredient; 
    public IngredientSO rightIngredient; 
}
