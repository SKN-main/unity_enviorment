using UnityEngine;

public class AgentController : MonoBehaviour
{
    // Ta nazwa musi zgadzać się wraz z nazwą elementu przeszkody gdzie jest collider
    // TODO: Dodaj system odbierania i dodawania nagród dla agenta w każdej z funkcji
    public string ObstacleObjectTag = "obstacle";
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    public Transform target;
    //TYMCZASOWA PROBA NAPRAWIENIA BRAKU WYWOLANIA KOLIZJI PRZY UDERZENIU PRZEZ SŁUPEK OD DOŁU
    public GameObject samochod, inny;
    Collider samochod_Collider, inny_Collider;
    


    private void Start() {
        if (samochod != null)
            samochod_Collider = samochod.GetComponent<Collider>();
        
        if (inny != null)
            inny_Collider = inny.GetComponent<Collider>();

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
}
