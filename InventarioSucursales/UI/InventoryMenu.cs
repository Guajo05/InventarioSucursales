using System;
using System.Collections.Generic;

namespace InventarioSucursales
{
    /// <summary>Menú de consola que conecta al usuario con la matriz de inventario.</summary>
    public class InventoryMenu
    {
        private readonly InventoryMatrix _inventory;

        public InventoryMenu(InventoryMatrix inventory)
        {
            _inventory = inventory;
        }

        public void Run()
        {
            int option;
            do
            {
                ShowMenu();
                option = ConsoleInput.ReadIntInRange("Seleccione una opción: ", 0, 7);

                switch (option)
                {
                    case 1: ShowFullInventory(); break;
                    case 2: CheckAvailability(); break;
                    case 3: UpdateAvailability(); break;
                    case 4: ShowBookTotal(); break;
                    case 5: ShowBranchTotal(); break;
                    case 6: ShowLowStock(); break;
                    case 7: ShowBestBranch(); break;
                    case 0: Console.WriteLine("\nHasta luego."); break;
                }

                if (option != 0)
                {
                    Console.WriteLine("\nPresione una tecla para continuar...");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            } while (option != 0);
        }

        private void ShowMenu()
        {
            Console.WriteLine("====== INVENTARIO POR SUCURSAL ======");
            Console.WriteLine();
            Console.WriteLine("1. Mostrar inventario completo");
            Console.WriteLine("2. Consultar disponibilidad");
            Console.WriteLine("3. Actualizar disponibilidad");
            Console.WriteLine("4. Total disponible de un libro");
            Console.WriteLine("5. Total de inventario por sucursal");
            Console.WriteLine("6. Mostrar libros con bajo inventario");
            Console.WriteLine("7. Sucursal con mayor disponibilidad");
            Console.WriteLine("0. Salir");
            Console.WriteLine();
        }

        // ---------- Selección de libro / sucursal ----------
        // El usuario trabaja con números desde 1; la matriz usa índices desde 0.

        private int SelectBook()
        {
            Console.WriteLine("\nLibros:");
            for (int i = 0; i < _inventory.BookCount; i++)
            {
                Console.WriteLine($"  {i + 1}. {_inventory.GetBookName(i)}");
            }

            int number = ConsoleInput.ReadIntInRange($"Número del libro (1-{_inventory.BookCount}): ", 1, _inventory.BookCount);
            return number - 1;
        }

        private int SelectBranch()
        {
            Console.WriteLine("\nSucursales:");
            for (int j = 0; j < _inventory.BranchCount; j++)
            {
                Console.WriteLine($"  {j + 1}. Sucursal {_inventory.GetBranchName(j)}");
            }

            int number = ConsoleInput.ReadIntInRange($"Número de la sucursal (1-{_inventory.BranchCount}): ", 1, _inventory.BranchCount);
            return number - 1;
        }

        // ---------- Opciones ----------

        /// <summary>
        /// 1. MOSTRAR MATRIZ COMPLETA - Big O: O(n * m)
        /// Dos ciclos anidados: filas (libros) y columnas (sucursales).
        /// </summary>
        private void ShowFullInventory()
        {
            Console.WriteLine("\n--- Inventario completo (filas = libros, columnas = sucursales) ---\n");

            // Encabezado con las sucursales
            Console.Write($"{"N°",-3} {"Libro",-28}");
            for (int j = 0; j < _inventory.BranchCount; j++)
            {
                Console.Write($"{_inventory.GetBranchName(j),10}");
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', 3 + 1 + 28 + 10 * _inventory.BranchCount));

            // Recorrido de la matriz
            for (int i = 0; i < _inventory.BookCount; i++)
            {
                Console.Write($"{i + 1,-3} {_inventory.GetBookName(i),-28}");

                for (int j = 0; j < _inventory.BranchCount; j++)
                {
                    Console.Write($"{_inventory.GetQuantity(i, j),10}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>2. CONSULTAR DISPONIBILIDAD - Big O: O(1) (acceso directo stock[fila, columna]).</summary>
        private void CheckAvailability()
        {
            Console.WriteLine("\n--- Consultar disponibilidad ---");
            int book = SelectBook();
            int branch = SelectBranch();

            int quantity = _inventory.GetQuantity(book, branch);
            Console.WriteLine($"\n\"{_inventory.GetBookName(book)}\" en Sucursal {_inventory.GetBranchName(branch)}: {quantity} unidades disponibles.");
        }

        /// <summary>3. ACTUALIZAR DISPONIBILIDAD - Big O: O(1) (asignación directa).</summary>
        private void UpdateAvailability()
        {
            Console.WriteLine("\n--- Actualizar disponibilidad ---");
            int book = SelectBook();
            int branch = SelectBranch();

            int current = _inventory.GetQuantity(book, branch);
            Console.WriteLine($"\nCantidad actual de \"{_inventory.GetBookName(book)}\" en Sucursal {_inventory.GetBranchName(branch)}: {current}");

            int newQuantity = ConsoleInput.ReadIntInRange("Nueva cantidad (0 o más): ", 0, int.MaxValue);
            _inventory.SetQuantity(book, branch, newQuantity);

            Console.WriteLine($"\nDisponibilidad actualizada: {current} -> {newQuantity}.");
        }

        /// <summary>4. TOTAL DE UN LIBRO - Big O: O(m).</summary>
        private void ShowBookTotal()
        {
            Console.WriteLine("\n--- Total disponible de un libro ---");
            int book = SelectBook();

            int total = _inventory.GetBookTotal(book);
            Console.WriteLine($"\n\"{_inventory.GetBookName(book)}\": {total} unidades en total ({_inventory.BranchCount} sucursales).");
        }

        /// <summary>5. INVENTARIO DE UNA SUCURSAL - Big O: O(n).</summary>
        private void ShowBranchTotal()
        {
            Console.WriteLine("\n--- Total de inventario por sucursal ---");
            int branch = SelectBranch();

            int total = _inventory.GetBranchTotal(branch);
            Console.WriteLine($"\nSucursal {_inventory.GetBranchName(branch)}: {total} unidades en total ({_inventory.BookCount} libros).");
        }

        /// <summary>6. BAJO INVENTARIO (cantidad &lt;= 3) - Big O: O(n * m).</summary>
        private void ShowLowStock()
        {
            Console.WriteLine($"\n--- Libros con bajo inventario (cantidad <= {InventoryMatrix.LowStockThreshold}) ---\n");

            List<StockPosition> lowStock = _inventory.GetLowStockPositions();

            if (lowStock.Count == 0)
            {
                Console.WriteLine("No hay libros con bajo inventario.");
                return;
            }

            Console.WriteLine($"{"Posición",-10} {"Libro",-28} {"Sucursal",-10} {"Cantidad",8}");
            Console.WriteLine(new string('-', 59));

            foreach (StockPosition item in lowStock)
            {
                string position = $"[{item.BookIndex},{item.BranchIndex}]";
                Console.WriteLine($"{position,-10} {_inventory.GetBookName(item.BookIndex),-28} {_inventory.GetBranchName(item.BranchIndex),-10} {item.Quantity,8}");
            }

            Console.WriteLine($"\nTotal de posiciones con bajo inventario: {lowStock.Count}");
        }

        /// <summary>7. SUCURSAL CON MAYOR DISPONIBILIDAD - Big O: O(m).</summary>
        private void ShowBestBranch()
        {
            Console.WriteLine("\n--- Sucursal con mayor disponibilidad de un libro ---");
            int book = SelectBook();

            List<int> branches = _inventory.GetBranchesWithMostStock(book, out int max);

            Console.WriteLine($"\n\"{_inventory.GetBookName(book)}\":");

            if (branches.Count == 1)
            {
                Console.WriteLine($"La sucursal con mayor disponibilidad es Sucursal {_inventory.GetBranchName(branches[0])} con {max} unidades.");
            }
            else
            {
                Console.WriteLine($"Hay un empate con {max} unidades en:");
                foreach (int branch in branches)
                {
                    Console.WriteLine($"  - Sucursal {_inventory.GetBranchName(branch)}");
                }
            }
        }
    }
}