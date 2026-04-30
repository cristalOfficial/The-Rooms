using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float range = 5f; // Aumentado para seguridad
    public LayerMask pickableLayer;

    // No necesitamos playerCamera si usas Camera.main correctamente

    void Update()
    {
        // Esto te permite ver el rayo en la ventana de Escena
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * range, Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
            PerformRaycast();
        }
    }

    void PerformRaycast()
    {
        RaycastHit hit;
        // USAMOS el rayo que sale de la cámara hacia adelante
        // quitamos pickableLayer por un segundo para probar detección total
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            Debug.Log("EL RAYO TOCÓ: " + hit.collider.name);

            // Buscamos la interfaz
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                Debug.Log("¡ÉXITO! Objeto interactuable detectado.");
                interactable.Interact();
            }
            else
            {
                Debug.LogWarning("Toqué a " + hit.collider.name + " pero no tiene el script GlockItem.");
            }
        }
        else
        {
            Debug.Log("EL RAYO NO TOCÓ NADA. Mira el Rayo Rojo en la ventana Scene.");
        }
    }
}