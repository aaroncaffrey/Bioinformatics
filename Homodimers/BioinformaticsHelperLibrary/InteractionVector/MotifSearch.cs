using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio;
using Bio.Extensions;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public class MotifSearch
    {

        public bool SearchSequenceForMotif(ISequence sequence, string motif)
        {
            var seq = sequence.ConvertToString();

            return SearchSequenceForMotif(seq, motif);
        }

        public static bool SearchSequenceForMotif(string sequence, string motif)
        {
            if (string.IsNullOrEmpty(sequence)) throw new ArgumentNullException(nameof(sequence));
            if (string.IsNullOrEmpty(motif)) throw new ArgumentNullException(nameof(motif));


            var motifSplit = motif.ToUpperInvariant().Split('-');

            for (var seqIndex = 0; seqIndex < sequence.Length; seqIndex++)
            {
                var match = true;
                var seqIndexLocal = seqIndex;

                foreach (var motifPart in motifSplit)
                {
                    if (seqIndexLocal > sequence.Length - 1)
                    {
                        match = false;
                        break;
                    }
                    else if (motifPart.StartsWith("{") && motifPart.EndsWith("}"))
                    {
                        if (motifPart.Contains(sequence[seqIndexLocal]))
                        {
                            match = false;
                            break;
                        }
                    }
                    else if (motifPart.StartsWith("[") && motifPart.EndsWith("]"))
                    {
                        if (!motifPart.Contains(sequence[seqIndexLocal]))
                        {
                            match = false;
                            break;
                        }
                    }
                    else if (motifPart.Length == 1)
                    {
                        if (motifPart[0] != sequence[seqIndexLocal] && (sequence[seqIndexLocal] != 'X' && motifPart[0] != 'X'))
                        {
                            match = false;
                            break;
                        }
                    }
                    else
                    {
                        match = false;
                        break;
                    }

                    seqIndexLocal++;
                }

                if (match)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
