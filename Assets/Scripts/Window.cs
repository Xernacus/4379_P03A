using UnityEngine;

public class Window : MonoBehaviour, IInteractable
{
    private bool _barricaded = false;
    private GameObject _barricade;

    [SerializeField]
    private GameObject _barricadePrefab;
    [SerializeField]
    private GameObject _barricadePoint;

    public GameObject VaultPoint;
    public GameObject OutsidePoint;

    private int _barricadeHealth;

    public bool Barricaded
    {
        get => _barricaded;
        set => _barricaded = value;
    }

    public void Interact(PlayerController player)
    {
        if (player.HasBarricade)
        {
            _barricade = Instantiate(_barricadePrefab, _barricadePoint.transform.position, gameObject.transform.rotation);
            player.HasBarricade = false;
            _barricaded = true;
            _barricadeHealth = 4;
        }
    }

    public void Damage()
    {
        _barricadeHealth--;
        Debug.Log(_barricadeHealth);
        if (_barricadeHealth < 0)
        {
            BreakBarricade();
        }
    }

    public void BreakBarricade()
    {
        _barricaded = false;
        Destroy(_barricade);
    }
}
