//
// O Programa recebe as coordenadas em Graus Minutos Segundos e converte para Graus Decimais
// ou recebe as coordenadas em Graus Decimais e converte em Graus Minutos Segundos
//

//longitude O - L +
//latitude  S - N +

using System;

namespace Program
{
	class Program
	{
		static void Main(string[] args)
		{
			char input;
			bool end = false;
			int option;
			
			while (!end)
			{
				do
				{
					Console.Clear();
					Centraliza("Conversor de Coordenadas");
					Centraliza("[1] GMS -> GD");
					Centraliza(" [2] GD  -> GMS");
					Centraliza(" [0] SAIR     ");
				
					input = Console.ReadKey(true).KeyChar;
				
					if ( input == '0' )
					{
						end = true;
						option = -1;
						break;
					}
				
					int.TryParse(input.ToString(), out option);
				} while ( option < 1 || option > 2 );
				
				switch (option)
				{
					case 1:
						GmsToGd();
						break;
					
					case 2:
						GdToGms();
						break;
					
					default:
						//block
						break;
				}
			
		}	// while(true)
		
		//Console.WriteLine();
		Console.Clear();
		Centraliza("Fim do Programa");
		Centraliza("[ENTER]");
		while( Console.ReadKey(true).Key != ConsoleKey.Enter );
		Console.Clear();
		
		} // Main()
		
		static void Centraliza(string txt)
		{
			int left = (Console.BufferWidth - txt.Length) / 2;
			
			Console.SetCursorPosition( left, Console.CursorTop );
			
			Console.WriteLine(txt);
		}	// Centraliza()
		
		static void GmsToGd()
		{
			double  LonGraus = 0.0, LonMin = 0.0, LonSeg = 0.0,
					LatGraus = 0.0, LatMin = 0.0, LatSeg = 0.0,
					Longitude , Latitude;
			int LonSig = 1, LatSig = 1;
			
			Console.Clear();
			
			Console.Write("Longitude: ");
			ReadInput(ref LonGraus, 180, 'º', false);
			ReadInput(ref LonMin, 60, '\'', false);
			ReadInput(ref LonSeg, 60, '\"', true);
			LonSig = CoordSelect('O', 'L');
			
			Console.Write("Latitude : ");
			ReadInput(ref LatGraus, 180, 'º', false);
			ReadInput(ref LatMin, 60, '\'', false);
			ReadInput(ref LatSeg, 60, '\"', true);
			LatSig = CoordSelect('S', 'N');

			Longitude = (double)LonSig * ( LonGraus + (LonMin/60.0) + (LonSeg/3600.0) );
			Latitude  = (double)LatSig * ( LatGraus + (LatMin/60.0) + (LatSeg/3600.0) );
			
			Console.WriteLine(Math.Truncate(Longitude*100.0)/100.0);	//Console.WriteLine("Longitude = {0}", Longitude.ToString("N5"));
			Console.WriteLine(Math.Truncate(Latitude*100.0)/100.0);		//Console.WriteLine("Latitude  = {0}", Latitude.ToString("N5"));
			
			Console.Write("[ENTER]");
			Console.ReadKey();
		}	// GmsToGd()
		
		static int CoordSelect(char opt1, char opt2)
		{
			int left = Console.CursorLeft, LonLatSig = 1;
			ConsoleKeyInfo cki;
			bool ok = false;
			
			Console.SetCursorPosition( 0, Console.CursorTop + 1 );
			Console.Write("{0} - {1} ? ", opt1, opt2);
			
			do
			{
				cki = Console.ReadKey(true);
			
				if( cki.Key == ConsoleKey.Enter )
				{
					if(ok)
					{
						Console.SetCursorPosition( left, Console.CursorTop - 1);
						if(LonLatSig == -1)
						{
							Console.Write(opt1);
						} else 
							Console.Write(opt2);
						ClearLine( 0, Console.CursorTop + 1, Console.BufferWidth );
						return LonLatSig;
					}
				} else if( cki.Key == ConsoleKey.Backspace )
				{
					if(ok)
					{
						ClearLine( Console.CursorLeft-1, Console.CursorTop, 1 );
						ok = false;
					}
				} else if( cki.KeyChar.ToString().ToUpper() == opt1.ToString().ToUpper() )
				{
					if( !ok )
					{
					LonLatSig = -1;
					Console.Write(opt1);
					ok = true;
					}
				
				} else if( cki.KeyChar.ToString().ToUpper() == opt2.ToString().ToUpper() )
				{
					if( !ok )
					{
						LonLatSig = 1;
						Console.Write(opt2);
						ok = true;
					}
				}
			} while (true);
		}	// CoordSelect()
		
		static void GdToGms()
		{
			double longitude = 0, latitude = 0;
			Console.Clear();
			
			ReadLonLatGd( ref longitude, "Longitude", 180);			
			ReadLonLatGd( ref latitude, "Latitude", 90);
			
			ConvGdToGmsPrint(longitude, 1);			
			ConvGdToGmsPrint(latitude, 2);
			
			Console.Write("[ENTER]");
			Console.ReadKey();
		}	//GdToGms()

