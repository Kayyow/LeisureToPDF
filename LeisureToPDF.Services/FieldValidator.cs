using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeisureToPDF.Services
{
	public class FieldValidator {
		private string _Title;
		private string _Email;
		private string _Phone;
		private string _Website;
		private string _Description;
		private string _AddrNumber;
		private string _AddrStreet;
		private string _AddrZipCode;
		private string _AddrCity;

		public FieldValidator (string title, string email, string phone, string website, string description, string addrNumber, string addrStreet, string addrZipCode, string addrCity) {
			this._Title = title;
			this._Email = email;
			this._Phone = phone;
			this._Website = website;
			this._Description = description;
			this._AddrNumber = addrNumber;
			this._AddrStreet = addrStreet;
			this._AddrZipCode = addrZipCode;
			this._AddrCity = addrCity;
		}

		public void ValidFields() {
			try {
				this.ValidTitle();
				this.ValidEmail();
				this.ValidPhone();
				this.ValidWebsite();
				this.ValidDescription();
				this.ValidAddrNumber();
				this.ValidAddrStreet();
				this.ValidAddrZipCode();
				this.ValidAddrCity();
			} catch (Exception e) {
				throw new Exception(e.Message);
			}
		}

		private void ValidTitle () {
			if (String.IsNullOrEmpty(this._Title.Trim())) {
				throw new Exception("Veuillez donner un titre au loisir.");
			} else {
				if (this._Title.Length > 45) {
					throw new Exception("Le titre du loisir ne doit pas dépasser 45 caractères.");
				}
			}
		}

		private void ValidEmail () {
			if (String.IsNullOrEmpty(this._Email.Trim())) {
				throw new Exception("Veuillez donner une adresse email au loisir.");
			} else {
				Regex rgx = new Regex(@"([^.@]+)(\.[^.@]+)*@([^.@]+\.)+([^.@]+)");
				if (!rgx.IsMatch(this._Email)) {
					throw new Exception("L'adresse email n'est pas valide. (Forme : adresse@domaine.ext)");
				}
			}
		}

		private void ValidPhone () {
			if (String.IsNullOrEmpty(this._Phone.Trim())) {
				throw new Exception("Veuillez donner un numéro de téléphone au loisir.");
			} else {
				Regex rgx = new Regex(@"\d{10}");
				if (!rgx.IsMatch(this._Phone)) {
					throw new Exception("Le numéro de téléphone doit comprendre 10 caractères. (Forme : 0601020304)");
				}
			}
		}

		private void ValidWebsite () {
			if (!String.IsNullOrEmpty(this._Website.Trim())) {
				if (!Uri.IsWellFormedUriString(this._Website, UriKind.Absolute)) {
					throw new Exception("L'adresse du site internet n'est pas valide. (Forme : http(s)://website.com)");
				}
			}
		}

		private void ValidDescription () {
			if (String.IsNullOrEmpty(this._Description.Trim())) {
				throw new Exception("Veuillez donner une description au loisir.");
			}
		}

		private void ValidAddrNumber () {
			if (String.IsNullOrEmpty(this._AddrNumber.Trim())) {
				throw new Exception("Veuillez préciser le numéro de l'adresse du loisir.");
			} else {
				Regex rgx = new Regex(@"^([1-9]|[1-9][0-9]|[1-9][0-9][0-9])$");
				if (!rgx.IsMatch(this._AddrNumber)) {
					throw new Exception("Veuillez entrer un numéro d'adresse valide. (Compris entre : 1 et 999)");
				}
			} 
		}

		private void ValidAddrStreet () {
			if (String.IsNullOrEmpty(this._AddrStreet.Trim())) {
				throw new Exception("Veuillez préciser la voie de l'adresse du loisir.");
			}
		}

		private void ValidAddrZipCode () {
			if (String.IsNullOrEmpty(this._AddrZipCode.Trim())) {
				throw new Exception("Veuillez préciser le code postal de l'adresse du loisir.");
			} else {
				Regex rgx = new Regex(@"\d{5}");
				if (!rgx.IsMatch(this._AddrZipCode)) {
					throw new Exception("Le code postal doit contenir 5 chiffres");
				}
			}
		}

		private void ValidAddrCity () {
			if (String.IsNullOrEmpty(this._AddrCity.Trim())) {
				throw new Exception("Veuillez préciser la ville de l'adresse du loisir.");
			}
		}
    }
}
