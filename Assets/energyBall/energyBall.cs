using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyBall : MonoBehaviour
{

    public Material ballNoise;
    float offset;
    
    // Start is called before the first frame update
    void Start()
    {
        ballNoise.SetFloat("_ballTime", 0);
    }

    // Update is called once per frame
    void Update()
    {
        offset = ballNoise.GetFloat("_ballTime");
        ballNoise.SetFloat("_ballTime", offset + Time.deltaTime );
    }
}
