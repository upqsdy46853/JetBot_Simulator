using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Text;

public class WebSocketSession
{
    public string ServerAddress;
    public string Session;
    public string Name; 
    private WebSocket ws;
    private bool isReceive = false;
    private MessageEventArgs events;

    public WebSocketSession(string ip, string sess, string name)
    {
        this.ServerAddress = ip;
        this.Session = sess;
        this.Name = name;
        this.ws = new WebSocket(ServerAddress + Session + "/" + name + "/session");
		this.ws.Connect();

        this.ws.OnMessage += (sender, e) => {
			this.events = e;
			this.isReceive = true;
		};
    }

    public MessageEventArgs Listening(){
        if(isReceive)
        {
            this.isReceive = false;
            return events;
        }
        else
        {
            return null;
        }
    }

    public void Send(byte[] byteArray){
        this.ws.Send(byteArray);
    }

}
