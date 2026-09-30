using System;
using System.Text;

namespace InventarioSucursales
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var inventory = new InventoryMatrix();
            var menu = new InventoryMenu(inventory);
            menu.Run();
        }
    }
}