using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveLoad
{
    public static void Save(float wood,float stone,int woodAmount,int stoneAmount){
        //PlayerPrefs.SetString("IdleSave",d1+"|"+d2);
        PlayerPrefs.SetFloat("wood",wood);
        PlayerPrefs.SetFloat("stone",stone);
        PlayerPrefs.SetInt("woodAmount",woodAmount);
        PlayerPrefs.SetInt("stoneAmount",woodAmount);
        Debug.Log("Saved");
    }
    // public static string Load(){
    //     string data = PlayerPrefs.GetString("IdleSave");
    //     Debug.Log("Loaded");
    //     return data;
    // }

}
