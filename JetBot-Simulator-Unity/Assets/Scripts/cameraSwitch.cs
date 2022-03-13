using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraSwitch : MonoBehaviour
{
    public Camera cameraFree;
    public Camera camera3rd;
    public Camera cameraTop;
    public Dropdown drop;
    public cameraMouseMove cm;
    // Start is called before the first frame update
    void Start()
    {
        cm.enabled = true;
        cameraFree.enabled = true;
        camera3rd.enabled = false;
        cameraTop.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCamera()
    {
        int id = drop.value;
        if(id==0)
        {
            cm.enabled = true;
            cameraFree.enabled = true;
            camera3rd.enabled = false;
            cameraTop.enabled = false;
        }

        if(id==1)
        {
            cm.enabled = false;
            cameraFree.enabled = false;
            camera3rd.enabled = true;
            cameraTop.enabled = false;
        }

        if(id==2)
        {
            cm.enabled = false;
            cameraFree.enabled = false;
            camera3rd.enabled = false;
            cameraTop.enabled = true;
        }
    }
}
