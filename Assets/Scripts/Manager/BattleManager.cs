using System;
using System.Collections;
using System.Collections.Generic;
//using System.Math;
using UnityEngine;
using TMPro;
using LitJson;
using System.IO;
using System.Linq;
using UnityEngine.Networking;


public class BattleManager : Singleton<BattleManager>
{
    public static Action<string,int> OnBattleEndRequest;
    [SerializeField] private Battle battleFab;
    public Transform rect;
    [SerializeField] public List<Battle> battleList;
    public List<BattleProperty> battlePropList;
    public List<EnemyProperty> EnemyPropList;
    public PlayerProperty PlayerProp;
    public Boolean isCoroutineRunning = false;
    public List<int> pausedList;
    //public List<Coroutine> routineList;

    public void initPlayerProp(){
        PlayerProp = new PlayerProperty();
        PlayerProp.HP = 30;
        PlayerProp.Attack  = 10;
        PlayerProp.Defense = 5;
    }

    // public void initBattleProp(){
    //     BattleProperty newProp = new BattleProperty();
    //     newProp.ID = 0;
    //     newProp.isPaused = 1;
    //     newProp.round= 0;
    //     newProp.player = JsonMapper.ToObject<PlayerProperty>(JsonMapper.ToJson(PlayerProp));
    //     newProp.enemy = JsonMapper.ToObject<EnemyProperty>(JsonMapper.ToJson(EnemyPropList[0]));
    //     battlePropList.Add(newProp);

       
    //     newProp = JsonMapper.ToObject<BattleProperty>(JsonMapper.ToJson(newProp));
    //     newProp.ID = 1;
    //     newProp.enemy = JsonMapper.ToObject<EnemyProperty>(JsonMapper.ToJson(EnemyPropList[1]));
    //     battlePropList.Add(newProp);

    //     newProp = JsonMapper.ToObject<BattleProperty>(JsonMapper.ToJson(newProp));
    //     newProp.ID = 2;
    //     newProp.enemy = JsonMapper.ToObject<EnemyProperty>(JsonMapper.ToJson(EnemyPropList[2]));
    //     battlePropList.Add(newProp);
    // }

    // public void initBattle(){
    //     for(int i=0;i<battlePropList.Count;i++){
    //         AddBattle(battlePropList[i]);
    //     }
    // }
    
    public void AddBattle(PlayerProperty player, EnemyProperty enemy){
        BattleProperty newProp = new BattleProperty();
        newProp.ID = battleList.Count;
        newProp.isPaused = 1;
        newProp.round= 0;
        newProp.player = JsonMapper.ToObject<PlayerProperty>(JsonMapper.ToJson(player));
        newProp.enemy = JsonMapper.ToObject<EnemyProperty>(JsonMapper.ToJson(enemy));
        Battle newBattle = Instantiate(battleFab,rect,false) as Battle;
        newBattle.prop = newProp;
        newBattle.SetButtonUI();
        battleList.Add(newBattle);      
    }
    public void AddBattle(BattleProperty prop){
        Battle newBattle = Instantiate(battleFab,rect,false) as Battle;
        newBattle.prop = prop;
        newBattle.SetButtonUI();
        battleList.Add(newBattle);

    }
    public void AddBattleByID(int ID){
        AddBattle(PlayerProp,EnemyPropList[ID]);
    }
    public void DestroyBattle(int ID){
        //关闭战斗时，先停止所有进程，重新赋予ID，再开始战斗
        //可能存在bug，一个tick走到一半停止，只有一方掉血。
        StopAllCoroutines();
        isCoroutineRunning = false;
        battleList.RemoveAt(ID);
        if(battleList.Count>0){
            for (int i = 0; i < battleList.Count; i++)
            {
                battleList[i].prop.ID = i;
            }
        }
        UpdateBattleStatus();
        GameManager.Instance.UpdateSpirit();
    }

