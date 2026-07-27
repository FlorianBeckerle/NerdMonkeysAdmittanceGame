using UnityEngine;

public class TriggerRelay : MonoBehaviour
{
    public enum TriggerSlot { A, B }

    [SerializeField] private TriggerSlot slot;
    private MixingStation _station;

    void Awake()
    {
        //Find the parent script
        _station = GetComponentInParent<MixingStation>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("PickupAble")) return;
        _station.OnIngredientEnter(slot, other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("PickupAble")) return;
        _station.OnIngredientExit(slot, other);
    }
}
