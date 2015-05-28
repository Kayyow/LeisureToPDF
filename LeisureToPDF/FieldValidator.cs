using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeisureToPDF.DAL;

namespace LeisureToPDF
{
    public static class FieldValidator {
		public static bool CheckLeisure(lavalloisirEntities db, leisure leisure) {
            bool leisureChecked = true;

            string titleField = leisure.title;
            string descriptionField = leisure.description;
            string emailField = leisure.email;
            string phoneField = leisure.phone;
            string websiteField = leisure.website;
            address addressField = leisure.address;
            category categoryField = leisure.category;

            if (titleField.Length > 255 || titleField.Length ==0) {
                leisureChecked = false;
                return leisureChecked;
            }

            if (descriptionField == null) {
                leisureChecked = false;
                return leisureChecked;
            }

            if (emailField.Length > 255 || emailField.Length == 0) {
                leisureChecked = false;
                return leisureChecked;
            }

            if (phoneField.Length > 10 || phoneField.Length == 0) {
                leisureChecked = false;
                return leisureChecked;
            }

            if (websiteField.Length > 255) {
                leisureChecked = false;
                return leisureChecked;
            }

            return leisureChecked;

        }

        public static bool CheckAddress(lavalloisirEntities db, address address) {
            bool addressChecked = true;

            int numberField = address.number;
            string streetField = address.street;
            string zipCodeField = address.zip_code;
            string cityField = address.city;

            if (numberField > 9999) {
                addressChecked = false;
                return addressChecked;
            }

            if (streetField.Length > 255 || streetField.Length == 0) {
                addressChecked = false;
                return addressChecked;
                
            }

            if (cityField.Length > 5 || cityField.Length == 0) {
                addressChecked = false;
                return addressChecked;
            }

            if (cityField.Length > 255 || cityField.Length == 0 ) {
                addressChecked = false;
                return addressChecked;
            }

            return addressChecked;
        }

    }
}
