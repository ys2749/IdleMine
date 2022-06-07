using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BagItem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI itemNameTMP;
    [SerializeField] public TextMeshProUGUI itemAmountTMP;
    public DropResourceStorage prop;
    // public string itemName{get;set;}
    // public int itemAmount{get;set;}

    private void Update() {
        //updateItemUI();
    }
    public void SetItemUI(){
        itemAmountTMP.text = prop.itemAmount.ToString();
        itemNameTMP.text = prop.itemName;
    }
    public bool CheckAmountAvailable(int p){
        if(prop.itemAmount>=p){
            return true;
        }else{
            return false;
        }
    }
    public void UseItem(int k){
        prop.itemAmount -= k;
        SetItemUI();
    }

    public void updateItemUI(){
        //itemAmountTMP.text  = ResoureceManager.Instance.bagStorage.ironOre.ToString();
    }

}
