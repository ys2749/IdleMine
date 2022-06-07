
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Battle : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI enemyNameText;
    [SerializeField] public TextMeshProUGUI playerHPText;
    [SerializeField] private TextMeshProUGUI playerAttackText;
    [SerializeField] private TextMeshProUGUI playerDefenseText;
    [SerializeField] public TextMeshProUGUI enemyHPText;
    [SerializeField] private TextMeshProUGUI enemyAttackText;
    [SerializeField] private TextMeshProUGUI enemyDefenseText;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject startButton;
    //[SerializeField] private Text 
    public BattleProperty prop;

    public void SetBattleUI(){
        
        roundText.text = "round: "+prop.round.ToString();
        enemyHPText.text = "HP: "+prop.enemy.HP.ToString();
        playerHPText.text = "HP: "+prop.player.HP.ToString();
        enemyAttackText.text = "A: "+prop.enemy.Attack.ToString();
        enemyDefenseText.text = "D: "+prop.enemy.Defense.ToString();
        playerAttackText.text = "A: "+prop.player.Attack.ToString();
        playerDefenseText.text = "D: "+prop.player.Defense.ToString();
        enemyNameText.text = prop.enemy.Name;
    }
    public void SetButtonUI(){
        if(prop.isPaused == 1){
           startButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "开始";
           roundText.gameObject.SetActive(false);
        }
        else{
            startButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "暂停";
            roundText.gameObject.SetActive(true);
        }
        // startButton.SetActive(false);
        // roundText.gameObject.SetActive(true);
        SetBattleUI();
    }

    public void OnClickStart(){
        if(prop.isPaused == 0){
            prop.isPaused = 1;
        }else{
            prop.isPaused = 0;
        }
        SetButtonUI();
        BattleManager.Instance.UpdateBattleStatus();

    }
    public void onClickClose(){
        BattleManager.Instance.DestroyBattle(prop.ID);
        Destroy(this.gameObject);
    }
}
