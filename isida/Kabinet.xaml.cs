using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace isida
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Kabinet : TabbedPage
    {
        public Kabinet()
        {
            InitializeComponent();
        }
    }
}