using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Threading;

[System.Serializable]
public class Neuron:MonoBehaviour
{
    //Neuron Parameters
    public int firingSteps = 5;
    public int afterFiringSteps = 2;
    public int maxConnections = 5;
    public float probabilityOfInhibitory = 0.1f;
    public float desiredActivity = 1f;
    public float connectionCreationRate = 0.04f;
    public float connectionDeletionRate = 0.04f;

    protected List<Synapse> dentrites;
    protected bool firing = false;
    protected bool afterfiring = false;
    protected int[] stepCounter;
    protected float potential = -70f;
    protected int globalActivity = 0;
    private List<GameObject> Neurons;
    protected GameObject brain;
    public GameObject synapsePrefab;
    protected GameObject cube;
    
    //===============================================================================
    //Neuron Related Tasks
    public virtual bool isFiring()
    {
        return firing;
    }

    public float GetActivityRatio()
    {
        float activity = globalActivity / desiredActivity;
        Debug.Log("Global Activity is:" + globalActivity);
        return activity;
        
    }
    //Asks the brain what is the global activity
    // *Parameter = -1 -> send message to brain
    // otherwise it means brain responded
    public void SetGlobalActivity(int activity = -1)
    {
        if (activity > -1)
            globalActivity = activity;
        else
            brain.SendMessage("GetActivity", gameObject);
    }

    public void RemoveFromSynapse(Synapse sin)
    {
        dentrites.Remove(sin);
    }

    //Sums the signals coming from the dentrites
    public virtual float SumOfSignals()
    {
       // Debug.Log("Activity: " + globalActivity + " / " + desiredActivity);
        float sum = 0f;
        if (dentrites.Count > 0)
        {
            for (int i = 0; i < dentrites.Count; i++)
            {
                sum = sum + dentrites[i].GetSignal();
            }
        }
        return sum;
    }



    // create synapse with random neuron in vecinity
    public virtual void CreateConnection(List<GameObject> vecinity)
    {
        float restructureProbability = (1f - ((float)globalActivity / desiredActivity))*connectionCreationRate;

        if (dentrites.Count < maxConnections && Random.value < restructureProbability)
        {
            int randomIndex = Random.Range(0, vecinity.Count);
            if (vecinity[randomIndex] != gameObject)
            {
                GameObject synapseGameObject = Instantiate(synapsePrefab, new Vector3(0, 0, 0), Quaternion.identity,gameObject.transform);
                synapseGameObject.transform.parent = transform;
                Synapse synapse = synapseGameObject.GetComponent<Synapse>();
                synapse.Connect(vecinity[randomIndex], gameObject);
                dentrites.Add(synapse);
            }
        }
    }

    public virtual void DeleteConnection(List<GameObject> vecinity)
    {
        float restructureProbability = (1f - ((float)globalActivity / desiredActivity))*0f ;
        
        if (dentrites.Count > 0 && Random.value < -restructureProbability)
        {
           
            int randomIndex = Random.Range(0, dentrites.Count - 1);

            dentrites[randomIndex].DeleteSynapse();
        }
    }

    //=========================================================================================
    //Draw Neuron
    public virtual void drawCube()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cube.transform.parent = gameObject.transform;
        cube.transform.position = gameObject.transform.position;
    }
    //Sets the color of the neuron
    protected virtual void ColorCube(int col)
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
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                break;
        }
    }

    // =========================================================================================
    // Unity Timeline Functions
    void Awake()
    {

        stepCounter = new int[5];
        dentrites = new List<Synapse>();
    }

    
  
    private void Start()
    {

        this.brain = gameObject.transform.parent.gameObject;
        Brain brainScript = brain.GetComponent<Brain>();
        desiredActivity = brainScript.noOfNeurons * brainScript.active_neurons_percentage / 100f;
        drawCube();
        this.Neurons = brain.GetComponent<Brain>().GetNeurons();
    }

    void FixedUpdate()
    {
        ThreadPool.QueueUserWorkItem(CalculatePotential);
        //CalculatePotential(new object());
        SetGlobalActivity();
        UpdateProcess();
    }

    private void CalculatePotential(object state)
    {
       this.potential = -70f + SumOfSignals();
    }

    private void UpdateProcess()
    {
        // at treshold
        if (potential > -50f)
        {
            if (firing == false)
            {
                //start spike
                firing = true;
                brain.SendMessage("IncreaseActivity");
                ColorCube(1);
                stepCounter[0] = firingSteps;
            }
        }
        else
        {
            DeleteConnection(Neurons);
        }

        // on firing;
        if (stepCounter[0] > 0)
        {
            if (stepCounter[0] == 1)
            {
                afterfiring = true;
                ColorCube(2);
                stepCounter[1] = afterFiringSteps;
            }
            stepCounter[0]--;
        }

        //after firing;
        if (stepCounter[1] > 0)
        {
            if (stepCounter[1] == 1)
            {
                afterfiring = false;
                brain.SendMessage("DecreaseActivity");
                firing = false;
                ColorCube(3);

            }
            stepCounter[1]--;
        }

        CreateConnection(Neurons);

        
    }

    // =========================================================================================






}

