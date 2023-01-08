using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emissionPower : MonoBehaviour
{

    public Material redButtonMat;
    // Start is called before the first frame update
   void EmisPowerOn()
    {
        redButtonMat.SetFloat("_emisPower", 1);
    }

    void EmisPowerOff()
    {
        redButtonMat.SetFloat("_emisPower", 0);
    }
}

