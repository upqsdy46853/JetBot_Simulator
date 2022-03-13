using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getReward : MonoBehaviour
{
    // Start is called before the first frame update
    public int reward = 0;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Checkpoint"){
            other.gameObject.SetActive(false);
            reward = 1;
        }
        if(other.gameObject.tag == "End"){
            reward = 1;
        }
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Obstacle")
            reward = -1;
    }

}
