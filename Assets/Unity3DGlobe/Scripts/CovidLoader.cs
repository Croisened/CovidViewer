using UnityEngine;
using System.Collections;

public class CovidLoader : MonoBehaviour
{
    public CovidVisualizer Visualizer;
    // Use this for initialization
    void Start()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("covidData");
        string json = jsonData.text;
        
        //CovidSeriesArray data = JsonUtility.FromJson<CovidSeriesArray>(json);
        Locations locationsInJson = JsonUtility.FromJson<Locations>(json);

        //Debug.Log($"How many locations: {locationsInJson.AllData.Length}");

        Visualizer.CreateMeshes(locationsInJson);

    }

    void Update()
    {

    }
}
