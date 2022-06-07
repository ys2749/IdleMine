using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using LitJson;
using UnityEngine.Networking;

public class GameManager : Singleton<GameManager>
{
    public int SpiritLimit;
    public bool ResInitialized = false;
    public MacroLevel MacroLevel;
    public SpiritProperty Spirit;
    public Dictionary<string,string> ItemUniqueIDs;
    public List<CraftRecipe> CraftRecipes;
    public List<EquipModifier> EquipModifiers;
    [SerializeField] public GameObject WelcomePage;
    [SerializeField] public Button ContinueGameBtn;
    public string RemarkPageDescription;

    public void UpdateSpirit(){
        Spirit.available = Spirit.limit - ResoureceManager.Instance.resourceList.Select(t => t.prop.workingAmount).ToList().Sum() - 
        BattleManager.Instance.battleList.Count*3;
        GlobalSave();
    }
    private void Start() {
        WelcomePage.SetActive(true);
        GlobalLoad();
        StartCoroutine(LoadDefaultItemList());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }
    public void ExitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else    
            Application.Quit();
        #endif
    }

    void GlobalSave(){
        string filepathsave = Path.Combine(Application.persistentDataPath,"GlobalSave.json");
        StreamWriter streamWriter = new StreamWriter(filepathsave);
        streamWriter.Write(JsonMapper.ToJson(Spirit));
        streamWriter.Close();
    }
    void GlobalLoad(){
        string filepathsave = Path.Combine(Application.persistentDataPath,"GlobalSave.json");
        if(File.Exists(filepathsave)){
            StreamReader streamReader = new StreamReader(filepathsave);
            string jsonStr = streamReader.ReadToEnd();
            Spirit = JsonMapper.ToObject<SpiritProperty>(jsonStr);
            streamReader.Close();
        }else{
            Spirit.limit = 10;
            Spirit.available = 10;
        }
    }
    IEnumerator LoadDefaultItemList(){
        string filepath = Path.Combine(Application.streamingAssetsPath,"DefaultItemList.json");
        UnityWebRequest request = UnityWebRequest.Get(filepath);
        yield return request.SendWebRequest();
        ItemUniqueIDs = JsonMapper.ToObject<Dictionary<string,string>>(request.downloadHandler.text);

        string filepathRecipe = Path.Combine(Application.streamingAssetsPath,"DefaultRecipe.json");
        UnityWebRequest requestRecipe = UnityWebRequest.Get(filepathRecipe);
        yield return requestRecipe.SendWebRequest();
        CraftRecipes = JsonMapper.ToObject<List<CraftRecipe>>(requestRecipe.downloadHandler.text);

        string filepathEquipModifier = Path.Combine(Application.streamingAssetsPath,"DefaultEquipModifier.json");
        UnityWebRequest requestEquipModifier = UnityWebRequest.Get(filepathEquipModifier);
        yield return requestEquipModifier.SendWebRequest();
        EquipModifiers = JsonMapper.ToObject<List<EquipModifier>>(requestEquipModifier.downloadHandler.text);

        ContinueGameBtn.interactable = true;
        
    }
    public void StartGameBtn(){
        WelcomePage.SetActive(false);
        BagManager.Instance.LoadTheGame();
        CraftManager.Instance.LoadTheGame();
        EquipmentManager.Instance.LoadTheGame();
    }
}
