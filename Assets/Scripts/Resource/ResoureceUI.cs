using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResoureceUI : MonoBehaviour
{
    // [SerializeField] public TextMeshProUGUI resoureceName;
    // [SerializeField] private TextMeshProUGUI resoureceAmount;
    // [SerializeField] private TextMeshProUGUI resoureceBase;
    // [SerializeField] private GameObject addButton;
    // [SerializeField] private GameObject minusButton;

    private Resource _resource;
    //private int initialBase = 1;
    void Awake() {
        _resource = GetComponent<Resource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void OnClickAddAmount(){
    //     ResoureceManager.Instance.AddWoodAmount();
    //     resoureceAmount.text = "Amount : "+ ResoureceManager.Instance._WoodAmount.ToString();

    // }
    // public void OnClickMinusAmount(){

    // }
    // public void AddResource(){
    //     ResoureceManager.Instance.AddResource();
    // }
    // public void AddResourceStone(){
    //     if(ResoureceManager.Instance.wood>=100){
    //         ResoureceManager.Instance.wood -= 100;
    //         ResoureceManager.Instance.AddResource("stone");
    //     }

    // }
    //public void SetResoureceUI(int ID){
        //resoureceName.text = "wood";
        //resoureceBase.text = initialBase.ToString();

    //}
}
