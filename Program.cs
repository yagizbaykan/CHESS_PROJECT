using System;
using System.Text; 
using System.IO;  

namespace CHESS_PROJECT
{
    class Program
    {
        public static string[,] tahta = new string[8, 8];
        public static bool siraKirmizida = true;

        public static bool oyunBitti = false;

        public static int enPassantSutun = -1;


        public static bool kirmiziSahOynadi = false;
        public static bool kirmiziSolKaleOynadi = false;
        public static bool kirmiziSagKaleOynadi = false;
        public static bool maviSahOynadi = false;
        public static bool maviSolKaleOynadi = false;
        public static bool maviSagKaleOynadi = false;

       
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            ChoosingMode();
        }

       

        public static void OyunuKaydet()
        {
            using (StreamWriter sw = new StreamWriter("kayit.txt"))
            {
                
                sw.WriteLine(siraKirmizida);
                sw.WriteLine(enPassantSutun);

                sw.WriteLine(kirmiziSahOynadi);
                sw.WriteLine(kirmiziSolKaleOynadi);
                sw.WriteLine(kirmiziSagKaleOynadi);
                sw.WriteLine(maviSahOynadi);
                sw.WriteLine(maviSolKaleOynadi);
                sw.WriteLine(maviSagKaleOynadi);

                // Sonra tahtayı kaydet
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (tahta[i, j] == " ") sw.WriteLine("EMPTY");
                        else sw.WriteLine(tahta[i, j]);
                    }
                }
            }
        }

        public static void OyunuYukle()
        {
            if (!File.Exists("kayit.txt")) return;

            using (StreamReader sr = new StreamReader("kayit.txt"))
            {
                siraKirmizida = bool.Parse(sr.ReadLine());
                enPassantSutun = int.Parse(sr.ReadLine());

                kirmiziSahOynadi = bool.Parse(sr.ReadLine());
                kirmiziSolKaleOynadi = bool.Parse(sr.ReadLine());
                kirmiziSagKaleOynadi = bool.Parse(sr.ReadLine());
                maviSahOynadi = bool.Parse(sr.ReadLine());
                maviSolKaleOynadi = bool.Parse(sr.ReadLine());
                maviSagKaleOynadi = bool.Parse(sr.ReadLine());

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        string okunan = sr.ReadLine();
                        tahta[i, j] = (okunan == "EMPTY") ? " " : okunan;
                    }
                }
            }
        }

        public static void ChoosingMode()
        {
        
            while (true)
            {
                Console.Clear();       
                DisplayMainMenu();     

                Console.WriteLine();
                Console.Write("Make a choice: ");
                string input = Console.ReadLine(); 

                switch (input)
                {
                    case "1":
                        if (File.Exists("kayit.txt"))
                        {
                            Console.Write("There is a saved game. Continue? (Y/N): ");
                            string cvp = Console.ReadLine().ToUpper();

                            if (cvp == "Y")
                            {
                                GameMode(true);
                            }
                            else
                            {
                                GameMode(false);
                            }
                        }
                        else
                        {
                            GameMode(false);
                        }
                        break;
                    case "2":
                        DemoMode();
                        break;
                    case "3":
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2 or 3.");
                        continue; 
                }
            }
        }

        public static void DisplayMainMenu()
        {
            Console.WriteLine("♛                                                ♛");
            Console.WriteLine("╔═════════════════════════════════════════════════╗");
            Console.WriteLine("║  DOKUZ EYLUL UNIVERSITY EED 1005 CHESS PROJECT  ║");
            Console.WriteLine("║                                                 ║");
            Console.WriteLine("║  ** 1- Play ** 2- Demo Mode ** 3- Quit Game **  ║");
            Console.WriteLine("╚═════════════════════════════════════════════════╝");
            Console.WriteLine("♛                                                ♛");
        }

        public static void TahtayiDiz()
        {
            enPassantSutun = -1;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++) tahta[i, j] = " ";
            }

            tahta[0, 0] = "r"; tahta[0, 1] = "n"; tahta[0, 2] = "b"; tahta[0, 3] = "q"; tahta[0, 4] = "k"; tahta[0, 5] = "b"; tahta[0, 6] = "n"; tahta[0, 7] = "r";
            for (int i = 0; i < 8; i++) tahta[1, i] = "p";

            tahta[7, 0] = "R"; tahta[7, 1] = "N"; tahta[7, 2] = "B"; tahta[7, 3] = "Q"; tahta[7, 4] = "K"; tahta[7, 5] = "B"; tahta[7, 6] = "N"; tahta[7, 7] = "R";
            for (int i = 0; i < 8; i++) tahta[6, i] = "P";
        }

        public static void EkraniCiz()
        {
            Console.WriteLine("   a  b  c  d  e  f  g  h");
            Console.WriteLine("  ┌────────────────────────┐");

            for (int i = 0; i < 8; i++)
            {
                Console.Write((8 - i) + " │");

                for (int j = 0; j < 8; j++)
                {
                    string tas = tahta[i, j]; 

                    if (tas == " ")
                    {
                        
                        if ((i + j) % 2 == 0) Console.Write(" * ");
                        else Console.Write("   ");
                    }
                    else 
                    {
                        if (char.IsUpper(tas[0])) Console.ForegroundColor = ConsoleColor.Red;
                        else Console.ForegroundColor = ConsoleColor.Cyan;

                        string sembol = tas.ToUpper();
                        Console.Write(" " + sembol + " ");

                        Console.ResetColor();
                    }
                }
                Console.WriteLine("│" + (8 - i));
            }
            Console.WriteLine("  └────────────────────────┘");
            Console.WriteLine("   a  b  c  d  e  f  g  h");
        }

        public static void DemoMode()
        {
            string dosyaYolu = "C:\\Users\\yagiz\\Desktop\\movements.txt";

            if (!File.Exists(dosyaYolu))
            {
                Console.WriteLine("Error: movements.txt file not found.");
                Console.WriteLine("Press any button to return main menu.");
                Console.ReadKey();
                return;
            }

            string[] hamleler = File.ReadAllLines(dosyaYolu);

            TahtayiDiz(); 
            siraKirmizida = true; 

            Console.Clear();
            EkraniCiz();

            Console.WriteLine("--- DEMO MODE ---");
            Console.WriteLine("SPACE: Next Move | P: Play | ESC: Exit");

            int hamleSirasi = 0;
            while (hamleSirasi < hamleler.Length)
            {
                string kim = siraKirmizida ? "RED" : "BLUE";
                Console.Write($"\nNext Movement ({kim}): {hamleler[hamleSirasi]}");

                ConsoleKeyInfo tus = Console.ReadKey(true);

                if (tus.Key == ConsoleKey.Spacebar)
                {
                    string gelenHamle = hamleler[hamleSirasi].Trim(); 

                    if (HamleCozVeOyna(gelenHamle))
                    {
                        siraKirmizida = !siraKirmizida;
                        Console.Clear();
                        EkraniCiz();
                        Console.WriteLine($"Move Played: {gelenHamle}");
                        hamleSirasi++; 
                    }
                    else
                    {
                        Console.WriteLine($"\nInvalid move in file: {gelenHamle}");
                        hamleSirasi++; 
                    }
                }
                else if (tus.Key == ConsoleKey.P)
                {
                    Console.WriteLine("Preparing Game Mode...");
                    GameMode(devamEdiliyor: true); 
                    return;
                }
                else if (tus.Key == ConsoleKey.Escape)
                {
                    return;
                }
            }
            Console.WriteLine("\nDemo movements are completed. Press any button to continue.");
            Console.ReadKey();
        }

        public static void GameMode(bool devamEdiliyor = false)
        {
            if (!devamEdiliyor)
            {
                TahtayiDiz();
                siraKirmizida = true;
                oyunBitti = false;

                // Yeni oyun başlarken rok ve en passant haklarını sıfırlamak iyi olur
                enPassantSutun = -1;
                kirmiziSahOynadi = false; kirmiziSolKaleOynadi = false; kirmiziSagKaleOynadi = false;
                maviSahOynadi = false; maviSolKaleOynadi = false; maviSagKaleOynadi = false;
            }
            else
            {
                // --- EKLEME 1: Eğer devam ediliyorsa dosyadan oku ---
                OyunuYukle();
            }

            while (!oyunBitti)
            {
                Console.Clear();
                EkraniCiz();

                bool sahTehdidi = SahTehditAltindaMi(siraKirmizida);
                bool hamleKalmadi = MatVarMi(siraKirmizida);

                if (sahTehdidi)
                {
                    if (hamleKalmadi)
                    {
                        Console.WriteLine("\nCHECK MATE! GAME OVER!");
                        Console.WriteLine("Winner: " + (!siraKirmizida ? "RED" : "BLUE"));
                        Console.ReadKey();

                        // Oyun bittiği için kayıt dosyasını silebilirsin (isteğe bağlı)
                        if (File.Exists("kayit.txt")) File.Delete("kayit.txt");
                        break;
                    }
                    Console.WriteLine("\nCHECK!");
                }
                else
                {
                    if (hamleKalmadi)
                    {
                        Console.WriteLine("\nSTALEMATE (PAT)! GAME DRAW!");
                        Console.ReadKey();
                        if (File.Exists("kayit.txt")) File.Delete("kayit.txt");
                        break;
                    }
                }

                Console.WriteLine("\nNotation: Pawn (e4), Knight (Nf3), Bishop (Bc4), Rook (Ra1), Queen (Qd1), King (Kd2)");
                Console.WriteLine("Short Castling (O-O), Long Castling (O-O-O)");
                Console.WriteLine();
                Console.WriteLine("Press 'SHIFT + H' for a Hint!");
                Console.WriteLine();
                Console.WriteLine("Type 'exit' to return main menu.");

                string kim = siraKirmizida ? "RED" : "BLUE";
                Console.WriteLine();
                Console.Write(kim + "'s movement: ");

                bool ipucuIstendi;
                string giris = OzelSatirOku(out ipucuIstendi);

                if (ipucuIstendi)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nThinking...");
                    string ipucu = IpucuUret(siraKirmizida);
                    Console.WriteLine("HINT: " + ipucu);
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                if (string.IsNullOrEmpty(giris)) continue;

                // --- EKLEME 2: Çıkış yapılırken kaydet ---
                if (giris.ToLower() == "exit")
                {
                    OyunuKaydet(); // Çıkmadan önce kaydet
                    Console.WriteLine("Game saved...");
                    System.Threading.Thread.Sleep(1000); // Kullanıcı görsün diye azıcık bekleme
                    return;
                }

                if (HamleCozVeOyna(giris))
                {
                    PiyonTerfiKontrol();
                    siraKirmizida = !siraKirmizida;
                }
                else
                {
                    Console.WriteLine("Invalid move. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }
        public static string OzelSatirOku(out bool ipucuTetiklendi)
        {
            StringBuilder yazi = new StringBuilder(); 
            ipucuTetiklendi = false;

            while (true)
            {
                ConsoleKeyInfo tus = Console.ReadKey(true);

                if (tus.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return yazi.ToString();
                }

                if (tus.Key == ConsoleKey.Backspace)
                {
                    if (yazi.Length > 0)
                    {
                        yazi.Remove(yazi.Length - 1, 1); 
                        Console.Write("\b \b"); 
                    }
                }
                else if (tus.Key == ConsoleKey.H && tus.Modifiers.HasFlag(ConsoleModifiers.Shift))
                {
                    ipucuTetiklendi = true;
                    return ""; 
                }
                else if (!char.IsControl(tus.KeyChar))
                {
                    Console.Write(tus.KeyChar); 
                    yazi.Append(tus.KeyChar);
                }
            }
        }

        public static string IpucuUret(bool kirmiziSirasi)
        {
            string yedekHamle = ""; 

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    string tas = tahta[r, c];
                    if (tas == " ") continue;
                    if (kirmiziSirasi != char.IsUpper(tas[0])) continue; 

                    for (int hr = 0; hr < 8; hr++)
                    {
                        for (int hc = 0; hc < 8; hc++)
                        {
                            if (HamleUygunMu(r, c, hr, hc, tas))
                            {
                                string hedefTas = tahta[hr, hc];
                                string kaynakTas = tahta[r, c];

                                tahta[hr, hc] = tas;
                                tahta[r, c] = " ";

                                bool illegal = SahTehditAltindaMi(kirmiziSirasi); 

                                tahta[r, c] = kaynakTas;
                                tahta[hr, hc] = hedefTas;

                                if (illegal) continue; 

                                yedekHamle = $"{tas} {KoordinatCevir(r, c)} -> {KoordinatCevir(hr, hc)}";

                                if (hedefTas != " ")
                                {
                                    int benimDegerim = TasDegeri(tas);       
                                    int hedefDeger = TasDegeri(hedefTas);    

                                    if (hedefDeger >= benimDegerim)
                                    {
                                        return $"Best Move: {yedekHamle} (Capture)";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (yedekHamle == "") return "No legal moves!";
            return $"Suggested: {yedekHamle}";
        }

        public static int TasDegeri(string tas)
        {
            char t = char.ToUpper(tas[0]);
            if (t == 'P') return 1;  
            if (t == 'N' || t == 'B') return 3;
            if (t == 'R') return 5;  
            if (t == 'Q') return 9;  
            return 0; 
        }

       
        public static string KoordinatCevir(int r, int c)
        {
            char sutun = (char)('a' + c); 
            int satir = 8 - r;            
            return $"{sutun}{satir}";
        }

       
        public static bool HamleCozVeOyna(string giris)
        {
            if (string.IsNullOrEmpty(giris)) return false;

            if (giris == "O-O" || giris == "0-0") return RokYap(true);
            if (giris == "O-O-O" || giris == "0-0-0") return RokYap(false);

            if (giris.Length < 2) return false;

            if (giris.Length == 4)
            {
                string baslangicKoor = giris.Substring(0, 2); 
                string hedefKoor = giris.Substring(2, 2);     

                int bSutun = baslangicKoor[0] - 'a';
                int bSatir = 8 - (baslangicKoor[1] - '0');
                int hSutun = hedefKoor[0] - 'a';
                int hSatir = 8 - (hedefKoor[1] - '0');

                if (bSutun < 0 || bSutun > 7 || bSatir < 0 || bSatir > 7 ||
                    hSutun < 0 || hSutun > 7 || hSatir < 0 || hSatir > 7) return false;

                string tas = tahta[bSatir, bSutun];

                if (tas == " ") return false;
                bool tasKirmizi = char.IsUpper(tas[0]);
                if (tasKirmizi != siraKirmizida) return false;

                if (HamleUygunMu(bSatir, bSutun, hSatir, hSutun, tas))
                {
                    return HamleyiUygula(bSatir, bSutun, hSatir, hSutun, tas);
                }
                return false;
            }


            char tip = 'P'; 
            string hedefKoorKisa = giris;

            if (giris.Length == 2)
            {
                tip = 'P'; hedefKoorKisa = giris;
            }
            else if (giris.Length == 3 && char.IsUpper(giris[0]))
            {
                tip = giris[0]; hedefKoorKisa = giris.Substring(1, 2);
            }
            else return false; 

            int hSutunKisa = hedefKoorKisa[0] - 'a';
            int hSatirKisa = 8 - (hedefKoorKisa[1] - '0');

            if (hSutunKisa < 0 || hSutunKisa > 7 || hSatirKisa < 0 || hSatirKisa > 7) return false;

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    string tas = tahta[r, c];
                    if (tas == " ") continue;

                    bool tasKirmizi = char.IsUpper(tas[0]);
                    if (tasKirmizi != siraKirmizida) continue;

                    char tasKarakter = char.ToUpper(tas[0]);
                    if (tasKarakter == tip) 
                    {
                        if (HamleUygunMu(r, c, hSatirKisa, hSutunKisa, tas))
                        {
                            return HamleyiUygula(r, c, hSatirKisa, hSutunKisa, tas);
                        }
                    }
                }
            }
            return false;
        }

        public static bool HamleyiUygula(int r, int c, int hSatir, int hSutun, string tas)
        {
            char tip = char.ToUpper(tas[0]);
            bool enPassant = (tip == 'P' && hSutun != c && tahta[hSatir, hSutun] == " ");

            string yedekHedef = tahta[hSatir, hSutun];
            string yedekKaynak = tahta[r, c];
            string yedekEnPassant = "";

            if (enPassant)
            {
                yedekEnPassant = tahta[r, hSutun];
                tahta[r, hSutun] = " ";
            }

            tahta[hSatir, hSutun] = tas;
            tahta[r, c] = " ";

            bool sahtaMi = SahTehditAltindaMi(siraKirmizida);

            tahta[r, c] = yedekKaynak;
            tahta[hSatir, hSutun] = yedekHedef;
            if (enPassant) tahta[r, hSutun] = yedekEnPassant;

            if (!sahtaMi)
            {
                if (enPassant) tahta[r, hSutun] = " ";

                tahta[hSatir, hSutun] = tas;
                tahta[r, c] = " ";

                if (tip == 'K')
                {
                    if (siraKirmizida) kirmiziSahOynadi = true;
                    else maviSahOynadi = true;
                }
                else if (tip == 'R')
                {
                    if (siraKirmizida)
                    {
                        if (r == 7 && c == 0) kirmiziSolKaleOynadi = true;
                        if (r == 7 && c == 7) kirmiziSagKaleOynadi = true;
                    }
                    else
                    {
                        if (r == 0 && c == 0) maviSolKaleOynadi = true;
                        if (r == 0 && c == 7) maviSagKaleOynadi = true;
                    }
                }

                if (tip == 'P' && Math.Abs(hSatir - r) == 2)
                    enPassantSutun = hSutun;
                else
                    enPassantSutun = -1;

                return true;
            }
            return false;
        }

        public static bool HamleUygunMu(int r1, int c1, int r2, int c2, string tas)
        {
            string hedefTas = tahta[r2, c2];
            if (hedefTas != " ")
            {
                bool kirmiziMi = char.IsUpper(tas[0]);
                bool hedefKirmizi = char.IsUpper(hedefTas[0]);
                if (kirmiziMi == hedefKirmizi) return false;
            }

            char tip = char.ToUpper(tas[0]);
            int dy = Math.Abs(r2 - r1); 
            int dx = Math.Abs(c2 - c1); 

            if (tip == 'P')
            {
                int yon = char.IsUpper(tas[0]) ? -1 : 1; 
                int baslangic = char.IsUpper(tas[0]) ? 6 : 1;

                if (c1 == c2 && tahta[r2, c2] == " ")
                {
                    if (r2 == r1 + yon) return true; 
                    if (r1 == baslangic && r2 == r1 + (2 * yon) && tahta[r1 + yon, c1] == " ") return true; 
                }
                else if (dx == 1 && r2 == r1 + yon)
                {
                    if (tahta[r2, c2] != " ") return true; 

                    if (tahta[r2, c2] == " " && c2 == enPassantSutun)
                    {
                        if (char.IsUpper(tas[0]) && r1 == 3) return true;
                        if (!char.IsUpper(tas[0]) && r1 == 4) return true;
                    }
                }
                return false;
            }

            if (tip == 'R') return (r1 == r2 || c1 == c2) && YolBosMu(r1, c1, r2, c2);

            if (tip == 'B') return (dy == dx) && YolBosMu(r1, c1, r2, c2);

            if (tip == 'Q') return ((r1 == r2 || c1 == c2) || (dy == dx)) && YolBosMu(r1, c1, r2, c2);

            if (tip == 'N') return (dy == 2 && dx == 1) || (dy == 1 && dx == 2);

            if (tip == 'K') return dy <= 1 && dx <= 1;

            return false;
        }

        public static bool YolBosMu(int r1, int c1, int r2, int c2)
        {
            int rArtis = Math.Sign(r2 - r1);
            int cArtis = Math.Sign(c2 - c1);
            int r = r1 + rArtis;
            int c = c1 + cArtis;

            while (r != r2 || c != c2)
            {
                if (tahta[r, c] != " ") return false; 
                r += rArtis;
                c += cArtis;
            }
            return true;
        }

        public static bool RokYap(bool kisaRok)
        {
            int satir = siraKirmizida ? 7 : 0;
            string sah = siraKirmizida ? "K" : "k";
            string kale = siraKirmizida ? "R" : "r";

            if (tahta[satir, 4] != sah) return false;

            if (siraKirmizida && kirmiziSahOynadi) return false;
            if (!siraKirmizida && maviSahOynadi) return false;

            if (kisaRok) 
            {
                if (siraKirmizida && kirmiziSagKaleOynadi) return false;
                if (!siraKirmizida && maviSagKaleOynadi) return false;

                if (tahta[satir, 7] == kale && tahta[satir, 5] == " " && tahta[satir, 6] == " ")
                {
                    if (!SahTehditAltindaMi(siraKirmizida))
                    {
                        tahta[satir, 4] = " "; tahta[satir, 7] = " ";
                        tahta[satir, 6] = sah; tahta[satir, 5] = kale;

                        if (siraKirmizida) kirmiziSahOynadi = true; else maviSahOynadi = true;
                        return true;
                    }
                }
            }
            else 
            {
                if (siraKirmizida && kirmiziSolKaleOynadi) return false;
                if (!siraKirmizida && maviSolKaleOynadi) return false;

                if (tahta[satir, 0] == kale && tahta[satir, 1] == " " && tahta[satir, 2] == " " && tahta[satir, 3] == " ")
                {
                    if (!SahTehditAltindaMi(siraKirmizida))
                    {
                        tahta[satir, 4] = " "; tahta[satir, 0] = " ";
                        tahta[satir, 2] = sah; tahta[satir, 3] = kale;

                        if (siraKirmizida) kirmiziSahOynadi = true; else maviSahOynadi = true;
                        return true;
                    }
                }
            }
            return false;
        }

        public static void PiyonTerfiKontrol()
        {
            for (int c = 0; c < 8; c++)
            {
                if (tahta[0, c] == "P")
                {
                    Console.Write("Piyon Terfi (Q, R, B, N): ");
                    string secim = Console.ReadLine().ToUpper();

                    if (secim == "R" || secim == "B" || secim == "N") tahta[0, c] = secim;
                    else tahta[0, c] = "Q";
                }
                if (tahta[7, c] == "p")
                {
                    Console.Write("Piyon Terfi (q, r, b, n): ");
                    string secim = Console.ReadLine().ToLower();
                    if (secim == "r" || secim == "b" || secim == "n") tahta[7, c] = secim;
                    else tahta[7, c] = "q";
                }
            }
        }

        public static bool SahTehditAltindaMi(bool kirmiziSah)
        {
            int sahR = -1, sahC = -1;
            char aranacakSah = kirmiziSah ? 'K' : 'k';


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (tahta[i, j].Length > 0 && tahta[i, j][0] == aranacakSah)
                    {
                        sahR = i; sahC = j;
                    }
                }
            }

            if (sahR == -1) return true; 

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string tas = tahta[i, j];
                    if (tas == " ") continue;

                    bool rakipMi = kirmiziSah ? char.IsLower(tas[0]) : char.IsUpper(tas[0]);

                    if (rakipMi)
                    {
                        if (HamleUygunMu(i, j, sahR, sahC, tas)) return true;
                    }
                }
            }
            return false;
        }

        public static bool MatVarMi(bool kirmiziTarafi)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    string tas = tahta[r, c];
                    if (tas == " ") continue;
                    if (kirmiziTarafi != char.IsUpper(tas[0])) continue;

                    for (int hr = 0; hr < 8; hr++)
                    {
                        for (int hc = 0; hc < 8; hc++)
                        {
                            if (HamleUygunMu(r, c, hr, hc, tas))
                            {
                                bool enPassant = (char.ToUpper(tas[0]) == 'P' && hc != c && tahta[hr, hc] == " ");
                                string yedekEnPassant = "";
                                string yedekH = tahta[hr, hc];
                                string yedekK = tahta[r, c];

                                if (enPassant)
                                {
                                    yedekEnPassant = tahta[r, hc];
                                    tahta[r, hc] = " ";
                                }

                                tahta[hr, hc] = tas;
                                tahta[r, c] = " ";

                                bool halaTehdit = SahTehditAltindaMi(kirmiziTarafi);

                                tahta[r, c] = yedekK;
                                tahta[hr, hc] = yedekH;
                                if (enPassant) tahta[r, hc] = yedekEnPassant;

                                if (!halaTehdit) return false;
                            }
                        }
                    }
                }
            }
            return true; 
        }
    }
}