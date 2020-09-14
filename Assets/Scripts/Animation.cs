using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public GameObject pickup;
    float yRotation = 0F;

    void Update()
    {
        yRotation += 0.8F;
        if(yRotation >= 180) yRotation=-180F;
        //pickup.transform.Rotate(new Vector3(0, yRotation * Time.deltaTime, 0));
        //float degrees = 90;
        Vector3 to = new Vector3(50, yRotation, 0);
        transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);
    }
    
}
