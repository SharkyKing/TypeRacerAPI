﻿using System.Data;
using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class WordsStyleClass
    {
        public int Id { get; set; }
        public string StyleName { get; set; }
        public string? fontStyle { get; set; }
        public string? fontWeight { get; set; }
        public string? fontFamily { get; set; }
        [JsonIgnore]
        public virtual ICollection<PlayerClass> Players { get; set; }

        public List<WordsStyleClass> Filler(DataTable table) {
            List<WordsStyleClass> wordsStyleClasses = new List<WordsStyleClass>();
			foreach (DataRow row in table.Rows)
			{
				WordsStyleClass wordStyle = new WordsStyleClass
				{
					Id = row.Field<int>("Id"),
					StyleName = row.Field<string>("StyleName"),
					fontStyle = row.Field<string>("fontStyle"),
					fontWeight = row.Field<string>("fontWeight"),
					fontFamily = row.Field<string>("fontFamily")
				};

				wordsStyleClasses.Add(wordStyle);
			}
			return wordsStyleClasses;
		}
    }
}
