using UnityEngine;

public class BarricadePickup : MonoBehaviour, IInteractable
{
    [SerializeField]
    private AudioClip _sfx;
    public void Interact(PlayerController player)
    {
        player.HasBarricade = true;
        if (_sfx != null)
        {
            AudioHelper.PlayClip2D(_sfx, 1f);
        }
        Destroy(gameObject);
    }
}
