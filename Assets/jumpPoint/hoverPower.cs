using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverPower : MonoBehaviour
{

    public Material hoverMat;
    public Material hoverEffect;
    float offset;
    // Start is called before the first frame update
   void EmisPowerOn()
    {
        hoverEffect.SetFloat("_hoverEffectEmis", 1.5f);
        hoverMat.SetFloat("_hoverCore", 1);
    }

    void EmisPowerOff()
    {
        hoverEffect.SetFloat("_hoverEffectEmis", 0f);
        hoverMat.SetFloat("_hoverCore", 0);
    }

    private void Update()
    {
        offset = hoverEffect.GetFloat("_hoverEffectTime");
        hoverEffect.SetFloat("_hoverEffectTime", offset + Time.deltaTime);
    }
}

