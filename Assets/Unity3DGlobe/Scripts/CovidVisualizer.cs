using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CovidVisualizer : MonoBehaviour
{
    public Material PointMaterial;
    public Gradient Colors;
    public GameObject Earth;
    public GameObject PointPrefab;
    public float ValueScaleMultiplier = 0.0001F;
    public float maxLengthOfDataPoint = 2.0f;
    GameObject[] seriesObjects;
    int currentSeries = 0;
    public void CreateMeshes(Locations covid)
    {
        //Debug.Log($"How many locations: {covid.AllData.Length}");

        seriesObjects = new GameObject[covid.AllData.Length];
        GameObject p = Instantiate<GameObject>(PointPrefab);
        Vector3[] verts = p.GetComponent<MeshFilter>().mesh.vertices;
        int[] indices = p.GetComponent<MeshFilter>().mesh.triangles;

        List<Vector3> meshVertices = new List<Vector3>(65000);
        List<int> meshIndices = new List<int>(117000);
        List<Color> meshColors = new List<Color>(65000);

        for (int i = 0; i < covid.AllData.Length; i++)
        {

            //Debug.Log($"Location: {covid.AllData[i].Province_State} lat: {covid.AllData[i].Lat} long: {covid.AllData[i].Long_}");
            float lat = GetSafeFloatFromString(covid.AllData[i].Lat);

            float lng = GetSafeFloatFromString(covid.AllData[i].Long_);

            float value = GetSafeFloatFromString(covid.AllData[i].Confirmed);

            if ((lat != 0.0f) && (lng != 0.0f))
            {


                GameObject seriesObj = new GameObject("COVID-19");
                seriesObj.transform.parent = Earth.transform;
                seriesObjects[i] = seriesObj;



                p.GetComponent<PointData>().loc = covid.AllData[i];

                Debug.Log($"Last location: {covid.AllData[i].Combined_Key}");

                AppendPointVertices(p, verts, indices, lng, lat, value, meshVertices, meshIndices, meshColors);

                /*
                if (meshVertices.Count + verts.Length > 65000)
                {
                    CreateObject(meshVertices, meshIndices, meshColors, seriesObj);
                    meshVertices.Clear();
                    meshIndices.Clear();
                    meshColors.Clear();
                }
                */

                CreateObject(meshVertices, meshIndices, meshColors, seriesObj, p);
                meshVertices.Clear();
                meshIndices.Clear();
                meshColors.Clear();
                //seriesObjects[i].SetActive(false);
            }


            seriesObjects[currentSeries].SetActive(true);
            Destroy(p);

        }
    }

    private float GetSafeFloatFromString(string floatToCheck)
    {
        float safeFloat = 0.0f;

        safeFloat = float.TryParse(floatToCheck, out safeFloat) ? safeFloat : 0.0f;

        return safeFloat;
    }


    private void AppendPointVertices(GameObject p, Vector3[] verts, int[] indices, float lng, float lat, float value, List<Vector3> meshVertices,
    List<int> meshIndices,
    List<Color> meshColors)
    {
        Color valueColor = Colors.Evaluate(value * ValueScaleMultiplier);
        Vector3 pos;
        pos.x = 0.5f * Mathf.Cos((lng) * Mathf.Deg2Rad) * Mathf.Cos(lat * Mathf.Deg2Rad);
        pos.y = 0.5f * Mathf.Sin(lat * Mathf.Deg2Rad);
        pos.z = 0.5f * Mathf.Sin((lng) * Mathf.Deg2Rad) * Mathf.Cos(lat * Mathf.Deg2Rad);
        p.transform.parent = Earth.transform;
        p.transform.position = pos;

        //Establish a maximum length of the mesh
        float theZ = Mathf.Max(.001f, value * ValueScaleMultiplier);
        if (theZ > maxLengthOfDataPoint)
        {
            theZ = maxLengthOfDataPoint;
        }

        p.transform.localScale = new Vector3(1, 1, theZ);

        p.transform.LookAt(pos * 2);

        int prevVertCount = meshVertices.Count;

        for (int k = 0; k < verts.Length; k++)
        {
            meshVertices.Add(p.transform.TransformPoint(verts[k]));
            meshColors.Add(valueColor);
        }
        for (int k = 0; k < indices.Length; k++)
        {
            meshIndices.Add(prevVertCount + indices[k]);
        }
    }
    private void CreateObject(List<Vector3> meshertices, List<int> meshindecies, List<Color> meshColors, GameObject seriesObj, GameObject p)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = meshertices.ToArray();
        mesh.triangles = meshindecies.ToArray();
        mesh.colors = meshColors.ToArray();
        GameObject obj = new GameObject();
        obj.transform.parent = Earth.transform;
        obj.AddComponent<MeshFilter>().mesh = mesh;
        obj.AddComponent<MeshRenderer>().material = PointMaterial;
        obj.AddComponent<MeshCollider>();
        obj.AddComponent<PointData>().loc = p.GetComponent<PointData>().loc;
        obj.layer = LayerMask.NameToLayer("DataPoints");
        obj.tag = "DataPoint";
        //obj.AddComponent<PointData>().Location = 
        obj.transform.parent = seriesObj.transform;
    }
    public void ActivateSeries(int seriesIndex)
    {
        if (seriesIndex >= 0 && seriesIndex < seriesObjects.Length)
        {
            seriesObjects[currentSeries].SetActive(false);
            currentSeries = seriesIndex;
            seriesObjects[currentSeries].SetActive(true);

        }
    }
}


