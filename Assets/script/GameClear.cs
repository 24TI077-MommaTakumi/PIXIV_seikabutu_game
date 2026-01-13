using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    [Header("音")] public AudioClip se;
    [Header("プレイヤー判定")] public PlayerTriggerCheck trigger;
    [Header("スピード")] public float speed = 3.0f;
    [Header("動く幅")] public float moveDis = 3.0f;

    private bool on = false;
    private float kakudo = 0.0f;
    private Vector3 defaultPos;
    void Start()
    {
        //初期化
        if (trigger == null)
        {
            Debug.Log("インスペクターの設定が足りません");
            Destroy(this);
        }
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが範囲内に入った
        if (trigger.isOn && !on)
        {
            on = true;
        }

        if (on)
        {
            if (kakudo < 180.0f)
            {
                //sinカーブで振動させる
                transform.position = defaultPos + Vector3.up * moveDis * Mathf.Sin(kakudo * Mathf.Deg2Rad);

                //途中からちっちゃくなる
                if (kakudo > 90.0f)
                {
                    transform.localScale = Vector3.one * (1 - ((kakudo - 90.0f) / 90.0f));
                }
                kakudo += 180.0f * Time.deltaTime * speed;
            }
            else
            {
                SceneManager.LoadScene("Clear");
                on = false;
            }
        }
    }
}

