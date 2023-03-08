using System.ComponentModel.DataAnnotations.Schema;

namespace ZIPCodeSuche_RotesBuch;

public class ZIPCodeSearch
{
    private class Item //initialise Attribute and Classification's properties 
    {
        public string Attribute { get; private set; }
        public string Classification { get; private set; }

        public Item(string attribute, string classification)
        {
            Classification = classification;
            Attribute = attribute;
        }
    }
    
    private class ItemSet //manage a single column as an independent Attribute
    {
        /*configure all the combinations of attribute and classification values for a column*/
        public string Column { get; private set; } //contains the column name of an independent attrubute
        public Dictionary<string, Item> Items { get; private set; }//an index where attribute and classification values are applied

        public ItemSet(string column)
        {
            Column = column;
            Items = new Dictionary<string, Item>(10);
        }

        public void AddItem(string attribute, string classification)//add all the combinations of the Item class.
        {
            var key = attribute + "->" + classification;

            Item item = null;
            if (Items.ContainsKey(key))
            {
                item = Items[key]; 
            }

            if (item == null) //The attribute-classification combination does not exist yet
            {
                item = new Item(attribute, classification);
                Items[key] = item;
            }
        }
    }
    public void Build(TextReader r)
    {
        string filePath = @"/Users/Desktop/geo-data.csv";
        using FileStream fs = File.Create(filePath); //applied FileStream
        r = new StreamWriter(fs);
        List<ItemSet> list = new List<ItemSet>();
        var header = r.ReadLine().Split(new char[] { ';' });
        foreach (var head in header) //read the header from the csv data
        {
            list.Add(new ItemSet(head));
        }

        string line;

        while ((line = r.ReadLine()) != null)
        {
            var tokens = line.Split(new char[] { ';' });

            string classification = tokens[0];

            for (int i = 0; i < tokens.Length; i++) //read the data and print out the content of .csv
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
