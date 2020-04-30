using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlowerClient
{
    public class Card
    {
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
        private string image;
        public string imageP
        {
            get { return image; }
            set { image = value; }
        }
    }
}
