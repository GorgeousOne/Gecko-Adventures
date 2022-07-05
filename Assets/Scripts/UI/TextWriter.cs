
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager class 
/// </summary>
public class TextWriter : MonoBehaviour {
	
	private static TextWriter instance;
	private List<TextWriterSingle> textWriters;

	private TextWriter() {
		instance = this;
	}

	private void Update() {
		for (int i = 0; i < textWriters.Count; ++i) {
			if (textWriters[i].Update()) {
				textWriters.RemoveAt(i--); 
			}
		}
	}

	private void AddWriter(Text uiText, string textToWrite, float timePerChar) {
		RemoveWriter(uiText);
		textWriters.Add(new TextWriterSingle(uiText, textToWrite, timePerChar));
	}

	private void RemoveWriter(Text uiText) {
		foreach (TextWriterSingle writer in textWriters) {
			if (writer.GetUiText()) {
				textWriters.Remove(writer);
				return;
			}
		}
	}
	
	public static void AddWriterStatic(Text uiText, string textToWrite, float timePerChar) {
		instance.AddWriter(uiText, textToWrite, timePerChar);
	}
	
	/// <summary>
	/// Class with the task to reveal a text inside a ui text element char by char over time
	/// </summary>
	public class TextWriterSingle {
		
		private Text uiText;
		private string textToWrite;
		private int charIndex;
		private float timePerChar;
		private float timer;

		public TextWriterSingle(Text uiText, string textToWrite, float timePerChar) {
			this.uiText = uiText;
			this.textToWrite = textToWrite;
			this.timePerChar = timePerChar;
		}

		public Text GetUiText() {
			return uiText;
		}

		public void WriteAll() {
			uiText.text = textToWrite;
			charIndex = textToWrite.Length;
		}
		
		public bool IsActive() {
			return charIndex < textToWrite.Length;
		}
		
		/// <summary>
		/// Advances the displaying of text char by char in relation to time
		/// </summary>
		/// <returns>True if no more characters are left to display</returns>
		public bool Update() { 
			timer -= Time.deltaTime;
			
			while (timer <= 0f) {
				timer += timePerChar;
				++charIndex;

				string text = textToWrite[..charIndex] + "<color=#00000000>" + textToWrite[charIndex..] + "</color>";
				uiText.text = text;

				if (charIndex >= textToWrite.Length) {
					return true;
				}
			}
			return false;
		}
	}
}
