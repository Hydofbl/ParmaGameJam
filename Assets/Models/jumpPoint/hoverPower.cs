using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPower : MonoBehaviour
{
    public Material hoverMat;
    public Material hoverEffect;
    float offset;

    [SerializeField] private GameObject[] doorActivatorsGO;
    public bool isActive;

    private void Start()
    {
        CheckIsActive();
    }

    private void Update()
    {
        offset = hoverEffect.GetFloat("_hoverEffectTime");
        hoverEffect.SetFloat("_hoverEffectTime", offset + Time.deltaTime);
    }

    public void CheckIsActive()
    {
        isActive = CanHoverWork();

        if (isActive)
        {
            EmisPowerOn();
        }
        else
        {
            EmisPowerOff();
        }
    }

    private bool CanHoverWork()
    {
        foreach (GameObject obj in doorActivatorsGO)
        {
            if (obj.TryGetComponent(out PressurePlate pressurePlate))
            {
                if (!pressurePlate.IsPressed)
                    return false;
            }
            else if (obj.TryGetComponent(out KeyHole keyHole))
            {
                if (!keyHole.IsActive)
                    return false;
            }
        }

        return true;
    }

    // Hatal� �al��yor. EmisPowerOff �a��r�ld�ktan hemen sonra bu da �a�r�l�yor.
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
}

