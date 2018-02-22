using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.ProproteinInterface
{
    public class ScanProproteinInterfaceParameters
    {
        [Flags]
        public enum TargetProteinDatabases
        {
            Default = 0x0,
            ProteinDataBank = 0x1,
            UniProtKbSwissProt = 0x2,
            UniProtKbTrEmbl = 0x3,
        }

        /// <summary>
        /// Sequence(s) to be scanned: UniProtKB accessions e.g. P98073 or identifiers e.g. ENTK_HUMAN or PDB identifiers e.g. 4DGJ or sequences in FASTA format or UniProtKB/Swiss-Prot format. 
        /// Do not repeat parameter; multiple sequences can be specified by separating them with new lines(%0A in url).
        /// </summary>
        public string seq;

        /// <summary>
        /// Motif(s) to scan against: PROSITE accession e.g. PS50240 or identifier e.g. TRYPSIN_DOM or your own pattern e.g. P-x(2)-G-E-S-G(2)-[AS]. 
        /// If not specified, all PROSITE motifs are used.
        /// Do not repeat parameter; multiple motifs can be specified by separating them with new lines(%0A in url).
        /// </summary>
        public string sig;

        /// <summary>
        /// Target protein database for scans of motifs against whole protein databases: 'sp' (UniProtKB/Swiss-Prot) or 'tr' (UniProtKB/TrEMBL) or 'pdb' (PDB). 
        /// Only work if 'seq' is not defined.Parameter can be repeated; 1 target db by 'db' parameter.
        /// </summary>
        public TargetProteinDatabases db;

        /// <summary>
        /// If true (defined, non empty, non zero): includes UniProtKB/Swiss-Prot splice variants. 
        /// Only works on scans against UniProtKB/Swiss-Prot.
        /// </summary>
        public string varsplic;

        /// <summary>
        /// Any taxonomical term e.g. 'Homo sapiens', e.g. 'Fungi; Arthropoda' or corresponding NCBI TaxID e.g. 9606, e.g. '4751; 6656' 
        /// Separate multiple terms with a semicolon.
        /// Only works on scans against UniProtKB/Swiss-Prot and UniProtKB/TrEMBL.
        /// </summary>
        public string lineage;

        /// <summary>
        /// Description (DE) filter: e.g. protease. 
        /// Only works on scans against UniProtKB/Swiss-Prot and UniProtKB/TrEMBL.
        /// </summary>
        public string description;

        /// <summary>
        /// Number of X characters in a scanned sequence that can be matched by a conserved position in a pattern. 
        /// Only works if 'sig' is defined, i.e.on scans of specific sequences/protein database(s) against specific motif(s). 
        /// Only works on scans against patterns.
        /// </summary>
        public string max_x;

        /// <summary>
        /// Output format: 'xml' or 'json' (or 'txt')
        /// </summary>
        public string output = "xml";

        /// <summary>
        /// If true (defined, non empty, non zero): excludes motifs with a high probability of occurrence. 
        /// Default: on.
        /// Only works if 'seq' is defined and 'sig' is not defined, i.e.on scans of specific sequence(s) against all PROSITE motifs.
        /// </summary>
        public string skip;

        /// <summary>
        /// If true (defined, non empty, non zero): shows matches with low level scores. 
        /// Default: off.
        /// Only works with PROSITE profiles.
        /// </summary>
        public string lowscore;

        /// <summary>
        /// If true (defined, non empty, non zero): does not scan against profiles. 
        /// Only works if 'seq' is defined and 'sig' is not defined, i.e.on scans of specific sequence(s) against all PROSITE motifs.
        /// </summary>
        public string noprofile;

        /// <summary>
        /// Mimimal number of hits per matched sequences. 
        /// Only works if 'sig' and 'db' are defined, i.e.on scans of protein database(s) against specific motif(s).
        /// </summary>
        public string minhits;

        public string QueryString(bool urlEncode = true)
        {

            var result = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("seq", seq),
                new KeyValuePair<string, string>("sig", sig),
                new KeyValuePair<string, string>("varsplic", varsplic),
                new KeyValuePair<string, string>("lineage", lineage),
                new KeyValuePair<string, string>("description", description),
                new KeyValuePair<string, string>("max_x", max_x),
                new KeyValuePair<string, string>("output", output),
                new KeyValuePair<string, string>("skip", skip),
                new KeyValuePair<string, string>("lowscore", lowscore),
                new KeyValuePair<string, string>("noprofile", noprofile),
                new KeyValuePair<string, string>("minhits", minhits)
            };

            if (db.HasFlag(TargetProteinDatabases.ProteinDataBank)) result.Add(new KeyValuePair<string, string>("db", "pdb"));
            if (db.HasFlag(TargetProteinDatabases.UniProtKbSwissProt)) result.Add(new KeyValuePair<string, string>("db", "sp"));
            if (db.HasFlag(TargetProteinDatabases.UniProtKbTrEmbl)) result.Add(new KeyValuePair<string, string>("db", "tr"));


            return string.Join("&", result.Where(a => a.Key != null && a.Value != null).Select(kvp => (urlEncode?WebUtility.UrlEncode(kvp.Key) :kvp.Key) + "=" + (urlEncode?WebUtility.UrlEncode(kvp.Value) :kvp.Value)).ToList());
        }

    }

}
