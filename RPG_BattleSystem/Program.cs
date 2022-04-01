using System;
using System.Collections.Generic;

namespace RPG_BattleSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            //Objects and Lists
            List<Fighter> fighters = new List<Fighter>();
            Enemy e1 = new Enemy();
            Player p1 = new Player();

            //Set Fighter's opponent as each other, then add objects to a list of fighters
            e1.opponent = p1;
            p1.opponent = e1;
            fighters.Add(p1);
            fighters.Add(e1);

            while (p1.IsDead != true && e1.IsDead != true) //Continue to call take action until a fighter has lost
            {
                foreach (Fighter f in fighters)
                {
                    if (f.CurrentHealth > 0) { f.TakeAction(); } //call their overrided take action function as long as their health is above 0
                }
            }
            if (p1.IsDead == true) //display message the player lost if isDead data member is true
            {
                Console.WriteLine("The player has died - you lose!");
            }
            else //otherwise, the enemy lost
            {
                Console.WriteLine("The enemy has died - you WIN!");
            }

        }
    }

    //*BASE CLASS*
    public abstract class Fighter
    {
        //Data Members
        private int currentHealth;
        private int maxHealth;
        private bool isDead;
        private int attackDamage;
        public Fighter opponent; //holds the opponent the current object will fight against

        //Accessors
        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }
        public int AttackDamage
        {
            get { return attackDamage; }
            set { attackDamage = value; }
        }
        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
        }

        //Methods
        public void Death() 
        { 
            isDead = true;
            Console.WriteLine("Defeated!");
        }
        public void TakeDamage()
        {
            currentHealth -= opponent.AttackDamage; //currentHealth of the derived object is decremented by the opponents attack damage points
            if (currentHealth < 1) { Death(); } //if objects currentHealth is 0 or less the death base class function is called
        }
        public abstract void TakeAction(); //is abstract so derived classes can implement their own actions for the function
    }

    //*DERIVED CLASSES*
    public class Enemy : Fighter
    {
        //Constructor
        public Enemy()
        {
            CurrentHealth = 12; //Start enemy with 12 health points when object is created
            IsDead = false;
            AttackDamage = 5; //set enemy attack damage points to 5
        }

        //Methods
        public override void TakeAction() //Display fighters health and message indicating the enemy attacked the player
        {
            Console.WriteLine("=== Enemy turn ===");
            Console.WriteLine("Player's Health: " + opponent.CurrentHealth);
            Console.WriteLine("Enemy's Health: " + CurrentHealth);
            Console.WriteLine("Attacking player for " + AttackDamage + " points of damage!");
            opponent.TakeDamage(); //Call take damage base class function for the opponent
        }
    }

    public class Player : Fighter
    {
        //Data Members
        private int numPotions; //holds players number of health potions
        private int choice; //holds players menu choice

        //Constructor
        public Player() 
        {
            MaxHealth = 10; 
            CurrentHealth = MaxHealth; //Start player with it's max health when object is created
            IsDead = false;
            AttackDamage = 2; //assign 2 as players attack damage points
            numPotions = 3; //Start player with 3 health potions 
        }

        //Methods
        public override void TakeAction() //display fighters health and choice menu 
        {
            Console.WriteLine("=== Player turn ===");
            Console.WriteLine("Player's Health: " + CurrentHealth);
            Console.WriteLine("Enemy's Health: " + opponent.CurrentHealth);
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1 - Fight\n2 - Drink Potion (" + numPotions + " remaining)");
            
            //Read input into variable and assign to choice by parsing to an int
            string entered = Console.ReadLine();
            choice = int.Parse(entered);

            if (choice == 1) //Call attack function if player chooses 1
            { 
                AttackEnemy();
            }
            else //Otherwise, call drink potion function
            {
                DrinkPotion();
            }

        }
        public void DrinkPotion()
        {
            if (numPotions > 0) //execute statements if Player isn't out of potions
            {
                if (CurrentHealth >= 3) { CurrentHealth = MaxHealth; } //set to maxhealth to not exceed 10 since potion adds 7 points
                else { CurrentHealth += 7; } //otherwise, add 7 to CurrentHealth
                --numPotions; //decrement Players number of potions
                Console.WriteLine("Restored 7 points of health");
            }
            else //otherwise, let Player know fight option will execute instead and call Attack function
            {
                Console.WriteLine("You have no remaining potions, the Fight option will execute.");
                AttackEnemy();
            }
        }
        public void AttackEnemy() //call take damage base class function for the opponent
        {
            opponent.TakeDamage();
            Console.WriteLine("Attacked the enemy for " + AttackDamage + " points of damage!");
        }
    }
}