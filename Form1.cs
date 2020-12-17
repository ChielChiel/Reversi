using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Reversi
{
    public partial class MainWindow : Form
    {
        public string playerOne;
        public string playerTwo;
        public bool wasViable = true;
        public int boardWidth = 6;
        public int boardHeight = 6;
        TableLayoutPanel board = new TableLayoutPanel();
        private int[,] boardState;
        int currentPlayer = 1;
        IDictionary<int, Color> pieceColor = new Dictionary<int, Color> { { 1, Color.Black }, { 2, Color.White } };

        private bool isHelping = false;
        private bool isBotPlaying = false;
        
        public MainWindow()

        {
            InitializeComponent();
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, 50, 50);

            this.playerOneIcon.BackColor = pieceColor[1];
            this.playerOneIcon.Region = new Region(path);
            this.playerOneIconName.ForeColor = Color.Red;

            this.playerTwoIcon.BackColor = pieceColor[2];
            this.playerTwoIcon.Region = this.playerOneIcon.Region;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Reversi";
            this.Width = 1000;
            this.Height = 900;
        }

        private void Clicked(object sender, MouseEventArgs mea)
        {
            Point translatedPoint = this.getPointFromMEA(mea.Location);
            int column = translatedPoint.X;
            int row = translatedPoint.Y;

            if ((this.currentPlayer == 2 && !this.isBotPlaying) || this.currentPlayer == 1)
            {
                //Get here the valid moves/flips for the current coordinates.
                placingCoord[] flips = getFlipsWithMove(column, row, currentPlayer);
                if (flips.Length != 0)
                {
                    this.boardState[column, row] = currentPlayer;
                    foreach (placingCoord flip in flips)
                    {
                        this.boardState[flip.X, flip.Y] = currentPlayer;
                    }
                    this.currentPlayer = (this.currentPlayer == 1 ? 2 : 1);
                    this.isHelping = false;
                }
                else
                {
                    //There are no pieces flippable so the current player has to do another turn
                    //MessageBox.Show("Hier kan je geen steen plaatsen. Kies een andere zet. Of gebruik help.");
                }
            }


            if (this.currentPlayer == 2 && this.isBotPlaying == true)
            {
               
                this.board.Refresh();
                Console.WriteLine("bot is aan de beurt.");
                
                //Je speelt tegen bot. Bot doet nu zijn move:
                int[,] boardStateToPass = new int[this.boardWidth, this.boardHeight];
                Array.Copy(this.boardState, boardStateToPass, this.boardHeight * this.boardWidth);
                ReversiBot botFrank = new ReversiBot(boardStateToPass, this.boardWidth, this.boardHeight);

                if (!botFrank.defMove.Equals(new placingCoord(-1, -1, 3)))
                {
                    //Een zet mogelijk van botFrank.
                    placingCoord[] botFlips = getFlipsWithMove(botFrank.defMove.X, botFrank.defMove.Y, this.currentPlayer);
                    Console.WriteLine($"{botFrank.defMove}, botflips: {botFlips.Length} ");

                    if (botFlips.Length != 0)
                    {
                        this.boardState[botFrank.defMove.X, botFrank.defMove.Y] = this.currentPlayer;

                        foreach (placingCoord botFlip in botFlips)
                            this.boardState[botFlip.X, botFlip.Y] = currentPlayer;

                        this.currentPlayer = 1;
                    }
                }
                else 
                {
                    Console.WriteLine("Bot frank passed ff");
                }
                
                Console.WriteLine("Bot is klaar met zn zet.");

            }

            // here we set the count of the stones on the board
            this.playerOneIconName.Text = this.playerOne + $" | {occurences(boardState, 1)} Punten";
            this.playerTwoIconName.Text = this.playerTwo + $" | {occurences(boardState, 2)} Punten";
            this.board.Invalidate();

            //checks if there are viable moves and if not it allows for another turn or tells who won.
            List<placingCoord> possibleMoves = viableMoves();
            if(possibleMoves.Count == 0)
            {
                if (this.wasViable)
                {
                    this.currentPlayer = (this.currentPlayer == 1 ? 2 : 1);
                    this.wasViable = false;
                }
                else
                {
                    int black = occurences(boardState, 1);
                    int white = occurences(boardState, 2);
                    if(black == white)
                    {
                        MessageBox.Show("It's a tie");
                    }else if(black > white)
                    {
                        MessageBox.Show("\"" + this.playerOne + "\" (Black) has won!");
                    }
                    else
                    {
                        MessageBox.Show("\"" + this.playerTwo + "\" (White) has won!");
                    }
                }

            }
            else
            {
                if (this.currentPlayer == 1)
                {
                    this.playerOneIconName.ForeColor = Color.Red;
                    this.playerTwoIconName.ForeColor = Color.Black;
                }
                else
                {
                    this.playerOneIconName.ForeColor = Color.Black;
                    this.playerTwoIconName.ForeColor = Color.Red;
                }
            }
        }

        //Converts x and y coordinates of the mouseclick to a column and row.
        private Point getPointFromMEA(Point meaPoint) 
        {
            return new Point((int)(meaPoint.X / 50f), (int)(meaPoint.Y / 50f));
        }


        //returns all of the board positions that get flipped to the current player if it were to click on that position.
        private placingCoord[] getFlipsWithMove(int column, int row, int currentPlayer)
        {
            int[] places = new int[] { -1, 0, 1 };
            List<placingCoord> flipsToReturn = new List<placingCoord>();

            //This means that the place is already occupied by one of the player's pieces.
            if (this.boardState[column, row] != 0 && this.boardState[column,row] != 3)
                return flipsToReturn.ToArray();

            //Loop through every piece surrounding the clicked one.
            for (int dx = 0; dx < places.Length; dx++) 
            {
                for (int dy = 0; dy < places.Length; dy++) 
                {
                    try
                    {
                        //Check if the neighbouring position is not from your or if its empty
                        if (this.boardState[column + places[dx], row + places[dy]] != currentPlayer && this.boardState[column + places[dx], row + places[dy]] != 0 )
                        {
                            //Check here whether the possible moves actually close-in pieces of the opponent.
                            List<placingCoord> tempFlipPieces = new List<placingCoord>();
                            try
                            {
                                int maxBoardDimension = (this.boardWidth <= this.boardHeight ? this.boardHeight : this.boardWidth);
                                for (int i = 1; i <= maxBoardDimension; i++) 
                                {
                                    int tempX = i * places[dx] + column;
                                    int tempY = i * places[dy] + row;
                                    int tempBoardStateInt = this.boardState[tempX, tempY];
                                    
                                    if (tempBoardStateInt == currentPlayer) //stop searching
                                        i = maxBoardDimension;
                                    else if (tempBoardStateInt != currentPlayer && tempBoardStateInt != 0)
                                        tempFlipPieces.Add(new placingCoord(tempX, tempY, currentPlayer));
                                    else
                                        tempFlipPieces.Clear();
                                }            
                            }
                            catch (IndexOutOfRangeException)
                            {
                                //Out of range. Clearing temp pieces
                                tempFlipPieces.Clear();
                            }
                            flipsToReturn.AddRange(tempFlipPieces);
                        }
                    }
                    catch (IndexOutOfRangeException) {

                        //Out of range. But nothing has to be done
                    }
                }
            }
            //Return all the pieces that will be flipped.
            return flipsToReturn.ToArray();
        }

        
    
        private void draw(object sender, TableLayoutCellPaintEventArgs tlcpea)
        {
            Brush brushColor = null;
            bool drawHelp = false;

            switch (this.boardState[tlcpea.Column, tlcpea.Row]) {
                case 1:
                    brushColor = new SolidBrush(pieceColor[1]);
                    break;
                case 2:
                    brushColor = new SolidBrush(pieceColor[2]);
                    break;
                case 3:
                    brushColor = new SolidBrush(Color.Yellow);
                    this.boardState[tlcpea.Column, tlcpea.Row] = 0;
                    drawHelp = true;
                    break;
                default:
                    break;
            }
            tlcpea.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            if (brushColor != null)
            {
                if (drawHelp)
                    tlcpea.Graphics.FillEllipse(brushColor, tlcpea.CellBounds.Location.X + 10, tlcpea.CellBounds.Location.Y + 10, tlcpea.CellBounds.Width - 20, tlcpea.CellBounds.Height - 20);
                else 
                    tlcpea.Graphics.FillEllipse(brushColor, tlcpea.CellBounds.Location.X + 5, tlcpea.CellBounds.Location.Y + 5, tlcpea.CellBounds.Width - 10, tlcpea.CellBounds.Height - 10);
            }
            tlcpea.Graphics.DrawRectangle(Pens.Black, tlcpea.CellBounds.Location.X, tlcpea.CellBounds.Location.Y, tlcpea.CellBounds.Width, tlcpea.CellBounds.Height);

            if (tlcpea.Column == this.boardWidth - 1 && tlcpea.Row == this.boardHeight - 1)
                this.isHelping = false;
        }
        //Buttons
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            using (PopupWindow newGame = new PopupWindow(true))
            {
                var result = newGame.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.playerOne = newGame.namePlayerOne;
                    this.playerTwo = newGame.namePlayerTwo;
                    this.boardWidth = newGame.gameBoardSizeWidth;
                    this.boardHeight = newGame.gameBoardSizeHeight;
                    Console.WriteLine("New \"" + this.playerOne + "\"v\"" + this.playerTwo + "\" game");
                    this.isBotPlaying = false;
                    this.NewGame(this.boardWidth, this.boardHeight);
                }
                else
                    Console.WriteLine("No new game");
            }
        }

        private void BotButton_Click(object sender, EventArgs e)
        {
            using (PopupWindow newGame = new PopupWindow(false))
            {
                var result = newGame.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.playerOne = newGame.namePlayerOne;
                    this.playerTwo = newGame.namePlayerTwo;
                    this.boardWidth = newGame.gameBoardSizeWidth;
                    this.boardHeight = newGame.gameBoardSizeHeight;
                    Console.WriteLine("New \"" + this.playerOne + "\"v\"" + this.playerTwo + "\" game");
                    this.isBotPlaying = true;
                    this.NewGame(this.boardWidth, this.boardHeight);
                    
                }
                else
                    Console.WriteLine("No new game");
            }
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            if (this.playerOne != null)
            {
                List<placingCoord> possibleMoves = viableMoves();
                if (possibleMoves.Count != 0)
                {
                    this.isHelping = true;
                    foreach (placingCoord helpMove in possibleMoves)
                        this.boardState[helpMove.X, helpMove.Y] = 3;
                    this.board.Invalidate();
                }
            }
            else 
            {
                Console.WriteLine("spel nog niet gestart");
                MessageBox.Show("Start het spel door op \"Tegen elkaar spelen\" of \"Tegen een AI spelen\" te klikken. De afmetingen van het veld moeten minimaal 3x3 zijn en maximaal 10x10. Veel speelplezier.");
            }
        }

        //Function that initialises a new game.
        private void NewGame(int columns, int rows) 
        {
            this.playerOneIconName.Text = this.playerOne + " | 2 Punten";
            this.playerTwoIconName.Text = this.playerTwo + " | 2 Punten";
            
            this.boardState = new int[columns, rows];

            this.board.Name = "board";
            this.board.Location = new Point(250, 260);
            this.board.Size = new Size(50 * columns + columns, 50 * rows + rows);
            this.board.ColumnCount = 0;
            this.board.RowCount = 0;
            this.board.BackColor = Color.DarkGreen;
            this.board.CellPaint += new TableLayoutCellPaintEventHandler(this.draw);
            this.board.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            this.board.MouseClick += new MouseEventHandler(this.Clicked);

            //Set rows and columns on the board.
            for (; this.board.ColumnCount < columns; this.board.ColumnCount++)
                this.board.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            for (; this.board.RowCount < rows; this.board.RowCount++)
                this.board.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            
            //Setup first 4 pieces in the middle of the board:
            this.boardState[columns / 2 - 1, rows / 2 - 1] = 1;
            this.boardState[columns / 2 - 1, rows / 2] = 2;
            this.boardState[columns / 2, rows / 2] = 1;
            this.boardState[columns / 2, rows / 2 - 1] = 2;
            Controls.Add(this.board);
        }
        

        //checks the occurences of a certain integer within an array, we use this to check the occurences of stones on the board
        private int occurences(int[,] array, int checkedKey)
        {
            int count = 0;
            foreach(int column in array) //Loop through the arrray and check for an occurence
                if (column == checkedKey)
                    count++;
            return count;
        }

        //Simply get all the valid moves for every piece of the current player:  
        private List<placingCoord> viableMoves()
        {
            List<placingCoord> possibleMoves = new List<placingCoord>();
            for (int x = 0; x < this.boardWidth; x++)
            {
                for (int y = 0; y < this.boardHeight; y++)
                {
                    //If the position is unoccupied and there is a valid move than x,y is a viable move. So add that move.
                    if (this.boardState[x, y] == 0 && this.getFlipsWithMove(x, y, currentPlayer).Length != 0)
                            possibleMoves.Add(new placingCoord(x, y, 3));
                }
            }
            return possibleMoves;
        }

    }

    //Struct holding the x and y coordinates of a (possible) flip as well as the current player.
    public struct placingCoord
    {
        public placingCoord(int x, int y, int player)
        {
            X = x;
            Y = y;
            currentPlayer = player;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int currentPlayer { get; set; }
        //Custom ToString method for easy debugging.
        public override string ToString() => $"({X}, {Y}) is now for: player{currentPlayer}";
    }


}
