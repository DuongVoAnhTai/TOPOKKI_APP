using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPOKKI_APP.Models
{
    public class Menu
    {
        public string Name { get; set; } // Tên món ăn
        public int Quantity { get; set; } // Số lượng
        public decimal Price { get; set; } // Đơn giá
        public decimal Total => Quantity * Price; // Thành tiền
    }
}
