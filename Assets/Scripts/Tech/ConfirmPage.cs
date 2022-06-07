using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ConfirmPage : MonoBehaviour
{   
    [SerializeField] public GameObject ConfirmPageRect;
    //[SerializeField] private GameObject yesButton;
    [SerializeField] private GameObject noButton;
    [SerializeField] public TextMeshProUGUI descriptionText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // public void onclickYesButton(){
    //     ConfirmPageRect.gameObject.SetActive(false);
    // }
    public void onclickNoButton(){
        ConfirmPageRect.gameObject.SetActive(false);
    }
    public void onclickYesButton(){
        
    }
    public void setUI(TechInfo info){
        descriptionText.text = info.description;
    }
    private void OnEnable() {
        TechBtn.OnTechBtnRequest = setUI;
    }
    private void OnDisable() {
        TechBtn.OnTechBtnRequest = null;
    }
}
