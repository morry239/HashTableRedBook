using System.ComponentModel.DataAnnotations.Schema;

namespace ZIPCodeSuche_RotesBuch;

public class ZIPCodeSearch
{
    private class Item //Eigenschaften der Attribute und der Classification zuzuweisen
    {
        public string Attribute { get; private set; }
        public string Classification { get; private set; }

        public Item(string attribute, string classification)
        {
            Classification = classification;
            Attribute = attribute;
        }
    }
    
    private class ItemSet //verwaltet eine einzelne Spalte als ein unabhängiges Attribut
    {
        /*dabei werden alle Kombinationen von Attributwerten und Klassifikationswerten für eine einzelne Spalte
         berücksichtigt.*/
        public string Column { get; private set; } //enthält den Spaltennamen des unabhängigen 
        public Dictionary<string, Item> Items { get; private set; }//Indiz, der den Text aus Attributwert und Klassifikationswert verwendet 

        public ItemSet(string column)
        {
            Column = column;
            Items = new Dictionary<string, Item>(10);
        }

        public void AddItem(string attribute, string classification)//jede Kombination aus einem Kombination der Itemset-Klasse hinzugefügt
        {
            var key = attribute + "->" + classification;

            Item item = null;
            if (Items.ContainsKey(key))
            {
                item = Items[key]; 
            }

            if (item == null) //Attribut- und Klassifikation Kombination existiert noch nicht
            {
                item = new Item(attribute, classification);
                Items[key] = item;
            }
        }
    }
    public void Build(TextReader r)
    {
        string filePath = @"/Users/mamikawamura/Desktop/geo-data.csv";
        r = File.OpenText(filePath);
        List<ItemSet> list = new List<ItemSet>();
        var header = r.ReadLine().Split(new char[] { ';' });
        foreach (var head in header) //Spaltenüberschriften lesen
        {
            list.Add(new ItemSet(head));
        }

        string line;

        while ((line = r.ReadLine()) != null)
        {
            var tokens = line.Split(new char[] { ';' });

            string classification = tokens[0];

            for (int i = 0; i < tokens.Length; i++) //Daten einlesen und Häufigkeiten ermitteln
            {
                string attribute = tokens[i];
                list[i].AddItem(attribute, classification);
            }
        }
        r.Close();
        list.Clear();
    }

    public string Classify(string value)
    {
        return Solution[value];
    }
    public Dictionary<string, string> Solution { get; private set; }


}