using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProteinBioinformaticsSharedLibrary.SequenceAlignment;

namespace ProteinBioinformaticsSharedLibrary.SequenceAlignment
{
    public abstract class SequenceAlignment : DynamicProgramming
    {

        protected int match;
        protected int mismatch;
        protected int space;
        protected String[] alignments;

        public SequenceAlignment(String sequence1, String sequence2) : this(sequence1, sequence2, 1, -1, -1)
        {
            
        }

        public SequenceAlignment(String sequence1, String sequence2, int match, int mismatch, int gap) : base(sequence1,sequence2)
        {

            this.match = match;
            this.mismatch = mismatch;
            this.space = gap;
        }

        protected override Object getTraceback()
        {
            StringBuilder align1Buf = new StringBuilder();
            StringBuilder align2Buf = new StringBuilder();
            Cell currentCell = getTracebackStartingCell();
            while (traceBackIsNotDone(currentCell))
            {
                if (currentCell.getRow() - currentCell.getPrevCell().getRow() == 1)
                {
                    align2Buf.Insert(0, sequence2[currentCell.getRow() - 1]);
                }
                else
                {
                    align2Buf.Insert(0, '-');
                }
                if (currentCell.getCol() - currentCell.getPrevCell().getCol() == 1)
                {
                    align1Buf.Insert(0, sequence1[currentCell.getCol() - 1]);
                }
                else
                {
                    align1Buf.Insert(0, '-');
                }
                currentCell = currentCell.getPrevCell();
            }

            String[] alignments = new String[]
            {
                align1Buf.ToString(),
                align2Buf.ToString()
            };

            return alignments;
        }

        protected abstract bool traceBackIsNotDone(Cell currentCell);

        public int getAlignmentScore()
        {
            if (alignments == null)
            {
                getAlignment();
            }

            int score = 0;
            for (int i = 0; i < alignments[0].Length; i++)
            {
                char c1 = alignments[0][i];
                char c2 = alignments[1][i];
                if (c1 == '-' || c2 == '-')
                {
                    score += space;
                }
                else if (c1 == c2)
                {
                    score += match;
                }
                else
                {
                    score += mismatch;
                }
            }

            return score;
        }

        public String[] getAlignment()
        {
            ensureTableIsFilledIn();
            alignments = (String[]) getTraceback();
            return alignments;
        }

        protected abstract Cell getTracebackStartingCell();
    }

}
