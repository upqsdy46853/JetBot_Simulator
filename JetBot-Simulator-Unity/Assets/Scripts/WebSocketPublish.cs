using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class WebSocketPublish
{
    public string ServerAddress;
    public string Session;
    public string Name; 
    private WebSocket ws;

    public WebSocketPublish(string ip, string sess, string name)
    {
        this.ServerAddress = ip;
        this.Session = sess;
        this.Name = name;
        this.ws = new WebSocket(ServerAddress + Session + "/" + name + "/publish");
		this.ws.Connect();
    }

    public void Send(byte[] byteArray){
        this.ws.Send(byteArray);
    }

}
