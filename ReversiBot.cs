using System;
using System.Collections.Generic;

namespace Reversi
{
    class ReversiBot
    {
        private int[,] boardState;
        public placingCoord defMove;
        private int boardWidth;
        private int boardHeight;

        public ReversiBot(int[,] currentGameState, int boardWidth, int boardHeight)
        {
            this.boardState = new int[boardWidth, boardHeight];
            Array.Copy(currentGameState, this.boardState, boardWidth * boardHeight);
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;

            Console.WriteLine("start searching:...");

            List<placingCoord> possibleMoves = this.viableMoves(currentGameState, 2);
            if (possibleMoves.Count == 0)
            {
                Console.WriteLine("No moves available for me...");
                this.defMove = new placingCoord(-1, -1, 3);
            }
            else
            {
                //Random random = new Random();
                //int start2 = random.Next(0, possibleMoves.Count);
                //this.defMove = possibleMoves[start2];
                int lastLen = 0;
                foreach (placingCoord move in possibleMoves)
                {
                    //picks move that gains the most flips in the short term.
                    int moveLen = this.getValidMoves(move.X, move.Y, this.boardState, 2).Length;
                    Console.WriteLine($"met move: {move.ToString()}, i get {moveLen} amount of flips");
                    if (moveLen >= lastLen)
                    {
                        Console.WriteLine($"Met move {move.ToString()} i can turn {moveLen} amount of flips, which is better than the move {this.defMove.ToString()}, which flipped {lastLen}");
                        lastLen = moveLen;
                        this.defMove = move;
                    }
                }
            }
            Console.WriteLine("done searching.");
        }

        private placingCoord[] getValidMoves(int column, int row, int[,] gameState, int currentPlayer)
        {
            int[] places = new int[] { -1, 0, 1 };

            List<placingCoord> flipsToReturn = new List<placingCoord>();

            //This means that the place is already occupied by one of the player's pieces.
            if (gameState[column, row] != 0 && gameState[column, row] != 3) 
                return flipsToReturn.ToArray();

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
                                    
                                    //stop searching
                                    if (gameState[tempX, tempY] == currentPlayer)
                                        i = maxBoardDimension;
                                    else if (gameState[tempX, tempY] != currentPlayer && gameState[tempX, tempY] != 0)
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
                    //If the position is unoccupied and there is a valid move than x,y is a viable move.
                    if (this.boardState[x, y] == 0 && this.getValidMoves(x, y, gameState, currentPlayer).Length != 0)
                        possibleMoves.Add(new placingCoord(x, y, currentPlayer));
                }
            }
            return possibleMoves;
        }

    }
   
}
