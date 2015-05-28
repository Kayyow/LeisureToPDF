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
    public partial class MainWindow : Window {
		#region Attributs
		private lavalloisirEntities _Db;
		private List<category> _Categories;
		private ObservableCollection<leisure> _Leisures;
        private category _SelectedCategory;
		#endregion

		#region Properties
		public List<category> Categories {
            get { return _Categories; }
            set { _Categories = value; }
        }

		public ObservableCollection<leisure> Leisures {
			get { return _Leisures; }
			set { _Leisures = value; }
		}

        public category SelectedCategory {
            get { return _SelectedCategory; }
            set { _SelectedCategory = value; }
        }		

        public leisure SelectedLeisure {
            get { return (leisure)GetValue(SelectedLeisureProperty); }
            set { SetValue(SelectedLeisureProperty, value); }
        }
		#endregion

		// Using a DependencyProperty as the backing store for SelectedLeisure.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLeisureProperty =
			DependencyProperty.Register("SelectedLeisure", typeof(leisure), typeof(Window), new PropertyMetadata(null));

		#region Constructor
		public MainWindow() {
			this._Db = new lavalloisirEntities();
            this._Categories = new List<category>();
			this._Leisures = new ObservableCollection<leisure>();

			foreach (category c in this._Db.category) {
                this._Categories.Add(c);
            }

			foreach (leisure l in this._Db.leisure) {
				this._Leisures.Add(l);
            }

            InitializeComponent();
            this.DataContext = this;
        }
		#endregion

		#region Private Methods Utils
		private void ResetTextBoxes() {
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

		private void UpdateSources() {
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
		#endregion

		private void SaveButton(object sender, RoutedEventArgs e) {
			leisure currentLeisure = SelectedLeisure != null ? SelectedLeisure : new leisure();

            currentLeisure.title = titleTextBox.Text;
            currentLeisure.email = emailTextBox.Text;
            currentLeisure.phone = phoneTextBox.Text;
            currentLeisure.website = websiteTextBox.Text;
            currentLeisure.description = descriptionRicheTextBox.Text;

            address currentAddress = null;

			if (SelectedLeisure == null) {
                currentAddress = new address();
                currentAddress.number =  Convert.ToInt32(numberTextBox.Text);
                currentAddress.street = streetTextBox.Text;
                currentAddress.zip_code = zipCodeTextBox.Text;
                currentAddress.city = cityTextBox.Text;

                currentLeisure.address = currentAddress;
            } else {
                currentLeisure.address.number = Convert.ToInt32(numberTextBox.Text);
                currentLeisure.address.street = streetTextBox.Text;
                currentLeisure.address.zip_code = zipCodeTextBox.Text;
                currentLeisure.address.city = cityTextBox.Text;
            }
            
            currentLeisure.category = (category)categoryComboBox.SelectedItem;

            if (FieldValidator.CheckLeisure(this._Db, currentLeisure) == true) {
				if (SelectedLeisure != null) {
					this._Db.SaveChanges();
                    MessageBox.Show("Loisir modifié ! ");
                } else {
					this._Db.address.Add(currentAddress);
					this._Db.leisure.Add(currentLeisure);
					this._Db.SaveChanges();
					this._Leisures.Add(currentLeisure);
					SelectedLeisure = currentLeisure;
                    MessageBox.Show("Loisir ajouté !");
                }

				UpdateSources();
            } else {
                MessageBox.Show("Un erreure s\'est produite lors de l\'ajout d\'un nouveau loisir.");
            }
        }

        private void NewLeisureButtonClick(object sender, RoutedEventArgs e) {
			SelectedLeisure = null;

			ResetTextBoxes();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e) {
			if (SelectedLeisure != null) {
				leisure leisureTotDelete = (from ld in this._Db.leisure
											where ld.title == SelectedLeisure.title
                                            select ld).First();

                MessageBoxResult result = MessageBox.Show("Etes-vous sûr de vouloir supprimer le loisir : " + leisureTotDelete.title, "Suppression de loisir", MessageBoxButton.YesNo);
                switch (result) {
                    case MessageBoxResult.Yes:
						this._Db.address.Remove(leisureTotDelete.address);
						this._Db.leisure.Remove(leisureTotDelete);
						this._Db.SaveChanges();
						Leisures.Remove(SelectedLeisure);
                        MessageBox.Show("Loisir supprimé !");
                        break;

                    case MessageBoxResult.No:
                        break;
                }
            } else {
                MessageBox.Show("Aucun loisir selectionné !");
            }
		}
	}
}
