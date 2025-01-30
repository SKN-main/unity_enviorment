using UnityEngine;

public class Lasersensor : MonoBehaviour
{
    public float rayDistance = 10f; // Maksymalna odległość promienia
    public float angleOffset = 30f; // Kąt odchylenia dla dodatkowych czujników
    public Transform carObject;


    void Update()
    {
        // Główny kierunek (do przodu)
        // Vector3 forwardDirection = Quaternion.Euler(90f, 90f, 0) * transform.forward;
        Vector3 forwardDirection = transform.forward;

        // Dodatkowe kierunki pod kątem
        Vector3 leftDirection = Quaternion.Euler(0, -angleOffset, 0) * forwardDirection;  // 30° w lewo
        Vector3 rightDirection = Quaternion.Euler(0, angleOffset, 0) * forwardDirection; // 30° w prawo

        // Tablica kierunków (można dodać więcej)
        Vector3[] directions = { forwardDirection, leftDirection, rightDirection };

        foreach (var direction in directions)
        {
            // Debugowanie promieni
            Debug.DrawRay(carObject.position, direction * rayDistance, Color.red);

            // Wykonanie Raycasta
            if (Physics.Raycast(carObject.position, direction, out RaycastHit hit, rayDistance))
            {
               // Debug.Log("Hit at distance: " + hit.distance + " in direction: " + direction);
            }
        }
    }
}
