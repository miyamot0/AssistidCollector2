using System;
using SQLite;

namespace AssistidCollector2.Storage
{
    public class SocialValidityModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int TaskNumber { get; set; }

        public int Rating { get; set; }

        public string Base64 { get; set; }
    }
}
