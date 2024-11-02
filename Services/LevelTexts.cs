using Newtonsoft.Json;

namespace TypeRacerAPI.Services
{
    public static class LevelTexts
    {
        private const string filePath = "textLevels.json";

        private class Texts
        {
            public List<string> Begginner { get; set; }
            public List<string> Normal { get; set; }
            public List<string> Advanced { get; set; }
        }

        private static Texts texts;

        static LevelTexts()
        {
            texts = ReadJsonFile(filePath);
        }
        public static string GetText(LevelText level)
        {
            List<string> selectedList;

            switch (level)
            {
                case LevelText.Beginner:
                    selectedList = texts.Begginner;
                    break;
                case LevelText.Normal:
                    selectedList = texts.Normal;
                    break;
                case LevelText.Advanced:
                    selectedList = texts.Advanced;
                    break;
                default:
                    throw new ArgumentException("Invalid level specified.");
            }

            if (selectedList == null || selectedList.Count == 0)
            {
                throw new InvalidOperationException("No texts available for this level.");
            }

            Random random = new Random();
            int randomIndex = random.Next(selectedList.Count);
            return selectedList[randomIndex];
        }
        private static Texts ReadJsonFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Texts>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading the JSON file: {ex.Message}");
                return null;
            }
        }

        public enum LevelText
        {
            Beginner, Normal, Advanced
        }
    }
}
