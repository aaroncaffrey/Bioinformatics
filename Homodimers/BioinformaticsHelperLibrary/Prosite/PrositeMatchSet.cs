using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ServiceModel;

namespace BioinformaticsHelperLibrary.ProproteinInterface
{
    [Serializable]
    [XmlSerializerFormat]
    [DataContract(Name = "matchset", Namespace = "urn:expasy:scanproproteinInterface")]
    [XmlRoot(ElementName = "matchset", Namespace = "urn:expasy:scanproproteinInterface")]
    public class ProproteinInterfaceMatchSet
    {
        [DataMember(Name = "match")]
        [XmlElement("match")]
        public ProproteinInterfaceMatchSetMatch[] Match;

        [DataMember(Name = "n_match", IsRequired = true)]
        [XmlAttribute("n_match")]
        public string NMatch;

        [DataMember(Name = "n_seq", IsRequired = true)]
        [XmlAttribute("n_seq")]
        public string NSeq;
    }
}
