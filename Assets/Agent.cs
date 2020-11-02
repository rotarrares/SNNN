using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{


    List<OutputNeuron> input = new List<OutputNeuron>();
    List<InputNeuron> output = new List<InputNeuron>();
    private Rigidbody rb;

    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void giveOutputNeurons(List<OutputNeuron> l)
    {
        input = l;
    }

    public void giveInputNeurons(List<InputNeuron> l)
    {
        output = l;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (input.Count>0)
        {
            if (input[0].isFiring())
            {
                rb.AddTorque(-1f*(Vector3.left * 400f * Time.deltaTime));
            }
            if (input[1].isFiring())
            {
                rb.AddTorque((Vector3.right * 400f * Time.deltaTime));
            }
        }
        if (output.Count > 0)
        {
            Ray rightRay = new Ray(transform.position, Vector3.back);
            Debug.DrawRay(transform.position+Vector3.forward, Vector3.forward, Color.red,Time.fixedDeltaTime);
            // Cast a ray straight downwards.
            if (Physics.Raycast(rightRay, out hit,20f))
            {
                if (hit.distance < 5f)
                {
                    output[0].SetSignal(21f);
                }
                else
                {
                    output[0].SetSignal(0f);

                }

            }
            Ray leftRay = new Ray(transform.position, Vector3.forward);
            Debug.DrawRay(transform.position+Vector3.back, Vector3.back, Color.red, Time.fixedDeltaTime);
            // Cast a ray straight downwards.
            if (Physics.Raycast(leftRay, out hit, 20f))
            {
                if (hit.distance < 5f)
                {
                    output[1].SetSignal(21f) ;

                }
                else
                {
                    output[1].SetSignal(0f);
                }
            }
        }
    }
}
