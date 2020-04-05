using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataPointManager : MonoBehaviour
{
    [Header("Viewport Settings")]
    public Support.Boundaries viewportBounds;

    [Header("Interaction Settings")]
    private Camera mainCamera;
    public float rayDistance = 100.0f;
    public LayerMask layers;

    [Header("Details Panel Fields")]
    public TMP_Text Feed;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnRequiredBubbles(false, transform.position));
        mainCamera = Camera.main;
        Feed.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastCheck();
    }


    private void RaycastCheck()
    {

        //Check if the right mouse button has been pressed
        if (Input.GetMouseButtonDown(1))
        {
            //Create the ray itself
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //Use to visualize the Ray genrated from our main Camera
            //Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);


            //Create the raycast
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, layers))
            {
                PointData pd = hit.transform.GetComponent<PointData>();

                Debug.Log($"Province or State: {pd.loc.Combined_Key} Confirmed: {pd.loc.Confirmed} Deaths: {pd.loc.Deaths}");
                UpdateDetails(pd);


            }

        }

    }

    private void UpdateDetails(PointData pd)
    {
        StopAllCoroutines();
        string feedOutput = string.Empty;

        //Build the feed....
        feedOutput = $"Country: {pd.loc.Country_Region} \n";
        feedOutput += $"Location: {pd.loc.Combined_Key} \n";
        feedOutput += $"Confirmed: {pd.loc.Confirmed} \n";
        feedOutput += $"Deaths: {pd.loc.Deaths} \n";
        feedOutput += $"Recoverd: {pd.loc.Recovered} \n";
        feedOutput += $"Active: {pd.loc.Active} \n";
        feedOutput += $"Last Updated: {pd.loc.Last_Update}";

        Feed.richText = true;
        Feed.text = feedOutput;

        StartCoroutine(UpdateFeed());

    }

    public IEnumerator UpdateFeed()
    {
        int totalVisibleCharacters = Feed.text.Length;
        Debug.Log($"Total Characters: {totalVisibleCharacters.ToString()}");

        int counter = 0;

        while (counter < totalVisibleCharacters)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);

            Debug.Log($"VisibleCount: {visibleCount}");

            Feed.maxVisibleCharacters = visibleCount;

           // if (visibleCount >= totalVisibleCharacters)
           // {
           //     yield return new WaitForSeconds(1.0f);
           // }

            counter += 1;
            yield return new WaitForSeconds(0.04f);
        }
    }



}
