using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Singleton Pattern
    public static GameManager instance;
    
    
    [Header("Global Variables")]
    [SerializeField] private IngredientCollectionSO _ingredientCollection;
    
    
    [Header("Default Settings")]
    [SerializeField] private IngredientSO _iRed;
    [SerializeField] private IngredientSO _iBlue;
    [SerializeField] private IngredientSO _iGreen;
    
    
    [SerializeField] private List<IngredientSO> _discoveredRecipes; 
    [SerializeField] private int _curMoney = 0;
    
    public float mouseSensitivity;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        AddDefaultIngredients();
        
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
    }

    
    //Setter and Getter for Money
    public void IncreaseMoney(int amount)
    {
        _curMoney += amount;
    }

    public void DecreaseMoney(int amount)
    {
        IncreaseMoney(-amount);
    }

    public int GetMoney()
    {
        return _curMoney;
    }
    
    //Getter for IngredientCollection
    public IngredientCollectionSO GetIngredientCollection()
    {
        return _ingredientCollection;
    }
    
    //Setter and Getter for Discovered Recipes
    public List<IngredientSO> GetDiscoveredRecipes()
    {
        return _discoveredRecipes;
    }

    public void TryAddingRecipes(IngredientSO ingredient)
    {
        if (!this._discoveredRecipes.Any(i => i.displayName == ingredient.displayName))
        {
            this._discoveredRecipes.Add(ingredient);
        }
    }


    //Add red, green and blue to discovered recipes
    public void AddDefaultIngredients()
    {
        _discoveredRecipes.Add(_iRed);
        _discoveredRecipes.Add(_iBlue);
        _discoveredRecipes.Add(_iGreen);
    }
}
