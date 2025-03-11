using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class AgentController : Agent
{
    // Ta nazwa musi zgadzać się wraz z nazwą elementu przeszkody gdzie jest collider
    // TODO: Dodaj system odbierania i dodawania nagród dla agenta w każdej z funkcji
    public string ObstacleObjectTag = "obstacle";
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    public Transform target;
    //TYMCZASOWA PROBA NAPRAWIENIA BRAKU WYWOLANIA KOLIZJI PRZY UDERZENIU PRZEZ SŁUPEK OD DOŁU // paskudne
    public GameObject samochod, inny;
    Collider samochod_Collider, inny_Collider;
    public Transform dziecko;
    public Rigidbody rBody;

    public Transform Target;

    private void Start() {
        if (samochod != null)
            samochod_Collider = samochod.GetComponent<Collider>();
        
        if (inny != null)
            inny_Collider = inny.GetComponent<Collider>();


        dziecko = transform.GetChild(0);
        rBody = dziecko.GetComponent<Rigidbody>();
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

}
