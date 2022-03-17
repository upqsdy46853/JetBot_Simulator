using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Text;


public class jetbotControl : MonoBehaviour
{
    public string ServerAddress = "ws://localhost:9002";
    public string Session = "/Actor1";
    private WebSocket ws;
    public jetbotWheel jw;
    private string nextCommand = null;
    public bool receive = false;
    
    void Start()
    {
        this.ws = new WebSocket(ServerAddress + Session + "/controller/session");
		this.ws.Connect();

        this.ws.OnMessage += (sender, e) => {
			string command = Encoding.UTF8.GetString(e.RawData);
			this.nextCommand = command;
            Debug.Log(e.RawData);
		};
        this.nextCommand = null;
    }

    void Update () {
		if(this.nextCommand != null) {
            Debug.Log("Receive");
            wheelControl(nextCommand);
		}
        this.nextCommand = null;
	}

    void wheelControl(string command)
    {
        jetbotMotorCommand cdata = JsonUtility.FromJson<jetbotMotorCommand>(command);
        receive = true;
        if (cdata.flag == 0)
            jw.reset();
        else if(cdata.flag == 1)
            jw.setLeftMotorValue(cdata.leftMotor);
        else if(cdata.flag == 2)
            jw.setRightMotorValue(cdata.rightMotor);
        else if(cdata.flag == 3)
            jw.addMotorValue(cdata.leftMotor, cdata.rightMotor);
        else
            jw.setMotorValue(cdata.leftMotor, cdata.rightMotor);
    }

    public void Send(byte[] byteArray)
    {
        this.ws.Send(byteArray);
    }

}

public class jetbotMotorCommand
{
    public float leftMotor;
    public float rightMotor;
    public int flag; //0:reset, 1:left, 2:right, 3:both, 
}
