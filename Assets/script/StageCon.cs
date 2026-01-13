using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCtrl : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;
    [Header("コンティニュー位置")] public GameObject[] continuePoint;
    [Header("ゲームオーバー")] public GameObject gameOverObj;
    [Header("フェード")] public FadeImage fade;
    [Header("ゲームオーバー時に鳴らすSE")] public AudioClip gameOverSE;
    [Header("リトライ時に鳴らすSE")] public AudioClip retrySE;
    [Header("ステージクリアーSE")] public AudioClip stageClearSE;
    [Header("ステージクリア判定")] public PlayerTriggerCheck stageClearTrigger;

    private Player p;
    private int nextStageNum;
    private bool startFade = false;
    private bool doGameOver = false;
    private bool retryGame = false;
    private bool doSceneChange = false;
    private bool doClear = false;

    // Start is called before the first frame update
    void Start()
    {
        if (playerObj != null && continuePoint != null && continuePoint.Length > 0 && gameOverObj != null && fade != null)
        {
            gameOverObj.SetActive(false);
            playerObj.transform.position = continuePoint[0].transform.position;
            p = playerObj.GetComponent<Player>();
            if (p == null)
            {
                Debug.Log("プレイヤーじゃない物がアタッチされている");
            }
        }
        else
        {
            Debug.Log("設定が足りてない");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバー時の処理
        if (Gmanager.instance.isGameOver && !doGameOver)
        {
            gameOverObj.SetActive(true);
            Gmanager.instance.PlaySE(gameOverSE);
            doGameOver = true;
        }
        //プレイヤーがやられた時の処理
        else if (p != null && p.IsContinueWaiting() && !doGameOver)
        {
            if (continuePoint.Length > Gmanager.instance.continueNum)
            {
                playerObj.transform.position = continuePoint[Gmanager.instance.continueNum].transform.position;
                p.ContinuePlayer();
            }
            else
            {
                Debug.Log("コンティニューポイントの設定が足りてない");
            }
        }
        else if (stageClearTrigger != null && stageClearTrigger.isOn && !doGameOver && !doClear)
        {
            StageClear();
            doClear = true;
        }

        //ステージを切り替える
        if (fade != null && startFade && !doSceneChange)
        {
            if (fade.IsFadeOutComplete())
            {
                //ゲームリトライ
                if (retryGame)
                {
                    Gmanager.instance.RetryGame();
                }
                //次のステージ
                else
                {
                    Gmanager.instance.stageNum = nextStageNum;
                }
                Gmanager.instance.isStageClear = false;
                SceneManager.LoadScene("stage" + nextStageNum);
                doSceneChange = true;
                Gmanager.instance.continueNum = 0;
            }
        }
    }


    /// 最初から始める
    public void Retry()
    {
        Gmanager.instance.PlaySE(retrySE);
        ChangeScene(1); //最初のステージに戻るので１
        retryGame = true;
    }

    /// ステージを切り替える
    /// "num"ステージ番号
    public void ChangeScene(int num)
    {
        if (fade != null)
        {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
        }
    }
    /// ステージをクリアした
    public void StageClear()
    {
        Gmanager.instance.isStageClear = true;
        Gmanager.instance.PlaySE(stageClearSE);
    }
}

