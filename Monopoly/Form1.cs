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
            SuspendLayout();
            Tile tile = new Tile("temp", 0, 0);
            for (int i = 0; i < board.tiles.Length; i++)
            {
                tile = board.tiles[i];
                this.tableLayoutPanel1.Controls.Add(tile.tileLayout, tile.column, tile.row);
                tile.tileLayout.ColumnCount = 1;
                tile.tileLayout.RowCount = 2;
                tile.tileLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
                tile.tileLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
                tile.tileLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
                tile.tileLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
                tile.tileLayout.Controls.Add(tile.header, 0, 0);
                tile.tileLayout.Controls.Add(tile.body, 0, 1);
                tile.tileLayout.Controls.Add(tile.footer, 0, 2);
                
                

            }    
            ResumeLayout(false);


            //gameManager.enterGameLoop();
        }

        class Tile
        {
            
            public System.Windows.Forms.TableLayoutPanel tileLayout;
            public System.Windows.Forms.Label body;
            public System.Windows.Forms.Label header;
            public System.Windows.Forms.Label footer = new System.Windows.Forms.Label();
            public String name;
            public int row;
            public int column;
            public System.Windows.Forms.Label TileName;
            public Tile(String name, int row, int column, Color? color = null)
            {
                this.name = name;
                this.row = row;
                this.column = column;
                body = new System.Windows.Forms.Label();
                header = new System.Windows.Forms.Label();
                tileLayout = new System.Windows.Forms.TableLayoutPanel();
                tileLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;

                header.BackColor = color ?? Color.Gray;
                header.Size = new Size(90, 90);
                header.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);

                body.Size = new Size(90, 90);
                body.BackColor = Color.White;
                body.TextAlign = ContentAlignment.TopCenter;
                body.Text = name;
                body.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);

                footer.Visible = false;
                
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
                body.BackColor = Color.Green;
            }

            public override void Land_On_Tile(Player player)
            {
                //bank pays player $200
            }
        }

        class GoToJail : Tile
        {
            public GoToJail(String name, int row, int column, Color color)
                : base(name, row, column, color)
            {

            }

            public override void Land_On_Tile(Player player)
            {
                // player jailed = true
                // player location set to (10, 0)
            }
        }

        class Tax : Tile
        {
            int taxAmount;
            public Tax(String name, int row, int column, Color color, int taxAmount)
                : base(name, row, column, color) 
            {
                this.taxAmount = taxAmount;
                footer.Text = "-$" + taxAmount;
                footer.TextAlign = ContentAlignment.TopCenter;
                footer.Visible = true;
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
                footer.TextAlign = ContentAlignment.TopCenter;
                footer.Text = "$" + price;
                footer.Visible = true;
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
                  new CommunityChest("Community Chest", 10, 8, Color.Gray),
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
                  new CommunityChest("Community Chest", 3, 0, Color.Gray),
                  new Land("Tennessee Ave", 2, 0, Color.Orange, bank, 180, 90, 100, new int[] {14, 70, 200, 550, 750}),
                  new Land("New York Ave", 1, 0, Color.Orange, bank, 200, 100, 100, new int[] {16, 80, 220, 600, 800}),
                  new Tile("Free Parking", 0, 0, Color.Gray),
                  new Land("Kentucky Ave", 0, 1, Color.Red, bank, 220, 110, 150, new int[] {18, 90, 250, 700, 875, 1050}),
                  new Chance("Chance", 0, 2, Color.Gray),
                  new Land("Indiana Ave", 0, 3, Color.Red, bank, 220, 110, 150, new int[] {18, 90, 250, 700, 875, 1050}),
                  new Land("Illinois Ave", 0, 4, Color.Red, bank, 240, 120, 150, new int[] {20, 100, 300, 750, 925, 1100}),
                  new Railroad("B&O Railroad", 0, 5, Color.Gray, bank, 200, 100),
                  new Land("Atlantic Ave", 0, 6, Color.Yellow, bank, 260, 130, 150, new int[] {22, 110, 330, 800, 975, 1150}),
                  new Land("Ventor Ave", 0, 7, Color.Yellow, bank, 260, 130, 150, new int[] {22, 110, 330, 800, 975, 1150}),
                  new Utility("Water Works", 0, 8, Color.Gray, bank, 150, 75),
                  new Land("Marvin Gardens", 0, 9, Color.Yellow, bank, 280, 140, 150, new int[] {24, 120, 360, 850, 1025, 1200}),
                  new GoToJail("Go To Jail", 0, 10, Color.Gray),
                  new Land("Pacific Ave", 1, 10, Color.Green, bank, 300, 150, 200, new int[] {26, 130, 390, 900, 1100, 1275} ),
                  new Land("North Carolina Ave", 2, 10, Color.Green, bank, 300, 150, 200, new int[] {26, 130, 390, 900, 1100, 1275} ),
                  new CommunityChest("Community Chest", 3, 10, Color.Gray),
                  new Land("Pennsylvania Ave", 4, 10, Color.Green, bank, 320, 160, 200, new int[] {28, 150, 450, 1000, 1200, 1400}),
                  new Railroad("Short Line", 5, 10, Color.Gray, bank, 200, 100),
                  new Chance("Chance", 6, 10, Color.Gray),
                  new Land("Park Place", 7, 10, Color.Blue, bank, 350, 175, 200, new int[] {35, 175, 500, 1100, 1300, 1500}),
                  new Tax("Luxury Tax", 8, 10, Color.Gray, 100),
                  new Land("Boardwalk", 9, 10, Color.Blue, bank, 400, 200, 200, new int[] {50, 200, 600, 1400, 1700, 2000})
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
