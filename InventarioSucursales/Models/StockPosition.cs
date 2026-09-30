namespace InventarioSucursales
{
    /// <summary>
    /// Representa una posición de la matriz (fila, columna) junto con su cantidad.
    /// Se usa para devolver los resultados de la búsqueda de bajo inventario.
    /// </summary>
    public class StockPosition
    {
        public int BookIndex { get; }      // Fila de la matriz
        public int BranchIndex { get; }    // Columna de la matriz
        public int Quantity { get; }       // Valor almacenado en stock[fila, columna]

        public StockPosition(int bookIndex, int branchIndex, int quantity)
        {
            BookIndex = bookIndex;
            BranchIndex = branchIndex;
            Quantity = quantity;
        }
    }
}