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
		private int _AddrNumber;
		private string _AddrStreet;
		private string _AddrZipCode;
		private string _AddrCity;

		public FieldValidator (string title, string email, string phone, string website, string description, int addrNumber, string addrStreet, string addrZipCode, string addrCity) {
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
				
			}
		}

		private void ValidPhone () {
			if (String.IsNullOrEmpty(this._Phone.Trim())) {
				throw new Exception("Veuillez donner un numéro de téléphone au loisir.");
			} else {
				if (this._Phone.Length > 10) {
					throw new Exception("Le numéro de téléphone ne peut pas excéder 10 caractères. (Forme : 0601020304)");
				}
			}
		}

		private void ValidWebsite () {
			if (!String.IsNullOrEmpty(this._Website.Trim())) {
				
			}
		}

		private void ValidDescription () {
			if (String.IsNullOrEmpty(this._Description.Trim())) {
				throw new Exception("Veuillez donner une description au loisir.");
			} else {

			}
		}

		private void ValidAddrNumber () {
			if (this._AddrNumber > 999) {
				throw new Exception("Veuillez ne pas entrer un numéro de rue trop élevé.");
			}
		}

		private void ValidAddrStreet () {
			if (String.IsNullOrEmpty(this._AddrStreet.Trim())) {
				throw new Exception("Veuillez préciser la voie de l'adresse du loisir.");
			} else {

			}
		}

		private void ValidAddrZipCode () {
			if (String.IsNullOrEmpty(this._AddrZipCode.Trim())) {
				throw new Exception("Veuillez préciser le code postal de l'adresse du loisir.");
			} else {
				if (this._AddrZipCode.Length != 5) {
					throw new Exception("Le code postal doit contenir 5 chiffres");
				}
			}
		}

		private void ValidAddrCity () {
			if (String.IsNullOrEmpty(this._AddrCity.Trim())) {
				throw new Exception("Veuillez préciser la ville de l'adresse du loisir.");
			} else {

			}
		}
    }
}
