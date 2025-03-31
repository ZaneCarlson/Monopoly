using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            NUM_OF_PLAYERS = 2;
            Player bank = new Player(99);
            bank.money = 100000000;
            Board board = new Board(bank);
            GameManager gameManager = new GameManager(NUM_OF_PLAYERS, board);
            

            InitializeComponent();
            SuspendLayout();
            Tile tile = new Tile("temp", 0, 0);
            this.tableLayoutPanel1.Controls.Add(gameManager.rollButton, 5, 5);
            this.tableLayoutPanel1.Controls.Add(gameManager.PlayerInfoButton, 5, 4);
            this.tableLayoutPanel1.Controls.Add(gameManager.showDiceRoll, 5, 6);
            for (int i = 0; i < board.tiles.Length; i++)
            {
                tile = board.tiles[i];
                this.tableLayoutPanel1.Controls.Add(tile.tileLayout, tile.column, tile.row);
                tile.tileLayout.ColumnCount = 1;
                tile.tileLayout.RowCount = 4;
                tile.tileLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
                tile.tileLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
                tile.tileLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
                tile.tileLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
                tile.tileLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));

                tile.tileLayout.Controls.Add(tile.header, 0, 0);
                tile.tileLayout.Controls.Add(tile.body, 0, 1);
                tile.tileLayout.Controls.Add(tile.playerSlots, 0, 3);
                tile.tileLayout.Controls.Add(tile.footer, 0, 2);
                

                tile.playerSlots.ColumnCount = 4;
                tile.playerSlots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
                tile.playerSlots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
                tile.playerSlots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
                tile.playerSlots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
                tile.playerSlots.RowCount = 1;
                tile.playerSlots.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

                tile.playerSlots.Controls.Add(tile.playerSlot[0], 0, 0);
                tile.playerSlots.Controls.Add(tile.playerSlot[1], 1, 0);
                tile.playerSlots.Controls.Add(tile.playerSlot[2], 2, 0);
                tile.playerSlots.Controls.Add(tile.playerSlot[3], 3, 0);
            }    
            ResumeLayout(false);
        }

        class Tile
        { 
            public System.Windows.Forms.TableLayoutPanel tileLayout;
            public System.Windows.Forms.TableLayoutPanel playerSlots;
            public System.Windows.Forms.Label body;
            public System.Windows.Forms.Label header;
            public System.Windows.Forms.Label footer = new System.Windows.Forms.Label();
            public System.Windows.Forms.Label[] playerSlot = { new System.Windows.Forms.Label(), new System.Windows.Forms.Label(), new System.Windows.Forms.Label(), new System.Windows.Forms.Label() };
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
                tileLayout.Margin = System.Windows.Forms.Padding.Empty;

                playerSlots = new System.Windows.Forms.TableLayoutPanel();
                playerSlots.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
                playerSlots.Margin = System.Windows.Forms.Padding.Empty;
                for( int i = 0; i < playerSlot.Length; i++ )
                {
                    playerSlot[i].Size = new Size(13, 5);
                    playerSlot[i].Visible = false;
                }
                playerSlot[0].BackColor = Color.Blue;
                playerSlot[1].BackColor = Color.Green;
                playerSlot[2].BackColor = Color.Red;
                playerSlot[3].BackColor = Color.Yellow;

                header.BackColor = color ?? Color.Gray;
                header.Size = new Size(90, 90);
                header.Margin = System.Windows.Forms.Padding.Empty;

                body.Size = new Size(90, 90);
                body.BackColor = Color.White;
                body.TextAlign = ContentAlignment.TopCenter;
                body.Text = name;
                body.Margin = System.Windows.Forms.Padding.Empty;


                footer.Visible = false;
                
            }

            public virtual void Land_On_Tile(Player player, GameManager gameManager)
            {
                Console.WriteLine("Player " + player.id + " Landed on " + name + "!");
            }
        }

        class GO : Tile
        {
            public GO(String name, int row, int column, Color color)
                : base(name, row, column, color)
            {
                body.BackColor = Color.Green;
            }

            public override void Land_On_Tile(Player player, GameManager gameManager)
            {
                Console.WriteLine("Player " + player.id + " Landed on " + name + "!" + " They earned a $200 bonus!");
                player.money += 200;
                Console.WriteLine(player.money);
            }
        }

        class GoToJail : Tile
        {
            public GoToJail(String name, int row, int column, Color color)
                : base(name, row, column, color)
            {

            }

            public override void Land_On_Tile(Player player, GameManager gameManager)
            {
                Console.WriteLine("Player " + player.id + " Landed on " + name + "!");
                // player jailed = true
                // player location set to (10, 0)
                player.location.playerSlot[player.id].Visible = false;
                player.location = gameManager.board.tiles[10];
                player.location.playerSlot[player.id].Visible = true;
                player.isJailed = true;
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
            public override void Land_On_Tile(Player player, GameManager gameManager)
            {
                Console.WriteLine("Player " + player.id + " Landed on " + name + "!");
                //passed in player pays bank the tax amount
            }
        }

        class CommunityChest : Tile
        {
            public CommunityChest(String name, int row, int column, Color color)
                : base(name, row, column, color)
            {

            }
            public override void Land_On_Tile(Player player, GameManager gameManager)
            {
                Console.WriteLine("Player " + player.id + " Landed on " + name + "!");
                //passed in player draws a CC card
            }
        }

        class Chance : Tile
        {
            public Chance(String name, int row, int column, Color color)
                : base(name, row, column, color)
            {

            }
            public override void Land_On_Tile(Player player, GameManager gameManager)
            {
                Console.WriteLine("Player " + player.id + " Landed on " + name + "!");
                //passed in player draws a chance card
            }
        }

        class Contract : Tile
        {
            public Player owner;               // I SET THIS TO PUBLIC
            public int price;               // I SET THIS TO PUBLIC
            public int mortgageValue;       // I SET THIS TO PUBLIC
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

            public override void Land_On_Tile(Player player, GameManager gameManager)
            {
                Console.WriteLine("Player " + player.id + " Landed on " + name + "!");


                if (owner.id == 99)
                {
                    //player can buy land
                    Console.WriteLine("owned by bank");
                    Console.WriteLine(name);

                    player.CanBuyProperty.Add(this);
                    gameManager.PlayerInfo(player, name);
                    

                }
                else
                {
                    Console.WriteLine($"owned by player: {owner.id}");
                }

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

            public override void Land_On_Tile(Player player, GameManager gameManager)
            {
                Console.WriteLine("Player " + player.id + " Landed on " + name + "!");

                if (owner.id == 99)
                {
                    //player can buy land
                    Console.WriteLine("owned by bank");
                    Console.WriteLine(name);

                    player.CanBuyProperty.Add(this);
                    gameManager.PlayerInfo(player, name);
                    

                }
                else
                {
                    Console.WriteLine($"owned by player: {owner.id}");
                }

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

            public override void Land_On_Tile(Player player, GameManager gameManager)
            {
                Console.WriteLine("Player " + player.id + " Landed on " + name + "!");

                if (owner.id == 99)
                {
                    //player can buy land
                    Console.WriteLine("owned by bank");
                    Console.WriteLine(name);

                    

                    player.CanBuyProperty.Add(this);
                    gameManager.PlayerInfo(player, name);

                    


                }
                else
                {
                    Console.WriteLine($"owned by player: {owner.id}");
                }

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
                  new Land("Virginia Ave", 6, 0, Color.Pink, bank, 160, 80, 100, new int[] {12, 60, 180, 500, 700, 900}),
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
           public Tile location;
           public int id;
           public int money;
           public bool isJailed;
           public int jailTimer; 
           public bool bankrupt;
           public List<Contract> ownedProperties = new List<Contract>();    // List of owned properties
            public List<Contract> CanBuyProperty = new List<Contract>(); 
            public Player(int id)
            {
                this.id = id;  
                isJailed = false;
                bankrupt = false;
            }


        }

        class GameManager
        {
            public Player[] players;
            public Player payToPlayer;
            public int payment;
            int i = 0;
            Random random;
            public Board board;
            public Button rollButton;
            public Button PlayerInfoButton;
            public Label showDiceRoll; // 
            int doublesCounter = 0;

            public GameManager(int NUM_OF_PLAYERS, Board board)
            {
                this.board = board;
                random = new Random();
                rollButton = new Button();
                rollButton.Text = "Roll Dice";
                rollButton.Click += new System.EventHandler(this.RollDice);
                rollButton.Size = new Size(80, 80);
                rollButton.BackColor = Color.Aquamarine;


                showDiceRoll = new Label();
                showDiceRoll.Text = "";
                showDiceRoll.Size = new Size(80, 50);
                //showDiceRoll.BackColor = Color.DarkGray;


                PlayerInfoButton = new Button();                                    // Create a new button for player info
                PlayerInfoButton.Text = "Player Info";                              // Button text
                PlayerInfoButton.Click += new System.EventHandler(this.PlayerInfo); // Calls the PlayerInfo method when clicked 
                PlayerInfoButton.Size = new Size(80, 30);                           // Size of button 
                PlayerInfoButton.BackColor = Color.DarkGray;                        // Color of button
                PlayerInfoButton.Dock = DockStyle.Bottom;                           // Dock button to bottom of cell to be right above roll button

                players = new Player[NUM_OF_PLAYERS];
                for (int i = 0; i < NUM_OF_PLAYERS; i++)
                {
                    board.tiles[0].playerSlot[i].Visible = true;
                    players[i] = new Player(i);
                    players[i].location = board.tiles[0];
                    players[i].money = 1500;                // set players starting money to game standard of $1500
                    //players[i].location.Land_On_Tile(players[i], this);k
                }
            }

            // Player Info Button when the button is pressed
            private void PlayerInfo(object sender, EventArgs e)
            {
                Player currentPlayer = players[i];
                ShowPlayerInfo(currentPlayer, null, false);
                
            }

            // Player Info Button that is trigged by player movement
            public void PlayerInfo(Player player, string name)
            {
                ShowPlayerInfo(player, name, false);
            }

            // Player Interface UI
            private void ShowPlayerInfo(Player player, string name, bool paying)
            {
                //display player info

                rollButton.Enabled = false;
                PlayerInfoButton.Enabled = false;

                //
                Form popup = new Form();                                   // Create a new form for the player info UI popup
                popup.Text = "Player Info";                                // Button text
                popup.Size = new Size(1000, 900);                           // Good size for player info, little smaller than board form
                popup.StartPosition = FormStartPosition.CenterScreen;      // Center the form on the screen
                popup.FormBorderStyle = FormBorderStyle.FixedDialog;
                popup.ControlBox = false;

                //
                TableLayoutPanel playerInfoTable = new TableLayoutPanel(); // Create a new table layout panel for the player info
                playerInfoTable.Dock = DockStyle.Fill;                     // Dock the table layout panel to fill the form
                playerInfoTable.RowCount = 4;                              // Set the row count to the number of players + 1 for the header
                playerInfoTable.ColumnCount = 3;                           // Set the column count to 3 for player id, location, and money

                //
                playerInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
                playerInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
                playerInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));

                // Add row heights
                for (int r = 0; r < playerInfoTable.RowCount; r++){playerInfoTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));}

                //
                Label playerID = new Label();                               // Create a new label for the player id
                playerID.Text = $"Player: #{player.id}";                // Set the text of the label to the players id
                playerID.Font = new Font("Arial", 30, FontStyle.Bold);      // Set the font of the label
                playerID.Dock = DockStyle.Fill;                             // Dock the label to fill the cell
                playerInfoTable.Controls.Add(playerID, 0, 0);               // Add the label to the table layout panel at row 0, column 0
                playerInfoTable.SetColumnSpan(playerID, 2);                 // Set the column span of the label to 3     

                // Button to close the player UI form
                if (!paying)
                {
                    Button Exit = new Button();
                    Exit.Text = "Exit";
                    Exit.Dock = DockStyle.Fill;
                    Exit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    Exit.Size = new Size(80, 40);                                // Adjust width & height

                    Exit.Font = new Font("Arial", 14, FontStyle.Bold);

                    Exit.Click += (s, ev) =>
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            DialogResult result = MessageBox.Show(
                                                        "Are you sure? This will end your turn",
                                                        "NOTICE",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question
                                                    );
                            if (result == DialogResult.Yes)
                            {
                                PlayerInfoButton.Enabled = true;
                                rollButton.Enabled = true;
                                player.CanBuyProperty.Clear();

                                popup.Close();
                            }
                        }
                        else
                        {
                            PlayerInfoButton.Enabled = true;
                            rollButton.Enabled = true;
                            player.CanBuyProperty.Clear();
                            popup.Close();
                        }
                    };
                    playerInfoTable.Controls.Add(Exit, 2, 0);
                }
                
                



                //
                Label currentPlayerMoney = new Label();
                currentPlayerMoney.Text = $"Balance: ${player.money}";  // Set the text of the label to the players money count
                currentPlayerMoney.Font = new Font("Arial", 20, FontStyle.Bold);    // Set the font of the label
                currentPlayerMoney.Dock = DockStyle.Fill;                           // Dock the label to fill the cell
                currentPlayerMoney.TextAlign = ContentAlignment.MiddleLeft;         // Center the text in the label
                playerInfoTable.SetColumnSpan(currentPlayerMoney, 2);               // Set the column span of the label to 3
                playerInfoTable.Controls.Add(currentPlayerMoney, 0, 1);             // Add the label to the table layout panel at row 0, column 0


                /* PAYMENT AND BANKRUPTCY BUTTONS*/
                TableLayoutPanel actionButtons = new TableLayoutPanel();
                if (paying)
                {
                    actionButtons.ColumnCount = 2;
                    actionButtons.Dock = DockStyle.Fill;
                    actionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    actionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    Button paymentButton = new Button();
                    paymentButton.Text = "Payment";
                    paymentButton.Font = new Font("Arial", 14, FontStyle.Bold);
                    paymentButton.Dock = DockStyle.Fill;
                    paymentButton.Click += (s, ev) => 
                    { 
                        if(player.money < payment)
                        {
                            DialogResult notEnoughMoney = MessageBox.Show("not enough money! Sell houses, mortgage properties, or declare bankruptcy.");
                        }
                        else
                        {
                            player.money -= payment;
                            payToPlayer.money += payment;
                            DialogResult notEnoughMoney = MessageBox.Show($"Player {player.id} paid ${payment} to Player {payToPlayer.id}");
                            popup.Close();
                        }
                    };
                    Button bankruptcyButton = new Button();
                    bankruptcyButton.Text = "Bankruptcy";
                    bankruptcyButton.Font = new Font("Arial", 14, FontStyle.Bold);
                    bankruptcyButton.Dock = DockStyle.Fill;
                    bankruptcyButton.Click += (s, ev) => MessageBox.Show("Bankruptcy clicked!");
                    actionButtons.Controls.Add(paymentButton, 0, 0);
                    actionButtons.Controls.Add(bankruptcyButton, 1, 0);
                }
                /* PAYMENT AND BANKRUPTCY BUTTONS END*/


                /* PROPERIES SECTION */
                // === PROPERTY SCROLLABLE PANEL ===
                Panel scrollPanel = new Panel();
                scrollPanel.Dock = DockStyle.Fill;
                scrollPanel.AutoScroll = true;
               

                // === PROPERTY GROUPS DATA ===
                var propertyGroups = new Dictionary<string, string[]>
                    {
                         { "Dark Purple", new[] { "Mediterranian Ave", "Baltic Ave" }
    },

                        { "Light Blue", new[] { "Oriental Ave", "Vermont Ave", "Connecticut Ave" } },

                        { "Pink", new[] { "New Charles Ave", "State Ave", "Virginia Ave" } },

                        { "Orange", new[] { "St. James Place", "Tennessee Ave", "New York Ave" } },

                        { "Red", new[] { "Kentucky Ave", "Indiana Ave", "Illinois Ave" } },

                        { "Yellow", new[] { "Atlantic Ave", "Ventor Ave", "Marvin Gardens" } },

                        { "Green", new[] { "Pacific Ave", "North Carolina Ave", "Pennsylvania Ave" } },

                        { "Dark Blue", new[] { "Park Place", "Boardwalk" } },

                        { "Railroads", new[] { "Reading Railroad", "Pennsylvania Railroad", "B&O Railroad", "Short Line" } },

                        { "Utilities", new[] { "Electric Company", "Water Works" } }

        
                 };

                int yOffset = 10;
                Dictionary<string, GroupBox> groupBoxMap = new Dictionary<string, GroupBox>();

                foreach (var group in propertyGroups)
                {
                    GroupBox groupBox = new GroupBox();
                    groupBox.Text = group.Key;
                    groupBox.Font = new Font("Arial", 10, FontStyle.Bold);

                    groupBox.Size = new Size(940, group.Value.Length * 40+ 30);

                    groupBox.Location = new Point(20, yOffset);

                    TableLayoutPanel table = new TableLayoutPanel();
                    table.Dock = DockStyle.Fill;
                    table.RowCount = group.Value.Length;
                    for (int r = 0; r < table.RowCount; r++)
                    {
                        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Force exact row height
                    }

                    table.ColumnCount = 5;
                    table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F)); // Property label
                    table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F)); // Buy button
                    table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F)); // Sell button
                    table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F)); // House button
                    table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F)); // Hotel button

                    int rowIndex = 0;
                    foreach (string prop in group.Value)
                    {
                        Label propLabel = new Label();
                        propLabel.Text = prop;
                        propLabel.Dock = DockStyle.Fill;
                        propLabel.TextAlign = ContentAlignment.MiddleLeft;
                        propLabel.BorderStyle = BorderStyle.FixedSingle;
                        propLabel.Visible = true; // For now all visible (later use ownership logic)

                        Button buyButton = new Button();
                        buyButton.Text = "Buy";
                        buyButton.Dock = DockStyle.Fill;
                        buyButton.Click += (s, ev) =>
                        {
                            // -->To buy the property that the player is on<--
                            if (name != null && name == prop) 
                            {
                                Contract TiletoBuy = player.CanBuyProperty[0];
                                int price = TiletoBuy.price;
                                
                                if(TiletoBuy.owner.id != players[i].id) // Make sure the player does not already own the property already
                                {
                                    if (players[i].money >= price)
                                    {
                                        players[i].money -= price;
                                        player.ownedProperties.Add(TiletoBuy);
                                        TiletoBuy.owner = player;
                                        
                                        buyButton.Enabled = false;

                                        MessageBox.Show($"Bought {prop} for ${price}");
                                        
                                    }
                                    else
                                    {
                                        MessageBox.Show("Not enough money to buy this property");
                                    }
                                    player.CanBuyProperty.Clear();
                                }
                            }
                            

                        };

                        Button MortgageButton = new Button();
                        MortgageButton.Text = "Mortgage";
                        MortgageButton.Dock = DockStyle.Fill;
                        MortgageButton.Click += (s, ev) => MessageBox.Show($"Sell clicked for {prop}");

                        Button BuyHouse = new Button();
                        BuyHouse.Text = "House";
                        BuyHouse.Dock = DockStyle.Fill;
                        BuyHouse.Click += (s, ev) =>
                        {
                        
                        };

                        Button Sell = new Button();
                        Sell.Text = "Sell";
                        Sell.Dock = DockStyle.Fill;
                        Sell.Click += (s, ev) => MessageBox.Show($"Hotel clicked for {prop}");


                        table.Controls.Add(propLabel, 0, rowIndex);
                        table.Controls.Add(buyButton, 1, rowIndex);
                        table.Controls.Add(MortgageButton, 2, rowIndex);
                        table.Controls.Add(BuyHouse, 3, rowIndex);
                        table.Controls.Add(Sell, 4, rowIndex);
                        

                        groupBoxMap[prop] = groupBox;

                        rowIndex++;
                    }

                    groupBox.Controls.Add(table);

                    groupBox.BackColor = Color.FromArgb(100, Color.Gray);
                    DisableAllButtonsIn(groupBox);

                    scrollPanel.Controls.Add(groupBox);
                    yOffset += groupBox.Height + 10;
                }

                /* ENABLEs ROW OF CURRENT TILE*/
                if (!string.IsNullOrEmpty(name) && groupBoxMap.ContainsKey(name))
                {

                    GroupBox targetGroup = groupBoxMap[name];

                   
                        foreach (Control ctrl in targetGroup.Controls)
                        {

                            if (ctrl is TableLayoutPanel table)
                            {
                                foreach (Control inner in table.Controls)
                                {
                                    if (inner is Label lbl && lbl.Text == name)
                                    {
                                        targetGroup.BackColor = Color.LightYellow; // visually highlight
                                        EnableRowButtons(lbl, table);               // enable this row
                                    }
                                }
                            }
                        }
                    
                }

                /* ENABLES ALL PROPERTIES OWNED BY THE PLAYER*/
                foreach (var owned in player.ownedProperties)
                {
                    string ownedName = owned.name;

                    if (groupBoxMap.ContainsKey(ownedName))
                    {
                        GroupBox ownedGroup = groupBoxMap[ownedName];

                        foreach (Control ctrl in ownedGroup.Controls)
                        {
                            if (ctrl is TableLayoutPanel table)
                            {
                                foreach (Control inner in table.Controls)
                                {
                                    if (inner is Label lbl && lbl.Text == ownedName)
                                    {
                                        ownedGroup.BackColor = Color.Transparent;
                                        EnableRowButtons(lbl, table);
                                    }
                                }
                            }
                        }
                    }
                }




                /* PROPERIES SECTION END */



                // === MASTER LAYOUT ===
                TableLayoutPanel mainLayout = new TableLayoutPanel();
                mainLayout.Dock = DockStyle.Fill;
                mainLayout.RowCount = 3;
                mainLayout.ColumnCount = 2;


                // ***Set sizes for rows and columns** 
                /* 
                FOR ROWS:
                Keep in mind this is a 2rows by column panel.
                The top section is exactly 150 Pixels tall. 
                The next secion fills 100% of the remaining space */
                mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));     // Top section height, Row 0
                mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));      // Bottom section fills remaining space, Row 1
                mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));      // Bottom section fills remaining space, Row 1

                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); // Left side (scrollPanel), column 0
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); // Right side (empty for now), column 1

                // Place the top section across both columns (header info, player id, balance and exit button)
                mainLayout.Controls.Add(playerInfoTable, 0, 0);
                mainLayout.SetColumnSpan(playerInfoTable, 2);

                // Place scrollable property panel in the bottom-left
                mainLayout.Controls.Add(scrollPanel, 0, 1);
                mainLayout.SetColumnSpan(scrollPanel, 2);

                mainLayout.Controls.Add(actionButtons, 0, 1);
                mainLayout.SetColumnSpan(actionButtons, 2);


                // Add everything to the popup form
                popup.Controls.Add(mainLayout);

                //popup.Controls.Add(playerInfoTable);
                popup.Show();
            }

            public void pay(Player payingPlayer, Player payToPlayer, int payment_amount)
            {
                this.payToPlayer = payToPlayer;
                this.payment = payment_amount;
                ShowPlayerInfo(payingPlayer, null, true);
            }
             


            
            private void RollDice(object sender, EventArgs e)
            {
                int currentIndex = Array.IndexOf(board.tiles, players[i].location);
                int dice1 = random.Next(1, 7);
                int dice2 = random.Next(1, 7);
                int diceRolled = dice1 + dice2;

                showDiceRoll.Text = "player #" + players[i].id + (System.Environment.NewLine) + "Dice: " + dice1 + (System.Environment.NewLine) + "Dice: " + dice2 + (System.Environment.NewLine) + "Total: " + diceRolled;

                if (players[i].isJailed)
                {
                    jailRoll(players[i], dice1, dice2);
                    i++;
                    if (i == players.Length)
                    {
                        i = 0;
                    }
                    return;
                }
                
                players[i].location.playerSlot[i].Visible = false;
                int nextLocation = currentIndex + dice1 + dice2;
                if (nextLocation >= board.tiles.Length)
                {
                    nextLocation = nextLocation % 40;
                    players[i].money += 200;
                    Console.WriteLine(players[i].money);
                }

                players[i].location = board.tiles[nextLocation];
                players[i].location.Land_On_Tile(players[i], this);
                players[i].location.playerSlot[i].Visible = true;

                
                if (dice1 == dice2)
                {
                    doublesCounter++;
                    Console.WriteLine("Player " + players[i].id + " rolled doubles! doubles counter: " + doublesCounter);
                    if(doublesCounter == 3)
                    {
                        players[i].isJailed = true;
                        players[i].location.playerSlot[i].Visible = false;
                        players[i].location = board.tiles[10];
                        players[i].location.playerSlot[i].Visible = true;
                        Console.WriteLine("Player " + players[i].id + " was caught speeding!");
                    }
                    else
                    {
                        return;
                    }
                   
                }

                //pay(players[0], players[1], 100);  //tests a hardcoded payment
                doublesCounter = 0;
                i++;
                if (i == players.Length)
                {
                    i = 0;
                }
            }

            void jailRoll(Player jailedPlayer, int dice1, int dice2)
            {
                jailedPlayer.jailTimer++;
                if (dice1 == dice2)
                {
                    Console.WriteLine("Player " + jailedPlayer.id + " rolled doubles and broke out of jail!");
                    jailedPlayer.isJailed = false;
                    jailedPlayer.jailTimer = 0; 
                    jailedPlayer.location.playerSlot[i].Visible = false;
                    jailedPlayer.location = board.tiles[10 + dice1 + dice2];
                    jailedPlayer.location.playerSlot[i].Visible = true;
                    
                    return;
                }
                else if (jailedPlayer.jailTimer == 3)
                {
                    jailedPlayer.isJailed = false;
                    jailedPlayer.jailTimer = 0;
                    jailedPlayer.location.playerSlot[i].Visible = false;
                    jailedPlayer.location = board.tiles[10 + dice1 + dice2];
                    jailedPlayer.location.playerSlot[i].Visible = true;
                    Console.WriteLine("Player " + jailedPlayer.id + " payed the 50$ fine (ADD THIS FEATURE) and is free.");
                    //TODO: Pay $50 to leave jail
                    return; 
                }
                
                Console.WriteLine("Player " + jailedPlayer.id + " remains in jail: " + jailedPlayer.jailTimer);
            }

            private void DisableAllButtonsIn(Control parent)
            {
                foreach (Control c in parent.Controls)
                {
                    if (c is Button button)
                    {
                        button.Enabled = false;
                    }
                    else
                    {
                        // Recursive check for nested containers like TableLayoutPanel
                        DisableAllButtonsIn(c);
                    }
                }
            }

            private void EnableRowButtons(Label label, TableLayoutPanel table)
            {
                var pos = table.GetPositionFromControl(label);
                int row = pos.Row;

                for (int col = 1; col < table.ColumnCount; col++)
                {
                    Control btn = table.GetControlFromPosition(col, row);
                    if (btn is Button button) button.Enabled = true;
                }
            }

        }
    }
}
