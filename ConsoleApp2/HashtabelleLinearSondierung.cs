using System.Collections;
using System.Linq;

namespace ConsoleApp2;
/*
 * die Klasse benötigt zwei generische Datentypen Tkey und TValue da beliebige Datentypen verwendet werden sowohl für den Schlüssel
 * als auch für den Wert. 
 */
public class HashtabelleLinearSondierung<TKey, TValue> 
{
    private class Paare //zur Umsetzung wird die Klasse Paare benötigt
    {
        public TKey Schluessel { get; private set; } //die Klasse speichert die Schlüssel-Paare
        public TValue Werte { get; set; } //die Klasse speichert die Werte-Paare
        public bool IstEliminiert { get; internal set; } //eigenschaft die anfragt, ob das Element bereits gelöscht wurde

        public Paare(TKey schluessel, TValue werte, bool istEliminiert = false)
        {
            Schluessel = schluessel;
            Werte = werte;
            IstEliminiert = istEliminiert;
        }
    }

    private Paare[] items; //arrays in dem die Schlüsse-Wert-Paare gespeichert werden sollen

    public int Zahlen { get; private set; }

    public HashtabelleLinearSondierung(int laenge = 10)
    {
        /**
         * bei einen Füllgrad von 50 Prozent zu gewährleisten wird das Array verdoppelt d.h.
         * die Anzahl der zu speichernenden Elemente übergeben wird, eingegeben wird. 
         */
        laenge = CalcPrimeLength(laenge * 2);

        items = new Paare[laenge];

        TValue[] collections = items.Select(KeyValuePair => KeyValuePair.Werte).ToArray();
        var result = new Hashtable();
        
        GetNewKVPair(collections, 0, String.Empty, result);

    }

    private static void GetNewKVPair(TValue[] collections, int collectionIndex, string currentIndex,
        Hashtable result)
    {
        TValue currentLocation = collections[collectionIndex];
        bool isTail = collections.Length == collectionIndex + 1;
        foreach (var s in (IEnumerable)currentLocation)
        {
            if (isTail)
            {
                result.Add(0, currentIndex + s);
            }
            else
            {
                GetNewKVPair(collections, collectionIndex + 1, currentIndex + s, result);
            }
        }
        {
            
        }
    }

    private static int CalcPrimeLength(int laenge)
    {
        int m = 0, flag = 0;
        m = laenge / 2;
        for (int i = 2; i < m; i++)
        {
            if (laenge % i == 0)
            {
                flag = 1;
                break;
            }
        }

        if (flag == 0)
        {
            return laenge;
        }

        return 0;
    }

    public void Einfuegen(TKey schluessel, TValue werte)
    {
        if (ContainsKey(schluessel)) //Frage, die methode ContainsKey schon integriert wird
        {
            throw new ArgumentException("items schon vorhanden");
        }

        if (Zahlen + 1 == items.Length) //Überlauf bei keine aufzunehmenden Elemente 
        {
            throw new ArgumentException("in dem Array gibt es kein Platz mehr");
        }
        /* über die Methode Calc... berechnet nächsthöhere Primzahl der Arraygröße, um zu gewährleisten, dass eine Primerzahl zur Berechnung des
     Modulo verwendet wird*/
        int haaaasssssh = Math.Abs(schluessel.GetHashCode()) % items.Length;
        //Eintrag an dem leeren Arrayplatz hier gespeichert werden als nächstes.
        /*Sofern der Arrayplatz leer ist, wird die while-Schreife übergangen */
        while (items[haaaasssssh] != null && !items[haaaasssssh].IstEliminiert) //einfugen eines Elements in die Hashtabelle
        {
            ++haaaasssssh; //fortwährendes Erhöhen des Index um 1 nach einem leeren oder geläschten Arrayplatz gesucht wird
            haaaasssssh %= items.Length; /* bei Erreichen der Obergrenze des Arrays wieder beim Index 0 zu beginnen.
            somit wird der Index über die Modulofunktion angepasst*/
        }

        items[haaaasssssh] = new Paare(schluessel, werte);
        Zahlen++;
    }

    public bool Loeschen(TKey schlussel)
    {
        //Hashwerte ermitteln
        int haaaasssssh = Math.Abs(schlussel.GetHashCode()) % items.Length;

        /*
         * innerhalb der while-Schreife wird das Element an dem berechneten Arrayplatz gesucht
         * anschließend wird linear weitergesucht
         */
        while (items[haaaasssssh] != null)
        {
            /*
             * Wurde das Element gefunden, wird das Läschenkennzeichen IstEliminiert auf true gesetzt.
             */
            if (items[haaaasssssh].Schluessel.Equals(schlussel) && !items[haaaasssssh].IstEliminiert)
            {
                items[haaaasssssh].IstEliminiert = true;
                Zahlen--;
                return true;
            }

            ++haaaasssssh; //fortwährendes Erhöhen des Index um 1 nach einem leeren oder geläschten Arrayplatz gesucht wird
            /* bei Erreichen der Obergrenze des Arrays wieder beim Index 0 zu beginnen.
            somit wird der Index über die Modulofunktion angepasst*/
            haaaasssssh %= items.Length;
        }

        return false;
    }

    //den Wert zu einem Schlüssel zu erhalten, ein Indexer soll verwendet werden
    public TValue this[TKey schlussel]
    {
        get
        {
            int haaaasssssh = Math.Abs(schlussel.GetHashCode()) % items.Length;
            while (items[haaaasssssh] != null)
            {
                if (items[haaaasssssh].Schluessel.Equals(schlussel) && !items[haaaasssssh].IstEliminiert)
                {
                    //den Wert des gefundenen Eintrags zurückliefern
                    return items[haaaasssssh].Werte;
                }
                ++haaaasssssh;
                haaaasssssh %= items.Length;
            }
          //wurde der Eintrag nicht gefunden, wird eine Ausnahme ausgelöst
            throw new ArgumentException("Schlussel nicht gefunden");
        }
    }
    
    //hierzu die Doppel-Hashing Methode
    //zur Suche nach einem leeren Arrayplatz wird eine variable Schrittweite (nicht 1) verwendet.
    //Berechung den leeren Arrayplatz/Eintrag
    private int GetHashSteps(TKey schluessel)
    {
        return 5 - (schluessel.GetHashCode() % 5);
    }

    private int GetHash(TKey schluessel)
    {
        return Math.Abs(schluessel.GetHashCode()) % items.Length;
    }

    public bool ContainsKey(TKey schluessel)
    {
        /*
         * zur Ermittlung des eigentlichen Arrayplatzes in Containskey-Methode ausgelarget. 
         */
        int haaaasssssh = GetHash(schluessel);
        int schritt = GetHashSteps(schluessel);

        while (items[haaaasssssh] != null)
        {
            if (items[haaaasssssh].Schluessel.Equals(schluessel) && !items[haaaasssssh].IstEliminiert)
            {
                return true;
            }
            haaaasssssh += schritt;
            haaaasssssh %= items.Length;
        }

        return false;
    }

}