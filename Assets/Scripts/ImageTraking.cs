using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTraking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;

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
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnePrefabs[trackedImage.referenceImage.name].SetActive(false);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        var tran = trackedImage.transform;
        GameObject prefab = spawnePrefabs[name];
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            prefab.transform.position = tran.position;
            prefab.transform.forward = Vector3.up; 
            prefab.SetActive(true);
        }
        if (trackedImage.trackingState == TrackingState.None)
        {
            prefab.SetActive(false);
        }
        if (trackedImage.trackingState == TrackingState.Limited)
        {
            prefab.SetActive(false);
        }
    }
}
