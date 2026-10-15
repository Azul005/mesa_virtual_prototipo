using System;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación de Mesa de Partes Digital.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configuración visual para alta resolución y estilos nativos de Windows Forms
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Iniciar la ventana principal del prototipo
            Application.Run(new FrmPrincipal());
        }
    }
}