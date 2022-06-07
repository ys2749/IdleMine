using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using LitJson;
using UnityEngine.Networking;

public class ResoureceManager : Singleton<ResoureceManager>
{
    [SerializeField] private Resource basicResourcePrefab;

    public Transform rect;
    public ResourceStorage resourceStorage;
    [SerializeField] public List<Resource> resourceList;
    public List<ResourceProperty> basicResourceList;
    public List<ResourceDefaultProperty> defaultResList;
    public int WorkingPointsLimit = 5;
    public int WorkingPointsLeft = 5;

    // Start is called before the first frame update
    void initialStorage(){
        resourceStorage = new ResourceStorage();
    }

    void Start()
    {
        StartCoroutine(LoadTheGame());

        StartCoroutine(Tick());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Tick(){
        while(true){
            yield return new WaitForSeconds(1);
            //解决煤炭导致木头为负的问题
            //当木头会<0,强制调整煤炭工作量 = 0
            if(resourceList[0].prop.outputPS + resourceList[0].prop.storage < 0){
                resourceList[2].MinusAmountToZero();
            }


            for(int i=0;i<resourceList.Count;i++){
                if(resourceList[i].prop.unlocked == 1){
                    if(resourceList[i].prop.storage < resourceList[i].totalStorageLimit){
                        resourceList[i].prop.storage += System.Math.Min(resourceList[i].prop.outputPS,(resourceList[i].totalStorageLimit-resourceList[i].prop.storage));
                    }                 

                }
            }
        }
    }
    public void AddResource(ResourceProperty prop){
        Resource newRes = Instantiate(basicResourcePrefab,rect,false) as Resource;
        newRes.prop = prop;
        newRes.SetResoureceUI();

        resourceList.Add(newRes);      
    }
    private void AddNewResource(string resname){
        //List<string> resnameList = new List<string>(defaultResList.Select(t=>t.name).ToList());
        int resIndex = defaultResList.Select(t=>t.name).ToList().IndexOf(resname);
        if(resIndex != -1){
            ResourceProperty newprop = new ResourceProperty();
            newprop.name = resname;
            newprop.unlocked = 1;
            newprop.workingAmount = 0;
            newprop.level = 0;
            newprop.storageLimt = defaultResList[resIndex].storageLimt;
            newprop.baseIncome = defaultResList[resIndex].baseIncome;
            newprop.tool = defaultResList[resIndex].tool;
            AddResource(newprop);
            
        }
    }
    public void UpdateResourceCost(){
        if(resourceList.Count>=3){
            resourceList[0].prop.costPS = 2 * resourceList[2].prop.workingAmount;
        }
        for (int i = 0; i < resourceList.Count; i++)
        {
            resourceList[i].UpdateOutputPS();
        }
    }
    public void deductResource(List<double> cost){
        //消耗资源
        for (int i = 0; i < cost.Count; i++)
        {
            if(cost[i]>0){
                resourceList[i].deductStorage(cost[i]);
            }
            
        }
    }
    public void UpdateResourceStorageLimit(){
        for (int i = 0; i < resourceList.Count; i++)
        {
            resourceList[i].UpdateTotalStorageLimit();
        }
    }
    void SaveTheGame(){
        string filepathsave = Path.Combine(Application.persistentDataPath,"resourceStorage.json");
        if(!File.Exists(filepathsave)){
            FileStream fs = File.Create(filepathsave);
            fs.Close();
        }
        StreamWriter streamWriter = new StreamWriter(filepathsave);
        List<ResourceProperty> basicResourceListToSave = new List<ResourceProperty>(resourceList.Select(t => t.prop).ToList());
        streamWriter.Write(JsonMapper.ToJson(basicResourceListToSave));
        streamWriter.Close();
    }
    void AutoSave(){
        SaveTheGame();
        Invoke("AutoSave",1f);
    }
    IEnumerator LoadTheGame(){
        string filepath = Path.Combine(Application.streamingAssetsPath,"defaultres.json");
        UnityWebRequest request = UnityWebRequest.Get(filepath);
        yield return request.SendWebRequest();
        defaultResList = JsonMapper.ToObject<List<ResourceDefaultProperty>>(request.downloadHandler.text);


        string filepathsave = Path.Combine(Application.persistentDataPath,"resourceStorage.json");
        if(File.Exists(filepathsave)){
            StreamReader streamReader = new StreamReader(filepathsave);
            string jsonStr = streamReader.ReadToEnd();
            basicResourceList = JsonMapper.ToObject<List<ResourceProperty>>(jsonStr);
            streamReader.Close();
            //initialButtonStatus();
        }else{
            initialBasic();
        }
        for(int i=0;i<basicResourceList.Count;i++){
            AddResource(basicResourceList[i]);
        }
        AutoSave();
        GameManager.Instance.ResInitialized = true;
    }
    // IEnumerator LoadDefaultResProp(){
    //     string filepath = Path.Combine(Application.streamingAssetsPath,"defaultres.json");
    //     UnityWebRequest request = UnityWebRequest.Get(filepath);
    //     yield return request.SendWebRequest();
    //     defaultResList = JsonMapper.ToObject<List<ResourceDefaultProperty>>(request.downloadHandler.text);
    // }
    void initialBasic(){
        //Debug.Log(defaultResList.Count);
        ResourceProperty newprop = new ResourceProperty();
        newprop.unlocked = 1;
        newprop.workingAmount = 0;
        newprop.level = 0;
        newprop.name = defaultResList[0].name;      
        newprop.baseIncome= defaultResList[0].baseIncome;
        newprop.storageLimt = defaultResList[0].storageLimt;
        newprop.tool = defaultResList[0].tool;
        basicResourceList.Add(newprop);

    }

    private void OnEnable() {
        TechManager.OnNewresUnlockedRequest += AddNewResource;
    }
    private void OnDisable() {
        TechManager.OnNewresUnlockedRequest -= AddNewResource;
    }


}

