using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class posText : MonoBehaviour
{
    public Transform jetbotTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Text t = GetComponent<Text>();
        t.text = "Pos: " + jetbotTransform.position.ToString() + "\nRot: " + jetbotTransform.eulerAngles.ToString();
    }
}
