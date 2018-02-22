using System;

namespace ProteinBioinformaticsSharedLibrary.AminoAcids
{
    public class AminoAcidProperties<T>
    {
        public string Code1L = null;
        public string Code3L = null;
        public string Name = null;
        public int Number = -1;

        //public T Acidic;
        public T Aliphatic;
        public T Aromatic;
        public T Charged;
        public T Hydrophobic;
        public T Hydroxylic;
        public T Negative;     
        public T Polar;
        public T Positive;
        public T Small;
        public T Sulphur;
        public T Tiny;

        public const int TotalProperties = 12;

        public T[] PropertiesArray()
        {
            var result = new[]
            {
                //Acidic,
                Aliphatic,
                Aromatic,
                Charged,
                Hydrophobic,
                Hydroxylic,
                Negative,
                Polar,
                Positive,
                Small,
                Sulphur,
                Tiny,
            };

            return result;
        }

        public readonly string[] PropertyNames =
        {
            //"Acidic",
            "Aliphatic",
            "Aromatic",
            "Charged",
            "Hydrophobic",
            "Hydroxylic",
            "Negative",
            "Polar",
            "Positive",
            "Small",
            "Sulphur",
            "Tiny",
        };

        public T PropertyFromName(string propertyName)
        {
            switch (propertyName)
            {
                //case "Acidic":
                    //return Acidic;
                case "Aliphatic":
                    return Aliphatic;
                case "Aromatic":
                    return Aromatic;
                case "Charged":
                    return Charged;
                case "Hydrophobic":
                    return Hydrophobic;
                case "Hydroxylic":
                    return Hydroxylic;
                case "Negative":
                    return Negative;
                case "Polar":
                    return Polar;
                case "Positive":
                    return Positive;
                case "Small":
                    return Small;
                case "Sulphur":
                    return Sulphur;
                case "Tiny":
                    return Tiny;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public AminoAcidProperties()
        {
        }

        public AminoAcidProperties(/*T acidic, */T aliphatic, T aromatic, T charged, T hydrophobic, T hydroxylic, T negative, T polar, T positive, T small, T sulphur, T tiny)
        {
            //Acidic = acidic;
            Aliphatic = aliphatic;
            Aromatic = aromatic;
            Charged = charged;
            Hydrophobic = hydrophobic;
            Hydroxylic = hydroxylic;
            Negative = negative;
            Polar = polar;
            Positive = positive;
            Small = small;
            Sulphur = sulphur;
            Tiny = tiny;
        }

        public AminoAcidProperties(int number, string name, string code1L, string code3L, /*T acidic,*/ T aliphatic, T aromatic, T charged, T hydrophobic, T hydroxylic, T negative, T polar, T positive, T small, T sulphur, T tiny) : this(/*acidic,*/ aliphatic, aromatic, charged, hydrophobic, hydroxylic, negative, polar, positive, small, sulphur, tiny)
        {
            Number = number;
            Name = name;
            Code1L = code1L;
            Code3L = code3L;
        }
    }
}