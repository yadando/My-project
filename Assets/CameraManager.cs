using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera cam;
    private float baseSize;
    private float baseY;

    void Start()
    {
        cam = GetComponent<Camera>();
        baseSize = cam.orthographicSize; // 最初のカメラサイズ
        baseY = transform.position.y;    // 最初のカメラの高さ
    }

    void Update()
    {
        // "Photo"という目印（タグ）がついた写真をすべて探す
        GameObject[] photos = GameObject.FindGameObjectsWithTag("Photo");
        float maxY = -10f;

        foreach (GameObject p in photos)
        {
            // 落とした後（重力がオンになっている）の写真だけを計算に入れる
            if (p.GetComponent<Rigidbody2D>().gravityScale > 0)
            {
                if (p.transform.position.y > maxY)
                {
                    maxY = p.transform.position.y; // 一番高い写真のY座標を記録
                }
            }
        }

        // 写真が一定の高さ（例：2.0）を超えたら、目標とするカメラサイズを大きくする
        float targetSize = baseSize;
        if (maxY > 2f)
        {
            targetSize = baseSize + (maxY - 2f) * 0.6f;
        }

        // 滑らかにズームアウトさせる
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * 3f);

        // ズームアウトしても画面の下（床）が見切れず、常に同じ位置に見えるようにカメラを上へ動かす
        float targetY = baseY + (cam.orthographicSize - baseSize);
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}