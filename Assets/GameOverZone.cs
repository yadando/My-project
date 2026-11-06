using UnityEngine;
using UnityEngine.SceneManagement; // ←シーン（画面）を読み込み直すために必要です

public class GameOverZone : MonoBehaviour
{
    [Header("ここにリトライボタンを入れます")]
    public GameObject retryButton; 

    void Start()
    {
        // ゲーム開始時はボタンを隠しておく
        if (retryButton != null)
        {
            retryButton.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            Debug.Log("ゲームオーバー！！");
            Time.timeScale = 0f; // 時間を止める

            // ボタンを画面に表示する
            if (retryButton != null)
            {
                retryButton.SetActive(true);
            }
        }
    }

    // ボタンが押された時に呼び出される処理
    public void OnRetryButtonClicked()
    {
        Time.timeScale = 1f; // 時間の進みを元に戻す
        // 今遊んでいる画面（シーン）をもう一度最初から読み込み直す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}