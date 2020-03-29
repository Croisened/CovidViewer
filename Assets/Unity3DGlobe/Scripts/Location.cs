using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Location
{
    //these variables are case sensitive and must match the strings "firstName" and "lastName" in the JSON.
    public string FIPS;
    public string Admin2;
    public string Province_State;
    public string Country_Region;
    public string Last_Update;
    public string Lat;
    public string Long_;
    public string Confirmed;
    public string Deaths;
    public string Recovered;
    public string Active;
    public string Combined_Key;
}
