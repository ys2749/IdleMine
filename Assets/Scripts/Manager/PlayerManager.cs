using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : Singleton<PlayerManager>
{
    public Dictionary<int,EquipItem>EquippedSlot; //记录当前有哪些位置已装备，不可再装备同类型。 不落地，动态生成。
    public PlayerProperty playerBasicProp;
    public PlayerProperty playerProp;
    [SerializeField] public Text PlayerHPText;
    [SerializeField] public Text PlayerAttackText;
    [SerializeField] public Text PlayerDefenseText;
    [SerializeField] public Text PlayerAgilityText;
    [SerializeField] public Text PlayerSetText;
    
    public void LoadTheGame(){
        //EquippedSlot = EquipmentManager.Instance.EquippedSlot;
        initPlayerProp();
        playerProp = new PlayerProperty();
        UpdatePlayerProp();

    }
    public void UpdatePlayerProp(){
        EquipModifier tempSlotMod =  EquipmentManager.Instance.EquippedSlotTotalModifier;
        EquipModifier tempSetMod = EquipmentManager.Instance.EquippedSetTotalModifier;
        playerProp.HP = playerBasicProp.HP*(1+tempSlotMod.HPModifierPct+tempSetMod.HPModifierPct/100)  +  tempSlotMod.HPModifier+tempSetMod.HPModifier;
        playerProp.Attack = playerBasicProp.Attack*(1+tempSlotMod.AttackModifierPct+tempSetMod.AttackModifierPct/100)  +  tempSlotMod.AttackModifier+tempSetMod.AttackModifier;
        playerProp.Defense = playerBasicProp.Defense*(1+tempSlotMod.DefenseModifierPct+tempSetMod.DefenseModifierPct/100)  +  tempSlotMod.DefenseModifier+tempSetMod.DefenseModifier;
        playerProp.Agility = playerBasicProp.Agility*(1+tempSlotMod.AgilityModifierPct+tempSetMod.AgilityModifierPct/100)  +  tempSlotMod.AgilityModifier+tempSetMod.AgilityModifier;
        setUI();        
        BattleManager.Instance.UpdatePlayerProp();
    }
    public void setUI(){
        PlayerHPText.text = "生命\n"+playerProp.HP.ToString();
        PlayerAttackText.text = "攻击\n"+playerProp.Attack.ToString();
        PlayerDefenseText.text = "防御\n"+playerProp.Defense.ToString();
        PlayerAgilityText.text = "敏捷\n"+playerProp.Agility.ToString();
        PlayerSetText.text =  EquipmentManager.Instance.EquippedSetTotalModifier.description;

    }
    public void initPlayerProp(){
        playerBasicProp = new PlayerProperty();
        playerBasicProp.HP = 30;
        playerBasicProp.Attack  = 10;
        playerBasicProp.Defense = 5;
        playerBasicProp.Agility = 50;
    }

}
