using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] CameraMove cameraMove;
    [SerializeField] int rlCount = 0;
    private List<Panel> panels;
    int rlCountMax = 0;

    float haveTime = 0.0f;
    int haveNum = 0;
    [SerializeField] Vector3 nowPanel1Pos = Vector3.zero;
    [SerializeField] Vector3 nowPanel2Pos = Vector3.zero;

    private Panel holdingPanel;
    private Panel targetPanel;
    bool isHolding = false; // 持っている状態を表すフラグ

    // スティックの入力感度を制御するための変数
    private float stickThreshold = 0.5f;
    private float stickCooldown = 0.4f;
    private float lastStickInputTime = 0f;

    // サウンドエフェクト
    [SerializeField] AudioClip pickUpSound; // パネルを持った時のサウンド
    [SerializeField] AudioClip rotateSound; // パネルを回転させた時のサウンド
    [SerializeField] AudioClip swapSound;   // パネルを入れ替えた時のサウンド
    private AudioSource audioSource;        // AudioSourceコンポーネント

    // Start is called before the first frame update
    void Start()
    {
        // listインスタンス生成
        panels = new List<Panel>();
        // tag「Panel」をもつオブジェクトをすべて取得
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Panel");
        foreach (GameObject obj in objects)
        {
            Panel p;
            // Panelコンポーネントを取得出来たらリストへ追加
            if (obj.TryGetComponent<Panel>(out p))
            {
                panels.Add(p);
            }
        }
        panels.Sort((x, y) => string.Compare(x.name, y.name));

        rlCountMax = panels.Count - 1;
        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得
    }

    // Update is called once per frame
    void Update()
    {
        Choose();
    }

    void Choose()
    {
        if (cameraMove.isSwitch)
        {
            arrow.SetActive(true);

            // 左スティックの入力を検出
            float stickInput = Input.GetAxis("Horizontal");

            if (Time.time - lastStickInputTime >= stickCooldown)
            {
                if (stickInput > stickThreshold)
                {
                    if (rlCount < rlCountMax)
                    {
                        rlCount++;
                        arrow.transform.position = new Vector3(panels[rlCount].transform.position.x, arrow.transform.position.y, arrow.transform.position.z);
                        SwapPanels();
                        PlaySound(swapSound); // 入れ替え時にサウンド再生
                        lastStickInputTime = Time.time;
                    }
                }
                else if (stickInput < -stickThreshold)
                {
                    if (rlCount > 0)
                    {
                        rlCount--;
                        arrow.transform.position = new Vector3(panels[rlCount].transform.position.x, arrow.transform.position.y, arrow.transform.position.z);
                        SwapPanels();
                        PlaySound(swapSound); // 入れ替え時にサウンド再生
                        lastStickInputTime = Time.time;
                    }
                }
            }

            // 回転
            if (Input.GetAxis("Jump") != 0 &&
                !panels[rlCount].rotating &&
                !panels[rlCount].isHave &&
                haveTime <= 1.0f)
            {
                panels[rlCount].rotateSet(panels[rlCount].transform.rotation * Quaternion.Euler(180, 0, 0), panels[rlCount].transform.position); // X方向に180度回転を追加
                panels[rlCount].rotating = true; // 回転開始
                PlaySound(rotateSound); // 回転時にサウンド再生
            }

            // Xボタンを押した時の処理
            if (Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                isHolding = !isHolding; // 持つ/離すの切り替え
                if (isHolding)
                {
                    haveNum = rlCount;
                    nowPanel1Pos = panels[haveNum].transform.position;
                    holdingPanel = panels[haveNum];
                    // 持つ処理
                    panels[haveNum].isHave = true;
                    nowPanel2Pos = panels[rlCount].transform.position;
                    PlaySound(pickUpSound); // 持つ時にサウンド再生
                }
                else
                {
                    // 離す処理
                    haveTime = 0.0f;
                    panels[rlCount].isHave = false;
                }
            }

            // その他の持つ判定用の処理（必要に応じて保持）
            if (!isHolding && (Input.GetKey(KeyCode.Space) || stickInput > stickThreshold || stickInput < -stickThreshold))
            {
                haveTime = 0.0f;
                panels[rlCount].isHave = false;
            }
        }
        else
        {
            arrow.SetActive(false);
        }
    }

    void SwapPanels()
    {
        if (holdingPanel != null && holdingPanel.isHave)
        {
            targetPanel = panels[rlCount];
            Vector3 tempPos = targetPanel.transform.position;
            targetPanel.transform.parent.position = holdingPanel.transform.position;
            holdingPanel.transform.parent.position = tempPos;

            // パネルのリストのインデックスを入れ替える
            int holdingIndex = panels.IndexOf(holdingPanel);
            int targetIndex = panels.IndexOf(targetPanel);
            panels[holdingIndex] = targetPanel;
            panels[targetIndex] = holdingPanel;
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
