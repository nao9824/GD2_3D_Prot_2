using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] AudioClip loadSound; // ステージ読み込み時のサウンド
    private AudioSource audioSource;      // AudioSourceコンポーネント
    private bool hasJumped = false;       // ジャンプキーが押されたかどうかのフラグ

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Jump") != 0 && !hasJumped)
        {
            hasJumped = true; // フラグを設定して音が繰り返し再生されないようにする
            StartCoroutine(LoadStageChoiceAfterSound());
        }
    }

    IEnumerator LoadStageChoiceAfterSound()
    {
        // SEを再生
        PlaySound(loadSound);

        // サウンドが再生し終わるまで待つ
        yield return new WaitForSeconds(loadSound.length);

        // StageChoiceシーンを読み込む
        SceneManager.LoadScene("StageChoice");
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
