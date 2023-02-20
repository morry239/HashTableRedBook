using System.Collections;
using System.Data;
using IronXL;
using Microsoft.VisualBasic;

namespace ZIPCodeSearch;

public class Finder
{
        public string City { get; private set; }
        public string State { get; private set; }
        
        public string Column { get; private set; }
        private string _column;
        public Hashtable Items { get; private set; }

        private static Hashtable _solution;

        public static Hashtable Solution
        {
            get
            {
                return _solution;
            }

            private set
            {
                _solution = value;
            }
        }

        public Finder() {} //default constructor

        public Finder(string state, string city, string column)
        {
            this.State = state;
            this.City = city;
            Column = column;
            Items = new Hashtable(10);
            Solution = new Hashtable();
        }

        public override int GetHashCode()
        {
            return State.GetHashCode() + City.GetHashCode();
        }
 
    /// <summary>
    /// this method will read the excel file and copy its data into a datatable
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public DataTable ReadCSV(string fileName)
    {
        WorkBook workbook = WorkBook.Load(fileName);
        WorkSheet sheet = workbook.DefaultWorkSheet;
        return sheet.ToDataTable(true);
    }

    public DataTable ActivateReader(string csvFilePath)
    {
        var csvFilereader = new DataTable();
        csvFilereader = ReadCSV(csvFilePath);
        DataTable dt = new DataTable();
        if (csvFilePath != null)
        {
            csvFilereader = dt;
        }

        return dt;
    }

 

        public void AddItem(string attribute, string classification, string column)
        {
            column = ActivateReader(@"/Users/geo-data.csv").ToString();
            var key = Convert.ToString(attribute + Convert.ToString("->")) + classification; //irgendwo hier soll csv datei abgelesen werden

            Finder item = null;
            if (Items.ContainsKey(key)) //pruefen ob die hash- werte state in der DatenTablle/CSVData habe --> zu ToStringMethode 
                ;

            if (item == null)
            {
                item = new Finder(attribute, classification, column);
                
            }
        }
        public override string ToString()
        {
            string s = Column + Convert.ToString(Constants.vbLf);

            foreach (Finder value in Items.Values)
                s += string.Format("  {0}->{1} : {2}" + Constants.vbLf, value.City, value.State);

            

            return s;
        }
 
    public void Build(TextReader r)
    {
        var list = new System.Collections.Generic.List<int>();

        string filePath = @"/Users/Desktop/geo-data.csv";
        r = File.OpenText(filePath);

        var header = r.ReadLine().Split(new char[] { ';' });

        // Spaltenüberschriften lesen
        foreach (string head in header)
        {
            list.Add(new Solution(head));
        }

        string line = r.ReadLine();
        
        while (line != null)
        {
            var tokens = line.Split(new char[] { ';' });

            string classfication = tokens[0];

            for (int i = 1; i <= tokens.Length - 1; i++)
            {
                Finder add = new Finder();
                string attribute = tokens[i];


                list[i].AddItem(attribute, classfication, "");
            }
            line = r.ReadLine();
        }
        r.Close();

    }

    public string Classify(string value)
    {
        return (string)Solution[value];
    }
}
