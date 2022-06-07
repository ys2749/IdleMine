using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropDownMenu : MonoBehaviour
{
    public TMP_Dropdown Drd_IPList;
    private int EnemyIndex;

    private void Start()
    {
        InitDropdown();
        //drop = this.GetComponent<Dropdown>();
        Drd_IPList.onValueChanged.AddListener(Change);
    }

    private void InitDropdown()
    {
        //清空默认节点
        Drd_IPList.options.Clear();
        Drd_IPList.captionText.text = "僵尸矿工";

        //初始化
        TMP_Dropdown.OptionData op1 = new TMP_Dropdown.OptionData();
        op1.text = "僵尸矿工";
        Drd_IPList.options.Add(op1);

        TMP_Dropdown.OptionData op2 = new TMP_Dropdown.OptionData();
        op2.text = "僵尸守卫";
        Drd_IPList.options.Add(op2);

        TMP_Dropdown.OptionData op3 = new TMP_Dropdown.OptionData();
        op3.text = "僵尸首领";
        Drd_IPList.options.Add(op3);
    }
    private void Change(int index)
    {
        EnemyIndex = index;
        //enemyName.text = index.ToString();
    }
    public void onClickAddBattle(){
        if(GameManager.Instance.Spirit.available>=3){
            BattleManager.Instance.AddBattleByID(EnemyIndex);
            GameManager.Instance.UpdateSpirit();
        }

    }
}
