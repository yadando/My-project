using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; 

public class PhotoDropper : MonoBehaviour
{
    [Header("落とす写真のプレハブを複数入れてください")]
    public GameObject[] photoPrefabs; 
    
    [Header("ここにスコアの文字(ScoreText)を入れます")]
    public TextMeshProUGUI scoreText;
    
    private GameObject currentPhoto;
    private int score = 0;

    // ★画面をタッチ（ドラッグ）して操作中かどうかを判定する変数
    private bool isDragging = false;

    void Start()
    {
        Time.timeScale = 1f;
        UpdateScoreDisplay();
        SpawnNextPhoto();
    }

    void Update()
    {
        if (currentPhoto != null)
        {
            float spawnY = Camera.main.transform.position.y + Camera.main.orthographicSize - 1.5f;

            // UI（回転ボタンなど）の上をタッチしているかチェック
            bool isOverUI = false;
            if (EventSystem.current.IsPointerOverGameObject()) isOverUI = true;
            if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) isOverUI = true;

            // 1. 画面に指が触れた瞬間
            if (Input.GetMouseButtonDown(0))
            {
                // 回転ボタンの上じゃなければ、操作（ドラッグ）開始！
                if (!isOverUI)
                {
                    isDragging = true;
                }
            }

            // 2. 画面に指が触れている間（スワイプ中）
            if (isDragging && Input.GetMouseButton(0))
            {
                // 指のX座標に合わせて写真を動かす
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentPhoto.transform.position = new Vector3(mousePos.x, spawnY, 0);
            }
            else if (!isDragging)
            {
                // タッチされるまでは、画面の真ん中で待機させる
                currentPhoto.transform.position = new Vector3(0, spawnY, 0);
            }

            // PC用の回転処理（残しておいてOK）
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
            {
                RotatePhoto();
            }

            // 3. 画面から指を離した瞬間（落とす）
            if (Input.GetMouseButtonUp(0))
            {
                // 操作中だった場合のみ落とす
                if (isDragging)
                {
                    currentPhoto.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
                    currentPhoto = null;
                    isDragging = false; // 操作終了
                    
                    score++; 
                    UpdateScoreDisplay(); 

                    Invoke("SpawnNextPhoto", 1.5f);
                }
            }
        }
    }

    // ボタンから呼び出せる回転処理
    public void RotatePhoto()
    {
        if (currentPhoto != null)
        {
            currentPhoto.transform.Rotate(0, 0, 30f);
        }
    }

    void SpawnNextPhoto()
    {
        int randomIndex = Random.Range(0, photoPrefabs.Length);
        GameObject selectedPrefab = photoPrefabs[randomIndex];
        float spawnY = Camera.main.transform.position.y + Camera.main.orthographicSize - 1.5f;
        
        // 最初は真ん中(X=0)に出現させる
        currentPhoto = Instantiate(selectedPrefab, new Vector3(0, spawnY, 0), Quaternion.identity);
        currentPhoto.GetComponent<Rigidbody2D>().gravityScale = 0f;
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + score*100;
        }
    }
}