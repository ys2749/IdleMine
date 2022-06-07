using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    const float devHeight = 12f;
    const float devWidth = 6f;
    // Start is called before the first frame update
    void Start()
    {
        float screenHeight = Screen.height;
        float orthographicSize = this.GetComponent<Camera>().orthographicSize;
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        float cameraWidth = orthographicSize * 2 * aspectRatio;
        if(cameraWidth < devWidth){
            orthographicSize = devWidth / (2*aspectRatio);
            this.GetComponent<Camera>().orthographicSize = orthographicSize;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
