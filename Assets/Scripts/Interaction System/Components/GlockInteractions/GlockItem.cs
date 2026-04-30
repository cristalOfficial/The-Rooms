using UnityEngine;


public class GlockItem : MonoBehaviour, IInteractable
{
    public Weapon weapon;

    public void Interact()
    {
        Destroy(gameObject);
        weapon.Glock_17.SetActive(true);

    }
}
