using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineToggler : MonoBehaviour
{

    private bool visible = false;
    private MeshRenderer meshRenderer;

    [SerializeField]
    public string outlinedSceneName;

    [SerializeField]
    BalloonTransporter transporter;

    [SerializeField]
    DynamicPortalPosition portalPosition;

    void Start()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.enabled = visible;
        portalPosition = FindObjectOfType<DynamicPortalPosition>();
    }

    void Update()
    {
        var target = transporter.sceneName;

        visible = target == outlinedSceneName;

        if(meshRenderer.enabled != visible)
        {
            meshRenderer.enabled = visible;
            var targetPortalPosition = visible ? this.transform.position : Vector3.zero;
            portalPosition.SetPortalPosition(targetPortalPosition);
        }
    }
}
