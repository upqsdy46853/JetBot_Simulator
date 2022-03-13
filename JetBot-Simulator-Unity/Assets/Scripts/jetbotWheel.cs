using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jetbotWheel : MonoBehaviour
{
    public Transform wheelLeft;
    public Transform wheelRight;
    public Transform vehicle;

    // Reset Transform
    public Vector3 reset_position = new Vector3(0,0,0);
    public Vector3 reset_rotation = new Vector3(0,0,0);

    // Vehicle Parameters
    private const float wheelRad = 30f; // mm
    private const float wheelDist = 54f; // mm
    
    // Kinematic Parameters
    public float leftMotor = 360.0f; // degree/sec
    public float rightMotor = 180.0f; // degree/sec
    private float v;
    private float w;

    public InputField inputFieldLeft;
    public InputField inputFieldRight;

    // Start is called before the first frame update
    void Start()
    {
        updateMotorText();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        // Wheel Rotation
        wheelLeft.Rotate(+1*leftMotor*dt,0,0);
        wheelRight.Rotate(-1*rightMotor*dt,0,0);
        
        // Vehicle Move
        v = 0.5f*wheelRad*(leftMotor*Mathf.PI/180f) + 0.5f*wheelRad*(rightMotor*Mathf.PI/180f);
        w = 0.5f*wheelRad*(leftMotor*Mathf.PI/180f)/wheelDist - 0.5f*wheelRad*(rightMotor*Mathf.PI/180f)/wheelDist;
        
        float delta_x = v*dt*Mathf.Cos(vehicle.eulerAngles.y*Mathf.PI/180f);
        float delta_y = v*dt*Mathf.Sin(vehicle.eulerAngles.y*Mathf.PI/180f);
        vehicle.position += new Vector3(delta_y, 0, delta_x);
        vehicle.eulerAngles += new Vector3(0,w*180/Mathf.PI*dt,0);            
    }

    public void updateMotorText()
    {
        inputFieldLeft.text = this.leftMotor.ToString();
        inputFieldRight.text = this.rightMotor.ToString();
    }
    
    public void setLeftMotorValue(float leftValue)
    {
        this.leftMotor = leftValue;
        updateMotorText();
    }

    public void setRightMotorValue(float rightValue)
    {
        this.rightMotor = rightValue;
        updateMotorText();
    }

    public void setMotorValue(float leftValue, float rightValue)
    {
        this.leftMotor = leftValue;
        this.rightMotor = rightValue;
        updateMotorText();
    }

    public void addMotorValue(float leftValue, float rightValue)
    {
        this.leftMotor += leftValue;
        this.rightMotor += rightValue;
        updateMotorText();
    }

    public void setLeftMotorInput()
    {
        float value = float.Parse(inputFieldLeft.text);
        this.leftMotor = value;
        updateMotorText();
    }

    public void setRightMotorInput()
    {
        float value = float.Parse(inputFieldRight.text);
        this.rightMotor = value;
        updateMotorText();
    }

    public void reset()
    {
        this.leftMotor = 0f;
        this.rightMotor = 0f;
        vehicle.position = this.reset_position;
        vehicle.eulerAngles = this.reset_rotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0,0,0);
        rb.angularVelocity = new Vector3(0,0,0);
        updateMotorText();
    }

    public void stop()
    {
        this.leftMotor = 0f;
        this.rightMotor = 0f;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0,0,0);
        rb.angularVelocity = new Vector3(0,0,0);
        updateMotorText();
    }

    public void debugMotorLeft(float value)
    {
        this.leftMotor += value;
        updateMotorText();
    }

    public void debugMotorRight(float value)
    {
        this.rightMotor += value;
        updateMotorText();
    }
}


