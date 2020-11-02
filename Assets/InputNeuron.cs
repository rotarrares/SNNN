
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[System.Serializable]
public class InputNeuron : Neuron
{

    private float signal;
    public override void drawCube()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = gameObject.transform;
        cube.transform.position = gameObject.transform.position;
    }

    public void SetSignal(float s)
    {
        signal = s;
    }
    private float GetSignal()
    {
        return signal;
    }

    public override float SumOfSignals()
    {
        
        return signal;
    }
    protected override void ColorCube(int col)
    {
        switch (col)
        {
            case 1:
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                break;
            case 2:
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                break;
            default:
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                break;
        }
    }

    public override void DeleteConnection(List<GameObject> vecinity)
    {

    }
    public override void CreateConnection(List<GameObject> vecinity)
    {
    }
}
