using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using LitJson;

public class BagManager : Singleton<BagManager>
{
    [SerializeField] private BagItem bagItemPrefab;
    //[SerializeField] public List<BagItem> bagItemList;
    public Transform rect;
    //public List<DropResourceStorage> bagStorageList;
    //private List<string> itemNameList;
    //storageDict用于储存到Json,读取后生成itemDict.
    public Dictionary<string,int> bagStorageDict;
    public Dictionary<string,BagItem> bagItemDict;



    // Start is called before the first frame update
    void Start()
    {
        //LoadTheGame();

        //AutoSave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void initialStorage(){
        bagItemDict = new Dictionary<string, BagItem>();
        foreach (string key in bagStorageDict.Keys)
        {
            DropResourceStorage tempProp = new DropResourceStorage();
            tempProp.itemID = key;
            tempProp.itemName = GameManager.Instance.ItemUniqueIDs[key];
            tempProp.itemAmount = bagStorageDict[key];
            AddBagItem(tempProp);
        }
        AutoSave();
    }

    public void AddBagItem(DropResourceStorage prop){
        BagItem newItem = Instantiate(bagItemPrefab,rect,false) as BagItem;
        newItem.prop = prop;
        newItem.SetItemUI();
        bagItemDict.Add(prop.itemID,newItem);
        //bagItemList.Add(newItem);
       
    }
    private void BattleEndRequest(string itemName,int dropAmount){
        UpgradeBagInfo(itemName,dropAmount);
    }
    private void CraftEndRequest(string itemName,int craftAmount){
        UpgradeBagInfo(itemName,craftAmount);
    }
    private void UpgradeBagInfo(string itemID,int dropAmount){
        //10000以上是装备，在equipmanager中处理，不放在背包
        if(System.Convert.ToInt32(itemID)<10000){
            if(bagItemDict.Count>0){
                if(bagItemDict.ContainsKey(itemID)){
                    bagItemDict[itemID].prop.itemAmount += dropAmount;
                    bagItemDict[itemID].SetItemUI();
                }else{
                DropResourceStorage tempProp = new DropResourceStorage();
                tempProp.itemID = itemID;
                tempProp.itemName =  GameManager.Instance.ItemUniqueIDs[itemID];
                tempProp.itemAmount = dropAmount;
                AddBagItem(tempProp);
                }
            }else{
                DropResourceStorage tempProp = new DropResourceStorage();
                tempProp.itemID = itemID;
                tempProp.itemName =  GameManager.Instance.ItemUniqueIDs[itemID];
                tempProp.itemAmount = dropAmount;
                AddBagItem(tempProp);
            }
        }

    }

    public bool CheckAmountAvailable(string ItemID,int Amount){
        if(bagItemDict.ContainsKey(ItemID)){
            if(bagItemDict[ItemID].CheckAmountAvailable(Amount)){
                return true;
            }else{
                return false;
            }
        }else{
            return false;
        }
    }


    private void OnEnable() {

        BattleManager.OnBattleEndRequest += BattleEndRequest;
        CraftManager.OnCtaftEndRequest += CraftEndRequest;
    }
    private void OnDisable() {
        BattleManager.OnBattleEndRequest -= BattleEndRequest;
        CraftManager.OnCtaftEndRequest -= CraftEndRequest;
    }

    void SaveTheGame(){
        if(bagItemDict.Count>0){
            string filepathsave = Path.Combine(Application.persistentDataPath,"DropResourceStorage.json");
            StreamWriter streamWriter = new StreamWriter(filepathsave);
            //List<DropResourceStorage> bagListToSave = new List<DropResourceStorage>(bagItemList.Select(t => t.prop).ToList());
            //streamWriter.Write(JsonMapper.ToJson(bagListToSave));
            bagStorageDict.Clear();
            foreach (string key in bagItemDict.Keys)
            {
                bagStorageDict.Add(key,bagItemDict[key].prop.itemAmount);
            }
            streamWriter.Write(JsonMapper.ToJson(bagStorageDict));
            streamWriter.Close();
        }

    }
    void AutoSave(){
        SaveTheGame();
        Invoke("AutoSave",1f);
    }
    public void LoadTheGame(){
        string filepathsave = Path.Combine(Application.persistentDataPath,"DropResourceStorage.json");
        if(File.Exists(filepathsave)){
            StreamReader streamReader = new StreamReader(filepathsave);
            string jsonStr = streamReader.ReadToEnd();
            //bagStorageList = JsonMapper.ToObject<List<DropResourceStorage>>(jsonStr);
            bagStorageDict = JsonMapper.ToObject<Dictionary<string,int>>(jsonStr);
            streamReader.Close();
            initialStorage();
        }else{
            bagStorageDict = new Dictionary<string, int>();
            initialStorage();
        }
    }

}
