using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Test Skill", order = 3)]
public class Skill_SO : ScriptableObject
{
    public string skillName;
    public float baseCooldown;

    public GameObject primarySkillPrefab;
}
