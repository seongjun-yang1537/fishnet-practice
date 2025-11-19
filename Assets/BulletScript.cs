using FishNet.Object;
using UnityEngine;

public class BulletScript : NetworkBehaviour
{
    public float speed = 10f;

    // [추가] 누가 쐈는지 기억하는 변수
    public int shooterId = -1;

    private void Update()
    {
        if (!base.IsServerStarted) return;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Invoke(nameof(DestroyBullet), 3f);
    }

    private void DestroyBullet()
    {
        if (base.IsServerStarted)
            base.Despawn(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!base.IsServerStarted) return;

        if (other.TryGetComponent<NetworkObject>(out NetworkObject otherNob))
        {
            // [수정] 주인(Owner) 대신 shooterId로 비교
            if (otherNob.OwnerId == shooterId) return;
        }

        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth health))
        {
            health.TakeDamage(10);
            base.Despawn(gameObject);
        }
    }
}