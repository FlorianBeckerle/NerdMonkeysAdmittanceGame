using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    private List<IngredientSO> _possibleOrders;
    [SerializeField] private int maxOrderSize = 6;

    [Header("Components")] 
    [SerializeField] private Canvas orderUICanvas;

    [SerializeField] private GameObject orderView;
    [SerializeField] private GameObject ingredientItemUIPrefab;
    
    [SerializeField] private Animator customerAnimator;
    
    [Header("Runtime Info")] private float ordersFullfilled = 0f;
    
    
    [Header("Sound")]
    [SerializeField] private AudioSource _audioSourceSpawn;
    [SerializeField] private AudioSource _audioSourceBag;
    [SerializeField] private AudioSource _audioSourceCash;


    private bool customerActive = false;

    private int currentReward = 0;
    

    
    //List of current IngredientSOs the player needs to provice
    [SerializeField] private List<IngredientSO> currentOrder = new List<IngredientSO>();
    //List of the ui elements from the current order
    [SerializeField] private List<GameObject> currentOrderUIElements = new List<GameObject>();
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _possibleOrders = GameManager.instance.GetDiscoveredRecipes();
        
        orderUICanvas.enabled = false;

        StartCoroutine(StartCustomers());
    }


    private IEnumerator StartCustomers()
    {
        yield return new WaitForSeconds(10f);
        customerAnimator.SetTrigger("Spawn");
        PlaySpawnSound();
        GenerateNewOrder();
        CalculateReward();
        SpawnUIElements();
        
        while (true)
        {
            
            yield return new WaitForSeconds(Random.Range(10f, 20f)); //wait 10-20 seconds between customers, duration might vary depending on when in the cycle the previous customer is finished
            if (customerActive) continue; //skip if customer is still active
            customerAnimator.SetTrigger("Spawn");
            PlaySpawnSound();
            _possibleOrders = GameManager.instance.GetDiscoveredRecipes(); //get new recipes if any discovered //could be handled with an event, but I'm short on time so this must suffice for now
            GenerateNewOrder();
            CalculateReward();
            SpawnUIElements();
        }
    }


    //generate a new order with n items
    private void GenerateNewOrder()
    {
        int maxDemand = Mathf.FloorToInt(ordersFullfilled / 3);
        if (maxDemand > maxOrderSize) //cap max demand at maxOrderSize (6)
        {
            maxDemand = maxOrderSize;
        }
        int demand = Random.Range(1, maxDemand +1);
        
        //Clear list
        currentOrder = new List<IngredientSO>();

        for (int i = 0; i < demand; i++)
        {
            int randomIndex = Random.Range(0, _possibleOrders.Count);
            currentOrder.Add(_possibleOrders[randomIndex]); //get random ingredient from available
        }
        
        customerActive = true;
    }

    private void SpawnUIElements()
    {
        orderUICanvas.enabled = true;

        while (orderView.transform.childCount > 0)
        {
            Destroy(orderView.transform.GetChild(0).gameObject);
        }

        const float xSpacing = 0.24f;
        const float ySpacing = 0.24f;
        const int maxColumns = 3;

        int count = currentOrder.Count;
        int rows = Mathf.CeilToInt(count / (float)maxColumns);

        for (int i = 0; i < count; i++)
        {
            int row = i / maxColumns;
            int col = i % maxColumns;

            // how many items are actually in this row 
            int itemsInRow = Mathf.Min(maxColumns, count - row * maxColumns);

            //some magic math from the internet 
            float x = (col - (itemsInRow - 1) / 2f) * xSpacing;
            float y = (row - (rows - 1) / 2f) * -ySpacing;

            SpawnUIElement(x, y, currentOrder[i].color);
        }
    }

    private void SpawnUIElement(float xPos, float yPos, Color color)
    {
        Vector3 pos = new Vector3(xPos, yPos, 0f);
        GameObject gameObject = Instantiate(ingredientItemUIPrefab, pos, Quaternion.identity, orderView.transform);
        Image image = gameObject.GetComponent<Image>();
        image.color = color;
        
        gameObject.transform.localPosition = pos;
        gameObject.transform.localRotation = Quaternion.Euler(0f,0f,0f);
        
        
        currentOrderUIElements.Add(gameObject);
    }

    //Calculates the total value of the order //for example: 2x red potion = 2x red potion .value 
    private void CalculateReward()
    {
        int reward = 0;

        foreach (var cur in currentOrder)
        {
            reward += cur.value;
        }
        
        this.currentReward = reward;
    }
    
    private void GiveReward()
    {
        GameManager.instance.IncreaseMoney(currentReward);
        currentReward = 0;
    }

    private void PlaySpawnSound()
    {
        PlaySound(_audioSourceSpawn);
    }
    
    private void PlayBagSound()
    {
        PlaySound(_audioSourceBag);
    }
    
    private void PlayCashSound()
    {
        PlaySound(_audioSourceCash);
    }

    public void PlaySound(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        //don't activate on trigger
        if (other.isTrigger) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("PickupAble")) return;
        //_station.OnIngredientEnter(slot, other);

        IngredientSO otherIngredientSo = other.gameObject.GetComponent<Ingredient>().ingredientSO;
        if (currentOrder.Contains(otherIngredientSo))
        {
            int i = 0;
            foreach (IngredientSO ingredientSO in currentOrder)
            {
                if (ingredientSO == otherIngredientSo)
                {
                    //Remove from current order
                    currentOrder.Remove(otherIngredientSo);
                    
                    //Remove ui element from list and canvas
                    GameObject uiElement = currentOrderUIElements[i];
                    currentOrderUIElements.Remove(currentOrderUIElements[i]);
                    Destroy(uiElement);
                    
                    //Destroy bottle that was handed in
                    Destroy(other.gameObject);
                    PlayBagSound();
                    break;
                }

                i++;
            }
        }

        if (currentOrder.Count <= 0)
        {
            orderUICanvas.enabled = false;
            customerAnimator.SetTrigger("Despawn");
            PlayCashSound();
            
            customerActive = false;

            GiveReward();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("PickupAble")) return;
        //_station.OnIngredientExit(slot, other);
    }
}
