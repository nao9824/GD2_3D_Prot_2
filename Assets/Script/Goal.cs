using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    int stageNumMax = 5;

    [SerializeField] Player player;
    [SerializeField] GameObject goalUI; // ゴール画像

    bool isGoal = false;
    string nextSceneName = "";

    public bool isAllClear = false;

    [SerializeField] AudioSource goalSE;


    void Start()
    {
        goalUI.SetActive(false);
    }

    void Update()
    {
        // リトライ
        if (!isGoal && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            player.isUpsideDown = false;
        }

        // ゴール後 Aボタンで進む
        if (isGoal && Input.GetButtonDown("Submit")) // Aボタン
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isGoal) return;

        isGoal = true;

        Debug.Log("ごーーーーーーる！！！");

        // プレイヤー停止
        player.enabled = false;

        // UI表示
        goalUI.SetActive(true);

        // SE再生
        goalSE.Play();


        // 次のシーン決定
        Scene nowScene = SceneManager.GetActiveScene();

        for (int i = 1; i <= stageNumMax; i++)
        {
            if (nowScene.name == "Stage" + i)
            {
                if (i == stageNumMax)
                {
                    isAllClear = true;
                    nextSceneName = "StageChoice";
                }
                else
                {
                    nextSceneName = "Stage" + (i + 1);
                }
                break;
            }
        }
    }
}
