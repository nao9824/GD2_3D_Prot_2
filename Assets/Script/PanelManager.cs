using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] CameraMove cameraMove;
    [SerializeField] int rlCount = 0;
    private List<Panel> panels;
    int rlCountMax = 0;

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
            if(obj.TryGetComponent<Panel>(out p))
            {
                panels.Add(p);
            }
        }
        panels.Sort((x,y)=> string.Compare(x.name,y.name));

        rlCountMax = panels.Count - 1;
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

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (rlCount < rlCountMax)
                {
                    rlCount++;
                    arrow.transform.position = new Vector3(panels[rlCount].transform.position.x, arrow.transform.position.y, arrow.transform.position.z);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (rlCount > 0)
                {
                    rlCount--;
                    arrow.transform.position = new Vector3(panels[rlCount].transform.position.x, arrow.transform.position.y, arrow.transform.position.z);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && !panels[rlCount].rotating)
            {
                panels[rlCount].rotateSet(panels[rlCount].transform.rotation * Quaternion.Euler(180, 0, 0)); // X方向に180度回転を追加
                panels[rlCount].rotating = true; // 回転開始
            }

        }
        else
        {
            arrow.SetActive(false);
        }
    }
}
