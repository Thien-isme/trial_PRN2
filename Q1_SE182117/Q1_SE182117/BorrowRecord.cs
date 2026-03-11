using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q1_SE182117
{
    public class BorrowRecord
    {
        public string BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }
    }
}
