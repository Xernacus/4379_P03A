using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController _gameController;
    private GameHUDController _uiControls;
    private Health _health;
    private bool _hasBarricade = false;

    [SerializeField]
    private LayerMask _interactLayers;
    [SerializeField]
    private LayerMask _attackLayers;
    [SerializeField]
    private float _attackRadius = .95f;
    [SerializeField]
    private float _interactRadius = 3f;
    private Collider[] colliders = new Collider[50];
    [SerializeField]
    private SpriteRenderer _sprite;

    private Animator _animator;

    [SerializeField]
    private AudioClip _sfx;

    [SerializeField]
    private GameObject _hitbox;
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

    public bool HasBarricade
    {
        get => _hasBarricade;
        set => _hasBarricade = value;
    }

    private CharacterController _characterController;

    private float _moveSpeed = .1f;

    private void Awake()
    {
        _health = gameObject.GetComponent<Health>();
    }

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

        _animator = gameObject.GetComponentInChildren<Animator>();

        _uiControls.SubscribeHealthEvents(_health);
    }

    // Update is called once per frame
    void Update()
    {
        if (_uiControls.JoystickInput.x < 0)
        {
            _sprite.flipX = true;
        }
        else if(_uiControls.JoystickInput.x > 0)
        {
            _sprite.flipX = false;
        }
        Vector3 _move = new Vector3(_uiControls.JoystickInput.x, 0, -_uiControls.JoystickInput.y);
        _characterController.Move(_move * Time.deltaTime * _moveSpeed);
    }

    private void OnDisable()
    {
        _uiControls.OnAttack -= Attack;
        _uiControls.OnInteract -= Interact;
        _health.OnDeath -= _gameController.EnterLoseState;
    }

    private void Attack()
    {
        //_animator.CrossFadeInFixedTime("VagrantAnim", 0.2f);
        _animator.Play("VagrantAnim", 0, 0.01f);
        Physics.OverlapSphereNonAlloc(_hitbox.transform.position, _attackRadius, colliders, _attackLayers, QueryTriggerInteraction.Collide);
        foreach (Collider col in colliders)
        {
            if (col != null)
            {
                Debug.Log(col);                
                col.gameObject.GetComponentInChildren<IDamageable>()?.Damage(this);   
            }
        }
        if (_sfx != null)
        {
            AudioHelper.PlayClip2D(_sfx, .8f);
        }
        //_animator.CrossFadeInFixedTime("VagrantIdle", 0.2f);
    }

    private void Interact()
    {
        
        Physics.OverlapSphereNonAlloc(transform.position, _interactRadius, colliders, _interactLayers, QueryTriggerInteraction.Collide);
        foreach (Collider col in colliders)
        {
            if (col != null)
            {
                Debug.Log(col);
                col.gameObject.GetComponentInChildren<IInteractable>()?.Interact(this);
            }
            
        }
    }

    public void SubscribeToHealthEvent()
    {
        _health.OnDeath += _gameController.EnterLoseState;
    }
}
