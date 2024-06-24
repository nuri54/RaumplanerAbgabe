using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Colorswap : MonoBehaviour
{
    [SerializeField] private GameObject _parentObject;
    [SerializeField] private EffectMesh mesh;
    private bool switchColor = false;
    private Renderer child;
    private Renderer target;

    public GameObject colorPanel;
    public GameObject editPanel;
    public GameObject MaterialPicker;
    public GameObject colorPicker;
    public Transform cameraTransform;
    public float distanceFromCamera = 2.0f;
    public LineRenderer lineRenderer;
    public GameObject MaterialButton;
    public GameObject DeleteButton;
    public List<Material> possibleMaterials;
    private int currentMaterial = 0;
    private Material previousMaterial;
    public Text materialIndex;
    
    private void Start()
    {
        // Transform childTransform = _parentObject.transform.Find("Table");
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 controllerPostion = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        Vector3 rayDirection = controllerRotation * Vector3.forward;
        Ray rayShoot = new Ray(controllerPostion, rayDirection);
        MRUK.Instance.GetCurrentRoom().Raycast(rayShoot, Mathf.Infinity, out RaycastHit hit, out MRUKAnchor anchorHit);


        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            
            if (anchorHit != null)
            {

                
                Renderer gameObject = new Renderer();
                switch (anchorHit.GetLabelsAsEnum())
                {
                    
                    case MRUKAnchor.SceneLabels.TABLE:
                        gameObject = anchorHit.transform.Find("VH_TABLE(PrefabSpawner Clone)").GetChild(0).GetComponent<Renderer>();
                        break;
                    case MRUKAnchor.SceneLabels.COUCH:
                        gameObject = anchorHit.transform.Find("VH_COUCH(PrefabSpawner Clone)").GetChild(0).GetComponent<Renderer>();
                        break;
                    case MRUKAnchor.SceneLabels.WINDOW_FRAME:
                        gameObject = anchorHit.transform.Find("VH_WINDOW(PrefabSpawner Clone)").GetChild(0).GetComponent<Renderer>();
                        break;
                    case MRUKAnchor.SceneLabels.DOOR_FRAME:
                        gameObject = anchorHit.transform.Find("VH_DOOR(PrefabSpawner Clone)").GetChild(0).GetComponent<Renderer>();
                        break;
                    case MRUKAnchor.SceneLabels.STORAGE:
                        gameObject = anchorHit.transform.Find("VH_STORAGE(PrefabSpawner Clone)").GetChild(0).GetComponent<Renderer>();
                        break;
                    case MRUKAnchor.SceneLabels.BED:
                        gameObject = anchorHit.transform.Find("VH_BED(PrefabSpawner Clone)").GetChild(0).GetComponent<Renderer>();
                        break;
                    case MRUKAnchor.SceneLabels.WALL_FACE or MRUKAnchor.SceneLabels.CEILING or MRUKAnchor.SceneLabels.FLOOR:
                        gameObject = anchorHit.transform.Find("VH_WALL(PrefabSpawner Clone)").GetChild(0).GetChild(0).GetComponent<Renderer>();
                        break;
                        // case MRUKAnchor.SceneLabels.SCREEN:
                        //     gameObject = anchorHit.transform.Find("VH_COUCH(PrefabSpawner Clone)").GetChild(0).GetComponent<Renderer>();
                        //     break;
                        // case MRUKAnchor.SceneLabels.LAMP:
                        //     gameObject = anchorHit.transform.Find("VH_BED(PrefabSpawner Clone)").GetChild(0).GetComponent<Renderer>();
                        //     break;
                        // // Plant 1,2,3,4 prefabs exist - will have to look at this in future
                        // case MRUKAnchor.SceneLabels.PLANT:
                        //     gameObject = anchorHit.transform.Find("VH_PLANT(PrefabSpawner Clone)").GetChild(0).GetComponent<Renderer>();
                        //     break;
                        // // Wallart 1,2,3 prefabs exist - will have to look at this in future
                        // case MRUKAnchor.SceneLabels.WALL_ART:
                        //     gameObject = anchorHit.transform.Find("VH_PLANT(PrefabSpawner Clone)").GetChild(0).GetComponent<Renderer>();
                        //     break;


                }

                if (gameObject != null)
                {
                    if (anchorHit.GetLabelsAsEnum() == MRUKAnchor.SceneLabels.WALL_FACE || anchorHit.GetLabelsAsEnum() == MRUKAnchor.SceneLabels.CEILING 
                        || anchorHit.GetLabelsAsEnum() == MRUKAnchor.SceneLabels.FLOOR )
                    {
                        ShowEditPanel(true);    
                    }
                    else
                    {
                        ShowEditPanel(false);
                    }
                }
                    
                    
                target = gameObject;
             
                Debug.Log(anchorHit.GetLabelsAsEnum());
                Debug.Log(anchorHit);
            }
        }
    }


    public void ShowColorPanel()
    {
        if (colorPanel != null && cameraTransform != null)
        {
            editPanel.SetActive(false);
            Vector3 newPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
            newPosition.y = 0;

            colorPanel.transform.position = newPosition;

            Quaternion newRotation = Quaternion.LookRotation(newPosition - cameraTransform.position);
            newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);

            colorPanel.transform.rotation = newRotation;

            colorPanel.SetActive(true);
            colorPicker.SetActive(true);
        }
    }

    public void ShowEditPanel(bool showMaterial)
    {
        if (editPanel != null && cameraTransform != null)
        {
            Vector3 newPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
            newPosition.y = 0;

            editPanel.transform.position = newPosition;

            Quaternion newRotation = Quaternion.LookRotation(newPosition - cameraTransform.position);
            newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);

            editPanel.transform.rotation = newRotation;

            editPanel.SetActive(true);
            if (showMaterial)
            {
                MaterialButton.SetActive(true);
            }
            else
            {
                DeleteButton.SetActive(true);
            }
        }
    }

    public void changeColor(Color color)
    {
        
        Debug.Log(color);
        target.material.color = color;
        colorPanel.SetActive(false);
        colorPicker.SetActive(false);
    }
    
    public void DeleteObject()
    {
        if (target != null)
        {
            Destroy(target);
            MaterialButton.SetActive(false);
            DeleteButton.SetActive(false);
            editPanel.SetActive(false);
        }

    }
    
    public void CloseEditPanel()
    {
        MaterialButton.SetActive(false);
        DeleteButton.SetActive(false);
        editPanel.SetActive(false);
        
    }

    public void showMaterialPicker()
    {
        
        
        if (MaterialPicker != null && cameraTransform != null)
        {
            previousMaterial = target.material;
            
            target.material = possibleMaterials[currentMaterial];
            
            MaterialPicker.SetActive(false);
            Vector3 newPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
            newPosition.y = 0;

            MaterialPicker.transform.position = newPosition;

            Quaternion newRotation = Quaternion.LookRotation(newPosition - cameraTransform.position);
            newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);

            MaterialPicker.transform.rotation = newRotation;

            materialIndex.text = "Current: " + currentMaterial;
            editPanel.SetActive(false);
            MaterialPicker.SetActive(true);
        }
    }

    public void setMaterial()
    {
        previousMaterial = target.material;
        MaterialPicker.SetActive(false);
        currentMaterial = 0;
    }

    public void closeMaterial()
    {
        target.material = previousMaterial;
        MaterialPicker.SetActive(false);
        currentMaterial = 0;
    }

    public void NextMaterial()
    {
        currentMaterial += 1;
        if (currentMaterial >= possibleMaterials.Count)
        {
            currentMaterial = 0;
        }
        materialIndex.text = "Current: " + currentMaterial;
        target.material = possibleMaterials[currentMaterial];
        
    }
    public void PreviousMaterial()
    {
        
        currentMaterial -= 1;
        if (currentMaterial< 0)
        {
            currentMaterial = possibleMaterials.Count - 1;
        }
        materialIndex.text = "Current: " + currentMaterial;
        target.material = possibleMaterials[currentMaterial];
        
    }
    

    public void SetupObject()
    {
        GameObject childTransform = _parentObject.transform.GetChild(0).gameObject;
        child = childTransform.GetComponent<Renderer>();
        Debug.Log("Hallooo");
        Debug.Log(child);
        Debug.Log(childTransform);
        Debug.Log(child.sharedMaterial);
    }
    
    
    
}
