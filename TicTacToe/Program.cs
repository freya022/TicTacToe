using System;
using static System.ConsoleKey;

namespace TicTacToe {
	internal class GameArray {
		internal int Size { get; }
		internal int Gaps => Size - 1;

		private readonly int[] _points;

		//Size is actually the size of a side
		internal GameArray(int size) {
			Size = size;
			_points = new int[size * size];

			Array.Fill(_points, 0);
		}

		internal int Get(int x, int y) {
			if (x >= Size || y >= Size || x < 0 || y < 0) return int.MaxValue; //Defined behavior for invalid values when checking for win conditions
			return _points[x + y * Size];
		}

		internal void Set(int x, int y, int point) {
			_points[x + y * Size] = point;
		}
	}
	
	internal static class Program {
		private static GameArray _gameArray;

		private static void Main() {
			Console.WriteLine("Welcome to Tic Tac Toe");

			int size;
			while (true) {
				Console.Write("What size of game do you want ? ");

				var isNumber = int.TryParse(Console.ReadLine(), out size);

				if (isNumber && size >= 3 && size < 30)
					break;
			}

			_gameArray = new GameArray(size);
			
			Console.Clear();

			var player = 1; //1 = X, -1 = O

			int x = 0, y = 0;
			while (true) {
				PrintGame();

				Console.WriteLine("\nPlayer 1 = X\t\tPlayer 2 = O");
				Console.WriteLine("Player " + GetPlayerNumber(player) + " can play. Select the slot you want to play on");
				
				while (true) {
					Console.SetCursorPosition(x, y);
				
					var key = Console.ReadKey(true).Key; //No need to redraw the game with this ! (no intercept = key overwrites char)

					if (key == Enter) {
						var realX = x / 2;
						var realY = y / 2;

						if (_gameArray.Get(realX, realY) == 0) {
							_gameArray.Set(realX, realY, player);

							break;
						}
					}

					if (key == UpArrow) {
						y = Math.Max(y - 2, 0);
					} else if (key == DownArrow) {
						y = Math.Min(y + 2, _gameArray.Size + _gameArray.Gaps - 1); //-1 to not select 1 too further
					} else if (key == LeftArrow) {
						x = Math.Max(x - 2, 0);
					} else if (key == RightArrow) {
						x = Math.Min(x + 2, _gameArray.Size + _gameArray.Gaps - 1); //-1 to not select 1 too further
					}
				}

				var won = CheckGame(player);

				if (won) {
					PrintGame(true);
					
					Console.WriteLine("\n\nPlayer " + GetPlayerNumber(player) + " won !");
					
					break;
				}

				if (!IsPlayable()) {
					PrintGame(true);
					
					Console.WriteLine("\n\nNobody won !");
					
					break;
				}

				player = -player;
			}
		}

		private static bool IsPlayable() {
			for (var y = 0; y < _gameArray.Size; y++) {
				for (var x = 0; x < _gameArray.Size; x++) {
					if (_gameArray.Get(x, y) == 0) { //Not played yet
						return true;
					}
				}
			}

			return false;
		}

		private static string GetPlayerNumber(int player) {
			return player == 1 ? "1" : "2";
		}

		private static bool CheckGame(int player) {
			for (var y = 0; y < _gameArray.Size; y++) {
				for (var x = 0; x < _gameArray.Size; x++) {
					if (CheckStraightRight(x, y, player) || CheckStraightDown(x, y, player) || CheckDiagonalRight(x, y, player) || CheckDiagonalLeft(x, y, player)) {
						return true;
					}
				}
			}

			return false;
		}
		
		private static bool CheckStraightRight(int x, int y, int player) {
			return _gameArray.Get(x, y) == player
			       && _gameArray.Get(x + 1, y) == player
			       && _gameArray.Get(x + 2, y) == player;
		}
		
		private static bool CheckStraightDown(int x, int y, int player) {
			return _gameArray.Get(x, y) == player
			       && _gameArray.Get(x, y + 1) == player
			       && _gameArray.Get(x, y + 2) == player;
		}

		private static bool CheckDiagonalRight(int x, int y, int player) {
			return _gameArray.Get(x, y) == player
			       && _gameArray.Get(x + 1, y + 1) == player
			       && _gameArray.Get(x + 2, y + 2) == player;
		}
		
		private static bool CheckDiagonalLeft(int x, int y, int player) {
			return _gameArray.Get(x, y) == player
			       && _gameArray.Get(x - 1, y + 1) == player
			       && _gameArray.Get(x - 2, y + 2) == player;
		}

		private static void PrintGame(bool forceClear = false) {
			var cursorTop = Console.CursorTop;
			var cursorLeft = Console.CursorLeft;

			if (forceClear || cursorTop > _gameArray.Size * 2 - 2 || cursorLeft > _gameArray.Size * 2 - 2) { //Clear if text larger than the play area has been displayed
				Console.Clear();
			} else {
				Console.SetCursorPosition(0, 0);
			}

			for (var y = 0; y < _gameArray.Size; y++) {
				for (var x = 0; x < _gameArray.Size; x++) {
					var playerNum = _gameArray.Get(x, y);
					var player = playerNum switch {
						-1 => "0",
						1 => "X",
						0 => " ",
						_ => "E"
					};

					if (x != _gameArray.Size - 1) {
						Console.Write(player + "|");
					} else {
						Console.Write(player);
					}
				}

				if (y != _gameArray.Size - 1) {
					Console.Write("\n" + new string('-', _gameArray.Size + _gameArray.Gaps) + "\n");
				}
			}
		}
	}
}