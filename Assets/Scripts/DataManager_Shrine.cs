using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DataManager_Shrine : MonoBehaviour
{
    private GameObject player;

    // API configs
    private string userId;
    [SerializeField] private string url;

    // UI Configs
    [SerializeField] private UIController_Shine uIController_Shine;
    [SerializeField] private GameObject coinParticle;
    [SerializeField] private Transform altar;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PhotonLocalPlayer");
    }
    public void getPlayerData() => StartCoroutine(CommunicateWithDB(userId));

    public IEnumerator InitPlayerDB(string id, string userName)
    {
        userId = id;

        WWWForm form = new WWWForm();
        form.AddField("command", "initData");
        form.AddField("id", id);
        form.AddField("userName", userName);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        uIController_Shine.SetPointUI(www.downloadHandler.text);
        www.Dispose();
    }

    public IEnumerator CommunicateWithDB(string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "checkPoint");
        form.AddField("id", id);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        if (www.downloadHandler.text == "success")
        {
            uIController_Shine.GetWorshipGameUI().gameObject.SetActive(true);
            coinParticle.GetComponent<ParticleSystem>().Play();

            if (player != null)
            {
                player.GetComponent<vThirdPersonInput>().enabled = false;
                // player.GetComponent<vThirdPersonController>().freeSpeed.rotationSpeed = 0;
                // player.GetComponent<vThirdPersonController>().freeSpeed.walkSpeed = 0;
                // player.GetComponent<vThirdPersonController>().freeSpeed.runningSpeed = 0;
                // player.GetComponent<vThirdPersonController>().freeSpeed.sprintSpeed = 0;

                Vector3 directionToAltar = (altar.position - player.transform.position).normalized;
                Vector3 newPosition = altar.position - directionToAltar * 2.0f;
                player.transform.position = newPosition;

                Vector3 lookDirection = altar.position - player.transform.position;
                float angleY = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
                player.transform.rotation = Quaternion.Euler(0, angleY, 0);
            }
            else
            {
                Debug.LogWarning("Player not found!");
            }
        }
        else if (www.downloadHandler.text == "fail")
        {
            uIController_Shine.SetWorshipFailUI(true);
        }
        www.Dispose();
        StartCoroutine(CheckPoint(id));
    }

    IEnumerator CheckPoint(string id)
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "getPoint");
        form.AddField("id", id);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        string point = www.downloadHandler.text;
        uIController_Shine.SetPointUI(point);

        www.Dispose();
    }

}
