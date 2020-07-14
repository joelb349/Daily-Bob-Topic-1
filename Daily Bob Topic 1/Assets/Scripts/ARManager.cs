using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.UI;

public class ARManager : MonoBehaviour
{
    private enum State : int
    {
        Idle = 0,
        FindingGround = 1,
        PickMesh = 2,
    }

    // Editor Fields
    public ARSessionOrigin sessionOrigin;
    public ARPlaneManager planeManager;
    public GameObject asset;
    public GameObject placementIndicator;
    public ModelDialog dialog;
    public Canvas canvasI;

    public List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    //Private Fields
    private State state = State.Idle;
    private bool placementPoseIsValid = false;
    private Pose placementPose;

    private void Start()
    {
        state = State.FindingGround;


    }

    private void Update()
    {
        if (state == State.FindingGround)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Instantiate(asset, placementPose.position, placementPose.rotation);
                planeManager.enabled = false;
                state = State.PickMesh;
            }
        }
        else if (state == State.PickMesh)
        {
            RayCastPickMesh();
        }
    }


    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
    }

    private void UpdatePlacementPose()
    {
        sessionOrigin.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), raycastHits, TrackableType.Planes);

        placementPoseIsValid = raycastHits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = raycastHits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(-1 * cameraBearing);
        }
    }

    private void RayCastPickMesh()
    {
        if (Input.GetMouseButton(0) && dialog.isVisible)
        {
            RaycastHit raycastHit;
            LayerMask layerMask = LayerMask.GetMask(new string[] { "3D Model" });
            Ray ray = sessionOrigin.camera.ScreenPointToRay(Input.mousePosition);
            bool collision = Physics.Raycast(ray, out raycastHit, 10, layerMask);
            if (collision)
            {
                dialog.Set("There are 421 ways of saying the word snow in Scotland!");
            }
        }
    }
}
