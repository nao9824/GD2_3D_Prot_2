using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    int stageNumMax = 5;
    [SerializeField]Player player;

    public bool isAllClear = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            player.isUpsideDown = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ごーーーーーーる！！！");

            // 現在のシーンを取得
            Scene nowScene = SceneManager.GetActiveScene();
            for (int i = 0; i < stageNumMax; i++)
            {
                if (nowScene.name == "Stage" + stageNumMax)
                {
                    isAllClear = true;
                    SceneManager.LoadScene("StageChoice");
                    
                }

                    if (nowScene.name == "Stage" + i)
                {
                    SceneManager.LoadScene("Stage" + (i + 1));
                }
            }
        }

    }
}
