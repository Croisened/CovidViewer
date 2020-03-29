using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPointManager : MonoBehaviour
{
    [Header("Viewport Settings")]
    public Support.Boundaries viewportBounds;

    [Header("Interaction Settings")]
    private Camera mainCamera;
    public float rayDistance = 100.0f;
    public LayerMask layers;

    [Header("Details Panel Fields")]
    public Text Country;
    public Text Location;
    public Text Confirmed;
    public Text Deaths;
    public Text Recoverd;
    public Text Active;
    public Text LastUpdated;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnRequiredBubbles(false, transform.position));
        mainCamera = Camera.main;
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
        Country.text = pd.loc.Country_Region;
        Location.text = pd.loc.Combined_Key;
        Confirmed.text = pd.loc.Confirmed;
        Deaths.text = pd.loc.Deaths;
        Recoverd.text = pd.loc.Recovered;
        Active.text = pd.loc.Active;
        LastUpdated.text = pd.loc.Last_Update;
    }

}
