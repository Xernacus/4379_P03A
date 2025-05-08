using UnityEngine;

public class Window : MonoBehaviour, IInteractable
{
    private bool _barricaded = false;
    private GameObject _barricade;

    [SerializeField]
    private GameObject _barricadePrefab;
    [SerializeField]
    private GameObject _barricadePoint;

    [SerializeField]
    private AudioClip _breakSfx;

    [SerializeField]
    private AudioClip _damageSfx;

    [SerializeField]
    private AudioClip _barricadeSfx;

    [SerializeField]
    private GameObject _hitVfx;

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
            if (_barricadeSfx != null)
            {
                AudioHelper.PlayClip2D(_barricadeSfx, 1f);
            }
        }
    }

    public void Damage()
    {
        _barricadeHealth--;
        Instantiate(_hitVfx, gameObject.transform.position, Quaternion.identity);
        if (_barricadeHealth < 0)
        {
            BreakBarricade();
        }
        else
        {
            if (_damageSfx != null)
            {
                
                AudioHelper.PlayClip2D(_damageSfx, 1f);
            }
        }
    }

    public void BreakBarricade()
    {
        _barricaded = false;
        if (_breakSfx != null)
        {
            AudioHelper.PlayClip2D(_breakSfx, 1f);
        }
        Destroy(_barricade);
    }
}
