using System;
using System.Text;

namespace SatrancOdevi
{
    class Program
    {
        // 10x10luk dizi
        static string[,] oyunTahtasi = new string[10, 10];
        static bool beyazSirasi = true;

        static void Main(string[] args)
        {
            TahtayiDiz();

            while (true)
            {
                Console.Clear();
                MenuGoster();
                TahtayiCiz();

                Console.WriteLine();

                // O sormus oldugun kisa satiri if-else ile degistirdim:
                string siraKimde = "";
                if (beyazSirasi == true)
                {
                    siraKimde = "BEYAZ (Buyuk Harf)";
                }
                else
                {
                    siraKimde = "SIYAH (Kucuk Harf)";
                }

                Console.WriteLine("Sira: " + siraKimde);
                Console.Write("Hamlenizi girin (Orn: e2 e4) veya Cikis icin 'q': ");

                string giris = Console.ReadLine();

                if (giris == "q" || giris == "Q")
                {
                    Console.WriteLine("Oyun kapatiliyor...");
                    break;
                }

                HamleYap(giris);
            }
        }

        static void MenuGoster()
        {
            // Bu satir kare karakterlerin (■) düzgün cikmasi icin gereklidir.
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("----------------------------------");
            Console.WriteLine("  DOKUZ EYLUL EEE - SATRANC V1.0  ");
            Console.WriteLine("----------------------------------");
        }

        static void TahtayiCiz()
        {
            Console.OutputEncoding = Encoding.UTF8;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    string kare = oyunTahtasi[i, j];

                    if (kare == "")
                    {
                        kare = " ";
                    }

                    Console.Write(" " + kare + " ");
                }
                Console.WriteLine();
            }
        }

        static void TahtayiDiz()
        {
            // Kenar suslemeleri dizilerle
            string[] harfler = { "#", "a", "b", "c", "d", "e", "f", "g", "h", "#" };
            string[] sayilar = { "#", "8", "7", "6", "5", "4", "3", "2", "1", "#" };

            for (int i = 0; i < 10; i++)
            {
                oyunTahtasi[0, i] = harfler[i];
                oyunTahtasi[9, i] = harfler[i];
                oyunTahtasi[i, 0] = sayilar[i];
                oyunTahtasi[i, 9] = sayilar[i];
            }

            for (int r = 1; r <= 8; r++)
            {
                for (int c = 1; c <= 8; c++)
                {
                    if ((r + c) % 2 == 0)
                    {
                        oyunTahtasi[r, c] = "■";
                    }
                    else
                    {
                        oyunTahtasi[r, c] = " ";
                    }
                }
            }

            oyunTahtasi[0, 0] = "#"; oyunTahtasi[0, 9] = "#";
            oyunTahtasi[9, 0] = "#"; oyunTahtasi[9, 9] = "#";

            string[] tasSirasi = { " ", "r", "n", "b", "q", "k", "b", "n", "r" };

            for (int k = 1; k <= 8; k++)
            {
                oyunTahtasi[2, k] = "p";
                oyunTahtasi[7, k] = "P";

                oyunTahtasi[1, k] = tasSirasi[k];
                oyunTahtasi[8, k] = tasSirasi[k].ToUpper();
            }
        }

        // --- YENI FONKSIYON: Sırf ASCII matematigi yapmamak icin ---
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
            string[] parcalar = giris.Split(' ');
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