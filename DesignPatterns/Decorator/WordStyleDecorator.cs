namespace TypeRacerAPI.DesignPatterns.Decorator
{
	public class WordStyleDecorator
	{
		public string Text { get; set; }

		public WordStyleDecorator(string text)
		{
			Text = text;
		}

		public virtual string GetStyledText()
		{
			return Text;
		}
	}
	public class FontStyleDecorator : WordStyleDecorator
	{
		private readonly WordStyleDecorator _word;
		private readonly string _fontStyle;

		public FontStyleDecorator(WordStyleDecorator word, string fontStyle) : base(word.Text)
		{
			_word = word;
			_fontStyle = fontStyle;
		}

		public override string GetStyledText()
		{
			return $"font-style:{_fontStyle}";
		}
	}
	public class FontWeightDecorator : WordStyleDecorator
	{
		private readonly WordStyleDecorator _word;
		private readonly string _fontWeight;

		public FontWeightDecorator(WordStyleDecorator word, string fontWeight) : base(word.Text)
		{
			_word = word;
			_fontWeight = fontWeight;
		}

		public override string GetStyledText()
		{
			return $"font-weight:{_fontWeight}";
		}
	}
	public class FontFamilyDecorator : WordStyleDecorator
	{
		private readonly WordStyleDecorator _word;
		private readonly string _fontFamily;

		public FontFamilyDecorator(WordStyleDecorator word, string fontFamily) : base(word.Text)
		{
			_word = word;
			_fontFamily = fontFamily;
		}

		public override string GetStyledText()
		{
			return $"font-family:{_fontFamily}";
		}
	}



}
