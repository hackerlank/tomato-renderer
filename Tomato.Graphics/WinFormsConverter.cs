using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tomato.Graphics
{
	public static class WinFormsConverter
	{
		public static Keys GetKey( KeyboardKeys key )
		{
			switch( key )
			{
				case KeyboardKeys.A: return Keys.A;
				case KeyboardKeys.B: return Keys.B;
				case KeyboardKeys.C: return Keys.C;
				case KeyboardKeys.D: return Keys.D;
				case KeyboardKeys.E: return Keys.E;
				case KeyboardKeys.F: return Keys.F;
				case KeyboardKeys.G: return Keys.G;
				case KeyboardKeys.H: return Keys.H;
				case KeyboardKeys.I: return Keys.I;
				case KeyboardKeys.J: return Keys.J;
				case KeyboardKeys.K: return Keys.K;
				case KeyboardKeys.L: return Keys.L;
				case KeyboardKeys.M: return Keys.M;
				case KeyboardKeys.N: return Keys.N;
				case KeyboardKeys.O: return Keys.O;
				case KeyboardKeys.P: return Keys.P;
				case KeyboardKeys.Q: return Keys.Q;
				case KeyboardKeys.R: return Keys.R;
				case KeyboardKeys.S: return Keys.S;
				case KeyboardKeys.T: return Keys.T;
				case KeyboardKeys.U: return Keys.U;
				case KeyboardKeys.V: return Keys.V;
				case KeyboardKeys.W: return Keys.W;
				case KeyboardKeys.X: return Keys.X;
				case KeyboardKeys.Y: return Keys.Y;
				case KeyboardKeys.Z: return Keys.Z;
				case KeyboardKeys.Shift: return Keys.ShiftKey;
				case KeyboardKeys.Control: return Keys.ShiftKey | Keys.LButton;
				case KeyboardKeys.Alt: return Keys.ShiftKey | Keys.RButton;
				case KeyboardKeys.Tab: return Keys.Tab;
				case KeyboardKeys.Enter: return Keys.Enter;
				case KeyboardKeys.Space: return Keys.Space;
				case KeyboardKeys.F1: return Keys.F1;
				case KeyboardKeys.F2: return Keys.F2;
				case KeyboardKeys.F3: return Keys.F3;
				case KeyboardKeys.F4: return Keys.F4;
				case KeyboardKeys.F5: return Keys.F5;
				case KeyboardKeys.F6: return Keys.F6;
				case KeyboardKeys.F7: return Keys.F7;
				case KeyboardKeys.F8: return Keys.F8;
				case KeyboardKeys.F9: return Keys.F9;
				case KeyboardKeys.F10: return Keys.F10;
				case KeyboardKeys.F11: return Keys.F11;
				case KeyboardKeys.F12: return Keys.F12;
				case KeyboardKeys.Number0: return Keys.NumPad0;
				case KeyboardKeys.Number1: return Keys.NumPad1;
				case KeyboardKeys.Number2: return Keys.NumPad2;
				case KeyboardKeys.Number3: return Keys.NumPad3;
				case KeyboardKeys.Number4: return Keys.NumPad4;
				case KeyboardKeys.Number5: return Keys.NumPad5;
				case KeyboardKeys.Number6: return Keys.NumPad6;
				case KeyboardKeys.Number7: return Keys.NumPad7;
				case KeyboardKeys.Number8: return Keys.NumPad8;
				case KeyboardKeys.Number9: return Keys.NumPad9;
				case KeyboardKeys.Plus: return Keys.Oemplus;
				case KeyboardKeys.Minus: return Keys.OemMinus;
				case KeyboardKeys.Pipe: return Keys.OemPipe;
				case KeyboardKeys.Question: return Keys.OemQuestion;
				case KeyboardKeys.Quotes: return Keys.OemQuotes;
				case KeyboardKeys.Semicolon: return Keys.OemSemicolon;
				case KeyboardKeys.Tilde: return Keys.Oemtilde;
				case KeyboardKeys.Backslash: return Keys.OemBackslash;
				case KeyboardKeys.OpenBrackets: return Keys.OemOpenBrackets;
				case KeyboardKeys.CloseBrackets: return Keys.OemCloseBrackets;
				case KeyboardKeys.Period: return Keys.OemPeriod;
				case KeyboardKeys.Comma: return Keys.Oemcomma;
				case KeyboardKeys.Insert: return Keys.Insert;
				case KeyboardKeys.Delete: return Keys.Delete;
				case KeyboardKeys.Home: return Keys.Home;
				case KeyboardKeys.End: return Keys.End;
				case KeyboardKeys.PageUp: return Keys.PageUp;
				case KeyboardKeys.PageDown: return Keys.PageDown;
				case KeyboardKeys.UpArrow: return Keys.Up;
				case KeyboardKeys.DownArrow: return Keys.Down;
				case KeyboardKeys.RightArrow: return Keys.Right;
				case KeyboardKeys.LeftArrow: return Keys.Left;
			}

			return Keys.None;
		}

		public static KeyboardKeys GetKey( Keys key )
		{
			switch( key )
			{
				case Keys.A: return KeyboardKeys.A;
				case Keys.B: return KeyboardKeys.B;
				case Keys.C: return KeyboardKeys.C;
				case Keys.D: return KeyboardKeys.D;
				case Keys.E: return KeyboardKeys.E;
				case Keys.F: return KeyboardKeys.F;
				case Keys.G: return KeyboardKeys.G;
				case Keys.H: return KeyboardKeys.H;
				case Keys.I: return KeyboardKeys.I;
				case Keys.J: return KeyboardKeys.J;
				case Keys.K: return KeyboardKeys.K;
				case Keys.L: return KeyboardKeys.L;
				case Keys.M: return KeyboardKeys.M;
				case Keys.N: return KeyboardKeys.N;
				case Keys.O: return KeyboardKeys.O;
				case Keys.P: return KeyboardKeys.P;
				case Keys.Q: return KeyboardKeys.Q;
				case Keys.R: return KeyboardKeys.R;
				case Keys.S: return KeyboardKeys.S;
				case Keys.T: return KeyboardKeys.T;
				case Keys.U: return KeyboardKeys.U;
				case Keys.V: return KeyboardKeys.V;
				case Keys.W: return KeyboardKeys.W;
				case Keys.X: return KeyboardKeys.X;
				case Keys.Y: return KeyboardKeys.Y;
				case Keys.Z: return KeyboardKeys.Z;
				case Keys.ShiftKey: return KeyboardKeys.Shift;
				case Keys.ShiftKey | Keys.LButton: return KeyboardKeys.Control;
				case Keys.ShiftKey | Keys.RButton: return KeyboardKeys.Alt;
				case Keys.Tab: return KeyboardKeys.Tab;
				case Keys.Enter: return KeyboardKeys.Enter;
				case Keys.Space: return KeyboardKeys.Space;
				case Keys.F1: return KeyboardKeys.F1;
				case Keys.F2: return KeyboardKeys.F2;
				case Keys.F3: return KeyboardKeys.F3;
				case Keys.F4: return KeyboardKeys.F4;
				case Keys.F5: return KeyboardKeys.F5;
				case Keys.F6: return KeyboardKeys.F6;
				case Keys.F7: return KeyboardKeys.F7;
				case Keys.F8: return KeyboardKeys.F8;
				case Keys.F9: return KeyboardKeys.F9;
				case Keys.F10: return KeyboardKeys.F10;
				case Keys.F11: return KeyboardKeys.F11;
				case Keys.F12: return KeyboardKeys.F12;
				case Keys.NumPad0: return KeyboardKeys.Number0;
				case Keys.NumPad1: return KeyboardKeys.Number1;
				case Keys.NumPad2: return KeyboardKeys.Number2;
				case Keys.NumPad3: return KeyboardKeys.Number3;
				case Keys.NumPad4: return KeyboardKeys.Number4;
				case Keys.NumPad5: return KeyboardKeys.Number5;
				case Keys.NumPad6: return KeyboardKeys.Number6;
				case Keys.NumPad7: return KeyboardKeys.Number7;
				case Keys.NumPad8: return KeyboardKeys.Number8;
				case Keys.NumPad9: return KeyboardKeys.Number9;
				case Keys.Oemplus: return KeyboardKeys.Plus;
				case Keys.OemMinus: return KeyboardKeys.Minus;
				case Keys.OemPipe: return KeyboardKeys.Pipe;
				case Keys.OemQuestion: return KeyboardKeys.Question;
				case Keys.OemQuotes: return KeyboardKeys.Quotes;
				case Keys.OemSemicolon: return KeyboardKeys.Semicolon;
				case Keys.Oemtilde: return KeyboardKeys.Tilde;
				case Keys.OemBackslash: return KeyboardKeys.Backslash;
				case Keys.OemOpenBrackets: return KeyboardKeys.OpenBrackets;
				case Keys.OemCloseBrackets: return KeyboardKeys.CloseBrackets;
				case Keys.OemPeriod: return KeyboardKeys.Period;
				case Keys.Oemcomma: return KeyboardKeys.Comma;
				case Keys.Insert: return KeyboardKeys.Insert;
				case Keys.Delete: return KeyboardKeys.Delete;
				case Keys.Home: return KeyboardKeys.Home;
				case Keys.End: return KeyboardKeys.End;
				case Keys.PageUp: return KeyboardKeys.PageUp;
				case Keys.PageDown: return KeyboardKeys.PageDown;
				case Keys.Up: return KeyboardKeys.UpArrow;
				case Keys.Down: return KeyboardKeys.DownArrow;
				case Keys.Right: return KeyboardKeys.RightArrow;
				case Keys.Left: return KeyboardKeys.LeftArrow;
			}

			return KeyboardKeys.None;
		}
	}
}
