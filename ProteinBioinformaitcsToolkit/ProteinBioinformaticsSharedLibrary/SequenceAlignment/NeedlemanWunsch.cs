using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;

namespace ProteinBioinformaticsSharedLibrary.SequenceAlignment
{
    public class NeedlemanWunsch : SequenceAlignment
    {

        public NeedlemanWunsch(String sequence1, String sequence2) : base(sequence1, sequence2)
        {

        }

        public NeedlemanWunsch(String sequence1, String sequence2, int match, int mismatch, int gap) :base(sequence1, sequence2, match, mismatch, gap)
        {
            
        }

        protected override void fillInCell(Cell currentCell, Cell cellAbove, Cell cellToLeft,
            Cell cellAboveLeft)
        {
            int rowSpaceScore = cellAbove.getScore() + space;
            int colSpaceScore = cellToLeft.getScore() + space;
            int matchOrMismatchScore = cellAboveLeft.getScore();
            if (sequence2[currentCell.getRow() - 1] == sequence1[currentCell.getCol() - 1])
            {
                matchOrMismatchScore += match;
            }
            else
            {
                matchOrMismatchScore += mismatch;
            }
            if (rowSpaceScore >= colSpaceScore)
            {
                if (matchOrMismatchScore >= rowSpaceScore)
                {
                    currentCell.setScore(matchOrMismatchScore);
                    currentCell.setPrevCell(cellAboveLeft);
                }
                else
                {
                    currentCell.setScore(rowSpaceScore);
                    currentCell.setPrevCell(cellAbove);
                }
            }
            else
            {
                if (matchOrMismatchScore >= colSpaceScore)
                {
                    currentCell.setScore(matchOrMismatchScore);
                    currentCell.setPrevCell(cellAboveLeft);
                }
                else
                {
                    currentCell.setScore(colSpaceScore);
                    currentCell.setPrevCell(cellToLeft);
                }
            }
        }


        protected override bool traceBackIsNotDone(Cell currentCell)
        {
            return currentCell.getPrevCell() != null;
        }


        protected override Cell getTracebackStartingCell()
        {
            return scoreTable[scoreTable.GetLength(0) - 1, scoreTable.GetLength(1) - 1];
        }

        public override String ToString()
        {
            return "[NeedlemanWunsch: sequence1=" + sequence1 + ", sequence2=" + sequence2 + "]";
        }

        protected override Cell getInitialPointer(int row, int col)
        {
            if (row == 0 && col != 0)
            {
                return scoreTable[row,col - 1];
            }
            else if (col == 0 && row != 0)
            {
                return scoreTable[row - 1,col];
            }
            else
            {
                return null;
            }
        }

        protected override int getInitialScore(int row, int col)
        {
            if (row == 0 && col != 0)
            {
                return col*space;
            }
            else if (col == 0 && row != 0)
            {
                return row*space;
            }
            else
            {
                return 0;
            }
        }

    }
}
