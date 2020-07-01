using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace bkfc.Models
{
    public class ReportView
    {
        public List<ReportEachVendor> reports {get;set;}
        public DateTime dayStart {get;set;}
        public DateTime dayEnd {get;set;}

        public String dayStartString {
            get{
                return dayStart.Month.ToString() +"/" + dayStart.Day.ToString() +"/" + dayStart.Year.ToString();
            }
        }
        public String dayEndString {
            get{
                return dayEnd.Month.ToString() +"/" + dayEnd.Day.ToString() +"/" + dayEnd.Year.ToString();
            }
        }
    }
    public class ReportEachVendor
    {
        public int vendorId {get;set;}
        public string vendorName {get;set;}
        public List<Food> listFood {get;set;}
        public double Income {get;set;}
    }
}