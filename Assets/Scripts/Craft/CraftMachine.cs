using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftMachine : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI machineNameText;
    public TMP_Dropdown recipeDropDown;
    public CraftBar ProgressBar;
    public Button QuestionBtn;
    public CraftMachineProperty prop;
    
    //public int recipeIndex = 0; //记得修改为读取存档
    public CraftRecipe selectedRecipe;


    void Start()
    {
        //InitDropdown();
        machineNameText.text = prop.Name;
        SetSelectedRecipe();
        SetUI();
        recipeDropDown.onValueChanged.AddListener(ChangeDropChoice);
    }
    public void SetUI(){
        ProgressBar.setProgress((float)prop.progressSec/ (float)selectedRecipe.Time);
        ProgressBar.setCraftUI(prop.N_waiting);

    }
    public void SetSelectedRecipe(){
        //根据recipeIndex,在自身Prop中recipeName序列中获取name,再从CraftManager 的 recipeDict中查询完整recipe
        selectedRecipe = CraftManager.Instance.DefaultRecipeDict[prop.recipeNameList[prop.SelectedIndex]];
    }
    public void InitDropdown()
    {
        //清空默认节点
        recipeDropDown.options.Clear();
        recipeDropDown.captionText.text = prop.recipeNameList[prop.SelectedIndex];
        recipeDropDown.AddOptions(prop.recipeNameList);
    }
    public void UpdateDropdownOption(){
        recipeDropDown.options.Clear();
        prop.SelectedIndex = 0;
        recipeDropDown.captionText.text = prop.recipeNameList[prop.SelectedIndex];
        recipeDropDown.AddOptions(prop.recipeNameList);
    }
    private void ChangeDropChoice(int index)
    {
        prop.SelectedIndex = index;
        SetSelectedRecipe();
    }
    public void OnClickQuestionBtn(){
        CraftManager.Instance.SetRemarkPageStatus(true);
        GameManager.Instance.RemarkPageDescription =selectedRecipe.Formula;// RecipeList[recipeIndex].Formula;
        RemarkPage.Instance.setUI();
    }
    public bool CheckRecipeInput(CraftRecipe recipe){
        //如果InputItem小于1000，代表为资源，从resourcelist中检查，位置与inputItem转化为int后一致。
        //如果大于1000，代表为背包物品，从bagItemDict中检查
        for (int i = 0; i < recipe.InputItem.Count; i++)
        {
            int item_id = Convert.ToInt32(recipe.InputItem[i]);
            if(item_id<1000){
                //暂时没有检查该资源是否解锁的函数，看情况
                if(!ResoureceManager.Instance.resourceList[item_id].CheckAmountAvailable(recipe.InputAmount[i])){
                    return false;
                }
            }else{
                //在BagManager 、 BagItemDict层面各有一个CheckAmountAvailable函数，BagManager会先检查字典中是否该item
                //如没有直接返回错误，如有再调用具体的item检查amount。
                if(!BagManager.Instance.CheckAmountAvailable(recipe.InputItem[i],recipe.InputAmount[i])){
                    return false;
                }
            }
        }
        return true;
    }
    public void UseRecipeInput(CraftRecipe recipe){
        for (int i = 0; i < recipe.InputItem.Count; i++)
        {
            int item_id = Convert.ToInt32(recipe.InputItem[i]);
            if(item_id<1000){
                ResoureceManager.Instance.resourceList[item_id].deductStorage(recipe.InputAmount[i]);
            }else{
                BagManager.Instance.bagItemDict[recipe.InputItem[i]].UseItem(recipe.InputAmount[i]);
            }
        }
    }
    public void onClickInputBtn(){
        if(CheckRecipeInput(selectedRecipe)){
            UseRecipeInput(selectedRecipe);
            prop.N_waiting += 1;
            ProgressBar.setCraftUI(prop.N_waiting);
            //在UpdateCraftStatus根据情况启动coroutine
            CraftManager.Instance.UpdateCraftStatus();
            //CraftManager.Instance.SaveTheGame(); // 即时存档，以防原材料丢失
        }
    }


}