    // Start is called before the first frame update
    void Start()
    {
        //player数据后期应从PlayerManager生成，暂时硬编码
        //initPlayerProp();
        LoadTheGame();
        //initBattle();
        UpdateBattleStatus();
        AutoSave();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Tick(){
        isCoroutineRunning = true;
        while(true){
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < battleList.Count; i++)
            {
                if(battleList[i].prop.isPaused == 0){
                    if(battleList[i].prop.enemy.HP>0){
                        battleList[i].prop.round +=1;
                        battleList[i].prop.enemy.HP -= Mathf.Max(( battleList[i].prop.player.Attack -  battleList[i].prop.enemy.Defense),0);
                        battleList[i].SetBattleUI();
                    }
                }else{
                    resetBattle(i);
                }
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < battleList.Count; i++)
            {
                if(battleList[i].prop.isPaused == 0){
                    if(battleList[i].prop.player.HP>0){
                        battleList[i].prop.player.HP-= Mathf.Max((battleList[i].prop.enemy.Attack - battleList[i].prop.player.Defense),0);
                        battleList[i].SetBattleUI();
                    }
                }else{
                    resetBattle(i);
                }
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < battleList.Count; i++)
            {
                if(battleList[i].prop.isPaused == 0){
                    if(battleList[i].prop.player.HP<=0){
                        resetBattle(i);
                    }else if(battleList[i].prop.enemy.HP<=0){       
                        int tempDrop = DropAmount(battleList[i].prop.enemy.DropItemProp);
                        if(tempDrop>0){
                            GiveDropItem(battleList[i].prop.enemy.DropItemName,tempDrop);
                        }                                        
                        resetBattle(i);
                    }
                }else{
                    resetBattle(i);
                }
            }


        }

    }


    public void GiveDropItem(string itemName,int dropAmount){
        OnBattleEndRequest?.Invoke(itemName,dropAmount);
    }
    public void UpdateBattleStatus(){
        pausedList = new List<int>(battleList.Select(t => t.prop.isPaused).ToList());
        if(pausedList.Sum()< pausedList.Count & !isCoroutineRunning){
            StartCoroutine(Tick());
        }
        if(pausedList.Sum()== pausedList.Count & isCoroutineRunning){
            StopAllCoroutines();
            isCoroutineRunning = false;
            for (int i = 0; i < pausedList.Count; i++)
            {
                resetBattle(i);
            }
        }

    }
    public void resetBattle(int i){
        battleList[i].prop.round= 0;
        battleList[i].prop.player = JsonMapper.ToObject<PlayerProperty>(JsonMapper.ToJson(PlayerProp));
        //battleList[i].prop.enemy = JsonMapper.ToObject<EnemyProperty>(JsonMapper.ToJson(prop));
        battleList[i].prop.enemy = JsonMapper.ToObject<EnemyProperty>(JsonMapper.ToJson(EnemyPropList[battleList[i].prop.enemy.ID]));
        battleList[i].SetBattleUI();

    }

    public int DropAmount(double threshold){
        int n;
        float rd = UnityEngine.Random.value;//Random.value;
        if(rd<threshold){
            n=1;
        }else{
            n=0;
        }

        return n;
    }
    void AutoSave(){
        SaveTheGame();
        Invoke("AutoSave",1f);
    }

    void SaveTheGame(){
        string filepathsave = Path.Combine(Application.persistentDataPath,"battleInfo.json");
        //Debug.Log(filepathsave);
        if(!File.Exists(filepathsave)){
            FileStream fs = File.Create(filepathsave);
            fs.Close();
        }
        StreamWriter streamWriter = new StreamWriter(filepathsave);
        List<BattleProperty> battlePropToSave = new List<BattleProperty>(battleList.Select(t => t.prop).ToList());
        streamWriter.Write(JsonMapper.ToJson(battlePropToSave));
        streamWriter.Close();

    }
    void LoadTheGame(){
        StartCoroutine(LoadEnemyPropList());
        initPlayerProp();
        string filepathsave = Path.Combine(Application.persistentDataPath,"battleInfo.json");
        if(File.Exists(filepathsave)){
            StreamReader streamReader = new StreamReader(filepathsave);
            string jsonStr = streamReader.ReadToEnd();
            battlePropList = JsonMapper.ToObject<List<BattleProperty>>(jsonStr);
            streamReader.Close();
            if(battlePropList.Count>0){
                for (int i = 0; i < battlePropList.Count; i++)
                {
                    AddBattle(battlePropList[i]);
                }
            }


            //initialStorage();
        }else{
            
            //initBattleProp();
        }
    }
    IEnumerator LoadEnemyPropList(){
        string filepath = Path.Combine(Application.streamingAssetsPath,"DefaultEnemyProp.json");
        UnityWebRequest request = UnityWebRequest.Get(filepath);
        yield return request.SendWebRequest();
        EnemyPropList = JsonMapper.ToObject<List<EnemyProperty>>(request.downloadHandler.text);
    }
}
