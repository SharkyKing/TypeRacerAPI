using Newtonsoft.Json;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Enum;

namespace TypeRacerAPI.DesignPatterns.Singleton.LevelTexts
{
    public class LevelTexts
    {
        private const string filePath = "textLevels.json";

        private static LevelTexts? _instance;

        private static readonly object _lock = new object();

        private Texts texts;

        public static LevelTexts GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new LevelTexts();
                    }
                }
            }

            return _instance;
        }

        public string GetText(GameLevel level, AppDbContext _context)
        {
            List<WordsClass> selectedList;

            switch (level)
            {
                case GameLevel.Beginner:
                    selectedList = _context.Words.Where(w => w.GameLevelId == 1).ToList();
                    break;
                case GameLevel.Normal:
                    selectedList = _context.Words.Where(w => w.GameLevelId == 2).ToList();
                    break;
                case GameLevel.Advanced:
                    selectedList = _context.Words.Where(w => w.GameLevelId == 3).ToList();
                    break;
                default:
                    selectedList = _context.Words.Where(w => w.GameLevelId == 1).ToList();
                    break;
            }

            Random random = new Random();
            int randomIndex = random.Next(selectedList.Count);
            return selectedList[randomIndex].Text;
        }
        private Texts ReadJsonFile(string filePath)
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

        public List<WordsClass> LoadTextsFromJson()
        {
            var texts = ReadJsonFile(filePath);
            if (texts == null) throw new InvalidOperationException("Failed to load texts.");

            List<WordsClass> wordsBaseEntries = new List<WordsClass>();

            wordsBaseEntries.AddRange(texts.Begginner.Select(text => new WordsClass { GameLevelId = 1, Text = text }));
            wordsBaseEntries.AddRange(texts.Normal.Select(text => new WordsClass { GameLevelId = 2, Text = text }));
            wordsBaseEntries.AddRange(texts.Advanced.Select(text => new WordsClass { GameLevelId = 3, Text = text }));

            for (int i = 1; i < wordsBaseEntries.Count + 1; i++)
            {
                wordsBaseEntries[i - 1].Id = i;
            }

            return wordsBaseEntries;
        }
    }
}
