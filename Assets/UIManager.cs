using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel;
    public Transform cameraTransform;
    public float distanceFromCamera = 2.0f;

    void Start()
    {    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            ShowUIPanel();
        }
    }

    public void ShowUIPanel()
    {
        if (uiPanel != null && cameraTransform != null)
        {
            Vector3 newPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
            newPosition.y = 0;

            uiPanel.transform.position = newPosition;

            Quaternion newRotation = Quaternion.LookRotation(newPosition - cameraTransform.position);
            newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);

            uiPanel.transform.rotation = newRotation;

            uiPanel.SetActive(!uiPanel.activeSelf);
        }
    }
}
