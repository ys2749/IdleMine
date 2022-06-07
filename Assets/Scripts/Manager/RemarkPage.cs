using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RemarkPage : Singleton<RemarkPage>
{   
    [SerializeField] private GameObject OKButton;
    [SerializeField] public TextMeshProUGUI descriptionText;
    // Start is called before the first frame update

    public void onclickOKButton(){
        this.gameObject.SetActive(false);
    }

    public void setUI(){
        descriptionText.text = GameManager.Instance.RemarkPageDescription;
    }

}
