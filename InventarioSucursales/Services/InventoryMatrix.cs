using System.Collections.Generic;

namespace InventarioSucursales
{
    /// <summary>
    /// Inventario de libros por sucursal almacenado en un arreglo bidimensional.
    ///   - Filas    (índice 0) = libros
    ///   - Columnas (índice 1) = sucursales
    ///   - stock[fila, columna] = cantidad disponible de ese libro en esa sucursal
    ///
    /// Notación Big O: n = cantidad de libros (filas), m = cantidad de sucursales (columnas).
    /// Esta clase NO imprime nada en consola: solo maneja datos.
    /// </summary>
    public class InventoryMatrix
    {
        public const int LowStockThreshold = 3;   // Umbral de bajo inventario (<= 3)

        private readonly string[] _books;
        private readonly string[] _branches;
        private readonly int[,] _stock;           // Matriz principal: [libros, sucursales]

        public InventoryMatrix()
        {
            _books = new string[]
            {
                "Cien años de soledad",
                "El principito",
                "1984",
                "Don Quijote de la Mancha",
                "Rayuela"
            };

            _branches = new string[] { "Centro", "Norte", "Este" };

            // 5 libros (filas) x 3 sucursales (columnas)
            //                     Centro  Norte  Este
            _stock = new int[,]
            {
                /* Cien años de soledad */ { 10,  4,  7 },
                /* El principito        */ {  2,  8,  5 },
                /* 1984                 */ {  6,  3, 12 },
                /* Don Quijote          */ {  9,  1,  4 },
                /* Rayuela              */ {  0,  5,  3 }
            };
        }

        // Dimensiones obtenidas directamente de la matriz
        public int BookCount => _stock.GetLength(0);     // Número de filas
        public int BranchCount => _stock.GetLength(1);   // Número de columnas

        public string GetBookName(int book) => _books[book];
        public string GetBranchName(int branch) => _branches[branch];

        /// <summary>
        /// CONSULTAR DISPONIBILIDAD - Big O: O(1)
        /// Acceso directo por índices: no se recorre nada. La posición en memoria
        /// se calcula con una fórmula, sin importar el tamaño de la matriz.
        /// </summary>
        public int GetQuantity(int book, int branch)
        {
            return _stock[book, branch];
        }

        /// <summary>
        /// ACTUALIZAR DISPONIBILIDAD - Big O: O(1)
        /// Asignación directa a una posición de la matriz.
        /// </summary>
        public void SetQuantity(int book, int branch, int quantity)
        {
            _stock[book, branch] = quantity;
        }

        /// <summary>
        /// TOTAL DE UN LIBRO - Big O: O(m)
        /// Se fija la fila y se recorren todas las columnas (sucursales).
        /// </summary>
        public int GetBookTotal(int book)
        {
            int total = 0;
            for (int branch = 0; branch < BranchCount; branch++)
            {
                total += _stock[book, branch];
            }
            return total;
        }

        /// <summary>
        /// TOTAL DE UNA SUCURSAL - Big O: O(n)
        /// Se fija la columna y se recorren todas las filas (libros).
        /// </summary>
        public int GetBranchTotal(int branch)
        {
            int total = 0;
            for (int book = 0; book < BookCount; book++)
            {
                total += _stock[book, branch];
            }
            return total;
        }

        /// <summary>
        /// BAJO INVENTARIO - Big O: O(n * m)
        /// Recorrido completo de la matriz con dos ciclos anidados:
        /// el externo recorre las filas y el interno las columnas.
        /// </summary>
        public List<StockPosition> GetLowStockPositions()
        {
            var result = new List<StockPosition>();

            for (int book = 0; book < BookCount; book++)
            {
                for (int branch = 0; branch < BranchCount; branch++)
                {
                    if (_stock[book, branch] <= LowStockThreshold)
                    {
                        result.Add(new StockPosition(book, branch, _stock[book, branch]));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// SUCURSAL CON MAYOR DISPONIBILIDAD DE UN LIBRO - Big O: O(m)
        /// Se recorre la fila del libro comparando las cantidades de cada sucursal.
        /// Si hay empate, se devuelven todas las sucursales que comparten el máximo
        /// (dos pasadas de m elementos: O(m) + O(m) = O(m)).
        /// </summary>
        public List<int> GetBranchesWithMostStock(int book, out int maxQuantity)
        {
            maxQuantity = _stock[book, 0];

            // Pasada 1: encontrar el valor máximo
            for (int branch = 1; branch < BranchCount; branch++)
            {
                if (_stock[book, branch] > maxQuantity)
                    maxQuantity = _stock[book, branch];
            }

            // Pasada 2: recolectar las sucursales que tienen ese máximo
            var branches = new List<int>();
            for (int branch = 0; branch < BranchCount; branch++)
            {
                if (_stock[book, branch] == maxQuantity)
                    branches.Add(branch);
            }

            return branches;
        }
    }
}