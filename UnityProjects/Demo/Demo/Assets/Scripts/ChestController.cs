using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : Interactable
{
    [SerializeField]
    private ChestType type;
    private ItemRarity dropRarity;
    private DropChance dropChanceValues;
    [SerializeField]
    private int cost;
    public List<Item> lootPool = new List<Item>();
    private CharacterInfo playerStats;

    public override void Start()
    {
        base.Start();
        base.uses = 1;
        base.interactType = InteractableType.Chest;
        this.dropRarity = ItemRarity.None;

        //Check what chest rarity/type to calculate drop 
        switch(type){
            case ChestType.Common:
            dropChanceValues = new DropChance(80, 15, 4, 1, 0);
            //set base cost
            cost = 25;
            break;
            case ChestType.Heroic:
            dropChanceValues = new DropChance(0, 80, 15, 4, 1);
            cost = 50;
            break;
            case ChestType.Mythic:
            dropChanceValues = new DropChance(0, 0, 80, 18, 2);
            cost = 125;
            break;
            case ChestType.Legendary:
            dropChanceValues = new DropChance(0, 0, 0, 90, 10);
            cost = 225;
            break;
            case ChestType.Champion:
            dropChanceValues = new DropChance(0, 0, 0, 0, 100);
            cost = 350;
            break;
            default:
                Debug.LogWarning("Chest does not have a valid ChestType!");
                break;
        }
        SetCost();
    }

    public override void Update()
    {
        if(playerStats != null){
            IsInteractable();
        }
    }

    public override void Interact()
    {
        //Determine rarity of drop with DropChance Struct
        int randNum = GenerateValue(1, 101);
        GetDropRarity(randNum);
        if(this.dropRarity == ItemRarity.None){
            Debug.LogWarning("Chest Failure, No DropRarity Found!");
            return;
        }
        //Grab relavant loop pool
        //lootPool = LootPool.GetPool(enum ItemRarity);
        //Determine which item
        //int dropIndex = GenerateValue(0, n(n = number of elements -1));
        //Item loot = lootPool.get(dropIndex);
    }

    public override bool OnTriggerEnter(Collider other)
    {
        if(base.OnTriggerEnter(other)){
            playerStats = base.player.GetComponent<CharacterInfo>();
            IsInteractable();
        }
        return true;
    }

    public override void OnTriggerExit()
    {
        base.OnTriggerExit();
        playerStats = null;
    }

    public override bool IsInteractable()
    {
        if(base.IsInteractable()){
            if(playerStats.currentStats.Wallet >= this.cost){
                base.outline.enabled = true;
                base.isInteractable = true;
                GameEvents.Instance.InteractionUITriggerEnter();
                return true;
            }
        }else{
            base.outline.enabled = false;
            base.isInteractable = false;
            GameEvents.Instance.InteractionUITriggerExit();
        }
        return false;
    }

    private int GenerateValue(int min, int max){
        return Random.Range(min, max);
    }

    private void GetDropRarity(int randNum){
        //Common drop
        bool chance0 = false;
        if(dropChanceValues.CommonChance != 0){
            chance0 = true;
            if(randNum >= 1 && randNum <= dropChanceValues.CommonChance){
                
                dropRarity = ItemRarity.Common;
                Debug.Log(dropRarity + " : " + dropRarity);
                return;
            }
        }
        //Heroic drop
        if(dropChanceValues.HeroicChance != 0){
            if(!chance0){
                chance0 = true;
                if(randNum >= 1 && randNum <= dropChanceValues.HeroicChance){
                    
                    dropRarity = ItemRarity.Heroic;
                    Debug.Log(dropRarity + " : " + dropRarity);
                    return;
                }
            }else{
                if(randNum >= (dropChanceValues.CommonChance + 1) && 
                    randNum <= (dropChanceValues.Add(2))){
                    
                    dropRarity = ItemRarity.Heroic;
                    Debug.Log(dropRarity + " : " + dropRarity);
                    return;
                }
            }
        }
        //Mythic drop
        if(dropChanceValues.MythicChance != 0){
            if(!chance0){
                chance0 = true;
                if(randNum >=1 && randNum <= dropChanceValues.MythicChance){

                    dropRarity = ItemRarity.Mythic;
                    Debug.Log(dropRarity + " : " + dropRarity);
                    return;
                }
            }else{
                if(randNum >= (dropChanceValues.Add(2) + 1) &&
                    randNum <= (dropChanceValues.Add(3))){

                    dropRarity = ItemRarity.Mythic;
                    Debug.Log(dropRarity + " : " + dropRarity);
                    return;
                }
            }
        }
        //Legendary drop
        if(dropChanceValues.LegendaryChance != 0){
            if(!chance0){
                chance0 = true;
                if(randNum >= 1 && randNum <= dropChanceValues.LegendaryChance){

                    dropRarity = ItemRarity.Legendary;
                    Debug.Log(dropRarity + " : " + dropRarity);
                    return;
                }
            }else{
                if(randNum >= (dropChanceValues.Add(3) +1) && 
                    randNum <= (dropChanceValues.Add(4))){
                    
                    dropRarity = ItemRarity.Legendary;
                    Debug.Log(dropRarity + " : " + dropRarity);
                    return;
                }
            }
        }
        //Champion drop
        if(dropChanceValues.ChampionChance != 0){
            if(!chance0){
                chance0 = true;
                if(randNum >= 1 && randNum <= dropChanceValues.ChampionChance){

                    dropRarity = ItemRarity.Champion;
                    Debug.Log(dropRarity + " : " + dropRarity);
                    return;
                }
            }else{
                if(randNum >= (dropChanceValues.Add(4) + 1) &&
                    randNum <= (dropChanceValues.Add(5))){

                    dropRarity = ItemRarity.Champion;
                    Debug.Log(dropRarity + " : " + dropRarity);
                    return;
                }
            }
        }
        Debug.LogWarning("No DropRarity selected!");
    }

    private void SetCost()
    {
        int gameStage = GameManager.Instance.GetGameStage();
        float costModifier = 1;
        //If on level 1, cost modifier is 1, otherwise it is (1 + (level/3))^2
        if(gameStage > 1){
            costModifier = costModifier + (float)gameStage/3; 
            costModifier *= costModifier;  
        }
        cost = (int)(cost * costModifier);
    }

    private readonly struct DropChance{
        public int CommonChance { get; }
        public int HeroicChance { get; }
        public int MythicChance { get; }
        public int LegendaryChance { get; }
        public int ChampionChance { get; }
        
        public DropChance(int commonChance, int heroicChance, int mythicChacne, 
                                int legendaryChance, int championChance){
            CommonChance = commonChance;
            HeroicChance = heroicChance;
            MythicChance = mythicChacne;
            LegendaryChance = legendaryChance;
            ChampionChance = championChance;
        }

        public int Add(int num){
            switch(num){
                case 1:
                    return CommonChance;
                case 2:
                    return CommonChance + HeroicChance;
                case 3:
                    return CommonChance + HeroicChance + MythicChance;
                case 4:
                    return CommonChance + HeroicChance + MythicChance + LegendaryChance;
                case 5: 
                    return CommonChance + HeroicChance + MythicChance + LegendaryChance + ChampionChance;
                default:
                    Debug.LogWarning("Invalid Int in DropChance.Add()!");
                    return -1;
            }
        }
    }
}

