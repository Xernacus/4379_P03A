using UnityEngine;

public class BarricadePickup : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController player)
    {
        player.HasBarricade = true;
        Destroy(gameObject);
    }
}
