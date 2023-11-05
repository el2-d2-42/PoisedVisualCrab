using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISS_Data : MonoBehaviour
{
    // Start is called before the first frame update
    new public string name { get; set; }
    public int id { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public double altitude { get; set; }
    public double velocity { get; set; }
    public string visibility { get; set; }
    public double footprint { get; set; }
    public long timestamp { get; set; }
    public double daynum { get; set; }
    public double solar_lat { get; set; }
    public double solar_lon { get; set; }
    public string units { get; set; }
}

public class Raspery_Data : MonoBehaviour
{
    public float latitude { get; set; }
    public float longitude { get; set; }
}