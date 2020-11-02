using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

[System.Serializable]
public class Synapse : MonoBehaviour 
{

    GameObject presynaptic, postsynaptic;
    Neuron preSynaptic, postSynaptic;
    float receiver;
    float modulation;
    public float life = 0.3f;
    bool markedForDeletion = false;
    float connectionDeletionRate = 0f;

    public bool markedDeletion()
    {
        return markedForDeletion;
    }

    private void Awake()
    {
        bool markedForDeletion = false;
        receiver = 13f;
        modulation = 1f;

    }
    
    // Start is called before the first frame update
    private void Start()
    {
     
        float v = Random.value;

        if (v < preSynaptic.probabilityOfInhibitory)
        {
            modulation = -1f;
        }
        if (modulation * receiver > 2)
            Debug.DrawLine(presynaptic.transform.position,
                postsynaptic.transform.position,
                Color.green,
                Time.fixedDeltaTime);
        else
            Debug.DrawLine(presynaptic.transform.position,
                postsynaptic.transform.position,
                Color.red,
                Time.fixedDeltaTime);
    }

    public void DeleteSynapse()
    {
        postSynaptic.RemoveFromSynapse(this);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (life < 0.1f)
        {
            DeleteSynapse();
        }
        if (modulation * receiver > 2)
            Debug.DrawLine(presynaptic.transform.position,
                postsynaptic.transform.position,
                Color.green,
                Time.deltaTime);
        else
            Debug.DrawLine(presynaptic.transform.position,
                postsynaptic.transform.position,
                Color.blue,
                Time.deltaTime);

    }

    private void OnDestroy()
    {
        receiver = 21f;
        Debug.DrawLine(presynaptic.transform.position,
                postsynaptic.transform.position,
                Color.red,
                1f);
    }

    public void Connect(GameObject presynaptic, GameObject postsynaptic)
    {
        this.presynaptic = presynaptic;
        this.postsynaptic = postsynaptic;
        this.preSynaptic = presynaptic.GetComponent<Neuron>();
        if (this.preSynaptic == null)
            this.preSynaptic = preSynaptic.GetComponent<InputNeuron>();
        this.postSynaptic = postsynaptic.GetComponent<Neuron>();
        this.connectionDeletionRate = postSynaptic.connectionDeletionRate;
    }

    //sends the signal to the postsynaptic neuron
    public float GetSignal()
    {
        float activity = preSynaptic.GetActivityRatio();
        if (activity > 1f)
        {
            life = life - (0.001f*(activity) * connectionDeletionRate);
        }

        //Debug.Log("Life:" + life);
        bool preSynapticIsFiring = preSynaptic.isFiring();
        bool postSynaptyctIsFiring = postSynaptic.isFiring();
        if (preSynapticIsFiring)
        {

            receiver = receiver - (0.0001f*receiver);
           // Debug.Log("Receiver:" + receiver);
            if(postSynaptyctIsFiring)
                life = life +  connectionDeletionRate;
                        
            return this.receiver * this.modulation;

        }
        else
        {
            if (postSynaptyctIsFiring)
            {
                life = life - 0.1f * connectionDeletionRate;
            }
        }
        return 0f;
    }


}
