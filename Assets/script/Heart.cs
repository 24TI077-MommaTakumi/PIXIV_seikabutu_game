using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{

    private Text heartText = null;
    private int oldHeartNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        heartText = GetComponent<Text>();
        if (Gmanager.instance != null)
        {
            heartText.text = "× " + Gmanager.instance.heartNum;
        }
        else
        {
            Debug.Log("ゲームマネージャー置き忘れてるよ！");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oldHeartNum != Gmanager.instance.heartNum)
        {
            heartText.text = "× " + Gmanager.instance.heartNum;
            oldHeartNum = Gmanager.instance.heartNum;
        }
    }
}
