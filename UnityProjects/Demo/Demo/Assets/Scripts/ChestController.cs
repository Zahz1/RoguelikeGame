using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : Interactable
{
    [SerializeField]
    private ChestType type;
    private DropChance dropChanceValues;
    private int cost;
    public List<Item> lootPool = new List<Item>();

    public override void Start()
    {
        base.Start();
        base.uses = 1;
        base.interactType = InteractableType.Chest;

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
        }
        SetCost();
    }

    public override void Interact()
    {
        
    }

    public override bool OnTriggerEnter(Collider other)
    {
        return true;
    }

    public override void OnTriggerExit()
    {
        
    }

    public override bool IsInteractable()
    {
        return true;
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
        private int CommonChance { get; }
        private int HeroicChance { get; }
        private int MythicChance { get; }
        private int LegendaryChance { get; }
        private int ChampionChance { get; }
        
        public DropChance(int commonChance, int heroicChance, int mythicChacne, 
                                int legendaryChance, int championChance){
            CommonChance = commonChance;
            HeroicChance = heroicChance;
            MythicChance = mythicChacne;
            LegendaryChance = legendaryChance;
            ChampionChance = championChance;
        }
    }
}

