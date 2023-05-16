using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager: MonoBehaviour

{
    [Header("Left Hand Teleportation Controller")]
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider teleportationProvider;

    [SerializeField] private InteractionLayerMask TeleportationLayers;

    [SerializeField] private InputActionProperty teleportModeActivate;
    [SerializeField] private InputActionProperty teleportModeCancel;
    [SerializeField] private InputActionProperty thumbMove;
    [SerializeField] private InputActionProperty GripModeActivate;

    private bool isTeleportationActivate;
    private InteractionLayerMask initialInteractionLayers;
    private List<IXRInteractable> interactables = new List<IXRInteractable>();

    // Start is called before the first frame update
    void Start()
    {
        teleportModeActivate.action.Enable();
        teleportModeCancel.action.Enable();
        thumbMove.action.Enable();
        GripModeActivate.action.Enable();

        teleportModeActivate.action.performed += OnTeleportActivate;
        teleportModeCancel.action.performed += OnTeleportCancel;

        initialInteractionLayers = rayInteractor.interactionLayers;
    }

    private void OnTeleportCancel(InputAction.CallbackContext obj)
    {
        TurnOffTeleportation();
    }

    private void OnTeleportActivate(InputAction.CallbackContext obj)
    {
        if (GripModeActivate.action.phase != InputActionPhase.Performed)
        {
            isTeleportationActivate = true;
            rayInteractor.lineType = XRRayInteractor.LineType.ProjectileCurve;
            rayInteractor.interactionLayers = TeleportationLayers;
        }
    }

    private void TurnOffTeleportation()
    {
        isTeleportationActivate = false;
        rayInteractor.lineType = XRRayInteractor.LineType.StraightLine;
        rayInteractor.interactionLayers = initialInteractionLayers;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTeleportationActivate)
            return;
        if (thumbMove.action.triggered)
            return;
        rayInteractor.GetValidTargets(interactables);
        if (interactables.Count == 0)
        {
            TurnOffTeleportation();
            return;
        }
        rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);
        TeleportRequest request = new TeleportRequest();
        if (interactables[0].interactionLayers == 1)
        {
            request.destinationPosition = hit.point;
        }
        teleportationProvider.QueueTeleportRequest(request);
        TurnOffTeleportation();
    }
}
