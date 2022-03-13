using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class snapshot : MonoBehaviour
{
    private string filePath = "Snapshot/";
    public RenderTexture tex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void save()
    {
        if(!Directory.Exists(this.filePath))
            Directory.CreateDirectory(this.filePath);
            
        Texture2D myTexture = toTexture2D(tex);
        byte[] byteArray = myTexture.EncodeToJPG();
        string filename = System.DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss");
        System.IO.File.WriteAllBytes(filePath+filename+".jpg", byteArray);
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        GameObject.Destroy(tex);
        return tex;
    }
}
