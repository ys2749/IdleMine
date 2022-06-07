using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalResourceUI : MonoBehaviour
{
    // [SerializeField] private TextMeshProUGUI totalWood;
    // [SerializeField] private TextMeshProUGUI totalStone;
    // [SerializeField] private TextMeshProUGUI totalCoal;
    [SerializeField] private GameObject WorkingPoints;
    [SerializeField] private GameObject woodHolder;
    [SerializeField] private GameObject stoneHolder;
    [SerializeField] private GameObject coalHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.ResInitialized){
            WorkingPoints.GetComponentInChildren<TextMeshProUGUI>().text = "精神力:"+ GameManager.Instance.Spirit.available.ToString()+
            "/"+GameManager.Instance.Spirit.limit.ToString();
            woodHolder.GetComponentInChildren<TextMeshProUGUI>().text = "木头: "+ ResoureceManager.Instance.resourceList[0].prop.storage.ToString() +"/" +
            ResoureceManager.Instance.resourceList[0].totalStorageLimit+" +"+
                ResoureceManager.Instance.resourceList[0].prop.outputPS.ToString()+ "/s";
            //totalWood.text = "木头: "+ ResoureceManager.Instance.resourceList[0].prop.storage.ToString() + " +"+
        //     ResoureceManager.Instance.resourceList[0].prop.outputPS.ToString()+ "/s";
            if(ResoureceManager.Instance.resourceList.Count>1){
                stoneHolder.SetActive(true);
                stoneHolder.GetComponentInChildren<TextMeshProUGUI>().text = "石头: "+ResoureceManager.Instance.resourceList[1].prop.storage.ToString() +"/" +
            ResoureceManager.Instance.resourceList[1].totalStorageLimit+ " +" +
                    ResoureceManager.Instance.resourceList[1].prop.outputPS.ToString()+ "/s";
            }
            if(ResoureceManager.Instance.resourceList.Count>2){
                coalHolder.SetActive(true);
                coalHolder.GetComponentInChildren<TextMeshProUGUI>().text = "煤炭: "+ResoureceManager.Instance.resourceList[2].prop.storage.ToString()+"/" +
            ResoureceManager.Instance.resourceList[2].totalStorageLimit+ " +" +
                    ResoureceManager.Instance.resourceList[2].prop.outputPS.ToString() + "/s";
            }               
        }

    
    }
}
