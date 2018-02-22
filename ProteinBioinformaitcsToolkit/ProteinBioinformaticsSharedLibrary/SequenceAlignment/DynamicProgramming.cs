using System;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;

namespace ProteinBioinformaticsSharedLibrary.SequenceAlignment
{
    public abstract class DynamicProgramming
    {
        protected String sequence1;
        protected String sequence2;
        protected Cell[,] scoreTable;
        protected bool tableIsFilledIn;
        protected bool isInitialized;

        public DynamicProgramming(String sequence1, String sequence2)
        {
            this.sequence1 = sequence1;
            this.sequence2 = sequence2;
            scoreTable = new Cell[sequence2.Length + 1, sequence1.Length + 1];
        }

        public int[,] getScoreTable()
        {
            ensureTableIsFilledIn();

            var matrix = new int[scoreTable.GetLength(0), scoreTable.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = scoreTable[i, j].getScore();
                }
            }

            return matrix;
        }

        protected void initializeScores()
        {
            for (int i = 0; i < scoreTable.GetLength(0); i++)
            {
                for (int j = 0; j < scoreTable.GetLength(1); j++)
                {
                    scoreTable[i, j].setScore(getInitialScore(i, j));
                }
            }
        }

        protected void initializePointers()
        {
            for (int i = 0; i < scoreTable.GetLength(0); i++)
            {
                for (int j = 0; j < scoreTable.GetLength(1); j++)
                {
                    scoreTable[i, j].setPrevCell(getInitialPointer(i, j));
                }
            }
        }

        protected void initialize()
        {
            for (int i = 0; i < scoreTable.GetLength(0); i++)
            {
                for (int j = 0; j < scoreTable.GetLength(1); j++)
                {
                    scoreTable[i, j] = new Cell(i, j);
                }
            }
            initializeScores();
            initializePointers();

            isInitialized = true;
        }

        protected abstract Cell getInitialPointer(int row, int col);

        protected abstract int getInitialScore(int row, int col);

        protected abstract void fillInCell(Cell currentCell, Cell cellAbove,
            Cell cellToLeft, Cell cellAboveLeft);

        protected void fillIn()
        {
            for (int row = 1; row < scoreTable.GetLength(0); row++)
            {
                for (int col = 1; col < scoreTable.GetLength(1); col++)
                {
                    Cell currentCell = scoreTable[row, col];
                    Cell cellAbove = scoreTable[row - 1, col];
                    Cell cellToLeft = scoreTable[row, col - 1];
                    Cell cellAboveLeft = scoreTable[row - 1, col - 1];
                    fillInCell(currentCell, cellAbove, cellToLeft, cellAboveLeft);
                }
            }

            tableIsFilledIn = true;
        }

        protected abstract Object getTraceback();

        public void printScoreTable()
        {
            ensureTableIsFilledIn();
            for (int i = 0; i < sequence2.Length + 2; i++)
            {
                for (int j = 0; j < sequence1.Length + 2; j++)
                {
                    if (i == 0)
                    {
                        if (j == 0 || j == 1)
                        {
                            Console.Write("  ");
                        }
                        else
                        {
                            if (j == 2)
                            {
                                Console.Write("     ");
                            }
                            else
                            {
                                Console.Write("   ");
                            }
                            Console.Write(sequence1[j - 2]);
                        }
                    }
                    else if (j == 0)
                    {
                        if (i == 1)
                        {
                            Console.Write("  ");
                        }
                        else
                        {
                            Console.Write(" " + sequence2[i - 2]);
                        }
                    }
                    else
                    {
                        String toPrint;
                        Cell currentCell = scoreTable[i - 1, j - 1];
                        Cell prevCell = currentCell.getPrevCell();
                        if (prevCell != null)
                        {
                            if (currentCell.getCol() == prevCell.getCol() + 1
                                && currentCell.getRow() == prevCell.getRow() + 1)
                            {
                                toPrint = "\\";
                            }
                            else if (currentCell.getCol() == prevCell.getCol() + 1)
                            {
                                toPrint = "-";
                            }
                            else
                            {
                                toPrint = "|";
                            }
                        }
                        else
                        {
                            toPrint = " ";
                        }
                        int score = currentCell.getScore();
                        String s = score.ToString().PadLeft(3, '0');
                        toPrint += s;
                        Console.Write(toPrint);
                    }

                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        protected void ensureTableIsFilledIn()
        {
            if (!isInitialized)
            {
                initialize();
            }
            if (!tableIsFilledIn)
            {
                fillIn();
            }
        }
    }
}
