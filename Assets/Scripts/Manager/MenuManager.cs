using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using LitJson;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] public List<GameObject> RectList;
    [SerializeField] public List<Button> BtnList;
    [SerializeField] public GameObject PlayerUpperRect;
    [SerializeField] public GameObject TotalResourceUIRect;
    private List<bool> IsOpenList; 
    public List<bool> unlockedList;

    void InitialIsOpenList(){
        IsOpenList = Enumerable.Repeat(false,RectList.Count).ToList();
    }
    void InitialUnlockedList(){
        unlockedList.Add(true);
        unlockedList.Add(true);
        unlockedList.Add(false);
        unlockedList.Add(false);
        unlockedList.Add(false);
        unlockedList.Add(true);
    }
    public void UpdateUnlockStatus(){
        BtnList[2].interactable = unlockedList[2];
        BtnList[3].interactable = unlockedList[3];
        BtnList[4].interactable = unlockedList[4];
        BtnList[5].interactable = unlockedList[5];
        SaveTheGame();
    }
    public void UnLockMenu(int i){
        unlockedList[i] = true;
        UpdateUnlockStatus();
    }
    void Start() {
        LoadTheGame();

    }
    public void CloseAllRect(int k){
        InitialIsOpenList();
        IsOpenList[k] = true;
        for (int i = 0; i < IsOpenList.Count; i++)
        {
            RectList[i].SetActive(IsOpenList[i]);
        }
        if(k==5){
            PlayerUpperRect.SetActive(true);
            TotalResourceUIRect.SetActive(false);
        }else{
            PlayerUpperRect.SetActive(false);
            TotalResourceUIRect.SetActive(true);
        }
    }

    void SaveTheGame(){
        //persistentpath只在运行时可读取
        string filepathsave = Path.Combine(Application.persistentDataPath,"TechLvlMenu.json");
        StreamWriter streamWriter = new StreamWriter(filepathsave);
        streamWriter.Write(JsonMapper.ToJson(unlockedList));
        streamWriter.Close();
        
    }
    void LoadTheGame(){
        string filepathsave = Path.Combine(Application.persistentDataPath,"TechLvlMenu.json");
        if(File.Exists(filepathsave)){
            StreamReader streamReader = new StreamReader(filepathsave);
            string jsonStr = streamReader.ReadToEnd();
            unlockedList = JsonMapper.ToObject<List<bool>>(jsonStr);
            streamReader.Close();
            //initialButtonStatus();
        }else{
            InitialUnlockedList();
        }
        for (int i = 0; i < BtnList.Count; i++)
        {
            int i2 = i;
            BtnList[i2].onClick.AddListener(delegate{CloseAllRect(i2);});

        }
        UpdateUnlockStatus();
    }

}
