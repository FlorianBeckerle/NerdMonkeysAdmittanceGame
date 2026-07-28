using TMPro;
using UnityEngine;

public class HudInfo : MonoBehaviour
{
    
    //Money Text
    [SerializeField] private TMP_Text _moneyText;
    
    //control window that shows controls
    [SerializeField] private GameObject _controlsView;
    
    //Small tooltop when hovering over objects
    [SerializeField] private TMP_Text _infoText;
    
    // Update is called once per frame
    public void UpdateInfoText(string text)
    {
        _infoText.text = text;
    }

    void Update()
    {
        //Check if control window should be updated
        if (_controlsView.activeInHierarchy != InputRouter.instance.ControlsPressed)
        {
            _controlsView.SetActive(InputRouter.instance.ControlsPressed);
        }
        
        //Check if money was updated
        UpdateMoney();
    }

    private void UpdateMoney()
    {
        int m = GameManager.instance.GetMoney();
        _moneyText.text = m.ToString() + " Score";
    }
}
