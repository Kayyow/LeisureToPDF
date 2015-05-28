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
        private lavalloisirEntities db = new lavalloisirEntities();

        private List<category> _Categories;
        private category _SelectedCategorie;
        private ObservableCollection<leisure> _Leisures;
        private leisure _SelectedLeisure;

        public List<category> Categories
        {
            get { return _Categories; }
            set { _Categories = value; }
        }

        public category SelectedCategorie
        {
            get { return _SelectedCategorie; }
            set { _SelectedCategorie = value; }
        }

        public ObservableCollection<leisure> Leisures
        {
            get { return _Leisures; }   
            set { _Leisures = value; }
        }

        public leisure SelectedLeisure
        {
            get { return (leisure)GetValue(SelectedLeisureProperty); }
            set { SetValue(SelectedLeisureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedLeisure.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLeisureProperty =
        DependencyProperty.Register("SelectedLeisure", typeof(leisure), typeof(Window), new PropertyMetadata(null));     


        public MainWindow()
        {

            Categories = new List<category>();
            Leisures = new ObservableCollection<leisure>();

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
            leisure currentLeisure = SelectedLeisure != null ? SelectedLeisure : new leisure();

            if (SelectedLeisure != null)
            {
                currentLeisure = SelectedLeisure;
            }
            else
            {
                currentLeisure = new leisure();
            }

            currentLeisure.title = titleTextBox.Text;
            currentLeisure.email = emailTextBox.Text;
            currentLeisure.phone = phoneTextBox.Text;
            currentLeisure.website = websiteTextBox.Text;
            currentLeisure.description = descriptionRicheTextBox.Text;

            address currentAddress =  null;

            if (SelectedLeisure == null)
            {
                currentAddress = new address();
                currentAddress.number =  Convert.ToInt32(numberTextBox.Text);
                currentAddress.street = streetTextBox.Text;
                currentAddress.zip_code = zipCodeTextBox.Text;
                currentAddress.city = cityTextBox.Text;

                currentLeisure.address = currentAddress;
            }
            else
            {
                currentLeisure.address.number = Convert.ToInt32(numberTextBox.Text);
                currentLeisure.address.street = streetTextBox.Text;
                currentLeisure.address.zip_code = zipCodeTextBox.Text;
                currentLeisure.address.city = cityTextBox.Text;
            }
            
            currentLeisure.category = (category)categoryComboBox.SelectedItem;

            if (FieldValidator.CheckLeisure(db, currentLeisure) == true)
            {
                if (SelectedLeisure != null)
                {
                    db.SaveChanges();
                    MessageBox.Show("Loisir modifié ! ");
                }
                else
                {
                    db.address.Add(currentAddress);
                    db.leisure.Add(currentLeisure);
                    db.SaveChanges();
                    Leisures.Add(currentLeisure);
                    SelectedLeisure = currentLeisure;
                    MessageBox.Show("Loisir ajouté !");
                }

                titleTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                emailTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                phoneTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                websiteTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                numberTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                streetTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                zipCodeTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                cityTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                descriptionRicheTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                categoryComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            }
            else
            {
                MessageBox.Show("Un erreure s\'est produite lors de l\'ajout d\'un nouveau loisir.");
            }
        }

        private void NewLeisureButtonClick(object sender, RoutedEventArgs e)
        {
            SelectedLeisure = null;

            titleTextBox.Text = null;
            emailTextBox.Text = null;
            phoneTextBox.Text = null;
            websiteTextBox.Text = null;
            numberTextBox.Text = null;
            streetTextBox.Text = null;
            zipCodeTextBox.Text = null;
            cityTextBox.Text = null;
            descriptionRicheTextBox.Text = null;
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            db = new lavalloisirEntities();
            if (SelectedLeisure != null)
            {
                leisure leisureTotDelete = (from ld in db.leisure
                                            where ld.title == SelectedLeisure.title
                                            select ld).First();

                MessageBoxResult result = MessageBox.Show("Etes-vous sûre de vouloire supprimer le loisir : " + leisureTotDelete.title, "Suppression de loisir", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        db.address.Remove(leisureTotDelete.address);
                        db.leisure.Remove(leisureTotDelete);
                        db.SaveChanges();
                        Leisures.Remove(SelectedLeisure);
                        MessageBox.Show("Loisir supprimé !");
                        break;

                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Auccun loisir selectionné ! ");
            }
        }

    }
}
