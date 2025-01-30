using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 10f;      // Speed for forward/backward movement

    private void Update()
    {
        // Forward and backward movement
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        // Move the car along its local forward direction
        transform.Translate(Vector3.forward * move);
    }
}