using SQLite;

namespace AssistidCollector2.Storage
{
    public class SocialStepModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int YIndex { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgBytes { get; set; }

        public int TaskType { get; set; }

    }
}
