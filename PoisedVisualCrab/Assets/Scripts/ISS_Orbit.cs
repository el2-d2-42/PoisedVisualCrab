using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISS_Orbit : MonoBehaviour
{
    Dictionary<string, float> payload = new Dictionary<string, float>();
    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, float> payload = GetCoordinates();
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the coordinates every frame
        Dictionary<string, float> payload = GetCoordinates();
        float timeDiff = payload["tn"] - payload["tn1"];

        float x = (payload["an"] - payload["an1"]);
        float y = (payload["bn"] - payload["bn1"]);
        float z = (payload["zn"] - payload["zn1"]);
        float xvel = (payload["an"] - payload["an1"]) / timeDiff;
        float yvel = (payload["bn"] - payload["bn1"]) / timeDiff;
        float zvel = (payload["zn"] - payload["zn1"]) / timeDiff;
        transform.position = new Vector3(x, y, z);
        Debug.Log("X Velocity " + xvel + ", Y Velocity " + yvel + ", Z Velocity" + zvel);
    }

    Dictionary<string, float> GetCoordinates()
    {
        float an;
        float an1;
        float bn;
        float bn1;
        float zn;
        float zn1;
        float tn;
        float tn1;

        Dictionary<string, float> payload = new Dictionary<string, float>();
        payload.Add("an", an);
        payload.Add("an1", an1);
        payload.Add("bn", bn);
        payload.Add("bn1", bn1);
        payload.Add("zn", zn);
        payload.Add("zn1", zn1);
        payload.Add("tn", tn);
        payload.Add("tn1", tn1);

        return payload;
    }
}