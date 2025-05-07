using UnityEngine;

public class Window : MonoBehaviour, IInteractable
{
    private bool _barricaded = false;
    private GameObject _barricade;

    [SerializeField]
    private GameObject _barricadePrefab;
    [SerializeField]
    private GameObject _barricadePoint;

    public void Interact(PlayerController player)
    {
        if (player.HasBarricade)
        {
            _barricade = Instantiate(_barricadePrefab, _barricadePoint.transform.position, gameObject.transform.rotation);
            player.HasBarricade = false;
            _barricaded = true;
        }
    }

    public void BreakBarricade()
    {
        _barricaded = false;
        Destroy(_barricade);
    }
}
