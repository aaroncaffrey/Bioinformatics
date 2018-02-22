using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace BioinformaticsHelperLibrary.ProproteinInterface
{
    [Serializable]
    [XmlSerializerFormat]
    [DataContract(Name = "match", Namespace = "")]
    public class ProproteinInterfaceMatchSetMatch
    {
        [DataMember(Name = "sequence_ac")]
        [XmlElement("sequence_ac")]
        public string SequenceAc;

        [DataMember(Name = "sequence_id")]
        [XmlElement("sequence_id")]
        public string SequenceId;

        [DataMember(Name = "sequence_db")]
        [XmlElement("sequence_db")]
        public string SequenceDb;

        [DataMember(Name = "start")]
        [XmlElement("start")]
        public string Start;

        [DataMember(Name = "stop")]
        [XmlElement("stop")]
        public string Stop;

        [DataMember(Name = "signature_ac")]
        [XmlElement("signature_ac")]
        public string SignatureAc;

        [DataMember(Name = "signature_id")]
        [XmlElement("signature_id")]
        public string SignatureId;

        [DataMember(Name = "level_tag")]
        [XmlElement("level_tag")]
        public string LevelTag;

        [DataMember(Name = "score")]
        [XmlElement("score")]
        public string Score;

        [DataMember(Name = "level")]
        [XmlElement("level")]
        public string Level;
    }
}
