using UnityEngine;

//permet de créer un nouveau item depuis le menu create de unity
[CreateAssetMenu(fileName = "Nouvel item", menuName ="Inventaire/Item")]
public class Items : ScriptableObject
{
    new public string name = "nouvel item";
    public Sprite icone =null;
    public bool isDefaultItem=false;
    enum type
    {
        ARMES,
        MEDICAMENTS,
        NOURRITURE,
        EQUIPEMENT,
        OUTILS,
        MATERIAUX
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
