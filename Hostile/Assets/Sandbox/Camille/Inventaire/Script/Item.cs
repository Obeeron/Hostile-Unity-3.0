using UnityEngine;

public class Item : Interactable
{
    public ItemData itemData;
    public int usure;
    public bool inInventory;
    public bool isSelected;

    float decayEnd;
    float decayStart;



    private MeshFilter meshFilter;
    

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = itemData.mesh;
        if (!inInventory)
            CalculTemps();
    }

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }


    void PickUp()
    {
        Debug.Log("item " + itemData.name + " récupéré!");
        
        if (Inventaire.instance.Add(this))
        {
            inInventory = true;
            this.gameObject.SetActive(false);
        }
    }
    void CalculTemps()
    {
        decayStart = Time.time;
        decayEnd = decayStart + itemData.decayTime;
    }

    public void Drop(Transform pos)
    {
        inInventory = false;
        CalculTemps();
        gameObject.transform.position=pos.position;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!itemData.persistent && !inInventory)
            if (Time.time >= decayEnd)
                Destroy(this.gameObject);
    }
}
