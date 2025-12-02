using System.Text;
namespace SatrancOdevi
{
    class Program
    {

        static string[,] oyunTahtasi = new string[8, 8];
        static bool beyazSirasi = true;

        public static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;
            DisplayingMenu();
            ChoosingMode();
        }

        public static void DisplayingMenu()
        {
            Console.WriteLine("♛                                                ♛");
            Console.WriteLine("╔═════════════════════════════════════════════════╗");
            Console.WriteLine("║  DOKUZ EYLUL UNIVERSITY EED 1005 CHESS PROJECT  ║");
            Console.WriteLine("║                                                 ║");
            Console.WriteLine("║  ** 1- Play ** 2- Demo Mode ** 3- Quit Game **  ║");
            Console.WriteLine("╚═════════════════════════════════════════════════╝");
            Console.WriteLine("♛                                                ♛");
        }

        public static void ChoosingMode()
        {
            while (true)
            {
                Console.Clear();   
                DisplayingMenu();  

                Console.WriteLine();
                Console.Write("Make a choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        GameMode();
                        break;
                    case "2":
                        DemoMode();
                        break;
                    case "3":
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1,2 or 3.");
                        continue;
                }
            }
        }

        public static void TasYerlestirme()
        {
            string[] whites = { "r", "n", "b", "q", "k", "b", "n", "r" };
            string[] blacks = { "r", "n", "b", "q", "k", "b", "n", "r" };

            // once tüm tahtayı sıfırla 
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    oyunTahtasi[i, j] = null;

            for (int i = 0; i < 8; i++)
            {
                oyunTahtasi[0, i] = blacks[i];       // siyah taşlar
                oyunTahtasi[1, i] = "p";             // siyah piyonlar

                oyunTahtasi[7, i] = whites[i].ToUpper(); // beyaz taşlar
                oyunTahtasi[6, i] = "P";                 // beyaz piyonlar
            }
        }

        public static void TahtayiCiz()
        {
            Console.WriteLine("\n     a  b  c  d  e  f  g  h");
            Console.WriteLine("   ┌────────────────────────┐");

            for (int i = 0; i < 8; i++)
            {
                Console.Write(" " + (8 - i) + " │"); // satır numarası

                for (int j = 0; j < 8; j++)
                {
                    string kareDegeri = oyunTahtasi[i, j];

                    if (kareDegeri != null)
                    {
                        Console.Write(" " + kareDegeri + " ");
                    }
                    else
                    {
                        //boş kareler
                        if ((i + j) % 2 == 0)
                            Console.Write("   "); // beyaz kare 
                        else
                            Console.Write(" ♦ "); // siyah kare 
                    }
                }
                Console.WriteLine("│ " + (8 - i));
            }

            Console.WriteLine("   └────────────────────────┘");
            Console.WriteLine("     a  b  c  d  e  f  g  h\n");
        }

