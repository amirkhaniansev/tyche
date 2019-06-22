using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestModelGen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Symbols : TabbedPage
    {
        public Symbols()
        {
            InitializeComponent();
        }
    }
}