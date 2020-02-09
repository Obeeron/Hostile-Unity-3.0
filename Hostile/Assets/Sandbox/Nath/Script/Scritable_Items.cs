using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Nouvel_item_Craft", menuName = "Inventaire/Item_Craft")]
public class Scritable_Items : ScriptableObject
{
    new public string name = "nouvel item";
    public Sprite icone = null;
    public List<Scritable_Materials> Material;
   
    public List<int> Number;

    
    /*public enum materials
    {
        Wood,
        Planks,
        Stone
    };*/

    private Button slot;
    private TextMeshProUGUI txt;
    private GameObject gm;
    public void OnEnter()
    {
        
        for (int i = 0; i < Material.Count; i++)
        {
            Debug.Log("il faut " + Number[i] + " de " + Material[i]);
            if(Number[i] > 0)
            {
                switch (i)
                {
                    case 0:
                        gm = GameObject.Find("Needed_1");

                        slot = gm.GetComponent<Button>();
                        gm.transform.Find("Needed_Number_1").gameObject.SetActive(true);
                        txt = gm.transform.Find("Needed_Number_1").gameObject.GetComponent<TextMeshProUGUI>();
                        txt.text = Number[i].ToString();
                        txt.fontSize = 105;
                        break;
                    case 1:
                        gm = GameObject.Find("Needed_2");

                        slot = gm.GetComponent<Button>();
                        gm.transform.Find("Needed_Number_2").gameObject.SetActive(true);
                        txt = GameObject.Find("Needed_2").GetComponentInChildren<TextMeshProUGUI>();
                        txt.text = Number[i].ToString();
                        txt.fontSize = 105;
                        break;
                    case 2:
                        gm = GameObject.Find("Needed_3");

                        slot = gm.GetComponent<Button>();
                        gm.transform.Find("Needed_Number_3").gameObject.SetActive(true);
                        txt = GameObject.Find("Needed_3").GetComponentInChildren<TextMeshProUGUI>();
                        txt.text = Number[i].ToString();
                        txt.fontSize = 105;
                        break;
                    default:
                        gm = GameObject.Find("Needed_1");

                        slot = gm.GetComponent<Button>();
                        gm.transform.Find("Needed_Number_1").gameObject.SetActive(true);
                        txt = GameObject.Find("Needed_1").GetComponentInChildren<TextMeshProUGUI>();
                        txt.text = Number[i].ToString();
                        txt.fontSize = 105;
                        break;
                }

                slot.image.sprite = Material[i].icone;

            }
        }
    }


}
