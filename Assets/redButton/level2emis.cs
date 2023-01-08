using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level2emis : MonoBehaviour
{

    public Material level2mat;

    void CableOn()
    {
        level2mat.SetFloat("_emisPower", 1);
    }
    void CableOff()
    {
        level2mat.SetFloat("_emisPower", 0);
    }
}
