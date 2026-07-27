using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public IngredientSO ingredientSO;

    private MeshRenderer _renderer;

    void Start()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        _renderer = GetComponent<MeshRenderer>();

        //get materials array
        Material[] materials = _renderer.materials;

        //Find the "Ingredient" material by name
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].name.Contains("Ingredient"))
            {
                materials[i].color = ingredientSO.color;
                break;
            }
        }

        //assign new materials
        _renderer.materials = materials;
    }
}