		static void ReadInput(ref double val, int limit, char simbol, bool dec)
		{
			
			char input;
			int inputCnt = 0, inputCntLim, inputDigit, left = Console.CursorLeft;
			bool decimalPoint = false;
			ConsoleKeyInfo cki;
			string valString = "";
			
			inputCntLim = dec ? 6 : limit.ToString().Length;
			
			Console.SetCursorPosition( left + inputCntLim, Console.CursorTop );
			Console.Write(simbol);
			Console.SetCursorPosition( left, Console.CursorTop );
			
			do{
				cki = Console.ReadKey(true);
				input = cki.KeyChar;
				int.TryParse(input.ToString(), out inputDigit);
				if( cki.Key == ConsoleKey.Backspace )
				{
					int valStringL = valString.Length;
					if (valStringL > 0)
					{
						if( valString.Substring(valStringL - 1, 1) == "," ) decimalPoint = false;
						valString = valString.Remove(valStringL - 1);
						inputCnt--;
						ClearLine( Console.CursorLeft-1, Console.CursorTop, 1 );
					}
				} else if( cki.Key == ConsoleKey.Enter )
				{
					if( !double.TryParse(valString, out val) )
					{
						ClearLine( left, Console.CursorTop, Console.BufferWidth - left );
					} else
					{
						if( val >= -(double)limit && val <= (double)limit )
						{
							Console.SetCursorPosition( left + inputCntLim + 2, Console.CursorTop );
							break;	// return
						} else
						{
							Console.SetCursorPosition( 0, Console.CursorTop + 1 );
							Console.Write("O valor deve estar entre -{0} e +{0}. [ENTER]", limit);
							Console.ReadKey(true);
							ClearLine( 0, Console.CursorTop, Console.BufferWidth );
							ClearLine( left, Console.CursorTop-1, Console.BufferWidth - left );
							Console.SetCursorPosition( left + inputCntLim, Console.CursorTop );
							Console.Write(simbol);
							Console.SetCursorPosition( left, Console.CursorTop );
						}
					}
					inputCnt = 0;
					valString = "";
					decimalPoint = false;
				} else if( inputCnt < inputCntLim )
				{
					if( input == ',' && dec == true && decimalPoint == false )
					{
						valString += ',';
						decimalPoint = true;
						inputCnt++;
						Console.Write(",");
					} else if( input >= '0' && input <= '9' )
					{
						valString += input;
						inputCnt++;
						Console.Write(input);
					}
				}
			} while(true);
		}
		
		static void ConvGdToGms(double lat_lon, ref double Grad, ref double Min, ref double Seg)
		{
			double aux = Math.Abs(lat_lon);
			Grad = Math.Abs(Math.Truncate(lat_lon));
			aux = ( aux - Grad ) * 60.0;
			Min = Math.Truncate( aux );
			aux = ( aux - Min ) * 60.0;
			Seg = aux;
		}
		
		static void ConvGdToGmsPrint(double lat_lon, int type)
		{
			double Grad = 0.0, Min = 0.0, Seg = 0.0;
			
			ConvGdToGms(lat_lon, ref Grad, ref Min, ref Seg);
			
			Console.Write("{0} = ", type == 1 ? "Longitude" : "Latitude ");
			Console.Write(Grad);
			if( lat_lon < 0 ) Console.Write(" {0} ", type == 1 ? "O" : "S");
			else Console.Write(" {0} ", type == 1 ? "L" : "N");
			Console.Write(Min);
			Console.Write("\' ");
			Console.Write(Math.Truncate(Seg*100.0)/100.0);	//Console.Write("{0}", Seg.ToString("N5"));
			Console.WriteLine("\"");
		}
		
		static void ReadLonLatGd( ref double lat_lon, string txt, int limit)
		{
			Console.Write("{0}: ", txt);
			int left = Console.CursorLeft;
			do
			{
				while( ! Double.TryParse(Console.ReadLine(), out lat_lon) )
				{
					ClearLine( left, Console.CursorTop - 1, Console.BufferWidth - left );
				}
				if ( lat_lon < -limit || lat_lon > limit )
				{
					Console.Write("{0} deve estar entre -{1} e +{1}. [ENTER]", txt, limit);
					Console.ReadKey();
					ClearLine( 0, Console.CursorTop, Console.BufferWidth );
					ClearLine( left, Console.CursorTop - 1, Console.BufferWidth - left );
				} else break;
			} while( true );
		}
		
		static void ClearLine( int left, int top, int cnt )
		{
			Console.SetCursorPosition( left, top );
			for ( int i = 0; i < cnt; i++ )
				Console.Write(" ");
			Console.SetCursorPosition( left, top );
		}
	}
}

// Compiller
// C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe