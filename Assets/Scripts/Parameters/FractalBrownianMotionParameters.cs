using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FBMParameters", menuName = "ScriptableObjects/FBMParameters", order = 3)]

public class FractalBrownianMotionParameters : ScriptableObject
{
    public float BaseFrequency;
    public int Octaves;
    public float Lacunarity;
    public float Gain;
    public float FractalWidthSpan;
}