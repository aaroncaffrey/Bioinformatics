using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstituteInterfaces
{
    public class SubstituteInterfaces
    {
        static void Main(string[] args)
        {
            // this program outputs sequence file with substituted interfaces
            // takes as input the interface data csv file

            var input_fasta_file = args[0];
            var interface_data_csv_file = args[1];
            var interface_interface_data_csv_file = args[2];
            var output_fasta_file = args[3];

            var interface_data = ComplexInterfaces.ComplexInterfaces.InterfaceData.Load(interface_data_csv_file);

            var interface_interface_data = ComplexInterfaces.ComplexInterfaces.InterfaceInterfaceData.Load(interface_interface_data_csv_file);



            // load sequence file
            //var result = MutateSequence.MutateSequence.MutateFastaSequence(input_fasta_file, chain_ids_split, start_position_split, end_position_split, offset_position_split, mutation_sequence_split);

            // load interface data file

            // 


        }
    }
}
