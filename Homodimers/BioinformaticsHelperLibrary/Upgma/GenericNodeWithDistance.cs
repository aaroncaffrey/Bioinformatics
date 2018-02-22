using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinformaticsHelperLibrary.Upgma
{
    public class GenericNodeWithDistance : IEquatable<GenericNodeWithDistance>
    {
        public GenericNode Node = null;
        public decimal DistanceToNode = 0;

        public GenericNodeWithDistance()
        {
            
        }

        public GenericNodeWithDistance(GenericNode node, decimal distanceToNode)
        {
            Node = node;
            DistanceToNode = distanceToNode;
        }

        public bool Equals(GenericNodeWithDistance other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Node, other.Node);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GenericNodeWithDistance) obj);
        }

        public override int GetHashCode()
        {
            return (Node != null ? Node.GetHashCode() : 0);
        }

        public static bool operator ==(GenericNodeWithDistance left, GenericNodeWithDistance right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GenericNodeWithDistance left, GenericNodeWithDistance right)
        {
            return !Equals(left, right);
        }
    }
}
