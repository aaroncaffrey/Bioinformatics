using System;
using System.Collections.Generic;
using BioinformaticsHelperLibrary.Spreadsheets;
using DocumentFormat.OpenXml;

namespace BioinformaticsHelperLibrary.InteractionProteinInterfaceDetection
{
    [Serializable]
    public class FullProteinInterfaceId : IEquatable<FullProteinInterfaceId>
    {
        public string ProteinId = null;
        public int ChainId = -1;
        public int ProteinInterfaceId = -1;
        public int ProteinInterfaceStartIndex = -1;
        public int ProteinInterfaceEndIndex = -1;

        public FullProteinInterfaceId()
        {

        }

        public FullProteinInterfaceId(string proteinId, int chainId, int proteinInterfaceId, int proteinInterfaceStartIndex, int proteinInterfaceEndIndex)
        {
            ProteinId = proteinId;
            ChainId = chainId;
            ProteinInterfaceId = proteinInterfaceId;
            ProteinInterfaceStartIndex = proteinInterfaceStartIndex;
            ProteinInterfaceEndIndex = proteinInterfaceEndIndex;
        }

        public FullProteinInterfaceId(FullProteinInterfaceId fullProteinInterfaceId)
        {
            ProteinId = fullProteinInterfaceId.ProteinId;
            ChainId = fullProteinInterfaceId.ChainId;
            ProteinInterfaceId = fullProteinInterfaceId.ProteinInterfaceId;
            ProteinInterfaceStartIndex = fullProteinInterfaceId.ProteinInterfaceStartIndex;
            ProteinInterfaceEndIndex = fullProteinInterfaceId.ProteinInterfaceEndIndex;
        }

        public bool Equals(FullProteinInterfaceId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ChainId == other.ChainId && string.Equals(ProteinId, other.ProteinId) && ProteinInterfaceId == other.ProteinInterfaceId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FullProteinInterfaceId)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = ChainId;
                hashCode = (hashCode * 397) ^ (ProteinId != null ? ProteinId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ProteinInterfaceId;
                return hashCode;
            }
        }

        public static bool operator ==(FullProteinInterfaceId left, FullProteinInterfaceId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FullProteinInterfaceId left, FullProteinInterfaceId right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return this.ProteinId + "_" + SpreadsheetFileHandler.AlphabetLetterRollOver(this.ChainId) + "_" + SpreadsheetFileHandler.AlphabetLetterRollOver(this.ProteinInterfaceId);

        }
    }


}
