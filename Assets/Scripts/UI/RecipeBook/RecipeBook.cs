using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RecipeBook : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private GameObject _view;
    [SerializeField] private TMP_Dropdown _ingredientA;
    [SerializeField] private TMP_Dropdown _ingredientB;
    [SerializeField] private Image _resultImage;
    [SerializeField] private TMP_Text _resultText;

    [SerializeField] private Button _submit;
    
    
    private List<IngredientSO> _ingredients;
    
    [Header("Inputs")]
    private IngredientCollectionSO _ingredientCollection;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _ingredientCollection = GameManager.instance.GetIngredientCollection();
        
        _submit.onClick.AddListener(OnSubmit);
        
        PopulateScrollView();
    }

    void Update()
    {
        _view.SetActive(InputRouter.instance.InventoryPressed);
    }

    private void PopulateScrollView()
    {
        _ingredients = _ingredientCollection.GetIngredientSos();

        var options = new List<TMP_Dropdown.OptionData>(_ingredients.Count);

        foreach (var ingredient in _ingredients)
        {
            Sprite colorSprite = CreateColorSprite(ingredient.color);
            options.Add(new TMP_Dropdown.OptionData(ingredient.name, colorSprite, ingredient.color));
        }

        _ingredientA.ClearOptions();
        _ingredientA.AddOptions(options);

        _ingredientB.ClearOptions();
        _ingredientB.AddOptions(options);
    }
    
    private Sprite CreateColorSprite(Color color)
    {
        var tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, color);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
    }

    private void OnSubmit()
    {
        IngredientSO ingredientA = _ingredients[_ingredientA.value];
        IngredientSO ingredientB = _ingredients[_ingredientB.value];

        IngredientSO result = _ingredientCollection.FindResult(ingredientA, ingredientB);

        if (result != null)
        {
            _resultText.text = result.name;
            _resultImage.color = result.color;
        }
        else
        {
            _resultText.text = "No result";
        }
    }

    
}
