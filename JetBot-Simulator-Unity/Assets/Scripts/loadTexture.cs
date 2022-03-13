using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class loadTexture : MonoBehaviour
{
    public Dropdown dropdown;
    public Text dropdownText;
    private List<mapInfo> mapInfoList = new List<mapInfo>();
    // Start is called before the first frame update
    public jetbotWheel jw;
    void Start()
    {
        loadMapSetting("mapSetting.txt");
        updateMap();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateMap()
    {
        loadMap(mapInfoList[dropdown.value].Path);
        jw.reset_position = mapInfoList[dropdown.value].ResetPos;
        jw.reset_rotation = mapInfoList[dropdown.value].ResetRot;
        jw.reset();
    }

    public void loadMapSetting(string path)
    {
        dropdown.options.Clear();
        string file = File.ReadAllText(path);
        string[] lines = file.Split('\n');
        
        for(int i=0; i<lines.Length; ++i)
        {
            mapInfo minfo = new mapInfo();
            string[] content = lines[i].Split(',');
            minfo.Name = content[0].Trim();
            minfo.Path = content[1].Trim();
            minfo.ResetPos = new Vector3(float.Parse(content[2]), float.Parse(content[3]), float.Parse(content[4]));
            minfo.ResetRot = new Vector3(float.Parse(content[5]), float.Parse(content[6]), float.Parse(content[7]));
            
            dropdown.options.Add(new Dropdown.OptionData() {text=content[0]});
            // Default
            if(i==0){
                dropdown.value = 0;
                dropdownText.text = content[0];
            }
            mapInfoList.Add(minfo);
        }
    }

    public void loadMap(string filePath)
    {
        if (File.Exists(filePath))     
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); 
            Material mapMaterial = GetComponent<Renderer>().material;
            mapMaterial.mainTexture = tex;
            Color colorOrig = mapMaterial.color;
            Color colorTrans = new Color (colorOrig.r, colorOrig.g, colorOrig.b, 1.0f);
            mapMaterial.color = colorTrans;
        }
        else{
            Material mapMaterial = GetComponent<Renderer>().material;
            Color colorOrig = mapMaterial.color;
            Color colorTrans = new Color (colorOrig.r, colorOrig.g, colorOrig.b, 0.0f);
            mapMaterial.color = colorTrans;
        }
    }
}

public class mapInfo
{
    public string Name;
    public string Path;
    public Vector3 ResetPos;
    public Vector3 ResetRot;
}