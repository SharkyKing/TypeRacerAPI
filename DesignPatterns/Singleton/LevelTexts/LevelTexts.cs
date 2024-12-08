using Newtonsoft.Json;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Enum;
using TypeRacerAPI.DesignPatterns.Iterator;
using System.Collections.Generic;

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

		public List<WordsClass> cnv(IterableCollection<WordsClass> iterableCollection)
		{
			List<WordsClass> wordsList = new List<WordsClass>();

			IIterator<WordsClass> iterator = iterableCollection.CreateIterator();
			while (iterator.HasNext())
			{
				wordsList.Add(iterator.Next());
			}

			return wordsList;
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

			IterableCollection<WordsClass> wordsBaseEntries = new IterableCollection<WordsClass>();
			List<WordsClass> words = new List<WordsClass>();

			foreach (var text in texts.Begginner)
			{
				wordsBaseEntries.Add(new WordsClass { GameLevelId = 1, Text = text });
			}

			foreach (var text in texts.Normal)
			{
				wordsBaseEntries.Add(new WordsClass { GameLevelId = 2, Text = text });
			}

			foreach (var text in texts.Advanced)
			{
				wordsBaseEntries.Add(new WordsClass { GameLevelId = 3, Text = text });
			}

			IIterator<WordsClass> iterator = wordsBaseEntries.CreateIterator();
			int id = 1;

			while (iterator.HasNext())
			{
				var word = iterator.Next();
				word.Id = id++;
			}
			return cnv(wordsBaseEntries);
		}
	}
}
