using RPG.UI;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] GameObject damageTextPrefab = null;
    CapsuleCollider capsuleCollider = null;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void SpawnText(int damage)
    {
        Vector3 heightOffset = new Vector3(0, capsuleCollider.height / 1.5f, 0);

        GameObject damageText_GO = Instantiate(damageTextPrefab, transform.position + heightOffset, Quaternion.identity);

        DamageText damageText = damageText_GO.GetComponent<DamageText>();
        damageText.Initialize(damage);
    }
}
