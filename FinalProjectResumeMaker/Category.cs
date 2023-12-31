﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectResumeMaker
{
    class Category
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }


        public override string ToString()
        {
            string formatted = String.Format("{0}\t {1}\t  {2}\t  {3}\t {4}\t {5}\t  {6}\t  {7}\t {8}", Id, FirstName, LastName, Age, City, Address, Phone, Email);
            return formatted;
        }
    }
}
