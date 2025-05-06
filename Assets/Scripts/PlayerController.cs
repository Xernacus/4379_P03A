using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController _gameController;
    private GameHUDController _uiControls;
    private Health _health;

    [SerializeField]
    private LayerMask _interactLayers;
    [SerializeField]
    private float _interactRadius = 3f;
    private Collider[] colliders = new Collider[50];
    public GameController GameController
    {
        get => _gameController;
        set => _gameController = value;
    }

    public Health Health
    {
        get => _health;
        set => _health = value;
    }

    private CharacterController _characterController;

    private float _moveSpeed = .1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        _health = gameObject.GetComponent<Health>();
        if (_gameController != null)
        {
            _uiControls = _gameController.GameHUDController;
            _gameController.Camera.Target.TrackingTarget = gameObject.transform;
        }

        _uiControls.OnAttack += Attack;
        _uiControls.OnInteract += Interact;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _move = new Vector3(_uiControls.JoystickInput.x, 0, -_uiControls.JoystickInput.y);
        _characterController.Move(_move * Time.deltaTime * _moveSpeed);
    }

    private void OnDisable()
    {
        _uiControls.OnAttack -= Attack;
        _uiControls.OnInteract -= Interact;
    }

    private void Attack()
    {

    }

    private void Interact()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _interactRadius, colliders, _interactLayers, QueryTriggerInteraction.Collide);
        foreach (Collider col in colliders)
        {
            col.gameObject.GetComponent<IInteractable>().Interact();
        }
    }
}
