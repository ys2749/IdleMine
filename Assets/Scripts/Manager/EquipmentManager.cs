using System;
using System.Collections;
using System.Collections.Generic;
//using System.Math;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LitJson;
using System.IO;
using System.Linq;
using UnityEngine.Networking;
public class EquipmentManager : Singleton<EquipmentManager>
{
    [SerializeField] public EquipItem EquipItemPrefab;
    [SerializeField] public Transform rect;
    [SerializeField] public TextMeshProUGUI EquipItemBasicAttText;
    [SerializeField] public TextMeshProUGUI EquipSetAttText;
    [SerializeField] public Text EquipBtnText;
    public Dictionary<string,EquipModifier> EquipModifierDict;
    public List<EquipmentProperty> equipmentPropertyList;
    public List<EquipItem> equipItemList;
    public Dictionary<int,EquipItem>EquippedSlot; //记录当前有哪些位置已装备，不可再装备同类型。 不落地，动态生成。
    public int currentUniqueID;
    public string selectedItemLocalID;
    public int selectedItemOrder;

    private void OnEnable() {
        CraftManager.OnCtaftEndRequest += CraftEndRequest;
    }
    private void OnDisable() {
        CraftManager.OnCtaftEndRequest -= CraftEndRequest;
    }
    private void CraftEndRequest(string itemID,int craftAmount){
        if(Convert.ToInt32(itemID)>10000){
            UpdateEquipItemList(itemID,craftAmount);
        }
    }
    private void UpdateEquipItemList(string itemID,int Amount){
        EquipmentProperty newProp = new EquipmentProperty();
        newProp.ID = itemID;
        newProp.localID = currentUniqueID.ToString();
        currentUniqueID +=1;
        SaveLocalID();
        newProp.name =  GameManager.Instance.ItemUniqueIDs[itemID];
        newProp.localName = newProp.name; //UI默认显示Localname,但可以还原为Name
        newProp.type = TypeItbyID(newProp.ID);
        newProp.material = "iron"; //先写死，以后改
        newProp.equiped = 0;
        //equipmentPropertyList.Add(newProp);
        AddEquipItem(newProp);
    }
    private void SaveLocalID(){
        PlayerPrefs.SetInt("currentUniqueID",currentUniqueID);
        PlayerPrefs.Save();
    }
    public void AddEquipItem(EquipmentProperty prop){
        EquipItem newItem = Instantiate(EquipItemPrefab,rect,false) as EquipItem;
        newItem.prop = prop;
        newItem.SetUI();
        equipItemList.Add(newItem);
    }
    public int TypeItbyID(string ID){
        //0武器1头2身体3腿4靴子
        if(Convert.ToInt32(ID)<10100){
            return 0;
        }else if(Convert.ToInt32(ID)<10200){
            return 1;
        }else if(Convert.ToInt32(ID)<10300){
            return 2;
        }else if(Convert.ToInt32(ID)<10400){
            return 3;
        }else{
            return 4;
        }
    }
    public void SetEquipAttributeUI(){
        for (int i = 0; i < equipItemList.Count; i++)
        {
            if(equipItemList[i].prop.localID == selectedItemLocalID){
                selectedItemOrder = i;
                break;
            }
        }
        EquipmentProperty tempProp = equipItemList[selectedItemOrder].prop;
        EquipModifier tempMod = EquipModifierDict[equipItemList[selectedItemOrder].prop.name];
        EquipModifier tempSet2 = EquipModifierDict[tempProp.material+"2"];
        EquipModifier tempSet4 = EquipModifierDict[tempProp.material+"4"];
        EquipModifier tempSet5 = EquipModifierDict[tempProp.material+"5"];

        EquipItemBasicAttText.text = TranslateAttribute(tempMod);        
        EquipSetAttText.text = "套装属性"+"\n2件套:"+TranslateAttribute(tempSet2)+"4件套:"+ TranslateAttribute(tempSet4)+"5件套:"+TranslateAttribute(tempSet5);

        if(equipItemList[selectedItemOrder].prop.equiped == 1){
            EquipBtnText.text = "卸除";
        }else{
            EquipBtnText.text = "装备";
        }
    }
    private string TranslateAttribute(EquipModifier m){
        var outString = new System.Text.StringBuilder();
        if(m.HPModifier>0){
            outString.AppendLine("血量+"+m.HPModifier.ToString());
        }
        if(m.AttackModifier>0){
            outString.AppendLine("攻击+"+m.AttackModifier.ToString());
        }        
        if(m.DefenseModifier>0){
            outString.AppendLine("防御+"+m.DefenseModifier.ToString());
        }
        if(m.AgilityModifier>0){
            outString.AppendLine("敏捷+"+m.AgilityModifier.ToString());
        }
        if(m.HPModifierPct>0){
            outString.AppendLine("基础血量+"+m.HPModifierPct.ToString());
        }        
        if(m.AttackModifierPct>0){
            outString.AppendLine("基础攻击+"+m.AttackModifierPct.ToString());
        }           
        if(m.DefenseModifierPct>0){
            outString.AppendLine("基础防御+"+m.DefenseModifierPct.ToString()+"%");
        }
        return outString.ToString();

    }

