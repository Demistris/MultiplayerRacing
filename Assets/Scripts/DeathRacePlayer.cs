using UnityEngine;

[CreateAssetMenu(fileName = "New Death Race Player")]
public class DeathRacePlayer : ScriptableObject
{
    public string PlayerName;
    public Sprite PlayerSprite;

    [Header("Weapon Properties")]
    public string Weapon;
    public float Damage;
    public float FireRate;
    public float BulletSpeed;
}
