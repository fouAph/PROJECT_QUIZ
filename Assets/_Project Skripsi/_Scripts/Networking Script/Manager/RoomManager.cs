using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public int ping;
    QuizManager quizManager => QuizManager.instance;
    private void Awake()
    {


        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    private void Update()
    {
        ping = PhotonNetwork.GetPing();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        GameObject playerObj = null;
        if (scene.buildIndex == 1)
        {
            playerObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
            playerObj.name = PhotonNetwork.NickName;

            quizManager.player = playerObj.GetComponent<PlayerManager>();
        }
    }

}
