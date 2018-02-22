using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioinformaticsHelperLibrary.InteractionSiteDetection;

namespace BioinformaticsHelperLibrary.InteractionVector
{
    public class InteractionVectorDictionaryContainer
    {
        public Dictionary<FullSiteId, VectorSiteWhole> InteractionVectorDictionary = new Dictionary<FullSiteId, VectorSiteWhole>();
    }
}
