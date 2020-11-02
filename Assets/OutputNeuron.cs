using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputNeuron : Neuron
{
    // Start is called before the first frame update


    public override void drawCube()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        cube.transform.parent = gameObject.transform;
        cube.transform.position = gameObject.transform.position;
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
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
                break;
        }
    }
    public virtual void CreateConnection(List<GameObject> vecinity)
    {
        float restructureProbability = (1f - ((float)globalActivity / desiredActivity)) * connectionCreationRate;
        if (dentrites.Count < maxConnections && Random.value < restructureProbability)
        {
            int randomIndex = Random.Range(0, vecinity.Count);
            if (vecinity[randomIndex] != gameObject)
            {
                GameObject synapseGameObject = Instantiate(synapsePrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
                synapseGameObject.transform.parent = transform;
                Synapse synapse = synapseGameObject.GetComponent<Synapse>();
                synapse.Connect(vecinity[randomIndex], gameObject);
                dentrites.Add(synapse);
            }
        }
    }
}
