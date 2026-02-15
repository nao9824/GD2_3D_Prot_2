using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageChoiceManager : MonoBehaviour
{
    [SerializeField] List<GameObject> block;
    [SerializeField] int rlCount = 0;
    int rlCountMax = 0;

    //拡縮
    private Vector3 initialScale = new Vector3(2, 2, 2);
    private Vector3 targetScale = new Vector3(2.5f, 2.5f, 2.5f);
    private float scaleDuration = 0.5f;
    private float scaleTimer = 0f;

    private GameObject previousBlock = null;

    private float horizontalInputCooldown = 0.5f; // 0.5秒のクールダウン
    private float horizontalInputTimer = 0f;

    //AllClear
    [SerializeField] Image image;
    //[SerializeField] Goal goal;

    // サウンドエフェクト
    [SerializeField] AudioClip selectSound; // ブロック選択時のサウンド
    [SerializeField] AudioClip loadSound;   // ステージ読み込み時のサウンド
    private AudioSource audioSource;        // AudioSourceコンポーネント

    // Start is called before the first frame update
    void Start()
    {
        rlCountMax = block.Count - 1;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInputTimer += Time.deltaTime;

        // 左右選ぶ
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInputTimer >= horizontalInputCooldown)
        {
            if (horizontalInput > 0.5f && rlCount < rlCountMax)
            {
                ChangeSelection(rlCount + 1);
                horizontalInputTimer = 0f;
                block[rlCount].transform.localScale = initialScale;
            }
            else if (horizontalInput < -0.5f && rlCount > 0)
            {
                ChangeSelection(rlCount - 1);
                horizontalInputTimer = 0f;
            }
        }

        //選択したステージに飛ぶ
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(LoadStageAfterSound("Stage" + (rlCount + 1)));
        }

        // 現在選択されているブロックを拡縮する処理
        if (rlCount >= 0 && rlCount < block.Count)
        {
            ScaleBlock(block[rlCount]);
        }

        /*if (goal.isAllClear)
        {
            image.gameObject.SetActive(true);
        }*/
    }

    void ChangeSelection(int newIndex)
    {
        if (previousBlock == null)
        {
            previousBlock = block[0];
        }
        if (previousBlock != null)
        {
            // 前回の選択ブロックを元のサイズに戻す
            previousBlock.transform.localScale = initialScale;
        }

        rlCount = newIndex;
        previousBlock = block[rlCount];

        // スケーリングのタイマーをリセット
        scaleTimer = 0f;

        // ブロック選択時にサウンド再生
        PlaySound(selectSound);
    }

    void ScaleBlock(GameObject block)
    {
        scaleTimer += Time.deltaTime;
        float t = Mathf.PingPong(scaleTimer / scaleDuration, 1f);

        // イージング関数を使用してスムーズな動きを実現
        t = Mathf.Sin(t * Mathf.PI * 0.5f);

        block.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    IEnumerator LoadStageAfterSound(string stageName)
    {
        // ステージ読み込み時にサウンド再生
        PlaySound(loadSound);

        // サウンドが再生し終わるまで待つ
        yield return new WaitForSeconds(loadSound.length);

        // ステージを読み込む
        SceneManager.LoadScene(stageName);
    }
}
