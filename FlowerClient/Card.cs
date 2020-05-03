using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlowerClient
{
    public class Card
    {
        private int id;
        public int idP 
        {
            get { return id; }
        }
        private string name;
        public string nameP
        {
            get { return name; }
            set { name = value; }
        }
        private string caption;
        public string captionP
        {
            get { return caption; }
            set { caption = value; }
        }
        private string author;
        public string authorP
        {
            get { return author; }
            set { author = value; }
        }
        private string exposition;
        public string expositionP
        {
            get { return exposition; }
            set { exposition = value; }
        }
        private string speciesName;
        public string speciesNameP
        {
            get { return speciesName; }
            set { speciesName = value; }
        }
        private string lifeForm;
        public string lifeFormP
        {
            get { return lifeForm; }
            set { lifeForm = value; }
        }
        private string group;
        public string groupP
        {
            get { return group; }
            set { group = value; }
        }
        private string economicGroup;
        public string economicGroupP
        {
            get { return economicGroup; }
            set { economicGroup = value; }
        }
        private string people;
        public string peopleP
        {
            get { return people; }
            set { people = value; }
        }
        private string history;
        public string historyP
        {
            get { return history; }
            set { history = value; }
        }
        private string buildings;
        public string buildingsP
        {
            get { return buildings; }
            set { buildings = value; }
        }
        private string category;
        public string categoryP
        {
            get { return category; }
            set { category = value; }
        }
        private string season;
        public string seasonP
        {
            get { return season; }
            set { season = value; }
        }
        private string year;
        public string yearP
        {
            get { return year; }
            set { year = value; }
        }

        public ImageSource ImageS { get; set; }

        public Card()
        {

        }

        public Card(DataRow t)
        {
            id = Convert.ToInt32(t[0].ToString());
            author = t[1].ToString();
            buildings = t[2].ToString();
            category = t[3].ToString();
            economicGroup = t[4].ToString();
            exposition = t[5].ToString();
            group = t[6].ToString();
            people = t[7].ToString();
            history = t[8].ToString();
            lifeForm = t[9].ToString();
            speciesName = t[10].ToString();

            nameP = t[10].ToString();

            year = t[11].ToString();
            season = t[12].ToString();
        }
    }
}
