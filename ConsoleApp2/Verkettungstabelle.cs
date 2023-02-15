namespace ConsoleApp2;

public class Verkettungstabelle<TKey, TValue>
{
    private class Paare
    {
        

        public TKey Schluessel { get; private set; }
        public TValue Werte { get; set; }

        public Paare(TKey schluessel, TValue werte)
        {
            Schluessel = schluessel;
            Werte = werte;
        }

        /*nicht wie im LinierSondierung, hier wird die Schlüssel-Wert-Paare direkt in einem List gespeichert werden d.h.
         werden die Paare im List zum Speichern der Schlüssel-Wert-Paare List-Instanzen angelegt.
         Deshalb wird das Löschenkennzeichen bool IsDeleted nicht mehr benötigt - innerhalb der verketteten Liste die Eintröge
         physisch gelöscht werden können!
         */
    }

    private List<Paare>[] items;
        public int Count { get; private set; }

        public Verkettungstabelle(int laenge = 10)
        {
            /**
             * bei einen Füllgrad von 50 Prozent zu gewährleisten wird das Array verdoppelt d.h.
             * die Anzahl der zu speichernenden Elemente übergeben wird, eingegeben wird. 
             */
            laenge = CalcPrimeLength(laenge * 2);

            items = new List<Paare>[laenge];
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

        public void Add(TKey schluessel, TValue werte)
        {
            /*prüfen ob der Hashwert bereits ein List-Objekt enthält*/
            if (ContainsKey(schluessel))
            {
                throw new ArgumentException("Item schon vorhanden");
            }

            /*sofern kein List-Objekt im Hashwert enthält dann wird ein neues List-Objekt instanziiert und dem
             ermittelten ListPlatz/ArrayPlatz zugewiesen*/
            int hashhash = GetHash(schluessel);
            List<Paare> liste = items[hashhash];

            /*dann wird das neue Schlüssel-Wert-Paar in die entsprechende List eingefügt*/
            if (liste == null)
            {
                liste = new List<Paare>();
                items[hashhash] = liste;

            }

            liste.Add(new Paare(schluessel, werte));

            Count++;
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
            int hashhash = GetHash(schluessel);
            int schritt = GetHashSteps(schluessel);

            while (items[hashhash] != null)
            {
                if (items[hashhash].Equals(schluessel))
                {
                    return true;
                }

                hashhash += schritt;
                hashhash %= items.Length;
            }

            return false;
        }

        /**
         * Bisher werden mehrere Methoden beschrieben, aber dazu soll eine iterierbare Methode Values implementiert werden,
         * mit der alle Werte der Hashtabelle durchlaufen werden können.
         * Zur iterierbaren Methode liefert sie die Schnittstelle IEnumerable zurück.
         */
        public IEnumerable<TValue> Values()
        {
            /*
             * Die äußere Schreife führt den Durchlauf für zwei folgendenen Elementen aus:
             * das Array mit den List-Objekten und in einer inneren Schreife (siehe for-schreife unten)
             */
            
            foreach (List<Paare> liste in items)
            {
                if (liste != null)
                {
                    /*Die vom Arrayplatz zugewiesene Elemente, die sich in dem Listobjekt befinden, laufen hier durch*/
                    for (int i = 0; i < liste.Count; i++)
                    {
                        yield return liste[i].Werte;
                    }                }
            }
        }
    }
