using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public static CardController instance;
    public GameObject CardPrefab, CardSpawnPoint;
    public List<Card> AllCardProfiles;
    private void Awake(){
        if(instance != null){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Start(){
        //SpawnCard();
    }
    public void SpawnCard(){
        for(int i=0;i<AllCardProfiles.Count; i++){
            GameObject myCard;
            myCard = Instantiate(CardPrefab, CardSpawnPoint.transform);
            myCard.GetComponent<CardTemplate>().card = AllCardProfiles[i];
        }
    }

    public void BuyCard(){
        int randomvalue;
        GameObject myCard;
        randomvalue = Random.Range(0, AllCardProfiles.Count);
        myCard = Instantiate(CardPrefab, CardSpawnPoint.transform);
        myCard.GetComponent<CardTemplate>().card = AllCardProfiles[randomvalue];
    }
}
