using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerJoinManager : MonoBehaviour
{
    //TODO
    //You have to raise it when you get userId and userName through your APIs.    
    public string id;
    public string userName;
    [SerializeField] private GameObject DataManager_Shrine;

    void Start()
    {
        //TODO
        //When player joined XANA Server
        //You guys should run this codes
        StartCoroutine(DataManager_Shrine.GetComponent<DataManager_Shrine>().InitPlayerDB(id, userName));
    }
}