        // parametre varsayılan olarak false yani sıfırdan başlar.
        public static void GameMode(bool devamEdiliyor = false)
        {
            // Eğer demo modundan gelmediysek taşları sıfırla
            if (devamEdiliyor == false)
            {
                TasYerlestirme();
                beyazSirasi = true; // Sırayı da sıfırla
            }

            while (true)
            {
                // mevcut döngü
                Console.Clear();
                TahtayiCiz();

                if (beyazSirasi)
                    Console.WriteLine("It's White's turn (Upper Letters) .");
                else
                    Console.WriteLine("It's Black's turn (Lower Letters) .");

                Console.Write("Enter a movement (eg: e2 e4) or write 'quit' to quit: ");
                string hamle = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(hamle)) continue;

                string komut = hamle.Trim().ToLower();

                if (komut == "quit" || komut == "exit" || komut == "q")
                {
                    break;
                }

                HamleYap(komut);
            }
        }
        public static void DemoMode()
        {
            string dosyaYolu = "C:\\Users\\yagiz\\Desktop\\movements.txt";

            if (!File.Exists(dosyaYolu))
            {
                Console.WriteLine("Error: movements.txt file not found!");
                Console.WriteLine("Press any button to continue...");
                Console.ReadKey();
                return;
            }

            string[] hamleler = File.ReadAllLines(dosyaYolu);

            TasYerlestirme();
            TahtayiCiz();

            Console.WriteLine("--- DEMO MODE ---");
            Console.WriteLine("SPACE: Next Movement | P: Play Mode | ESC: Exit");

            int hamleSirasi = 0;
            while (hamleSirasi < hamleler.Length)
            {
                Console.Write($"\nPress SPACE button for next movement ({hamleler[hamleSirasi]})");

                ConsoleKeyInfo tus = Console.ReadKey(true); // true parametresi basılan tuşu ekrana yazmaz

                if (tus.Key == ConsoleKey.Spacebar)
                {
                    string gelenHamle = hamleler[hamleSirasi];

                    Console.WriteLine(gelenHamle); 
                    HamleYap(gelenHamle);          

                    Console.Clear();
                    TahtayiCiz();                  

                    Console.WriteLine($"Move Played: {gelenHamle}");
                    hamleSirasi++;

                    Console.WriteLine();
                    Console.WriteLine();
                }
                  
                else if (tus.Key == ConsoleKey.P)
                {
                    Console.WriteLine("Preparing Play Mode...");
                    GameMode(devamEdiliyor: true); // GameMode'u parametreli yapmamız gerekecek
                    return;
                }
                else if (tus.Key == ConsoleKey.Escape)
                {
                    return;
                }
                

            }

            Console.WriteLine("Demo Mode is over, returning to the main menu...");
            Console.ReadKey();
        }

        // --- YARDIMCI METOTLAR ---

        static int HarfiSayiyaCevir(char harf)
        {
            switch (harf)
            {
                case 'a': return 0; // Dizi indeksi 0'dan başlar
                case 'b': return 1;
                case 'c': return 2;
                case 'd': return 3;
                case 'e': return 4;
                case 'f': return 5;
                case 'g': return 6;
                case 'h': return 7;
                default: return -1;
            }
        }

        static int SatirNumarasiniBul(char rakam)
        {
            // '8' -> index 0
            // '1' -> index 7
            if (rakam >= '1' && rakam <= '8')
                return 8 - (rakam - '0');
            return -1;
        }

        public static void HamleYap(string giris)
        {
            if (string.IsNullOrWhiteSpace(giris)) return;

            string[] parcalar = giris.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parcalar.Length != 2)
            {
                HataMesaji("Enter two coordinates. (eg: a2 a3).");
                return;
            }

            string from = parcalar[0].ToLower();
            string to = parcalar[1].ToLower();

            if (from.Length != 2 || to.Length != 2)
            {
                HataMesaji("Coordinates must be two characters.");
                return;
            }

            int sutunBas = HarfiSayiyaCevir(from[0]);
            int satirBas = SatirNumarasiniBul(from[1]);

            int sutunHedef = HarfiSayiyaCevir(to[0]);
            int satirHedef = SatirNumarasiniBul(to[1]);

            if (sutunBas == -1 || satirBas == -1 || sutunHedef == -1 || satirHedef == -1)
            {
                HataMesaji("Invalid coordinate.");
                return;
            }

            // Tas Kontrolu
            string secilenTas = oyunTahtasi[satirBas, sutunBas];

            // ÖNEMLİ DÜZELTME: null kontrolü
            if (secilenTas == null)
            {
                HataMesaji("No chessman on the square you've choosen.");
                return;
            }

            // Sira Kontrolu
            bool tasBeyaz = char.IsUpper(secilenTas[0]);
            if (tasBeyaz != beyazSirasi)
            {
                HataMesaji("It's not your turn.");
                return;
            }

            // Hedefteki Tas Kontrolu (Kendi taşını yeme)
            string hedefTas = oyunTahtasi[satirHedef, sutunHedef];
            if (hedefTas != null)
            {
                bool hedefBeyaz = char.IsUpper(hedefTas[0]);
                if (tasBeyaz == hedefBeyaz)
                {
                    HataMesaji("You cannot take your chessman.");
                    return;
                }
            }

            // Kurallara Uygunluk Kontrolü
            if (HareketGecerliMi(satirBas, sutunBas, satirHedef, sutunHedef, secilenTas) == false)
            {
                HataMesaji("Invalid movement.");
                return;
            }

            // --- HAMLE İŞLEME ---
            // Eski yeri null yapıyoruz ki tahta deseni bozulmasın
            oyunTahtasi[satirBas, sutunBas] = null;

            // Yeni yere taşı koy
            oyunTahtasi[satirHedef, sutunHedef] = secilenTas;

            // Sırayı değiştir
            beyazSirasi = !beyazSirasi;
        }

        static bool HareketGecerliMi(int r1, int c1, int r2, int c2, string tas)
        {
            string tip = tas.ToLower(); // p, r, n, b, q, k

            int dY = r2 - r1; // satır farkı
            int dX = c2 - c1; // sütun farkı

            // yön bağımsız mesafe
            int absDY = Math.Abs(dY);
            int absDX = Math.Abs(dX);

            // 1. PIYON
            if (tip == "p")
            {
                int yon = 0;
                int baslangic = 0;

                if (tas == "P") // BEYAZ
                {
                    yon = -1;       // yukarı gider (index azalır)
                    baslangic = 6;  // Beyaz piyonlar 6. indextedir (Tahtada 2. satır)
                }
                else // SIYAH
                {
                    yon = 1;        // Aşağı gider (index artar)
                    baslangic = 1;  // Siyah piyonlar 1. indextedir (Tahtada 7. satır)
                }

                // Düz Gitme
                if (c1 == c2)
                {
                    // 1 adım
                    if (r2 == r1 + yon && oyunTahtasi[r2, c2] == null)
                        return true;

                    // 2 adım (Başlangıçtaysa ve yol boşsa)
                    if (r1 == baslangic && r2 == r1 + (2 * yon))
                    {
                        // Önündeki ve hedef kare boş olmalı
                        if (oyunTahtasi[r1 + yon, c1] == null && oyunTahtasi[r2, c2] == null)
                            return true;
                    }
                }
                // Çapraz Yeme
                else if (absDX == 1 && r2 == r1 + yon)
                {
                    if (oyunTahtasi[r2, c2] != null) // Hedefte taş varsa (rengi yukarıda kontrol edildi)
                        return true;
                }
                return false;
            }

            // 2. KALE (ROOK)
            if (tip == "r")
            {
                // Ya satır aynı ya sütun aynı olmalı
                if (r1 == r2 || c1 == c2)
                    return YolBosMu(r1, c1, r2, c2);
                return false;
            }

            // 3. FIL (BISHOP)
            if (tip == "b")
            {
                // Çapraz gitmeli (dY ve dX eşit olmalı)
                if (absDY == absDX)
                    return YolBosMu(r1, c1, r2, c2);
                return false;
            }

            // 4. VEZIR (QUEEN)
            if (tip == "q")
            {
                // Kale gibi veya Fil gibi gidebilir
                if ((r1 == r2 || c1 == c2) || (absDY == absDX))
                    return YolBosMu(r1, c1, r2, c2);
                return false;
            }

            // 5. AT (KNIGHT)
            if (tip == "n")
            {
                // L Hareketi: 2'ye 1 veya 1'e 2
                if ((absDY == 2 && absDX == 1) || (absDY == 1 && absDX == 2))
                    return true; // At taşların üzerinden atlar, yol kontrolü gerekmez.
                return false;
            }

            // 6. SAH (KING)
            if (tip == "k")
            {
                // Her yöne 1 birim
                if (absDY <= 1 && absDX <= 1)
                    return true;
                return false;
            }

            return false;
        }

        static bool YolBosMu(int r1, int c1, int r2, int c2)
        {
            int rArtis = 0;
            if (r2 > r1) rArtis = 1;
            else if (r2 < r1) rArtis = -1;

            int cArtis = 0;
            if (c2 > c1) cArtis = 1;
            else if (c2 < c1) cArtis = -1;

            int r = r1 + rArtis;
            int c = c1 + cArtis;

            // hedef kareye gelene kadar aradaki karelere bak
            while (r != r2 || c != c2)
            {
                if (oyunTahtasi[r, c] != null)
                {
                    return false; // arada taş var
                }

                r += rArtis;
                c += cArtis;
            }
            return true;
        }

        public static void HataMesaji(string mesaj)
        {
            Console.WriteLine(">>> Error: " + mesaj);
            Console.WriteLine("Enter any button to continue...");
            Console.ReadKey();
        }
    }
}