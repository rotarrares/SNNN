using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class Brain : MonoBehaviour
{
    public GameObject neuronPrefab,inputNeuronPrefab,outputNeuronPrefab;
    public GameObject agent;
    private List<Neuron> neurons;
    private List<OutputNeuron> outputNeurons;
    private List<InputNeuron> inputNeurons;
    private List<GameObject> Neurons;
    public int noOfInputs = 2;
    public int noOfOutputs = 2;
    public int noOfNeurons = 10;
    public float timepassing = 2f;
    public float active_neurons_percentage = 10f;
    private int activity = 0;
    
    
    // Start is called before the first frame update

    private void Awake()
    {
        Time.fixedDeltaTime = timepassing;
        neurons = new List<Neuron>();
        Neurons = new List<GameObject>();
        inputNeurons = new List<InputNeuron>();
        outputNeurons = new List<OutputNeuron>();
    }

    public List<GameObject> GetNeurons()
    {
        return this.Neurons;
    }

    public void DecreaseActivity()
    {
        activity--;
    }
    public void IncreaseActivity()
    {
        activity++;
    }
    public void GetActivity(GameObject neu) 
    {
        neu.SendMessage("SetGlobalActivity", activity);
    }
    
    public void LoadNew(Brain b)
    {
        neurons = b.neurons;
        inputNeurons = b.inputNeurons;
        Neurons = b.Neurons;
        noOfNeurons = b.noOfNeurons;
        timepassing = b.timepassing;
        activity = b.activity;
    }



    void Start()
    {

        for (int i = 0; i < noOfNeurons; i++)
        {
            float x, y, z;
            x = Random.Range(1, 10)*3;
            y = Random.Range(1, 10)*3;
            z = Random.Range(1, 10) * 1;
            
            GameObject N = Instantiate(neuronPrefab, new Vector3(x, y, z), Quaternion.identity,gameObject.transform);
            Neuron n = N.GetComponent<Neuron>();
            neurons.Add(n);
            Neurons.Add(N);
            
        }
        //input neurons
        for (int i = 0; i < noOfInputs; i++)
        {
            float x, y, z;
            x = Random.Range(1, 10) * 3;
            y = Random.Range(1, 10) * 3;
            z = Random.Range(1, 10)*1;
            GameObject N = Instantiate(inputNeuronPrefab, new Vector3(x, y, z), Quaternion.identity, gameObject.transform);
            InputNeuron inputNeuron = N.GetComponent<InputNeuron>();
            inputNeurons.Add(inputNeuron);
            Neurons.Add(N);
        }
        for( int i = 0;i < noOfOutputs; i++)
        {
            float x, y, z;
            x = Random.Range(1, 10) * 3;
            y = Random.Range(1, 10) * 3;
            z = Random.Range(1, 10) * 1;
            GameObject N = Instantiate(outputNeuronPrefab, new Vector3(x, y, z), Quaternion.identity, gameObject.transform);
            OutputNeuron outputNeuron = N.GetComponent<OutputNeuron>();
            neurons.Add(outputNeuron);
            outputNeurons.Add(outputNeuron);
            Neurons.Add(N);
        }

        Agent agentScript = agent.GetComponent<Agent>();
        agentScript.giveOutputNeurons(outputNeurons);
        agentScript.giveInputNeurons(inputNeurons);
    }

    private void OnValidate()
    {
        if (timepassing > 0.001)
            Time.fixedDeltaTime = timepassing;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
      
    }
}
