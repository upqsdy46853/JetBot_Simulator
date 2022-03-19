using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;

public class jetbotCamera : MonoBehaviour
{
    public string ServerAddress = "ws://localhost:9002";
	public string Session = "/Actor1";
    private WebSocket ws;
    public RenderTexture tex;
    float invoke_timer = 0f;
    public float invoke_interval = 1.0f;
    public bool send = false;

    // Start is called before the first frame update
    void Start()
    {
        ws = new WebSocket(ServerAddress + Session + "/camera/publish");
		ws.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if(!send){
            send = true;
            SendMessage();
        }
        /*invoke_timer += Time.deltaTime;
        if(invoke_timer >= invoke_interval)
        {
            SendMessage();
            invoke_timer = 0;
            //Debug.Log("Invoke !");
        }*/
    }

    void SendMessage()
    {
        byte[] img = sendCameraTexture();
        byte[] reward = BitConverter.GetBytes(GetComponent<getReward>().reward);
        byte[] done = BitConverter.GetBytes(GetComponent<getReward>().done);
        byte[] byteArray = new byte[img.Length + reward.Length + done.Length];
        Buffer.BlockCopy(reward, 0, byteArray, 0, reward.Length);
        Buffer.BlockCopy(done, 0, byteArray, reward.Length, done.Length);
        Buffer.BlockCopy(img, 0, byteArray, reward.Length + done.Length, img.Length);
        ws.Send(byteArray);
        GetComponent<getReward>().reward = 0;
    }

    byte[] sendCameraTexture(){
        Texture2D myTexture = toTexture2D(tex);
        byte[] byteArray = myTexture.EncodeToJPG();
        return byteArray;
        //System.IO.File.WriteAllBytes("test.jpg", byteArray);
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
