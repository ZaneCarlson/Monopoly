using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monopoly
{
    
    public partial class Form1 : Form
    {
        public int NUM_OF_PLAYERS;

        public Form1()
        {
            NUM_OF_PLAYERS = 4;
            Player bank = new Player();
            Board board = new Board(bank);
            GameManager gameManager = new GameManager(NUM_OF_PLAYERS);
            InitializeComponent();

            for (int i = 0; i < board.tiles.Length; i++)
            {
                this.tableLayoutPanel1.Controls.Add(board.tiles[i].button, board.tiles[i].column, board.tiles[i].row);
            }    


            //gameManager.enterGameLoop();
        }

        class Tile
        {
            public System.Windows.Forms.Button button;
            public String name;
            public int row;
            public int column;
            public System.Windows.Forms.Label TileName;
            public Tile(String name, int row, int column, Color? color = null)
            {
                this.name = name;
                this.row = row;
                this.column = column;
                button = new System.Windows.Forms.Button();
                button.BackColor = color ?? Color.Gray;
                button.Size = new Size(73, 73);
                button.Text = name;
            }

            public virtual void Land_On_Tile(Player player)
            {
                ;
            }
        }

        class GO : Tile
        {
            public GO(String name, int row, int column, Color color)
                : base(name, row, column, color)
            {

            }

            public override void Land_On_Tile(Player player)
            {
                //bank pays player $200
            }
        }

        class Tax : Tile
        {
            int taxAmount;
            public Tax(String name, int row, int column, Color color, int taxAmount)
                : base(name, row, column, color) 
            {
                this.taxAmount = taxAmount;
                //Tax Constructor
            }
            public override void Land_On_Tile(Player player)
            {
                //passed in player pays bank the tax amount
            }
        }

        class CommunityChest : Tile
        {
            public CommunityChest(String name, int row, int column, Color color)
                : base(name, row, column, color)
            {

            }
            public override void Land_On_Tile(Player player)
            {
                //passed in player draws a CC card
            }
        }

        class Chance : Tile
        {
            public Chance(String name, int row, int column, Color color)
                : base(name, row, column, color)
            {

            }
            public override void Land_On_Tile(Player player)
            {
                //passed in player draws a chance card
            }
        }

        class Contract : Tile
        {
            Player owner;
            int price;
            int mortgageValue;
            public Contract(String name, int row, int column, Color color, Player owner, int price, int mortgageValue)
                : base(name, row, column, color)
            {
                this.owner = owner;
                this.price = price;
                this.mortgageValue = mortgageValue;
                //Contract Constructor
            }
        }

        class Land : Contract
        {
            int buildPrice;
            int[] incomes;
            public Land(String name, int row, int column, Color color, Player owner, int price, int mortgageValue, int buildPrice, int[] incomes)
                :base(name, row, column, color, owner, price, mortgageValue)
            {
                this.buildPrice = buildPrice;
                this.incomes = incomes;
                //Land Constructor
            }

            public override void Land_On_Tile(Player player)
            {
                //if owner == bank then let Player buy land.
                //if player land != owner, then pay income to owner from passed in player.
            }
        }

        class Railroad : Contract
        {
            int income = 25;
            public Railroad(String name, int row, int column, Color color, Player owner, int price, int mortgageValue)
                : base(name, row, column, color, owner, price, mortgageValue)
            {
                
            }

            public override void Land_On_Tile(Player player)
            {
                //if owner == bank then let Player buy Railroad.
                //if player != owner then pay based on number of railroads owner owns
                //formula for rent: 25 x 2^(n-1) where n = number of railroads owner owns.
            }
        }

        class Utility : Contract
        {
            public Utility(String name, int row, int column, Color color, Player owner, int price, int mortgageValue)
                : base(name, row, column, color, owner, price, mortgageValue)
            {
                
            }

            public override void Land_On_Tile(Player player)
            {
                //if owner == bank then let Player buy utility.
                //if player != owner then pay based on number of utilities owner owns
                //formula for rent: 4 x dice roll
                //if two owned then 10 x dice roll
            }
        }
        class Board
        {
            public Tile[] tiles;

            public Board(Player bank)
            {
                /* Tile Constructor(NAME, ROW, COLUMN, COLOR)
                 * Tax  Constructor(NAME, ROW, COLUMN, COLOR, TAX_AMOUNT)
                 * Land Constructor(NAME, ROW, COLUMN, COLOR, PLAYER, PRICE, MORTGAGE, BUILD_PRICE, INCOMES[])
                 *
                 * 
                 */
                tiles = new Tile[]
                { new GO("GO", 10, 10, Color.Green),
                  new Land("Mediterranian Ave", 10, 9, Color.Indigo , bank, 60, 30, 50, new int[] {2, 10, 30, 90, 160, 250}),
                  new CommunityChest("CommunityChest", 10, 8, Color.Gray),
                  new Land("Baltic Ave", 10, 7, Color.Indigo, bank, 60, 30, 50, new int[] {4, 20, 60, 180, 320, 450}),
                  new Tax("Income Tax", 10, 6, Color.Gray, 200),
                  new Railroad("Reading Railroad", 10, 5, Color.Gray, bank, 200, 100),
                  new Land("Oriental Ave", 10, 4, Color.LightBlue, bank, 100, 50, 50, new int[] {6, 30, 90, 270, 400, 550}),
                  new Chance("Chance", 10, 3, Color.Gray),
                  new Land("Vermont Ave", 10, 2, Color.LightBlue, bank, 100, 50, 50, new int[] {6, 30, 90, 270, 400, 550}),
                  new Land("Connecticut Ave", 10, 1, Color.LightBlue, bank, 120, 60, 50, new int[] {8, 40, 100,300, 450, 600}),
                  new Tile("Jail", 10, 0),
                  new Land("New Charles Ave", 9, 0, Color.Pink, bank, 140, 70, 100, new int[] {10, 50, 150, 450, 625, 750}),
                  new Utility("Electric Company", 8, 0, Color.Gray, bank, 150, 75),
                  new Land("State Ave", 7, 0, Color.Pink, bank, 140, 70, 100, new int[] {10, 50, 150, 450, 625, 750}),
                  new Land("Virgina Ave", 6, 0, Color.Pink, bank, 160, 80, 100, new int[] {12, 60, 180, 500, 700, 900}),
                  new Railroad("Pennsylvania Railroad", 5, 0, Color.Gray, bank, 200, 100),
                  new Land("St. James Place", 4, 0, Color.Orange, bank, 180, 90, 100, new int[] {14, 70, 200, 550, 750}),
                  new CommunityChest("CommunityChest", 3, 0, Color.Gray),
                  new Land("Tennessee Ave", 2, 0, Color.Orange, bank, 180, 90, 100, new int[] {14, 70, 200, 550, 750}),
                  new Land("New York Ave", 1, 0, Color.Orange, bank, 200, 100, 100, new int[] {16, 80, 220, 600, 800}),
                  new Tile("Free Parking", 0, 0, Color.Gray)
                };
            }

        }

        class Player
        {
           
        }


        class GameManager
        {
            public Player[] players;
            public GameManager(int NUM_OF_PLAYERS )
            {
                players = new Player[NUM_OF_PLAYERS];
            }
        }
    }


   

}
