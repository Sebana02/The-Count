using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace The_Count
{
    public class SkinDesc : ObservableObject
    {
        public int id { get; private set; }
        public string nombre { get; private set; }
        public string image { get; private set; }

        private SolidColorBrush color_;
        public SolidColorBrush color
        {
            get { return color_; }
            set { Set(ref color_, value); }
        }

        public SkinDesc(int id_, string image_, string nombre_, SolidColorBrush color_)
        {
            id = id_;
            nombre = nombre_;
            image = image_;
            color = color_;
        }
    }
    public class SkinLogic : ObservableObject
    {
        public ObservableCollection<SkinDesc> aspectos { get; private set; }

        public SkinLogic()
        {
            aspectos = new ObservableCollection<SkinDesc>
            {
                new SkinDesc(0,"Assets\\skin2.png","Aspecto 1",new SolidColorBrush(Colors.Green)),
                new SkinDesc(1,"Assets\\skin2.png","Aspecto 2",new SolidColorBrush(Colors.White)),
                new SkinDesc(2,"Assets\\skin2.png","Aspecto 3",new SolidColorBrush(Colors.White)),
                new SkinDesc(3,"Assets\\skin2.png","Aspecto 4",new SolidColorBrush(Colors.White)),
                new SkinDesc(4,"Assets\\skin2.png","Aspecto 5",new SolidColorBrush(Colors.White)),
                new SkinDesc(5,"Assets\\skin2.png","Aspecto 6",new SolidColorBrush(Colors.White)),
                new SkinDesc(6,"Assets\\skin2.png","Aspecto 7",new SolidColorBrush(Colors.White)),
                new SkinDesc(7,"Assets\\skin2.png","Aspecto 8",new SolidColorBrush(Colors.White)),
                new SkinDesc(8,"Assets\\skin2.png","Aspecto 9",new SolidColorBrush(Colors.White)),
                new SkinDesc(9,"Assets\\skin2.png","Aspecto 10",new SolidColorBrush(Colors.White))
            };

            sel = 0;
        }

        public int sel;
    }
}
