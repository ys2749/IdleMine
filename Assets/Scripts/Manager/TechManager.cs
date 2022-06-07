using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TechManager : Singleton<TechManager>
{
    public Transform rect;
    [SerializeField] public TechBtn techFab;
    [SerializeField] public GameObject ConfirmPage;
    public List<TechInfo> techInfoList;
    public List<TechBtn> buttonList;
    public TechInfo recordedTechInfo{get;set;}
    public int TechLvlStorage = 0;

    public static Action<string> OnNewresUnlockedRequest;
    public void unlockNewResource(string resname){
        //广播新资源名字，在resourcemanager里接受并创建新资源。
        OnNewresUnlockedRequest?.Invoke(resname);
    }


    private void initialTechInfo(){
        TechInfo newinfo0 = new TechInfo();
        newinfo0.ID = 0;
        newinfo0.unlockable = 1;
        newinfo0.unlocked = 0;
        newinfo0.costRes = new List<double> {10,0,0};
        newinfo0.unlockTechID = new List<int>{1,2,3};
        newinfo0.description = "Unlock Various Wood Tool. Cost 10 Wood.";
        techInfoList.Add(newinfo0);

        TechInfo newinfo1 = new TechInfo();
        newinfo1.ID = 1;
        newinfo1.unlockable = 0;
        newinfo1.unlocked = 0;
        newinfo1.costRes = new List<double> {50,0,0};
        newinfo1.unlockTechID = new List<int>{};
        newinfo1.description = "Unlock Wood Axe,wood base +1. Cost 50 Wood.";
        techInfoList.Add(newinfo1);

        TechInfo newinfo2 = new TechInfo();
        newinfo2.ID = 2;
        newinfo2.unlockable = 0;
        newinfo2.unlocked = 0;
        newinfo2.costRes = new List<double> {50,0,0};
        newinfo2.unlockTechID = new List<int>{4};
        newinfo2.description ="Unlock Wood Pickaxe, could mine Stone. Cost 50 Wood.";
        techInfoList.Add(newinfo2);

        TechInfo newinfo3 = new TechInfo();
        newinfo3.ID = 3;
        newinfo3.unlockable = 0;
        newinfo3.unlocked = 0;
        newinfo3.costRes = new List<double> {50,0,0};
        newinfo3.unlockTechID = new List<int>{};
        newinfo3.description ="Unlock Wood chest, Increase Storage Limit. Cost 50 Wood.";
        techInfoList.Add(newinfo3);

        TechInfo newinfo4 = new TechInfo();
        newinfo4.ID = 4;
        newinfo4.unlockable = 0;
        newinfo4.unlocked = 0;
        newinfo4.costRes = new List<double> {0,50,0};
        newinfo4.description = "Unlock Various Stone Tool. Cost 50 stone.";
        techInfoList.Add(newinfo4);

    }
    private void initialButtonStatus(){
        for (int i = 0; i < techInfoList.Count; i++)
        {
            addTechBtn(techInfoList[i]);
        }
        UpdateButtonStatus();
    }
    public void addTechBtn(TechInfo info){
        TechBtn newTech = Instantiate(techFab,rect,false) as TechBtn;
        newTech.btnInfo = info;
        newTech.setBtnUI();
        buttonList.Add(newTech);
    }

    private void UpdateButtonStatus(){
        //List<int> interactableList = new List<int>{};
        //检索已解锁科技，unlocked == 1,的unlockTechID信息
        //对每个可解锁科技，检查是否已经解锁
        //如尚未解锁，设置unlockable =1
        for(int i=0;i<buttonList.Count;i++){
            if(buttonList[i].btnInfo.unlocked==1){
                var unlockList = buttonList[i].btnInfo.unlockTechID;
                //Debug.Log(unlockList.Count);
                if(unlockList.Count>0){
                    for(int j=0;j<unlockList.Count;j++){
                        if(buttonList[unlockList[j]].btnInfo.unlocked==0){
                            buttonList[unlockList[j]].btnInfo.unlockable = 1;
                        }
                    }
                }
                if(buttonList[i].btnInfo.ID == 5){
                    TechLvlStorage = 1;
                    ResoureceManager.Instance.UpdateResourceStorageLimit();
                }
                if(buttonList[i].btnInfo.ID == 11){
                    //暂时写死，以后改
                    //石头剑可以解锁menu栏战斗+背包
                    MenuManager.Instance.UnLockMenu(2);
                    MenuManager.Instance.UnLockMenu(3);
                   
                    //MenuManager.Instance.UpdateUnlockStatus();

                }
                if(buttonList[i].btnInfo.ID == 16){
                     //熔炼炉可以解锁menu栏合成
                    MenuManager.Instance.UnLockMenu(4);
                    //CraftManager.Instance.UnlockMachineWithProperty("熔炼炉","铁锭");
                }
            }
        }
        for(int i=0;i<buttonList.Count;i++){
            buttonList[i].setInteractStatus();
        }

    }

    void Start()
    {
        LoadTheGame();
        //initialButtonStatus();
        
        AutoSave();
    }

    void Update()
    {
        
    }
    void AutoSave(){
        SaveTheGame();
        Invoke("AutoSave",1f);
    }
    void SaveTheGame(){
        //persistentpath只在运行时可读取
        string filepathsave = Path.Combine(Application.persistentDataPath,"PlayerTechInfo.json");
        //Debug.Log(filepathsave);
        if(!File.Exists(filepathsave)){
            FileStream fs = File.Create(filepathsave);
            fs.Close();
        }
        StreamWriter streamWriter = new StreamWriter(filepathsave);
        List<TechInfo>  techInfoListToSave= new List<TechInfo>(buttonList.Select(t => t.btnInfo).ToList());
        streamWriter.Write(JsonMapper.ToJson(techInfoListToSave));
        streamWriter.Close();
        
    }
    void LoadTheGame(){
        string filepathsave = Path.Combine(Application.persistentDataPath,"PlayerTechInfo.json");


        if(File.Exists(filepathsave)){
            StreamReader streamReader = new StreamReader(filepathsave);
            string jsonStr = streamReader.ReadToEnd();
            techInfoList = JsonMapper.ToObject<List<TechInfo>>(jsonStr);
            streamReader.Close();
            initialButtonStatus();
        }else{
            StartCoroutine(LoadInitial());
            //LoadInitial();
        }
    }
    IEnumerator LoadInitial(){
        string filepath = Path.Combine(Application.streamingAssetsPath,"DefaultTechInfo.json");
        UnityWebRequest request = UnityWebRequest.Get(filepath);
        yield return request.SendWebRequest();

        //request.SendWebRequest();
        techInfoList = JsonMapper.ToObject<List<TechInfo>>(request.downloadHandler.text);
        //Debug.Log(request.downloadHandler.text);
        initialButtonStatus();
    }
    public void IsOpenConfirmPage(Boolean isopen){
        ConfirmPage.SetActive(isopen);
    }
    // public void recordOnClickedBtnInfo(TechInfo info){
    //     recordedTechInfo = info;
    //     selectedTechButton = info.ID;
    // }
    public Boolean checkCondition(TechInfo Info){
        //检查所有cost大于零的资源储量是否满足
        List<double> cost = Info.costRes;
        for(int i=0;i<cost.Count;i++){
            //检查资源List长度，防止对比未解锁资源报错
            // i=1,means two res, resourceList.Count 应大于等于2
            if(cost[i]>0){
                if(ResoureceManager.Instance.resourceList.Count-1<i){
                    return false;
                }
                if(ResoureceManager.Instance.resourceList[i].prop.storage < cost[i]){
                    return false;
                }
            }
        }
        if(Info.costItem.Count>0){
            for (int j = 0; j < Info.costItem.Count; j++)
            {
                if(BagManager.Instance.bagItemDict.TryGetValue(Info.costItem[j],out BagItem item)){
                    if(item.prop.itemAmount < Info.costItemNumber[j]){
                        return false;
                    }
                }else{
                    return false;
                }
            }
        }
        return true;

    }
    public void UnlockTech(){
        if(checkCondition(recordedTechInfo)){
            ResoureceManager.Instance.deductResource(recordedTechInfo.costRes);
            //根据comsumable来使用item，待完成
            //BagManager.Instance.UseItem(recordedTechInfo);
            

            switch(recordedTechInfo.ID)
            {
                case 3:
                    //GameManager.Instance.MacroLevelUp("Tool",1);
                    ResoureceManager.Instance.resourceList[0].prop.level +=1;
                    ResoureceManager.Instance.resourceList[0].UpdateOutputPS();
                    break;
                case 4:
                    unlockNewResource("石头");
                    break;
                case 9:
                    ResoureceManager.Instance.resourceList[0].prop.level +=1;
                    ResoureceManager.Instance.resourceList[0].UpdateOutputPS();
                    break;
                case 10:
                    ResoureceManager.Instance.resourceList[1].prop.level +=1;
                    ResoureceManager.Instance.resourceList[1].UpdateOutputPS();
                    break;
                case 13:
                    unlockNewResource("煤炭");
                    break;
                case 15:
                    CraftManager.Instance.UnlockMachineWithProperty("锤炼台","铁板");
                    break;
                case 16:
                    CraftManager.Instance.UnlockMachineWithProperty("熔炼炉","铁锭");
                    break;
                case 17:
                    CraftManager.Instance.UnlockMachineWithProperty("锻造台","铁剑");
                    CraftManager.Instance.AddRecipeToMachine("锻造台","铁头盔");
                    CraftManager.Instance.AddRecipeToMachine("锻造台","铁胸甲");
                    CraftManager.Instance.AddRecipeToMachine("锻造台","铁护腿");
                    CraftManager.Instance.AddRecipeToMachine("锻造台","铁靴子");
                    break;                                           
                    
            }
            recordedTechInfo.unlocked = 1;
            recordedTechInfo.unlockable = 0;
            buttonList[recordedTechInfo.ID].btnInfo = recordedTechInfo;
            //buttonList[recordedTechInfo.ID].setInteractStatus();
            UpdateButtonStatus();
            IsOpenConfirmPage(false);
        }
    }
    // private void OnEnable() {
    //     TechBtn.OnTechBtnRequest = recordOnClickedBtnInfo;
    // }
    // private void OnDisable() {
    //     TechBtn.OnTechBtnRequest -= recordOnClickedBtnInfo;
    // }
}
