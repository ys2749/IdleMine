// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class CraftDropDown : MonoBehaviour
// {
//     public TMP_Dropdown RecipeList;
//     public int recipeIndex;

//     // Start is called before the first frame update
//     void Start()
//     {
//         InitDropdown();
//         RecipeList.onValueChanged.AddListener(Change);
        
//     }

//     private void InitDropdown()
//     {
//         //清空默认节点
//         RecipeList.options.Clear();
//         RecipeList.captionText.text = "铁锭";

//         //初始化
//         TMP_Dropdown.OptionData op1 = new TMP_Dropdown.OptionData();
//         op1.text = "铁锭";
//         RecipeList.options.Add(op1);

//         TMP_Dropdown.OptionData op2 = new TMP_Dropdown.OptionData();
//         op2.text = "铜锭";
//         RecipeList.options.Add(op2);

//         TMP_Dropdown.OptionData op3 = new TMP_Dropdown.OptionData();
//         op3.text = "金锭";
//         RecipeList.options.Add(op3);
//     }
//     private void Change(int index)
//     {
//         recipeIndex = index;
//         //enemyName.text = index.ToString();
//     }
// }
