using System.Collections;

namespace SonnenschutzVorhersagen;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

public class OneR
{
    private class Item
    {
        public string Attribute { get; private set; }
        public string Classification { get; private set; }
        public int Frequency { get; set; }

        /*In der Klasse Item wurden diese beiden Methoden überschreiben, um das Prinzip zu verdeutlichen - ein String-Objekt ist
         schon als Index verwendet.*/

        public Item(string attribute, string classification, int frequency = 1)
        {
            this.Classification = classification;
            this.Attribute = attribute;
            this.Frequency = frequency;
        }
        
        //zu BEACHTEN: die Methoden Equals und GetHashCode der Klasse müssen zur Verwendung einer Klasse als Schlüssel überschrieben werden

        public override bool Equals(object obj)
        {
            Item item = obj as Item;

            return (Attribute.Equals(item.Attribute) && Classification.Equals(item.Classification));
        }
        
        /*Der Referenzzeiger der Instanz zur Erzeugung eines Hashcodes und auch zum Vergleich verwendet
         In der GetHashCode müssen daher die einzelnen Wertelemente in der Klasse des Schlüssels verwendet werden,
         um einen sinnvollen Hashcode zu erzeugen.*/

        public override int GetHashCode()
        {
            //Für Werttypen erzeugen die entsprechenden Methoden bereits sinnvolle Hashcodes
            return Classification.GetHashCode() + Attribute.GetHashCode();
        }
    }

    private class ItemSet
    {
        public string Column { get; private set; }
        private string _column;
        public Hashtable Items { get; private set; }

        public double ErrorRate { get; private set; }
        private double _errorRate;

        public ItemSet(string column)
        {
            Column = column;
            Items = new Hashtable(10);
        }

        public void AddItem(string attribute, string classification)
        {
            var key = Convert.ToString(attribute + Convert.ToString("->")) + classification;

            Item item = null;
            if (Items.ContainsKey(key))
                ;

            if (item == null)
            {
                item = new Item(attribute, classification);
                
            }
            else
                item.Frequency += 1;
        }

        public void Process()
        {
            var result = from Item item in Items.Values
                orderby item.Attribute, item.Frequency descending
                select item;

            int total = 0;
            int correct = 0;
            string attribute = null;

            foreach (Item item in result)
            {
                total += item.Frequency;

                var key = item.Attribute + "->" + item.Classification;

                // Gruppenwechsel
                if (attribute == null || attribute != item.Attribute)
                {
                    attribute = item.Attribute;
                    correct += item.Frequency;
                }
                else
                    Items.Remove(key);
            }

            ErrorRate = 100.0 - (correct * 100.0 / total);
        }

        public override string ToString()
        {
            string s = Column + Convert.ToString(Constants.vbLf);

            foreach (Item value in Items.Values)
                s += string.Format("  {0}->{1} : {2}" + Constants.vbLf, value.Attribute, value.Classification, value.Frequency);

            if (!double.IsNaN(ErrorRate))
                s += string.Format(Constants.vbLf + "  ErrorRate: {0} %" + Constants.vbLf, ErrorRate);

            return s;
        }
    }

    public Hashtable Solution { get; set; }
    

    public OneR()
    {
        Solution = new Hashtable();
    }

    public void Build(TextReader r)
    {
        var list = new List<ItemSet>();

        var header = r.ReadLine().Split(new char[] { ';' });

        // Spaltenüberschriften lesen
        foreach (string head in header)
            list.Add(new ItemSet(head));

        // Daten einlesen und Häufigkeiten ermitteln
        string line = r.ReadLine();

        while (line != null)
        {
            var tokens = line.Split(new char[] { ';' });

            string classfication = tokens[0];

            for (int i = 1; i <= tokens.Length - 1; i++)
            {
                string attribute = tokens[i];

                list[i].AddItem(attribute, classfication);
            }
            line = r.ReadLine();
        }
        r.Close();

        // Gruppen mit bester Vorhersage pro Spalte ermitteln
        foreach (ItemSet items in list)
            // voher: Ausgabe alle Häufigkeitsverteilungen
            // Console.WriteLine(items);
            // nacher: Ausgabe bester Häufigkeitsverteilungen
            // Console.WriteLine(items);
            items.Process();

        // Spalte mit geringster Fehlerrate ermitteln
        var result = (from items in list
                      where !double.IsNaN(items.ErrorRate)
                      orderby items.ErrorRate
                      select items);

        ItemSet sol = result.First();

        // Lösung für Vorhersage speichern
        foreach (Item item in sol.Items.Values)
            Solution.Add(item.Attribute, item.Classification);

        Console.WriteLine("".PadLeft(80, '_'));
        Console.WriteLine("Regeln: " + sol.ToString());

        list.Clear();
    }

    public string Classify(string value)
    {
        return (string)Solution[value];
    }
}
