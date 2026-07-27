using TMPro;
using UnityEngine;

public class HudInfo : MonoBehaviour
{
    
    [SerializeField] private TMP_Text _text;
    
    // Update is called once per frame
    public void UpdateInfoText(string text)
    {
        _text.text = text;
    }
}
