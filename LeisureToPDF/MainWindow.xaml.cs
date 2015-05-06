using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LeisureToPDF.DAL;
using System.Collections.ObjectModel;

namespace LeisureToPDF
{

    public partial class MainWindow : Window
    {
        private lavalloisirEntities db;

        private List<category> _Categories;

        public List<category> Categories
        {
            get { return _Categories; }
            set { _Categories = value; }
        }

        private List<leisure> _Leisures;

        public List<leisure> Leisures
        {
            get { return _Leisures; }
            set { _Leisures = value; }
        }

        private leisure _SelectedLeisure;

        public leisure SelectedLeisure
        {
            get { return _SelectedLeisure; }
            set { _SelectedLeisure = value; }
        }
        
        
         public MainWindow()
        {
                        
            db = new lavalloisirEntities();

            Categories = new List<category>();
            Leisures = new List<leisure>();

            foreach (category c in db.category)
            {
                Categories.Add(c);
            }

            foreach (leisure l in db.leisure)
            {
                Leisures.Add(l);
            }

            InitializeComponent();
            this.DataContext = this;
        }

         private void SaveButton(object sender, RoutedEventArgs e)
         {
             db = new lavalloisirEntities();
             this.DataContext = db;

             Field_Util checkField = new Field_Util();
             leisure currentLeisure = new leisure();

             currentLeisure.title = titleTextBox.Text;
             currentLeisure.email = emailTextBox.Text;
             currentLeisure.phone = phoneTextBox.Text;
             currentLeisure.website = websiteTextBox.Text;
             currentLeisure.description = descriptionRicheTextBox.Text;

            // var categorySelected = categoryComboBox.SelectedItem;

             //category cat = (from c in db.category
             //                where c.id == idselected
             //                select c).FirstOrDefault();

             //currentLeisure.category = cat;

             address currentAddress = new address();

             if (numberTextBox.Text != "")
             {
                 currentAddress.number = Convert.ToInt32(numberTextBox.Text);
             }
             currentAddress.street = streetTextBox.Text;
             currentAddress.zip_code = zipCodeTextBox.Text;
             currentAddress.city = cityTextBox.Text;

             if (checkField.CheckAddress(db, currentAddress) == true)
             {
                 currentLeisure.address = currentAddress;
             }

             if (checkField.CheckLeisure(db, currentLeisure) == true)
             {
                 db.leisure.Add(currentLeisure);
                 db.SaveChanges();
             }
             else
             {
                 MessageBox.Show("Un erreure s\'est produite lors de l\'ajout d\'un nouveau loisir.");
             }
         }

         private void SelectionLeisureClick(object sender, SelectionChangedEventArgs e)
         {
             titleTextBox.Text = SelectedLeisure.title;
             emailTextBox.Text = SelectedLeisure.email;
             phoneTextBox.Text = SelectedLeisure.phone;
             websiteTextBox.Text = SelectedLeisure.website;
             numberTextBox.Text = Convert.ToString(SelectedLeisure.address.number);
             streetTextBox.Text = SelectedLeisure.address.street;
             zipCodeTextBox.Text = SelectedLeisure.address.zip_code;
             cityTextBox.Text = SelectedLeisure.address.city;
             descriptionRicheTextBox.Text = SelectedLeisure.description;
             categoryComboBox.SelectedItem = SelectedLeisure.category;
         }
    }
}
