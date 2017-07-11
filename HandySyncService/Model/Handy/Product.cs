using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncService.Model
{
    public class HandyProduct
    {
        private string Code = "";//Obligatorio en Handy

        public string code
        {
            get { return Code; }
            set { Code = value; }
        }
        private string Description = "--"; //Obligatorio en Handy, valor default en caso de no se especifique

        public string description
        {
            get { return Description; }
            set
            {
                if (value == "" || value == " ")
                    Description = "";
                else
                    Description = value;
            }
        }
        private string Family = "Generales"; //Obligatorio en Handy, valor default en caso de que no se especifique

        public string product_family
        {
            get { return Family; }
            set
            {
                if (value == "" || value == " ")
                    Family = "Generales";
                else
                    Family = value;
            }

        }
        private double Price = 0; //Obligatorio en Handy

        public double price
        {
            get { return Price; }
            set { Price = value; }
        }
        private string Barcode = "";

        public string barcode
        {
            get { return Barcode; }
            set { Barcode = value; }
        }
        private bool Enabled = true;

        public bool enabled
        {
            get { return Enabled; }
            set { Enabled = value; }
        }
        private bool Apply_discount = false;

        public bool apply_discounts
        {
            get { return Apply_discount; }
            set { Apply_discount = value; }
        }
        private string Details = "";

        public string details
        {
            get { return Details; }
            set { Details = value; }
        }
        private bool IsNew = false;

        public bool isNew
        {
            get { return IsNew; }
            set { IsNew = value; }
        }
    }
}
