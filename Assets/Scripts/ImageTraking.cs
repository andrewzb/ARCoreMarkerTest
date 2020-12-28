using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTraking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;

    public Text text;
    private Dictionary<string, GameObject> spawnePrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackableImageManager;

    private void Awake()
    {
        trackableImageManager = FindObjectOfType<ARTrackedImageManager>();
        foreach (GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            newPrefab.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            newPrefab.SetActive(false);
            spawnePrefabs.Add(prefab.name, newPrefab);
        }
    }


    private void OnEnable()
    {
        trackableImageManager.trackedImagesChanged += ImageChange;
    }

    private void OnDisable()
    {
        trackableImageManager.trackedImagesChanged -= ImageChange;
    }

    private void ImageChange(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            text.text = "add";
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            text.text = "update";
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            text.text = "remove";
            spawnePrefabs[trackedImage.referenceImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        var tran = trackedImage.transform;
        GameObject prefab = spawnePrefabs[name];
        prefab.transform.position = tran.position;
        prefab.transform.forward = Vector3.up; 
        //prefab.transform.rotation = tran.rotation;
        prefab.SetActive(true);
        /*
        foreach (GameObject item in spawnePrefabs.Values)
        {
            if (item.name != name)
            {
                item.SetActive(false);
            }
        }
        */
    }
}
