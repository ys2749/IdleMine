using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItem : Button
{
    public EquipmentProperty prop;
    
    public void SetUI(){
        this.GetComponentInChildren<Text>().text = prop.localName;
        Transform equippedMark = transform.Find("装备标志");
        equippedMark.gameObject.SetActive(System.Convert.ToBoolean(prop.equiped));
    }
    public void onClicked(){
        EquipmentManager.Instance.selectedItemLocalID = prop.localID;
        EquipmentManager.Instance.SetEquipAttributeUI();
    }
}
