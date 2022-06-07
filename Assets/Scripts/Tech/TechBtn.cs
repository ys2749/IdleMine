using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TechBtn : Button
{
    
    public TechInfo btnInfo;
    //[SerializeField] public TextMeshProUGUI btnNameText;
    //[SerializeField] public GameObject ConfirmPage;
    public static Action<TechInfo> OnTechBtnRequest;
    public void onClicked(){
        //ConfirmPage.SetActive(true);
        TechManager.Instance.IsOpenConfirmPage(true);
        tryToUnlock();
    }
    private void tryToUnlock(){
        //广播新资源名字，在TechManager里接受并创建新资源。
        OnTechBtnRequest?.Invoke(btnInfo);
        TechManager.Instance.recordedTechInfo = btnInfo;

    }
    public void setInteractStatus(){
        this.interactable = Convert.ToBoolean(btnInfo.unlockable);
        if(btnInfo.unlocked == 1){
            this.GetComponent<Image>().color = Color.green;
        }
    }
    public void setBtnUI(){
        
        if(btnInfo.techName == "blank"){
            //Debug.Log("alpha changed");
            image = this.GetComponent<Image>();
            var tempColor = image.color;
            tempColor.a = 0f;
            image.color = tempColor;
            this.GetComponentInChildren<TextMeshProUGUI>().text =  "";
        }else{
            this.GetComponentInChildren<TextMeshProUGUI>().text =  btnInfo.techName;
        }

    }

}
