using Castle.Components.DictionaryAdapter.Xml;
using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.Data
{
    public static class SeedData
    {
        // Game Level Seed Data
        public static List<GameLevelClass> GameLevelsSeed = new List<GameLevelClass>
        {
            new GameLevelClass { Id = 1, GameLevelName = "Beginner" },
            new GameLevelClass { Id = 2, GameLevelName = "Normal" },
            new GameLevelClass { Id = 3, GameLevelName = "Advanced" }
        };

        // Player Game Result Type Seed Data
        public static List<PlayerGameResultTypeClass> PlayerGameResultTypesSeed = new List<PlayerGameResultTypeClass>
        {
            new PlayerGameResultTypeClass { Id = 1, Title = "You WON!", Text = "Congratulations!", GifUrl = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExOGkweWlweTBuanJjeWN0d2xna3R2YzJ0YWVoZTRkNmZhMTV5MjZrayZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/xT0GqssRweIhlz209i/giphy.gif" },
            new PlayerGameResultTypeClass { Id = 2, Title = "You lost :(", Text = "Better luck next time", GifUrl = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExNnBpM3dvYjgyYWdhaXJ0dzk3M2NkY3U3NzVzdzExamd6N2VkYTYweiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/B4uP3h97Hi2UaqS0E3/giphy.gif" },
            new PlayerGameResultTypeClass { Id = 3, Title = "Nobody won this game", Text = "Be faster next time!", GifUrl = "https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExNnBpM3dvYjgyYWdhaXJ0dzk3M2NkY3U3NzVzdzExamd6N2VkYTYweiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/B4uP3h97Hi2UaqS0E3/giphy.gif" }
        };

        // Log Type Seed Data
        public static List<LogTypeClass> LogTypesSeed = new List<LogTypeClass>
        {
            new LogTypeClass { Id = 1, LogTypeName = "Error" },
            new LogTypeClass { Id = 2, LogTypeName = "GameLog" },
            new LogTypeClass { Id = 3, LogTypeName = "Information" }
        };

        // Game Type Seed Data
        public static List<GameTypeClass> GameTypesSeed = new List<GameTypeClass>
        {
            new GameTypeClass { Id = 1, GameTypeName = "TimeAttack" },
            new GameTypeClass { Id = 2, GameTypeName = "FluentType" }
        };

        // Player Power Seed Data
        public static List<PlayerPowerClass> PlayerPowersSeed = new List<PlayerPowerClass>
        {
            new PlayerPowerClass { Id = 1, PlayerPowerName = "Freeze", PlayerPowerKey = "F", ImagePath = "/images/freeze.png", CooldownTime = 10, IsTimedPower = true },
            new PlayerPowerClass { Id = 2, PlayerPowerName = "Rewind", PlayerPowerKey = "R", ImagePath = "/images/rewind.png", CooldownTime = 5, IsTimedPower = false },
            new PlayerPowerClass { Id = 3, PlayerPowerName = "Invisible", PlayerPowerKey = "I", ImagePath = "/images/invisible.png", CooldownTime = 15, IsTimedPower = true }
        };

        // Words Style Seed Data
        public static List<WordsStyleClass> WordsStylesSeed = new List<WordsStyleClass>
        {
            new WordsStyleClass { Id = 1, StyleName = "BoldDecorator", fontFamily = "Arial, sans-serif", fontWeight = "bold", fontStyle = null },
            new WordsStyleClass { Id = 2, StyleName = "ItalicDecorator", fontFamily = "Georgia, serif", fontWeight = null, fontStyle = null },
            new WordsStyleClass { Id = 3, StyleName = "FancyFontDecorator", fontFamily = "Courier New, monospace", fontWeight = "normal", fontStyle = "normal" }
        };

        public static List<WordsClass> WordsSeed = new List<WordsClass>()
        {
            new WordsClass { Id = 1, GameLevelId = 1, Text = "She goes to the zoo. She sees a lion. ..." },
            new WordsClass { Id = 2, GameLevelId = 1, Text = "She sits in the car. Her dad turns on the radio. ..." },
            new WordsClass { Id = 3, GameLevelId = 1, Text = "Today is Father's Day. Daniel surprises his father. ..." },
            new WordsClass { Id = 4, GameLevelId = 1, Text = "She eats a slice of cake. She drops a crumb. ..." },
            new WordsClass { Id = 5, GameLevelId = 1, Text = "He goes to the petting zoo. There are many different animals. ..." },
            new WordsClass { Id = 6, GameLevelId = 2, Text = "In the resplendent glow of the cerulean twilight, she contemplated ..." },
            new WordsClass { Id = 7, GameLevelId = 2, Text = "The dichotomy between epistemology and ontology presents a formidable ..." },
            new WordsClass { Id = 8, GameLevelId = 2, Text = "In the realm of astrophysics, the phenomenon of black holes ..." },
            new WordsClass { Id = 9, GameLevelId = 2, Text = "The ramifications of the Industrial Revolution were far-reaching ..." },
            new WordsClass { Id = 10, GameLevelId = 2, Text = "The painter's oeuvre is a veritable tapestry of emotive expression ..." },
            new WordsClass { Id = 11, GameLevelId = 3, Text = "In Lithuanian culture, tradicijos play a vital role in the community ..." },
            new WordsClass { Id = 12, GameLevelId = 3, Text = "When contemplating existence, many ask, \"Koks yra tikrasis zmogaus prigimtis?\" ..." },
            new WordsClass { Id = 13, GameLevelId = 3, Text = "Climate change is a pressing global issue, and Lithuania is no exception ..." },
            new WordsClass { Id = 14, GameLevelId = 3, Text = "Lithuanian authors often reflect social and political changes ..." }
        };
    }
}
