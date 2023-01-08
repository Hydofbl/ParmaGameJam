using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyScript : MonoBehaviour
{

    public Material energyPlane;
    float offset;
    
    // Start is called before the first frame update
    void Start()
    {
        energyPlane.SetFloat("_energyTime", 0);
    }

    // Update is called once per frame
    void Update()
    {
        offset = energyPlane.GetFloat("_energyTime");
        energyPlane.SetFloat("_energyTime", offset + Time.deltaTime );
    }
}
