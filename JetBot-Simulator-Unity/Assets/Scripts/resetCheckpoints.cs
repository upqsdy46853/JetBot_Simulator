using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetCheckpoints : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] checkpoints;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reset()
    {
        foreach (var checkpoint in checkpoints){
            checkpoint.SetActive(true);
        }
    }
}
