using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [Header("Components")] 
    [SerializeField] private GameObject hotBar;

    [SerializeField] private GameObject hotBarItemPrefab;
    
    
    [Header("Runtime Info")]
    [SerializeField] private List<GameObject> hotBarItems;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hotBarItems = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToInventory(IngredientSO ingredient)
    {
        
    }

    public void DropItem()
    {
        
    }
    
}
