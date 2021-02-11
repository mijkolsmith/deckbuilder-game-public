using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public int playerHealth, playerMana;
    public Text HealthDisplay, ManaDisplay;

    private void Awake(){
        if(instance != null){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start(){
        HealthDisplay.text = playerHealth.ToString();
        ManaDisplay.text = playerMana.ToString();
    }
    public void UseMana(int value){
        playerMana -= value;
    }
    public void UseHealth(int value){
        playerHealth -= value;
    }
}
