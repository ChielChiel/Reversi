using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    class ReversiBot
    {
        private int[,] boardState;
        private int defVoorsprong = -20;
        public placingCoord defMove;
        private int boardWidth;
        private int boardHeight;
        //bot is always player 2. So no variable needed there.

        public ReversiBot(int[,] currentGameState, int boardWidth, int boardHeight)
        {
            this.boardState = currentGameState;
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;

            Console.WriteLine("start searching:...");
            this.getBestMove(currentGameState, 2, 3);
            Console.WriteLine("done searching:...");

        }


        private void getBestMove(int[,] gameState, int player, int searchDepth)
        {
            placingCoord bestMove = new placingCoord(-1, -1, 2);

            placingCoord[] possibleMoves = viableMoves(gameState, player).ToArray();

            foreach (placingCoord move in possibleMoves)
            { //Loop door alle mogelijke moves

                int oldVoorsprong = this.defVoorsprong;
                int[,] tempGameState = gameState;
                tempGameState[move.X, move.Y] = player;
                placingCoord[] flips = getValidMoves(move.X, move.Y, tempGameState, player);
                foreach (placingCoord flip in flips)
                {
                    tempGameState[flip.X, flip.Y] = player;
                }

                getBestMovePerInitialpossibilities(tempGameState, turnOf(2), searchDepth);
                Console.WriteLine($"def voorsprong: {this.defVoorsprong}, bij move {move.ToString()}");
                /*
                int TempVoorsprong = occurences(tempGameState, 2) - occurences(tempGameState, 1);
                if (TempVoorsprong > voorsprong)
                { //check of de voorsprong die je met deze move bereikt groter is dan de vorige
                    voorsprong = TempVoorsprong;
                    bestMove = move;
                }
                */
                if (this.defVoorsprong > oldVoorsprong)
                {
                    this.defMove = move;
                }
                else {
                    this.defVoorsprong = oldVoorsprong;
                }
                

            }

        }


        private void getBestMovePerInitialpossibilities(int[,] gameState, int player, int searchDepth)
        {
            //0.
            if (searchDepth == 0)
            { //Klaar met zoeken, de put is bereikt
                int voorsprong = -20;

                placingCoord[] possibleMoves2 = viableMoves(gameState, player).ToArray();

                foreach (placingCoord move in possibleMoves2)
                { //Loop door alle mogelijke moves
                    int[,] tempGameState = gameState;
                    tempGameState[move.X, move.Y] = player;
                    placingCoord[] flips = getValidMoves(move.X, move.Y, tempGameState, player);
                    foreach (placingCoord flip in flips)
                    {
                        tempGameState[flip.X, flip.Y] = player;
                    }

                    int TempVoorsprong = occurences(tempGameState, 2) - occurences(tempGameState, 1);
                    if (TempVoorsprong > voorsprong)
                    { //check of de voorsprong die je met deze move bereikt groter is dan de vorige
                        voorsprong = TempVoorsprong;
                    }

                    if (voorsprong > this.defVoorsprong)
                    {
                        this.defVoorsprong = voorsprong;
                    }

                }
                return;
            }

            //1. Haal alle mogelijke zetten op met deze gameState
            placingCoord[] possibleMoves = viableMoves(gameState, player).ToArray();

            //2. Loop through each of these moves
            foreach (placingCoord move in possibleMoves)
            {
                int[,] tempGameState = gameState;
                tempGameState[move.X, move.Y] = player;
                placingCoord[] flips = getValidMoves(move.X, move.Y, tempGameState, turnOf(player));
                foreach (placingCoord flip in flips)
                {
                    tempGameState[flip.X, flip.Y] = player;
                }
                //Nu zit dus in gameState de nieuw hypothethische state van deze move.
                //run dan nu getBestMove nog een keer voor de tegenpartij.
                //getBestMove(gameState, turnOf(player), searchDepth - 1)
                getBestMovePerInitialpossibilities(tempGameState, turnOf(player), searchDepth - 1);

            }
        }

        private placingCoord[] getValidMoves(int column, int row, int[,] gameState, int currentPlayer)
        {
            int[] places = new int[] { -1, 0, 1 };

            List<placingCoord> flipsToReturn = new List<placingCoord>();

            if (gameState[column, row] != 0 && gameState[column, row] != 3)
            {
                //This means that the place is already occupied by one of the player's pieces.
                return flipsToReturn.ToArray();
            }

            //Loop through every piece surrounding the clicked one.
            for (int dx = 0; dx < places.Length; dx++)
            {
                for (int dy = 0; dy < places.Length; dy++)
                {
                    try
                    {
                        if (gameState[column + places[dx], row + places[dy]] != currentPlayer && gameState[column + places[dx], row + places[dy]] != 0)
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

                                    if (gameState[tempX, tempY] == currentPlayer)
                                    {
                                        //stop searching
                                        i = maxBoardDimension;
                                    }
                                    else if (gameState[tempX, tempY] != currentPlayer && gameState[tempX, tempY] != 0)
                                    {
                                        tempFlipPieces.Add(new placingCoord(tempX, tempY, currentPlayer));
                                    }
                                    else
                                    {
                                        tempFlipPieces.Clear();
                                    }
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
                    catch (IndexOutOfRangeException)
                    {
                        //Out of range. But nothing has to be done
                    }


                }
            }
            //Return all the pieces that will be flipped.
            return flipsToReturn.ToArray();
        }


        private List<placingCoord> viableMoves(int[,] gameState, int currentPlayer)
        {
            List<placingCoord> possibleMoves = new List<placingCoord>();
            for (int x = 0; x < this.boardWidth; x++)
            {
                for (int y = 0; y < this.boardHeight; y++)
                {
                    if (this.boardState[x, y] == 0)
                    {
                        //Thus the position is not yet occupied by one of the two player's pieces.
                        if (this.getValidMoves(x, y, gameState, currentPlayer).Length != 0)
                        {
                            //Thus there are possible flips for this coordinate, so x,y is a valid move.
                            possibleMoves.Add(new placingCoord(x, y, currentPlayer));
                        }
                    }
                }
            }
            return possibleMoves;
        }

        private int occurences(int[,] array, int checkedKey)
        {
            int count = 0;

            //Loop through the arrray and check for an occurence

            foreach (int column in array)
            {
                if (column == checkedKey)
                    count++;

            }
            return count;
        }


        //Whose turn is it?
        private int turnOf(int player)
        {
            return (player == 1 ? 2 : 1);
        }


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
            public override string ToString() => $"({X}, {Y}) now for: player{currentPlayer}";

            public placingCoord Sample() => new placingCoord(-1, -1, 2);

        }


    }
}
