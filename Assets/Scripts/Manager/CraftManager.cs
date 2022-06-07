using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using LitJson;
public class CraftManager : Singleton<CraftManager>
{
    public bool isCoroutineRunning = false;
    //public CraftBar bar;
    public static Action<string,int> OnCtaftEndRequest;    
    [SerializeField] public Dictionary<string,CraftMachine> MachineDict;
    [SerializeField] private CraftMachine MachineFab;
    [SerializeField] public Transform rect;
    [SerializeField] public GameObject RemarkPage;
    public List<CraftRecipe> FurnaceRecipeList;
    public Dictionary<string,CraftRecipe> DefaultRecipeDict;
    public List<CraftMachineProperty> craftPropList;

    void Start() {
    
    }
    // public void Init(){
    //     //初始化函数在gamemanager中调用，需等待default reipce文件加载完毕
    //     InitialRecipeDict();
    //     InitialMachineDict();
    //     AutoSave();
    // }
    public void InitialRecipeDict(){
        DefaultRecipeDict = new Dictionary<string,CraftRecipe>();
        foreach (var item in GameManager.Instance.CraftRecipes)
        {
            DefaultRecipeDict.Add(item.Name,item);
        }
    }
    public void InitialMachineDict(){
        // MachineDict = new Dictionary<string,CraftMachine>();
        // CraftMachineProperty furnaceProp = new CraftMachineProperty();
        // furnaceProp.Name = "熔炼炉";
        // List<string> FurnaceRecipeNameList = new List<string>();
        // FurnaceRecipeNameList.Add("铁锭");
        // FurnaceRecipeNameList.Add("铝锭");
        // furnaceProp.recipeName = FurnaceRecipeNameList; //仅在prop中输入recipe的名字，在craftMachine内部去生成完整的recipe.
        // AddMachine(furnaceProp);  
        // CraftMachineProperty hammerProp = new CraftMachineProperty();
        // hammerProp.Name = "锤炼台";
        // List<string> HammerRecipeNameList = new List<string>();
        // HammerRecipeNameList.Add("铁板");            
        // hammerProp.recipeName = HammerRecipeNameList;
        // AddMachine(hammerProp);  
        UnlockMachineWithProperty("熔炼炉","铁锭");
        AddRecipeToMachine("熔炼炉","铝锭");
        UnlockMachineWithProperty("锤炼台","铁板");
    }
    public void UnlockMachineWithProperty(string machineName, string recipeName){
        CraftMachineProperty  newProp = new CraftMachineProperty();
        newProp.Name = machineName;
        List<string> recipeList = new List<string>();
        recipeList.Add(recipeName);
        newProp.recipeNameList = recipeList;
        AddMachine(newProp);
    }
    public void AddRecipeToMachine(string machineName, string recipeName){
        MachineDict[machineName].prop.recipeNameList.Add(recipeName);
        MachineDict[machineName].UpdateDropdownOption();
    }
    public void AddMachine(CraftMachineProperty prop){
        CraftMachine newMachine = Instantiate(MachineFab,rect,false) as CraftMachine;
        newMachine.prop = prop;
        newMachine.InitDropdown();
        newMachine.SetSelectedRecipe();
        newMachine.SetUI();
        MachineDict.Add(prop.Name,newMachine);
    }


    IEnumerator Crafting(){
        isCoroutineRunning = true;
        while(checkWaitingList()>0){
            foreach (CraftMachine machine in MachineDict.Values)
            {
                if(machine.prop.N_waiting>0){
                    machine.prop.progressSec +=1;
                    machine.SetUI();                               
                }
            }
            yield return new WaitForSeconds(0.5f);
            foreach (CraftMachine machine in MachineDict.Values)
            {
                if(machine.prop.progressSec >= machine.selectedRecipe.Time){

                    machine.prop.N_waiting -= 1;
                    machine.prop.progressSec = 0;
                    machine.SetUI();  

                    GiveCraftItem(machine.selectedRecipe.OutputItem,machine.selectedRecipe.OutputAmount);

                }
                    
            }
            yield return new WaitForSeconds(0.5f);
        }
        isCoroutineRunning = false;
         yield return new WaitForSeconds(0.5f);
    }

    public void UpdateCraftStatus(){
        if(!isCoroutineRunning & checkWaitingList()>0){
            StartCoroutine(Crafting());
        }
    }
    public int checkWaitingList(){
        int n = 0;
        foreach (CraftMachine machine in MachineDict.Values)
        {
            n += machine.prop.N_waiting;
        }
        return n;
    }

    public void GiveCraftItem(List<string> itemName,List<int> craftAmount){
        for (int i = 0; i < itemName.Count; i++)
        {
            OnCtaftEndRequest?.Invoke(itemName[i],craftAmount[i]);
        }
        
    }
    public void SetRemarkPageStatus(bool status){
        RemarkPage.SetActive(status);
    }

    void AutoSave(){
        SaveTheGame();
        Invoke("AutoSave",1f);
    }

    public void SaveTheGame(){
        if(MachineDict.Count>0){
            string filepathsave = Path.Combine(Application.persistentDataPath,"CraftInfo.json");
            StreamWriter streamWriter = new StreamWriter(filepathsave);
            List<CraftMachineProperty>  craftPropToSave = new List<CraftMachineProperty>();
            foreach (CraftMachine machine in MachineDict.Values)
            {
                craftPropToSave.Add(machine.prop);
            }
            //List<CraftMachineProperty> craftPropToSave = new List<CraftMachineProperty>(MachineDict.Values.ToList());
            streamWriter.Write(JsonMapper.ToJson(craftPropToSave));
            streamWriter.Close();
        }


    }
    public void LoadTheGame(){
        InitialRecipeDict();
        MachineDict = new Dictionary<string,CraftMachine>();    
        string filepathsave = Path.Combine(Application.persistentDataPath,"CraftInfo.json");
        if(File.Exists(filepathsave)){
            StreamReader streamReader = new StreamReader(filepathsave);
            string jsonStr = streamReader.ReadToEnd();
            craftPropList = JsonMapper.ToObject<List<CraftMachineProperty>>(jsonStr);
            streamReader.Close();
        }
        if(craftPropList.Count>0){      
            for (int i = 0; i < craftPropList.Count; i++)
            {
                AddMachine(craftPropList[i]);
            }
        }
        UpdateCraftStatus();
        AutoSave();


    }
}
