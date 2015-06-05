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
using System.Collections.ObjectModel;
using LeisureToPDF.Database;
using LeisureToPDF.Services;
using Microsoft.Win32;
using System.IO;

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

		public string Notification {
			get { return (string)GetValue(NotificationProperty); }
			set { SetValue(NotificationProperty, value); }
		}
		#endregion

		// Using a DependencyProperty as the backing store for SelectedLeisure.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLeisureProperty =
            DependencyProperty.Register("SelectedLeisure", typeof(leisure), typeof(Window), new PropertyMetadata(null));

		// Using a DependencyProperty as the backing store for Notification.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty NotificationProperty =
			DependencyProperty.Register("Notification", typeof(string), typeof(Window), new PropertyMetadata(null));

		#region Constructor
		public MainWindow() {
			this._Db = new lavalloisirEntities();
            this._Categories = new List<category>();
            this._Leisures = new ObservableCollection<leisure>();

			foreach (category c in this._Db.category) {
                this._Categories.Add(c);
            }

            foreach (leisure l in this._Db.leisure) {
				this._Leisures.Add((leisure)l);
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
            Notification = "";  
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

		private void MapLeisure(leisure leisure) {
			leisure.title = titleTextBox.Text;
			leisure.email = emailTextBox.Text;
			leisure.phone = phoneTextBox.Text;
			leisure.website = websiteTextBox.Text;
			leisure.description = descriptionRicheTextBox.Text;

            if (SelectedLeisure == null) {
                address address = new address();
                address.number = Convert.ToInt32(numberTextBox.Text);
                address.street = streetTextBox.Text;
                address.zip_code = zipCodeTextBox.Text;
                address.city = cityTextBox.Text;

                leisure.address = address;
            } else {
                leisure.address.number = Convert.ToInt32(numberTextBox.Text);
                leisure.address.street = streetTextBox.Text;
                leisure.address.zip_code = zipCodeTextBox.Text;
                leisure.address.city = cityTextBox.Text;
            }
			
			leisure.category = (category)categoryComboBox.SelectedItem;
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveButton(object sender, RoutedEventArgs e) {
            leisure leisure = SelectedLeisure == null ? new leisure() : SelectedLeisure;
			bool isSelectedCategory = categoryComboBox.SelectedItem != null;

			try {
				FieldValidator fv = new FieldValidator(titleTextBox.Text, emailTextBox.Text, phoneTextBox.Text,
					websiteTextBox.Text, descriptionRicheTextBox.Text, numberTextBox.Text,
					streetTextBox.Text, zipCodeTextBox.Text, cityTextBox.Text, isSelectedCategory);
				fv.ValidFields();
			} catch (Exception ex) {
                Notification = ex.Message;
                notificationTextBlock.Foreground = Brushes.Red;
				return;
			}

			this.MapLeisure(leisure);

			if (SelectedLeisure == null) { // If add leisure
				// Add to database
				this._Db.address.Add(leisure.address);
				this._Db.leisure.Add(leisure);
				
				// Add to ListBox
				this._Leisures.Add(leisure);
				SelectedLeisure = leisure;
			}

			this._Db.SaveChanges();
			this.UpdateSources();
            Notification = "L'enregistrement a été effectué avec succès.";
            notificationTextBlock.Foreground = Brushes.Green;
        }

		/// <summary>
		///	
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NewLeisureButtonClick(object sender, RoutedEventArgs e) {
			SelectedLeisure = null;

			this.ResetTextBoxes();
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void DeleteButtonClick(object sender, RoutedEventArgs e) {
			if (SelectedLeisure != null) {
                leisure leisureToDelete = (leisure)(from ld in this._Db.leisure
											where ld.title == SelectedLeisure.title
                                            select ld).First();

                MessageBoxResult result =
					MessageBox.Show("Etes-vous sûr de vouloir supprimer le loisir : " + leisureToDelete.title,
					"Suppression de loisir", MessageBoxButton.YesNo);
                switch (result) {
                    case MessageBoxResult.Yes:
						this._Db.address.Remove(leisureToDelete.address);
						this._Db.leisure.Remove(leisureToDelete);
						this._Db.SaveChanges();
                        Notification = "Le loisir '" + SelectedLeisure.title + "' a été supprimé avec succès.";
                        notificationTextBlock.Foreground = Brushes.Green;
                        Leisures.Remove(SelectedLeisure);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            } else {
                MessageBox.Show("Aucun loisir selectionné !");
            }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void GeneratePDFButtonClick(object sender, RoutedEventArgs e) {
            string pathDirectory;

            try {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = SelectedLeisure.title + ".pdf";
                sfd.ShowDialog();
                
                pathDirectory = sfd.FileName;

                PDFGenerator.GeneratePDF(this.SelectedLeisure, pathDirectory);
                Notification = "Génération de la plaquette '" + SelectedLeisure.title + "' effectué avec succès."  ;
                notificationTextBlock.Foreground = Brushes.Green;
            } catch (Exception ex) {
                Notification = ex.Message;
                notificationTextBlock.Foreground = Brushes.Red;
                return;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChosePictureButtonClick(object sender, RoutedEventArgs e) {
            try {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();

                leisure l = (leisure)(from lsr in this._Db.leisure
                             where lsr.id == 1
                             select lsr).FirstOrDefault();
                
                File.WriteAllBytes(@"C:\Users\Administrateur\Documents\Images\"+ l.title + ".png", l.picture);

            } catch (Exception) {
                throw;
            }

        }

	}
}
