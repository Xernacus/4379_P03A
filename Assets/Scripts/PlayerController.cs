using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController _gameController;
    private GameHUDController _uiControls;

    public GameController GameController
    {
        get => _gameController;
        set => _gameController = value;
    }

    private CharacterController _characterController;

    private float _moveSpeed = .1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();

        if (_gameController != null)
        {
            _uiControls = _gameController.GameHUDController;
            _gameController.Camera.Target.TrackingTarget = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _move = new Vector3(_uiControls.JoystickInput.x, 0, -_uiControls.JoystickInput.y);
        _characterController.Move(_move * Time.deltaTime * _moveSpeed);
    }
}
