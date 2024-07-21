using UnityEngine;

[CreateAssetMenu(fileName = "New Artifact", menuName = "Artifact")]
public class Artifact : ScriptableObject
{
    public string artifactName;
    public Sprite icon;
    public float damageBonus;
    public float healthBonus;
    public float critRateBonus;
    public float critDamageBonus;
    public bool isEquipped;

    public void Equip()
    {
        isEquipped = true;
    }

    public void Unequip()
    {
        isEquipped = false;
    }
}
