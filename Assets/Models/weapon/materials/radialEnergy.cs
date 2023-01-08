using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radialEnergy : MonoBehaviour
{

    public Material radialMat;
    float offset;
    
    // Start is called before the first frame update
    void Start()
    {
        radialMat.SetFloat("_energyRadial", 0);
    }

    // Update is called once per frame
    void Update()
    {
        offset = radialMat.GetFloat("_energyRadial");
        radialMat.SetFloat("_energyRadial", offset + Time.deltaTime );
    }
}
