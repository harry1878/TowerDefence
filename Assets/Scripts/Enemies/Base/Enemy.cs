using UnityEngine;
using UnityEngine.UI;

public class Enemy : EnemyTransform
{
    public int Prize;
    public float MaxHp;
    private float currentHp;

    public RectTransform HealthBar { get; set; } = null;
    public Image ProgressBar { get; set; } = null;

    public void SetParameter(int index)
    {
        MaxHp = MaxHp * index;
        Speed = Speed * index/2;
        Prize = Prize + Mathf.FloorToInt(index / 2f);

        currentHp = MaxHp;
    }

    public void OnHit(float damage)
    {
        if (ProgressBar == null)
            ProgressBar = HealthBar.GetChild(0).GetChild(0).GetComponent<Image>();

        currentHp -= damage;
        ProgressBar.fillAmount = currentHp / MaxHp;

        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private Vector3 FlexibleVector = new Vector3(-0.5f, 2f, 0f);
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (HealthBar == null) return;
        HealthBar.position = transform.position + FlexibleVector;
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        if (GameManager.Get == null) return;
        if (GameManager.Get.moneyText == null) return;
#endif

        SpawnManager.Get.DestroyEnemy(this);

        if (HealthBar != null)
            Destroy(HealthBar.gameObject);

        if (currentHp <= 0)
        GameManager.Get.Money += Prize;
    }
}
