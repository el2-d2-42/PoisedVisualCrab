using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISS_Data : MonoBehaviour
{
    // Start is called before the first frame update
    public string Name { get; set; }
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public double Velocity { get; set; }
    public string visibility { get; set; }
    public double footprint { get; set; }
    public long Timestamp { get; set; }
    public double daynum { get; set; }
    public double solar_lat { get; set; }
    public double solar_lon { get; set; }
    public string units { get; set; }
}
