using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBv3.Models
{
    public class Droplist
    {
        public enum Gender
        {
            Female,
            Male
        }
        public enum Position
        {
            Maid,
            Manager,
            Receptionist,
            Keeper,
            Owner,
            Laundry_staff
        }
        public enum YesorNo
        {
            Yes,
            No
        }
        public enum MONTH
        {
            Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec
        }
        public enum DAY
        {
            Mon, Tue, Wed, Thu, Fri, Sat, Sun
        }
        public enum ROOMTYPE
        {
            Regular,VIP
        }
    }
}