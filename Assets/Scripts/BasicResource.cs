using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicResource", menuName = "IdleMine/BasicResource", order = 0)]
public class BasicResource : ScriptableObject {
    public string resourceName;
    public float baseIncome;
    public Sprite resourceImage;
    public Sprite ToolImage;
    public float CalculateIncome(int amount){
        return baseIncome * amount;

    }
    
}
