using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncService.Model
{
    public class Client
    {
        private string Code = "";//Obligatorio en Handy

        public string code
        {
            get { return Code; }
            set { Code = value; }
        }
        private string Description = "";//Obligatorio en Handy

        public string description
        {
            get { return Description; }
            set { Description = value; }
        }
        private double Latitude = 0.0d;

        public double latitude
        {
            get { return Latitude; }
            set { Latitude = value; }
        }
        private double Longitude = 0.0d;

        public double longitude
        {
            get { return Longitude; }
            set { Longitude = value; }
        }
        private double Accuracy = 0.0d;

        public double accuracy
        {
            get { return Accuracy; }
            set { Accuracy = value; }
        }
        private string Zone = "";//Obligatorio en Handy

        public string zone_description
        {
            get { return Zone; }
            set { Zone = value; }
        }
        private string Address = "";

        public string address
        {
            get { return Address; }
            set { Address = value; }
        }
        private string City = "";

        public string city
        {
            get { return City; }
            set { City = value; }
        }
        private string Zip = "";

        public string postalCode
        {
            get { return Zip; }
            set { Zip = value; }
        }
        private string Owner = "";

        public string owner
        {
            get { return Owner; }
            set { Owner = value; }
        }
        private string Phone = "";

        public string phoneNumber
        {
            get { return Phone; }
            set { Phone = value; }
        }
        private string Comments = "";

        public string comments
        {
            get { return Comments; }
            set { Comments = value; }
        }
        private string Email = "";

        public string email
        {
            get { return Email; }
            set { Email = value; }
        }
        private bool IsProspect = false;

        public bool is_prospect
        {
            get { return IsProspect; }
            set { IsProspect = value; }
        }
        private bool IsMobile = false;

        public bool is_mobile
        {
            get { return IsMobile; }
            set { IsMobile = value; }
        }
        private bool Enable = true;

        public bool enabled
        {
            get { return Enable; }
            set { Enable = value; }
        }
        private double Discount = 0; //Debe estar entre 0 y 99.9

        public double discount
        {
            get { return Discount; }
            set
            {
                if (value < 0)
                    Discount = 0;
                else if (value > 99.9)
                    Discount = 99.9;
                else
                    Discount = value;
            }
        }

        private bool IsNew = false;

        public bool isNew
        {
            get { return IsNew; }
            set { IsNew = value; }
        }
    }
}
