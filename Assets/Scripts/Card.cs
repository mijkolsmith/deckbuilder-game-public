using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCard", menuName = "Card")]
public class Card : ScriptableObject {

    public new string name;
    public string Description;
    public Sprite Image;
    public int Mana, AttackDamage, Health;

    public void DoAttack(){
        //Execute attack
    }

    public void DoHeal(){
        //Execute heal
    }
}
