using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlanetInteractable : MonoBehaviour
{
    [SerializeField] private string subgameName;
    private XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener(OnClick);
    }

    private void OnClick(ActivateEventArgs args)
    {
        Debug.Log("Object Clicked!");
        StarSystemController.Instance.LaunchSubgame(subgameName);
    }

    private void OnDestroy()
    {
        interactable.activated.RemoveListener(OnClick);
    }
}
