using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public IngredientSO ingredientSO;

    private MeshRenderer _renderer;
    
    //Audio Detection
    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        ChangeColor();
    }

    public void ChangeColor()
    {
        if (ingredientSO == null) return;
        
        
        _renderer = GetComponent<MeshRenderer>();

        //get materials array
        Material[] materials = _renderer.materials;

        //Find the "Ingredient" material by displayName
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


    void OnTriggerEnter(Collider other)
    {
        //if bottle is not still in crate and audiosorce is not already playing a sound
        if (this.transform.parent == null && _audioSource.isPlaying == false)
        {
            _audioSource.pitch = Random.Range(0.8f, 1.2f);
            _audioSource.Play();
        }
    }
}
