using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Actuators;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class AgentController : Agent
{
    // Ta nazwa musi zgadzać się wraz z nazwą elementu przeszkody gdzie jest collider
    // TODO: Dodaj system odbierania i dodawania nagród dla agenta w każdej z funkcji
    public string ObstacleObjectTag = "obstacle";
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    //TYMCZASOWA PROBA NAPRAWIENIA BRAKU WYWOLANIA KOLIZJI PRZY UDERZENIU PRZEZ SŁUPEK OD DOŁU // paskudne
    public GameObject samochod, inny;
    Collider samochod_Collider, inny_Collider;
    private Transform dziecko;
    private Rigidbody rBody;

    public Transform Target;
    public float forceMultiplier = 10;

    private void Start() {
        if (samochod != null)
            samochod_Collider = samochod.GetComponent<Collider>();
        
        if (inny != null)
            inny_Collider = inny.GetComponent<Collider>();


        //assumes that the car is the 2nd child
        //dziecko = transform.GetChild(1);
        //rBody = dziecko.GetComponent<Rigidbody>();
        rBody = transform.GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }


  

    private void resetPosition() {
        transform.position = initialPosition;
        transform.eulerAngles = initialRotation;
    }

    private bool checkCollision(Collision collision, string message) {
        string objTag = collision.gameObject.tag;
        if (objTag == ObstacleObjectTag)
        {
            Debug.Log(message);
            return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision) {
        //Debug.Log("Wejście do OnCollisionEnter");
        if (checkCollision(collision, "START")) {
            //resetPosition();
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (checkCollision(collision, "TRWA")) {
            Debug.Log("Wejście do OnCollisionEnter");
            resetPosition();
        }
        
    }

    private void OnCollisionExit(Collision collision) {
        if (checkCollision(collision, "KONIEC")) {
            //resetPosition();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // pozycja celu i agenta
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Predkosc
        sensor.AddObservation(rBody.linearVelocity.y);
        //sensor.AddObservation(rBody.linearVelocity.z);
    }

    public override void OnEpisodeBegin()
    {
       // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < -10)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.linearVelocity = Vector3.zero;
            Debug.Log("Spadl na y < -10");
            resetPosition();
        }
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Fell off platform
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }


}
