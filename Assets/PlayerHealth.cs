using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.UI; // [추가 1] UI 사용을 위해 필수

public class PlayerHealth : NetworkBehaviour
{
    public readonly SyncVar<int> Health = new SyncVar<int>(100);

    // [추가 2] 슬라이더 연결 변수
    public Slider healthSlider;

    private void Awake()
    {
        Health.OnChange += OnHealthChanged;
    }

    private void OnDestroy()
    {
        Health.OnChange -= OnHealthChanged;
    }

    public void TakeDamage(int amount)
    {
        if (!base.IsServerStarted) return;

        Health.Value -= amount;

        if (Health.Value <= 0)
        {
            Despawn(gameObject);
        }
    }

    private void OnHealthChanged(int oldVal, int newVal, bool asServer)
    {
        // [추가 3] UI 갱신
        if (healthSlider != null)
        {
            healthSlider.value = (float)newVal / 100f; // 0.0 ~ 1.0 비율로 변환
        }
    }
}