using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapDropDownMenu : MonoBehaviour
{
    public TMP_Dropdown MapDDList;
    private int EnemyIndex;

    private void Start()
    {
        InitDropdown();
        //drop = this.GetComponent<Dropdown>();
        //MapDDList.onValueChanged.AddListener(Change);
    }

    private void InitDropdown()
    {
        //清空默认节点
        MapDDList.options.Clear();
        MapDDList.captionText.text = "僵尸矿洞";

        //初始化
        TMP_Dropdown.OptionData op1 = new TMP_Dropdown.OptionData();
        op1.text = "僵尸矿洞";
        MapDDList.options.Add(op1);
    }
    // private void Change(int index)
    // {
    //     EnemyIndex = index;
    //     //enemyName.text = index.ToString();
    // }
    // public void onClickAddBattle(){
    //     BattleManager.Instance.AddBattleByID(EnemyIndex);
    // }
}
