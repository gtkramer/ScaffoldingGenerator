//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /home/george/Documents/Projects/C#/amsg/src/IO/StlAscii.g4 by ANTLR 4.7.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public partial class StlAsciiLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, NAME=9, 
		FLOAT=10, WHITESPACE=11;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "NAME", 
		"FLOAT", "WHITESPACE"
	};


	public StlAsciiLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public StlAsciiLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'solid'", "'endsolid'", "'facet'", "'endfacet'", "'normal'", "'outer loop'", 
		"'endloop'", "'vertex'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, "NAME", "FLOAT", 
		"WHITESPACE"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "StlAscii.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static StlAsciiLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x2', '\r', '\x84', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
		'\x4', '\x3', '\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', 
		'\x5', '\x4', '\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', 
		'\t', '\b', '\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', 
		'\t', '\v', '\x4', '\f', '\t', '\f', '\x3', '\x2', '\x3', '\x2', '\x3', 
		'\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x4', '\x3', '\x4', '\x3', 
		'\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x5', '\x3', 
		'\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', 
		'\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x6', '\x3', '\x6', '\x3', 
		'\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', 
		'\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', 
		'\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', 
		'\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', 
		'\x3', '\b', '\x3', '\b', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', 
		'\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\n', '\x6', '\n', 
		'Z', '\n', '\n', '\r', '\n', '\xE', '\n', '[', '\x3', '\v', '\x5', '\v', 
		'_', '\n', '\v', '\x3', '\v', '\a', '\v', '\x62', '\n', '\v', '\f', '\v', 
		'\xE', '\v', '\x65', '\v', '\v', '\x3', '\v', '\x5', '\v', 'h', '\n', 
		'\v', '\x3', '\v', '\x6', '\v', 'k', '\n', '\v', '\r', '\v', '\xE', '\v', 
		'l', '\x3', '\v', '\x3', '\v', '\x5', '\v', 'q', '\n', '\v', '\x3', '\v', 
		'\x6', '\v', 't', '\n', '\v', '\r', '\v', '\xE', '\v', 'u', '\x5', '\v', 
		'x', '\n', '\v', '\x3', '\f', '\x5', '\f', '{', '\n', '\f', '\x3', '\f', 
		'\x3', '\f', '\x6', '\f', '\x7F', '\n', '\f', '\r', '\f', '\xE', '\f', 
		'\x80', '\x3', '\f', '\x3', '\f', '\x2', '\x2', '\r', '\x3', '\x3', '\x5', 
		'\x4', '\a', '\x5', '\t', '\x6', '\v', '\a', '\r', '\b', '\xF', '\t', 
		'\x11', '\n', '\x13', '\v', '\x15', '\f', '\x17', '\r', '\x3', '\x2', 
		'\x6', '\a', '\x2', '/', '/', '\x32', ';', '\x43', '\\', '\x61', '\x61', 
		'\x63', '|', '\x4', '\x2', '-', '-', '/', '/', '\x3', '\x2', '\x32', ';', 
		'\x5', '\x2', '\v', '\v', '\xE', '\xE', '\"', '\"', '\x2', '\x8E', '\x2', 
		'\x3', '\x3', '\x2', '\x2', '\x2', '\x2', '\x5', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\a', '\x3', '\x2', '\x2', '\x2', '\x2', '\t', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '\v', '\x3', '\x2', '\x2', '\x2', '\x2', '\r', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\xF', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x11', '\x3', '\x2', '\x2', '\x2', '\x2', '\x13', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x15', '\x3', '\x2', '\x2', '\x2', '\x2', '\x17', '\x3', 
		'\x2', '\x2', '\x2', '\x3', '\x19', '\x3', '\x2', '\x2', '\x2', '\x5', 
		'\x1F', '\x3', '\x2', '\x2', '\x2', '\a', '(', '\x3', '\x2', '\x2', '\x2', 
		'\t', '.', '\x3', '\x2', '\x2', '\x2', '\v', '\x37', '\x3', '\x2', '\x2', 
		'\x2', '\r', '>', '\x3', '\x2', '\x2', '\x2', '\xF', 'I', '\x3', '\x2', 
		'\x2', '\x2', '\x11', 'Q', '\x3', '\x2', '\x2', '\x2', '\x13', 'Y', '\x3', 
		'\x2', '\x2', '\x2', '\x15', '^', '\x3', '\x2', '\x2', '\x2', '\x17', 
		'~', '\x3', '\x2', '\x2', '\x2', '\x19', '\x1A', '\a', 'u', '\x2', '\x2', 
		'\x1A', '\x1B', '\a', 'q', '\x2', '\x2', '\x1B', '\x1C', '\a', 'n', '\x2', 
		'\x2', '\x1C', '\x1D', '\a', 'k', '\x2', '\x2', '\x1D', '\x1E', '\a', 
		'\x66', '\x2', '\x2', '\x1E', '\x4', '\x3', '\x2', '\x2', '\x2', '\x1F', 
		' ', '\a', 'g', '\x2', '\x2', ' ', '!', '\a', 'p', '\x2', '\x2', '!', 
		'\"', '\a', '\x66', '\x2', '\x2', '\"', '#', '\a', 'u', '\x2', '\x2', 
		'#', '$', '\a', 'q', '\x2', '\x2', '$', '%', '\a', 'n', '\x2', '\x2', 
		'%', '&', '\a', 'k', '\x2', '\x2', '&', '\'', '\a', '\x66', '\x2', '\x2', 
		'\'', '\x6', '\x3', '\x2', '\x2', '\x2', '(', ')', '\a', 'h', '\x2', '\x2', 
		')', '*', '\a', '\x63', '\x2', '\x2', '*', '+', '\a', '\x65', '\x2', '\x2', 
		'+', ',', '\a', 'g', '\x2', '\x2', ',', '-', '\a', 'v', '\x2', '\x2', 
		'-', '\b', '\x3', '\x2', '\x2', '\x2', '.', '/', '\a', 'g', '\x2', '\x2', 
		'/', '\x30', '\a', 'p', '\x2', '\x2', '\x30', '\x31', '\a', '\x66', '\x2', 
		'\x2', '\x31', '\x32', '\a', 'h', '\x2', '\x2', '\x32', '\x33', '\a', 
		'\x63', '\x2', '\x2', '\x33', '\x34', '\a', '\x65', '\x2', '\x2', '\x34', 
		'\x35', '\a', 'g', '\x2', '\x2', '\x35', '\x36', '\a', 'v', '\x2', '\x2', 
		'\x36', '\n', '\x3', '\x2', '\x2', '\x2', '\x37', '\x38', '\a', 'p', '\x2', 
		'\x2', '\x38', '\x39', '\a', 'q', '\x2', '\x2', '\x39', ':', '\a', 't', 
		'\x2', '\x2', ':', ';', '\a', 'o', '\x2', '\x2', ';', '<', '\a', '\x63', 
		'\x2', '\x2', '<', '=', '\a', 'n', '\x2', '\x2', '=', '\f', '\x3', '\x2', 
		'\x2', '\x2', '>', '?', '\a', 'q', '\x2', '\x2', '?', '@', '\a', 'w', 
		'\x2', '\x2', '@', '\x41', '\a', 'v', '\x2', '\x2', '\x41', '\x42', '\a', 
		'g', '\x2', '\x2', '\x42', '\x43', '\a', 't', '\x2', '\x2', '\x43', '\x44', 
		'\a', '\"', '\x2', '\x2', '\x44', '\x45', '\a', 'n', '\x2', '\x2', '\x45', 
		'\x46', '\a', 'q', '\x2', '\x2', '\x46', 'G', '\a', 'q', '\x2', '\x2', 
		'G', 'H', '\a', 'r', '\x2', '\x2', 'H', '\xE', '\x3', '\x2', '\x2', '\x2', 
		'I', 'J', '\a', 'g', '\x2', '\x2', 'J', 'K', '\a', 'p', '\x2', '\x2', 
		'K', 'L', '\a', '\x66', '\x2', '\x2', 'L', 'M', '\a', 'n', '\x2', '\x2', 
		'M', 'N', '\a', 'q', '\x2', '\x2', 'N', 'O', '\a', 'q', '\x2', '\x2', 
		'O', 'P', '\a', 'r', '\x2', '\x2', 'P', '\x10', '\x3', '\x2', '\x2', '\x2', 
		'Q', 'R', '\a', 'x', '\x2', '\x2', 'R', 'S', '\a', 'g', '\x2', '\x2', 
		'S', 'T', '\a', 't', '\x2', '\x2', 'T', 'U', '\a', 'v', '\x2', '\x2', 
		'U', 'V', '\a', 'g', '\x2', '\x2', 'V', 'W', '\a', 'z', '\x2', '\x2', 
		'W', '\x12', '\x3', '\x2', '\x2', '\x2', 'X', 'Z', '\t', '\x2', '\x2', 
		'\x2', 'Y', 'X', '\x3', '\x2', '\x2', '\x2', 'Z', '[', '\x3', '\x2', '\x2', 
		'\x2', '[', 'Y', '\x3', '\x2', '\x2', '\x2', '[', '\\', '\x3', '\x2', 
		'\x2', '\x2', '\\', '\x14', '\x3', '\x2', '\x2', '\x2', ']', '_', '\t', 
		'\x3', '\x2', '\x2', '^', ']', '\x3', '\x2', '\x2', '\x2', '^', '_', '\x3', 
		'\x2', '\x2', '\x2', '_', '\x63', '\x3', '\x2', '\x2', '\x2', '`', '\x62', 
		'\t', '\x4', '\x2', '\x2', '\x61', '`', '\x3', '\x2', '\x2', '\x2', '\x62', 
		'\x65', '\x3', '\x2', '\x2', '\x2', '\x63', '\x61', '\x3', '\x2', '\x2', 
		'\x2', '\x63', '\x64', '\x3', '\x2', '\x2', '\x2', '\x64', 'g', '\x3', 
		'\x2', '\x2', '\x2', '\x65', '\x63', '\x3', '\x2', '\x2', '\x2', '\x66', 
		'h', '\a', '\x30', '\x2', '\x2', 'g', '\x66', '\x3', '\x2', '\x2', '\x2', 
		'g', 'h', '\x3', '\x2', '\x2', '\x2', 'h', 'j', '\x3', '\x2', '\x2', '\x2', 
		'i', 'k', '\t', '\x4', '\x2', '\x2', 'j', 'i', '\x3', '\x2', '\x2', '\x2', 
		'k', 'l', '\x3', '\x2', '\x2', '\x2', 'l', 'j', '\x3', '\x2', '\x2', '\x2', 
		'l', 'm', '\x3', '\x2', '\x2', '\x2', 'm', 'w', '\x3', '\x2', '\x2', '\x2', 
		'n', 'p', '\a', 'g', '\x2', '\x2', 'o', 'q', '\t', '\x3', '\x2', '\x2', 
		'p', 'o', '\x3', '\x2', '\x2', '\x2', 'p', 'q', '\x3', '\x2', '\x2', '\x2', 
		'q', 's', '\x3', '\x2', '\x2', '\x2', 'r', 't', '\t', '\x4', '\x2', '\x2', 
		's', 'r', '\x3', '\x2', '\x2', '\x2', 't', 'u', '\x3', '\x2', '\x2', '\x2', 
		'u', 's', '\x3', '\x2', '\x2', '\x2', 'u', 'v', '\x3', '\x2', '\x2', '\x2', 
		'v', 'x', '\x3', '\x2', '\x2', '\x2', 'w', 'n', '\x3', '\x2', '\x2', '\x2', 
		'w', 'x', '\x3', '\x2', '\x2', '\x2', 'x', '\x16', '\x3', '\x2', '\x2', 
		'\x2', 'y', '{', '\a', '\xF', '\x2', '\x2', 'z', 'y', '\x3', '\x2', '\x2', 
		'\x2', 'z', '{', '\x3', '\x2', '\x2', '\x2', '{', '|', '\x3', '\x2', '\x2', 
		'\x2', '|', '\x7F', '\a', '\f', '\x2', '\x2', '}', '\x7F', '\t', '\x5', 
		'\x2', '\x2', '~', 'z', '\x3', '\x2', '\x2', '\x2', '~', '}', '\x3', '\x2', 
		'\x2', '\x2', '\x7F', '\x80', '\x3', '\x2', '\x2', '\x2', '\x80', '~', 
		'\x3', '\x2', '\x2', '\x2', '\x80', '\x81', '\x3', '\x2', '\x2', '\x2', 
		'\x81', '\x82', '\x3', '\x2', '\x2', '\x2', '\x82', '\x83', '\b', '\f', 
		'\x2', '\x2', '\x83', '\x18', '\x3', '\x2', '\x2', '\x2', '\xE', '\x2', 
		'[', '^', '\x63', 'g', 'l', 'p', 'u', 'w', 'z', '~', '\x80', '\x3', '\b', 
		'\x2', '\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
