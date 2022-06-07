using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using LitJson;

public class CraftBar : MonoBehaviour
{
    [SerializeField] public Image BarImage;
    [SerializeField] public TextMeshProUGUI WaitingNumberText;
    //public int N_waiting;



    public void setCraftUI(int N){
        WaitingNumberText.text = "队列中："+N.ToString();
    }
    public void setProgress(float p){
        BarImage.fillAmount = p;
    }
    public void onClickInputBtn(){

    }
}
