/* cb.txt  http://www.infojegyzet.hu/erettsegi/informatika-ismeretek/kozep-prog-2019okt/
• a bejegyzés percéhez tartozó óra, egész szám (6–13), például: 6
• a bejegyzés percértéke, egész szám (0–59), például 1
• a megadott percen belül a sofőr által indított adások száma, egész szám, például: 3
• a sofőr beceneve, szöveges adat, például: Bandi
﻿Ora;Perc;AdasDb;Nev
6;0;2;Laci
*/

using System;                       // Console
using System.IO;                    // StreamReader() StreamWriter()
using System.Collections.Generic;   // List<>
using System.Linq;                  // from where select

class CB
{
    public int    ora  {get; set;}
    public int    perc {get; set;}
    public int    adas {get; set;}
    public string nev  {get; set;}

    public CB(string sor)
    {
       var s = sor.Split(';');
        ora =  int.Parse(s[0]);
        perc = int.Parse(s[1]);
        adas = int.Parse(s[2]);
        nev = s[3];
    }
}

class Program
{
    // 6. Készítsen AtszamolPercre azonosítóval egész típusú értékkel visszatérő metódust vagy függvényt, ami a paraméterként megadott óra- és percértéket percekre számolja át! Egy óra 60 percből áll. Például: 8 óra 5 perc esetén a visszatérési érték: 485 (perc).

    static int AtszamolPercre(int ora, int perc)
    {
        return ora * 60 + perc; 
    }

    static void Main(string[] args)
    {
        var lista = new List<CB>();
        var sr    = new StreamReader("cb.txt");

        var elsosor = sr.ReadLine();
        while(!sr.EndOfStream)
        {
            var sor = sr.ReadLine();
            lista.Add( new CB(sor) );
        }
        sr.Close();

        // 3. Határozza meg és írja ki a képernyőre a minta szerint, hogy hány bejegyzés található a forrásállományban!

        Console.WriteLine($"3. feladat: Bejegyzések száma: {lista.Count} db");

        // 4. Döntse el és írja ki a képernyőre a minta szerint, hogy található-e a naplóban olyan bejegyzés, amely szerint a sofőr egy percen belül pontosan 4 adást indított! A keresést ne folytassa, ha az eredményt meg tudja határozni!

        var van_negy_adas = false;
        foreach (var item in lista)
        {
            if (item.adas == 4)
            {
                van_negy_adas = true;
                break;
            }
        }
        if (van_negy_adas)  Console.WriteLine($"4. feladat: Volt négy adást indító sofőr.");
        else                Console.WriteLine($"4. feladat: Nem volt négy adást indító sofőr.");
        
        // 5. Kérje be a felhasználótól egy sofőr nevét, majd határozza meg a sofőr által indított hívások számát a napló bejegyzéseiből! Az eredményt a minta szerint írja ki a képernyőre! Ha olyan sofőr nevét adja meg a felhasználó, aki nem szerepel a naplóban, akkor a „Nincs ilyen nevű sofőr!” mondat jelenjen meg!

        Console.Write(       $"5. feladat: Kérek egy nevet: ");
        var nev = Console.ReadLine();
        var hivasok = 
        (
            from sor in lista
            where sor.nev == nev
            select sor.adas
        );
        if (hivasok.Any()) 
        {
            Console.WriteLine($"        {nev} {hivasok.Sum()}x használta a CB rádiót.");
        }
        else
        {  
            Console.WriteLine($"        Nincs ilyen nevű sofőr."); 
        }

        // 7. Készítsen szöveges állományt cb2.txt néven, melybe a forrásállományban található bejegyzéseket írja ki új formátumban! Az órákat és a perceket percekre számolja át az előző feladatban elkészített metódus (függvény) hívásával! Az új állomány első sorát és az adatsorokat a minta szerint alakítsa ki!

        var sw = new StreamWriter("cb2.txt");
        sw.WriteLine("Kezdes;Nev;AdasDb");
        foreach (var sor in lista)
        {
            var kezdes = AtszamolPercre( sor.ora, sor.perc);
            sw.WriteLine($"{kezdes};{sor.nev};{sor.adas}");
        }
        sw.Close();

        // 8. Határozza meg és írja ki a minta szerint a sofőrök számát a forrásállományban található becenevek alapján! Feltételezheti, hogy nincs két azonos becenév.
        
        var statisztika = new Dictionary<string, int>();
        foreach(var sor in lista)
        {
            if (statisztika.ContainsKey(sor.nev)) statisztika[sor.nev] += sor.adas; 
            else                                  statisztika[sor.nev]  = sor.adas;
        }
        Console.WriteLine($"8. feladat: Sofőrök száma: {statisztika.Count()} fő");

        // 9. Határozza meg a legtöbb adást indító sofőr nevét! A sofőr neve és az általa indított hívások száma a minta szerint jelenen meg a képernyőn.

        var adas = 
        (
            from item in statisztika
            orderby item.Value
            select item
        ).Last();

        Console.WriteLine($"9. feladat: Legtöbb adást indító sofőr");
        Console.WriteLine($"        Név: {adas.Key}");
        Console.WriteLine($"        Adások száma: {adas.Value} alkalom");

    } // --- end of Main() ---
} // ####### end of Program class #########