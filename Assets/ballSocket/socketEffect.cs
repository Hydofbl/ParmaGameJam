using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class socketEffect : MonoBehaviour
{

    public Material socketMat;
    float offset;
    // Start is called before the first frame update
   void EmisPowerOn()
    {
        socketMat.SetFloat("_socketPower", 5f);
    }

    void EmisPowerOff()
    {
        socketMat.SetFloat("_socketPower", 0f);
    }

    private void Update()
    {
        offset = socketMat.GetFloat("_socketTime");
        socketMat.SetFloat("_socketTime", offset + Time.deltaTime);
    }
}

