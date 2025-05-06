using UnityEngine;

public class Window : MonoBehaviour, IInteractable
{
    private bool _barricaded = false;
    private GameObject _barricade;

    [SerializeField]
    private GameObject _barricadePrefab;
    [SerializeField]
    private GameObject _barricadePoint;

    public void Interact()
    {

    }
}
