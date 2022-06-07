using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TechButton : MonoBehaviour
{
    [SerializeField] public Button WoodToolButton;
    [SerializeField] public Button WoodAxeButton;
    [SerializeField] public Button WoodPicAxeButton;
    [SerializeField] public Button WoodChestButton;
    [SerializeField] public Button StoneToolButton; //drag-n-drop the button in the CustomButton field
    [SerializeField] public Button StoneAxeButton;
    [SerializeField] public Button StonePicaxeButton;
    [SerializeField] public Button StoneFurnaceButton;
    [SerializeField] public GameObject ConfirmPage;
    [SerializeField] private GameObject yesButton;
    [SerializeField] public TextMeshProUGUI descriptionText;


    // public static Action<string> OnNewresUnlockedRequest;
    // public void unlockNewResource(string resname){
    //     //广播新资源名字，在resourcemanager里接受并创建新资源。
    //     OnNewresUnlockedRequest?.Invoke(resname);
    // }
    public string selectedTechButton{get;set;}
    void Start() {
        
       //WoodToolButton.interactable=true;
        // WoodToolButton.onClick.AddListener(UnlockWoodTool);
        // WoodPicAxeButton.onClick.AddListener(UnlockWoodPicAxe);
        // StoneToolButton.onClick.AddListener(UnlockStoneTool);
        // StoneFurnaceButton.onClick.AddListener(UnlockStoneFurnace);

    }
    // public void test1(){
    //     ConfirmPage.SetActive(true);
    //     yesButton.onClick.AddListener(AddResourceStone);
    // }
    // public void UnlockTech(){
    //     switch(selectedTechButton){
    //         case "UnlockWoodTool":
    //             if(ResoureceManager.Instance.resourceList[0].prop.storage>10){
    //                 ResoureceManager.Instance.resourceList[0].prop.storage -= 10;
    //                 WoodToolButton.GetComponent<Image>().color = Color.green;
    //                 WoodToolButton.enabled = false;
    //                 WoodAxeButton.interactable=true;
    //                 WoodPicAxeButton.interactable=true;
    //                 WoodChestButton.interactable=true;
    //                 ConfirmPage.SetActive(false);
    //             }
    //             break;    
    //         case "UnlockWoodPicAxe":
    //             if(ResoureceManager.Instance.resourceList[0].prop.storage>50){
    //                ResoureceManager.Instance.resourceList[0].prop.storage -= 50;
    //                 //ResoureceManager.Instance.AddResource("stone");
    //                 //unlockNewResource("stone");
    //                 WoodPicAxeButton.GetComponent<Image>().color = Color.green;
    //                 WoodPicAxeButton.enabled = false;
    //                 StoneToolButton.interactable=true;
    //                 ConfirmPage.SetActive(false);
    //             }
    //             break;   
    //         case "UnlockStoneTool":
    //             if(ResoureceManager.Instance.resourceStorage.stone>=10){
    //                 ResoureceManager.Instance.resourceStorage.stone -= 10;
    //                 StoneToolButton.GetComponent<Image>().color = Color.green;
    //                 StoneToolButton.enabled = false;
    //                 StoneAxeButton.interactable=true;
    //                 StonePicaxeButton.interactable=true;
    //                 StoneFurnaceButton.interactable=true;
    //                 ConfirmPage.SetActive(false);
    //             }
    //             break;  
    //         case "UnlockStoneFurnace":
    //             if(ResoureceManager.Instance.resourceStorage.stone>=50){
    //                 ResoureceManager.Instance.resourceStorage.stone -= 50;
    //                 //ResoureceManager.Instance.AddResource("coal");
    //                 StoneFurnaceButton.GetComponent<Image>().color = Color.green;
    //                 StoneFurnaceButton.enabled = false;
    //                 //StoneToolButton.interactable=true;
    //                 ConfirmPage.SetActive(false);
    //             }
    //             break;   
    //     }

    // }
    public void UnlockWoodTool(){
        selectedTechButton = "UnlockWoodTool";
        ConfirmPage.SetActive(true);
        descriptionText.text = "Unlock Various Wood Tool. Cost 10 Wood.";
    }
    public void UnlockWoodPicAxe(){
        //add resource stone
        selectedTechButton = "UnlockWoodPicAxe";
        ConfirmPage.SetActive(true);
        descriptionText.text = "Unlock Wood Pickaxe, could mine Stone. Cost 50 Wood.";
    }
    public void UnlockStoneTool(){
        selectedTechButton = "UnlockStoneTool";
        ConfirmPage.SetActive(true);
        descriptionText.text = "Unlock Various Stone Tool, could 10 stone.";
    }
    public void UnlockStonePicAxe(){
        selectedTechButton = "UnlockStoneTool";
        ConfirmPage.SetActive(true);
        descriptionText.text = "Unlock Stone Pickaxe, could mine Iron. Cost 50 Stone.";
    }
    public void UnlockStoneFurnace(){
        //石头熔炉
        //add resource Coal
        selectedTechButton = "UnlockStoneFurnace";
        ConfirmPage.SetActive(true);
        descriptionText.text = "Unlock Stone Furnace, could make Coal from Wood. Cost 50 Stone.";
    }
    void Awake(){
        //StoneToolButton.onClick.AddListener(AddResourceStone); //subscribe to the onClick event
    }


}
