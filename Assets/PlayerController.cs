using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 5f;

    public GameObject bulletPrefab;

    // [변경점 1] [SyncVar] 어트리뷰트 대신 SyncVar<T> 타입 사용
    public readonly SyncVar<Color> NetColor = new SyncVar<Color>();

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        // [변경점 2] 값이 바뀔 때 실행될 함수를 여기서 구독(연결)함
        NetColor.OnChange += OnColorChanged;
    }

    private void OnDestroy()
    {
        // [변경점 3] 객체가 사라질 때 구독 해제 (안전장치)
        NetColor.OnChange -= OnColorChanged;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (_meshRenderer != null)
            _meshRenderer.material.color = NetColor.Value; // .Value로 값 접근
    }

    private void Update()
    {
        if (!base.IsOwner) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, 0, v) * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ServerChangeColor();
        }

        // [추가 2] 발사 입력
        if (Input.GetKeyDown(KeyCode.F))
        {
            ServerFire();
        }
    }

    [ServerRpc]
    private void ServerChangeColor()
    {
        Color newColor = new Color(Random.value, Random.value, Random.value);

        // [변경점 4] 값을 넣을 때도 .Value에 넣으면 자동으로 동기화됨
        NetColor.Value = newColor;
    }

    [ServerRpc]
    private void ServerFire()
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation);

        // [추가] 스폰하기 전에 "이거 내가 쏜 거야"라고 이름표 붙이기
        // OwnerId는 FishNet이 플레이어에게 부여한 고유 번호야.
        spawnedBullet.GetComponent<BulletScript>().shooterId = base.OwnerId;

        // [수정] base.Owner 제거 -> 이러면 서버 소유가 됨 -> NetworkTransform이 서버 위치를 받아옴
        base.Spawn(spawnedBullet);
    }

    // SyncVar<T>의 OnChange 함수 형태
    private void OnColorChanged(Color oldColor, Color newColor, bool asServer)
    {
        if (_meshRenderer != null)
            _meshRenderer.material.color = newColor;
    }
}