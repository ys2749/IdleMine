using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Resource : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI resourceNameText;
    [SerializeField] private TextMeshProUGUI resourceAmountText;
    [SerializeField] private TextMeshProUGUI resourceBaseText;
    [SerializeField] private TextMeshProUGUI resourceTechLvlText;
    [SerializeField] private GameObject addButton;
    [SerializeField] private GameObject minusButton;
    [SerializeField] private Transform iconLocation;
    public ResourceProperty prop;
    public double totalStorageLimit;
    private void Start() {
        UpdateTotalStorageLimit();
    }


    public void SetResoureceUI(){
        resourceNameText.text = prop.name;
        resourceAmountText.text = "工作点数 :"+prop.workingAmount.ToString();
        resourceBaseText.text = "基础产量 : "+ (prop.baseIncome*(prop.level+1)).ToString();
        resourceTechLvlText.text = "科技等级 : "+ prop.tool[prop.level];
        
    }
    public double ExtraStorageByTech(){
        if(TechManager.Instance.TechLvlStorage == 1){
            return 400;
        }else{
            return 0;
        }
    }
    public void UpdateTotalStorageLimit(){
        totalStorageLimit = prop.storageLimt + ExtraStorageByTech();
    }
    public void OnClickAddAmount(){
        //ResoureceManager.Instance.AddWoodAmount();
        //resourceAmountText.text = "Amount : "+ ResoureceManager.Instance._WoodAmount.ToString();
        if(GameManager.Instance.Spirit.available>0){
            prop.workingAmount++;
            GameManager.Instance.UpdateSpirit();
        }
        // if(prop.workingAmount<5){
        //     prop.workingAmount++;
        // }

        //更新关联资源的costps,如烧煤消耗木头
        ResoureceManager.Instance.UpdateResourceCost();
        //ResoureceManager.Instance.basicResourceList[0].workingAmount++;
        //resourceAmountText.text = "Amount : "+ prop.workingAmount.ToString();
        UpdateOutputPS();

    }
    public void OnClickMinusAmount(){
        if(prop.workingAmount>0){
            prop.workingAmount--;
            GameManager.Instance.UpdateSpirit();
        }
        ResoureceManager.Instance.UpdateResourceCost();
        UpdateOutputPS();

    }
    //一次性将工作量减到0
    public void MinusAmountToZero(){
        prop.workingAmount = 0;
        GameManager.Instance.UpdateSpirit();
        ResoureceManager.Instance.UpdateResourceCost();
        UpdateOutputPS();
    }
    public void updateIncomePS(){
        prop.incomePS = CalculateIncome();
        
    }
    public double CalculateIncome(){
        return prop.workingAmount*prop.baseIncome*(prop.level+1);    
    }

    public void UpdateOutputPS(){
        updateIncomePS();
        prop.outputPS = prop.incomePS - prop.costPS;
        SetResoureceUI();
    }

    public void deductStorage(double deltaStorage){
        prop.storage -= deltaStorage;
    }
    public void acquireStorage(double deltaStorage){
        prop.storage += deltaStorage;
    }
    public bool CheckAmountAvailable(double amount){
        if(prop.storage>amount){
            return true;
        }else{
            return false;
        }
    }
}
