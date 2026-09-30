using System;

namespace InventarioSucursales
{
    /// <summary>Utilidades para leer y validar datos ingresados por consola.</summary>
    public static class ConsoleInput
    {
        /// <summary>Lee un entero dentro del rango [min, max], repitiendo hasta que sea válido.</summary>
        public static int ReadIntInRange(string message, int min, int max)
        {
            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    if (value >= min && value <= max)
                        return value;

                    Console.WriteLine($"Valor fuera de rango. Ingrese un número entre {min} y {max}.");
                }
                else
                {
                    Console.WriteLine("Valor inválido. Ingrese un número entero.");
                }
            }
        }
    }
}