    // public void UpdateEquippedSlot(){

    // }
    public void OnClickEquipBtn(){
        EquipItem tempItem = equipItemList[selectedItemOrder];
        if(tempItem.prop.equiped == 0){
            if(EquippedSlot.ContainsKey(tempItem.prop.type)){
                EquippedSlot[tempItem.prop.type].prop.equiped = 0;
                EquippedSlot[tempItem.prop.type].SetUI();
                EquippedSlot.Remove(tempItem.prop.type);
            }
            tempItem.prop.equiped = 1;
            tempItem.SetUI();
            EquippedSlot.Add(tempItem.prop.type,tempItem);      
        }else{
            tempItem.prop.equiped = 0;
            tempItem.SetUI();
            EquippedSlot.Remove( equipItemList[selectedItemOrder].prop.type);
        }
    }
    public void OnClickSellBtn(){

    }
    public void OnClickRenameBtn(){

    }
    void AutoSave(){
        SaveTheGame();
        Invoke("AutoSave",1f);
    }
    void SaveTheGame(){
        if(equipItemList.Count>0){
            string filepathsave = Path.Combine(Application.persistentDataPath,"EquipInfo.json");
            StreamWriter streamWriter = new StreamWriter(filepathsave);
            List<EquipmentProperty> equipPropToSave = new List<EquipmentProperty>(equipItemList.Select(t => t.prop).ToList());
            streamWriter.Write(JsonMapper.ToJson(equipPropToSave));
            streamWriter.Close();
        }
    }
    public void LoadTheGame(){
        //该数据有上限，需定期重置。
        currentUniqueID = PlayerPrefs.GetInt("currentUniqueID",0);

        equipItemList = new List<EquipItem>();
        //将Gamemanager中的EquiModifier默认信息从List转化为Dictionary
        EquipModifierDict = new Dictionary<string, EquipModifier>();
        EquippedSlot = new Dictionary<int, EquipItem>();

        foreach (EquipModifier item in GameManager.Instance.EquipModifiers)
        {
            EquipModifierDict.Add(item.Name,item);
        }

        string filepathsave = Path.Combine(Application.persistentDataPath,"EquipInfo.json");
        if(File.Exists(filepathsave)){
            StreamReader streamReader = new StreamReader(filepathsave);
            string jsonStr = streamReader.ReadToEnd();
            equipmentPropertyList = JsonMapper.ToObject<List<EquipmentProperty>>(jsonStr);
            streamReader.Close();
        }
        if(equipmentPropertyList.Count>0){      
            for (int i = 0; i < equipmentPropertyList.Count; i++)
            {
                AddEquipItem(equipmentPropertyList[i]);
            }
        }
        foreach (var item in equipItemList)
        {
            if(item.prop.equiped == 1){
                EquippedSlot.Add(item.prop.type,item);
            }
        }
        AutoSave();


    }
}
