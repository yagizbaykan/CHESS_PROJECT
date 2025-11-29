using System.Text;

namespace SatrancOdevi
{
    class Program
    {
        // 10x10luk dizi
        static string[,] oyunTahtasi = new string[8, 8];
        static bool beyazSirasi = true;

        public static void Main(string[] args)
        {
            displayingMenu();
            choosingMenu();
        }

        public static void displayingMenu()
        {
            Console.WriteLine("|-------------------------------------------------|");
            Console.WriteLine("|  DOKUZ EYLUL UNIVERSITY EED 1005 CHESS PROJECT  |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|      1- Play ** 2- Demo Mode 3- Quit Game       |");
            Console.WriteLine("|-------------------------------------------------|");
        }

        public static void choosingMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("Select a mode: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int secim))
                {
                    Console.WriteLine("Gecersiz secim. Lutfen 1, 2 veya 3 girin.");
                    continue;
                }

                switch (secim)
                {
                    case 1:
                        displayingGame();
                        break;
                    case 2:
                        //demoMode();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Gecersiz secim. Lutfen 1, 2 veya 3 girin.");
                        break;
                }
            }
        }

        public static void displayingGame()
        {
            // Tahtayi kur
            TahtayiDiz();
            while (true)
            {
                Console.Clear();
                Console.OutputEncoding = Encoding.UTF8;
                TahtayiCiz();
                Console.WriteLine();
                Console.WriteLine(beyazSirasi ? "Beyazin sirasi." : "Siyahin sirasi.");
                Console.WriteLine("Hamle girin (ornegin: e2 e4). 'menu' donus, 'quit' cikis.");
                Console.Write("Hamle: ");
                string giris = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(giris)) continue;
                if (giris.Equals("menu", StringComparison.OrdinalIgnoreCase)) return;
                if (giris.Equals("quit", StringComparison.OrdinalIgnoreCase)) Environment.Exit(0);

                HamleYap(giris);
                // sonraki adimda tahtayi gormek icin kisa bir bekleme veya Console.ReadKey() koyabilirsiniz
            }
        }

        

        public static void TahtayiDiz()
        {
            // Önce tahtayı sıfırla
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    oyunTahtasi[i, j] = null; // Boş kareler null olsun

            string[] taslar = { "k", "q", "b", "n", "r", "b", "n", "r" };
            // Sıralamayı düzelttim: Genelde Kale(r), At(n), Fil(b), Vezir(q), Şah(k)... gider
            string[] arkaSira = { "r", "n", "b", "q", "k", "b", "n", "r" };

            for (int i = 0; i < 8; i++)
            {
                // Siyahlar (Üstte - Küçük Harf)
                oyunTahtasi[0, i] = arkaSira[i];
                oyunTahtasi[1, i] = "p";

                // Beyazlar (Altta - Büyük Harf)
                oyunTahtasi[7, i] = arkaSira[i].ToUpper();
                oyunTahtasi[6, i] = "P";
            }
        }

        public static void TahtayiCiz()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Çerçeve çizgileri için

            
            Console.WriteLine("\n     a  b  c  d  e  f  g  h");
            Console.WriteLine("   ┌────────────────────────┐");

            for (int i = 0; i < 8; i++)
            {
                
                Console.Write(" " + (8 - i) + " │"); //sol kenar çerçevesi ve numarası

                for (int j = 0; j < 8; j++)
                {
                    string kareDegeri = oyunTahtasi[i, j];

                    // Eğer karede taş varsa onu yaz
                    if (kareDegeri != null)
                    {
                        Console.Write(" " + kareDegeri + " ");
                    }
                    // Taş yoksa, tahta deseni ver (Nokta ve Boşluk ile)
                    else
                    {
                        // (i + j) % 2 == 0 ise açık renk, değilse koyu renk kare
                        if ((i + j) % 2 == 0)
                            Console.Write("   "); // Beyaz kare
                        else
                            Console.Write(" · "); // Siyah kare
                    }
                }

                // Sağ kenar çerçevesi ve numarası
                Console.WriteLine("│ " + (8 - i));
            }

            // Alt Çerçeve ve Harfler
            Console.WriteLine("   └────────────────────────┘");
            Console.WriteLine("     a  b  c  d  e  f  g  h\n");
        }
        

        // --- YENI FONKSIYON: Sırf ASCII matematigi yapmamak icin ----
        static int HarfiSayiyaCevir(char harf)
        {
            // switch-case ile tek tek kontrol ediyoruz. Tam ogrenci isi.
            switch (harf)
            {
                case 'a': return 1;
                case 'b': return 2;
                case 'c': return 3;
                case 'd': return 4;
                case 'e': return 5;
                case 'f': return 6;
                case 'g': return 7;
                case 'h': return 8;
                default: return -1; // Hata kodu
            }
        }

        // --- YENI FONKSIYON: Sayiyi tersine cevirmek icin (Satir hesabi) ---
        static int SatirNumarasiniBul(char rakam)
        {
            // Kullanici '1' girdiginde dizide 8. indekse denk gelir
            switch (rakam)
            {
                case '1': return 8;
                case '2': return 7;
                case '3': return 6;
                case '4': return 5;
                case '5': return 4;
                case '6': return 3;
                case '7': return 2;
                case '8': return 1;
                default: return -1;
            }
        }

        static void HamleYap(string giris)
        {
            string[] parcalar = giris.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parcalar.Length != 2)
            {
                HataMesaji("Lutfen iki koordinat girin.");
                return;
            }

            string nereden = parcalar[0].ToLower();
            string nereye = parcalar[1].ToLower();

            if (nereden.Length != 2 || nereye.Length != 2)
            {
                HataMesaji("Koordinatlar 2 karakter olmali.");
                return;
            }

            // BURADA DEGISIKLIK YAPTIK: Artik matematik yok, fonksiyon cagiriyoruz.
            int sutunBas = HarfiSayiyaCevir(nereden[0]);
            int satirBas = SatirNumarasiniBul(nereden[1]);

            int sutunHedef = HarfiSayiyaCevir(nereye[0]);
            int satirHedef = SatirNumarasiniBul(nereye[1]);

            if (sutunBas == -1 || satirBas == -1 || sutunHedef == -1 || satirHedef == -1)
            {
                HataMesaji("Gecersiz koordinat girdiniz.");
                return;
            }

            // Tas Kontrolu
            string secilenTas = oyunTahtasi[satirBas, sutunBas];
            if (secilenTas == " " || secilenTas == "■")
            {
                HataMesaji("Orada tas yok.");
                return;
            }

            // Sira Kontrolu
            bool tasBeyaz = char.IsUpper(secilenTas[0]);
            if (tasBeyaz != beyazSirasi)
            {
                HataMesaji("Sira sizde degil!");
                return;
            }

            // Hedefteki Tas Kontrolu
            string hedefTas = oyunTahtasi[satirHedef, sutunHedef];
            if (hedefTas != " " && hedefTas != "■")
            {
                bool hedefBeyaz = char.IsUpper(hedefTas[0]);
                if (tasBeyaz == hedefBeyaz)
                {
                    HataMesaji("Kendi tasini yiyemezsin.");
                    return;
                }
            }

            if (HareketGecerliMi(satirBas, sutunBas, satirHedef, sutunHedef, secilenTas) == false)
            {
                HataMesaji("Gecersiz hamle.");
                return;
            }

            // Hamle Isleme
            if ((satirBas + sutunBas) % 2 == 0)
            {
                oyunTahtasi[satirBas, sutunBas] = "■";
            }
            else
            {
                oyunTahtasi[satirBas, sutunBas] = " ";
            }

            oyunTahtasi[satirHedef, sutunHedef] = secilenTas;

            // Sirayi degistir (Tersini al)
            beyazSirasi = !beyazSirasi;
        }

        static bool HareketGecerliMi(int r1, int c1, int r2, int c2, string tas)
        {
            string tip = tas.ToLower();

            // Math.Abs yerine basit cikarma ve if kullanabiliriz ama 
            // Math.Abs genelde bilinen bir seydir. Yine de garanti olsun diye elle yapalim:
            int dY = r2 - r1;
            if (dY < 0) dY = -dY; // Mutlak deger alma (elle)

            int dX = c2 - c1;
            if (dX < 0) dX = -dX;

            // PIYON
            if (tip == "p")
            {
                // Ternary operator (? :) kaldirildi, if-else yapildi
                int yon = 0;
                int baslangic = 0;

                if (tas == "P")
                {
                    yon = -1;       // Beyaz yukari gider (index azalir)
                    baslangic = 7;  // Beyazin baslangic satiri
                }
                else
                {
                    yon = 1;        // Siyah asagi gider (index artar)
                    baslangic = 2;  // Siyahin baslangic satiri
                }

                if (c1 == c2) // Duz gitme
                {
                    if (r2 == r1 + yon && (oyunTahtasi[r2, c2] == " " || oyunTahtasi[r2, c2] == "■"))
                        return true;

                    if (r1 == baslangic && r2 == r1 + (yon + yon))
                    {
                        if (YolBosMu(r1, c1, r2, c2) == true)
                        {
                            if (oyunTahtasi[r2, c2] == " " || oyunTahtasi[r2, c2] == "■")
                                return true;
                        }
                    }
                }
                else // Capraz yeme
                {
                    int xFark = c2 - c1;
                    if (xFark < 0) xFark = -xFark;

                    if (xFark == 1 && r2 == r1 + yon)
                    {
                        string hedef = oyunTahtasi[r2, c2];
                        if (hedef != " " && hedef != "■")
                            return true;
                    }
                }
                return false;
            }

            // KALE
            if (tip == "r")
            {
                if (r1 == r2 || c1 == c2)
                    return YolBosMu(r1, c1, r2, c2);
                return false;
            }

            // FIL
            if (tip == "b")
            {
                if (dY == dX)
                    return YolBosMu(r1, c1, r2, c2);
                return false;
            }

            // VEZIR
            if (tip == "q")
            {
                if ((r1 == r2 || c1 == c2) || (dY == dX))
                    return YolBosMu(r1, c1, r2, c2);
                return false;
            }

            // AT
            if (tip == "n")
            {
                if ((dY == 2 && dX == 1) || (dY == 1 && dX == 2))
                    return true;
                return false;
            }

            // SAH
            if (tip == "k")
            {
                if (dY <= 1 && dX <= 1)
                    return true;
                return false;
            }

            return false;
        }

        static bool YolBosMu(int r1, int c1, int r2, int c2)
        {
            // Math.Sign yerine elle kontrol (Prosedurel mantik)
            int rArtis = 0;
            if (r2 > r1) rArtis = 1;
            if (r2 < r1) rArtis = -1;

            int cArtis = 0;
            if (c2 > c1) cArtis = 1;
            if (c2 < c1) cArtis = -1;

            int r = r1 + rArtis;
            int c = c1 + cArtis;

            while (r != r2 || c != c2)
            {
                if (oyunTahtasi[r, c] != " " && oyunTahtasi[r, c] != "■")
                {
                    return false; // Engel var
                }

                r = r + rArtis;
                c = c + cArtis;
            }
            return true;
        }

        static void HataMesaji(string mesaj)
        {
            Console.WriteLine(">>> HATA: " + mesaj);
            Console.WriteLine("Devam etmek icin bir tusa basin...");
            Console.ReadKey();
        }
    }
}