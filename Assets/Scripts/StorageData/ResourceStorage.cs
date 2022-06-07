using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceStorage{

    // public float wood{get;set;}
    // public float stone{get;set;}
    // public float coal{get;set;}
    public float wood{get;set;}
    public float stone{get;set;}
    public float coal{get;set;}
}
[System.Serializable]
public class ResourceProperty{
    public int unlocked{get;set;}
    public string name{get;set;}
    public double storage{get;set;}
    public double storageLimt{get;set;}
    public int workingAmount{get;set;}
    public double baseIncome{get;set;}
    public double incomePS{get;set;}
    public double costPS{get;set;}
    public double outputPS{get;set;}
    public List<string> tool{get;set;}
    public int level{get;set;}
}
[System.Serializable]
public class ResourceDefaultProperty{
    public string name{get;set;}
    public double storageLimt{get;set;}
    public double baseIncome{get;set;}
    public List<string> tool{get;set;}
}
[System.Serializable]
public class TechInfo{
    public int ID{get;set;}
    public int unlockable{get;set;}
    public int unlocked{get;set;}
    public string techName{get;set;}
    public string description{get;set;}
    public List<double> costRes{get;set;}
    public List<string> costItem{get;set;}
    public List<int> costItemNumber{get;set;}
    public List<bool> ItemCosumeable{get;set;}
    public List<int> unlockTechID{get;set;}
}
[System.Serializable]
public class BattleProperty{
    public int ID{get;set;}
    public int isPaused{get;set;}
    public int round{get;set;}
    public PlayerProperty player = new PlayerProperty();
    public EnemyProperty enemy = new EnemyProperty();
}
[System.Serializable]
public class PlayerProperty{
    public float HP{get;set;}
    public float Attack{get;set;}
    public float Defense{get;set;}
}
[System.Serializable]
public class EnemyProperty{
    public float HP{get;set;}
    public float Attack{get;set;}
    public float Defense{get;set;}
    public int ID{get;set;}
    public string Name{get;set;}
    public string DropItemName{get;set;}
    public double DropItemProp{get;set;}
}
[System.Serializable]
public class DropResourceStorage{
    public string itemID{get;set;}
    public string itemName{get;set;}
    public int itemAmount{get;set;}
}
[System.Serializable]
public class SpiritProperty{
    public int limit{get;set;}
    public int available{get;set;}
    
}
[System.Serializable]
public class MacroLevel{
    public int Menu{get;set;}
    public int Tool{get;set;}
    public int Storage{get;set;}
    public int Spirit{get;set;}


}
[System.Serializable]
public class ItemUniqueID{
    public string ID;
    public string name;
}
[System.Serializable]
public class EquipmentProperty{
    public string ID;
    public string name;
    public string localID; //自动生成的unique ID 
    public string localName; //接受自定义输入
    public int equiped;  //是否已装备
    public int type;   //0武器1头2身体3腿4靴子
    public string material;
    public string enchantType;
    public int enchantLevel;

}
[System.Serializable]
public class EquipModifier{
    public string Name;
    public float HPModifier;
    public float AttackModifier;
    public float DefenseModifier;
    public float AgilityModifier;
    public float HPModifierPct;
    public float AttackModifierPct;
    public float DefenseModifierPct;
    public float AgilityModifierPct;
    public float FireResist;
    public float PierceResist;
    public float PoisonResist;
    public float EnderResist;
    public float FireResistPct;
    public float PierceResistPct;
    public float PoisonResistPct;
    public float EnderResistPct;
}
[System.Serializable]
public class CraftRecipe{
    public string Name;
    public string Formula;
    public List<string> InputItem;
    public List<int> InputAmount;
    public List<string> OutputItem;
    public List<int> OutputAmount;
    public int Time;
}
[System.Serializable]
public class CraftMachineProperty{
    public string Name;
    public List<string> recipeNameList;
    public int SelectedIndex;
    public int progressSec;
    public int N_waiting;
}