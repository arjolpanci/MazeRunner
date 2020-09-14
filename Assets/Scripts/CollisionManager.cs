using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    public LabyrinthGenerator lbG; 

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Finish") {
            Debug.Log("Current Position: " + transform.position);
            transform.position = new Vector3(0, 1.2F, 0);
            Debug.Log("New Position: " + transform.position);
            lbG.resetLevel();
        }
    }
    
}
