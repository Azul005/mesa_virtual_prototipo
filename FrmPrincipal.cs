using System;
using System.Drawing;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    /// <summary>
    /// =========================================================================================
    /// FORMULARIO PRINCIPAL: FrmPrincipal
    /// Interfaz gráfica construida 100% por código limpio (sin dependencia de diseñador visual).
    /// Dividida exactamente en 2 apartados mediante un TabControl principal:
    ///   - Apartado 1: "1. Registro de Expedientes" y Matriz 2D de Estadísticas en tiempo real.
    ///   - Apartado 2: "2. Búsqueda y Listado" con algoritmos Secuencial O(n), Binario O(log n) y Bubble Sort.
    /// =========================================================================================
    /// </summary>
    public class FrmPrincipal : Form
    {
        // --------------------------------------------------------------------------------------
        // CONTROLES DE LA CABECERA Y ESTRUCTURA GENERAL
        // --------------------------------------------------------------------------------------
        private Panel pnlCabecera = null!;
        private Label lblTitulo = null!;
        private Label lblSubtitulo = null!;
        private Label lblBadgeTotal = null!;
        private TabControl tabControlPrincipal = null!;

        // --------------------------------------------------------------------------------------
        // CONTROLES - APARTADO 1: REGISTRO DE EXPEDIENTES
        // --------------------------------------------------------------------------------------
        private TabPage tabRegistro = null!;
        private GroupBox grpFormulario = null!;
        private TextBox txtCodigoAuto = null!;
        private TextBox txtFechaAuto = null!;
        private TextBox txtDni = null!;
        private TextBox txtCiudadano = null!;
        private TextBox txtAsunto = null!;
        private ComboBox cboArea = null!;
        private ComboBox cboEstado = null!;
        private Button btnRegistrar = null!;
        private Button btnLimpiar = null!;
        private Button btnCargarPrueba = null!;

        // Matriz 2D en tiempo real
        private GroupBox grpEstadisticas2D = null!;
        private DataGridView dgvMatrizEstadisticas = null!;
        private Label lblExplicacionMatriz = null!;

        // --------------------------------------------------------------------------------------
        // CONTROLES - APARTADO 2: BÚSQUEDA Y LISTADO
        // --------------------------------------------------------------------------------------
        private TabPage tabListado = null!;
        private GroupBox grpBarraBusqueda = null!;
        private TextBox txtBuscar = null!;
        private Button btnBuscarSecuencial = null!;
        private Button btnBuscarBinaria = null!;
        private Button btnOrdenarAZ = null!;
        private Button btnOrdenarZA = null!;
        private Button btnRefrescar = null!;

        // Panel de diagnóstico algorítmico
        private Panel pnlDiagnostico = null!;
        private Label lblDiagnosticoTitulo = null!;
        private Label lblDiagnosticoDetalle = null!;

        // Grilla principal de expedientes (cargada manualmente sin LINQ/List)
        private DataGridView dgvExpedientes = null!;
        private Label lblEstadoListado = null!;

        // --------------------------------------------------------------------------------------
        // CONSTRUCTOR DEL FORMULARIO
        // --------------------------------------------------------------------------------------
        public FrmPrincipal()
        {
            InicializarComponentesManual();
        }

        /// <summary>
        /// Configura y ensambla todos los controles de la interfaz por código.
        /// </summary>
        private void InicializarComponentesManual()
        {
            // 1. Configuración de la Ventana Principal
            this.Text = "Sistema de Mesa de Partes Digital - Ley N.º 31170 (Prototipo Académico)";
            this.Size = new Size(1160, 760);
            this.MinimumSize = new Size(1000, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // 2. Banner de Cabecera (Diseño Institucional)
            ConstruirCabecera();

            // 3. TabControl Principal (Exactamente 2 Apartados)
            tabControlPrincipal = new TabControl();
            tabControlPrincipal.Dock = DockStyle.Fill;
            tabControlPrincipal.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            tabControlPrincipal.Padding = new Point(20, 8);

            // Apartado 1: Registro y Matriz 2D
            tabRegistro = new TabPage("  1. Registro de Expedientes  ");
            tabRegistro.BackColor = Color.FromArgb(248, 249, 251);
            ConstruirApartadoRegistro();

            // Apartado 2: Búsqueda y Listado
            tabListado = new TabPage("  2. Búsqueda y Listado  ");
            tabListado.BackColor = Color.FromArgb(248, 249, 251);
            ConstruirApartadoListado();

            tabControlPrincipal.TabPages.Add(tabRegistro);
            tabControlPrincipal.TabPages.Add(tabListado);

            // Agregar a la ventana
            this.Controls.Add(tabControlPrincipal);
            this.Controls.Add(pnlCabecera);

            // Evento Load del formulario
            this.Load += FrmPrincipal_Load;
        }

        // --------------------------------------------------------------------------------------
        // CONSTRUCCIÓN DEL BANNER SUPERIOR
        // --------------------------------------------------------------------------------------
        private void ConstruirCabecera()
        {
            pnlCabecera = new Panel();
            pnlCabecera.Dock = DockStyle.Top;
            pnlCabecera.Height = 72;
            pnlCabecera.BackColor = Color.FromArgb(18, 38, 68); // Azul institucional oscuro
            pnlCabecera.Padding = new Padding(20, 10, 20, 10);

            lblTitulo = new Label();
            lblTitulo.Text = "MESA DE PARTES DIGITAL — LEY N.° 31170";
            lblTitulo.Font = new Font("Segoe UI", 13.5f, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 10);
            lblTitulo.AutoSize = true;

            lblSubtitulo = new Label();
            lblSubtitulo.Text = "Curso: Fundamentos de Programación | Arquitectura Modular y Arreglos Paralelos (Sin POO)";
            lblSubtitulo.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            lblSubtitulo.ForeColor = Color.FromArgb(180, 205, 235);
            lblSubtitulo.Location = new Point(22, 40);
            lblSubtitulo.AutoSize = true;

            lblBadgeTotal = new Label();
            lblBadgeTotal.Text = "Expedientes: 0";
            lblBadgeTotal.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblBadgeTotal.ForeColor = Color.FromArgb(20, 38, 68);
            lblBadgeTotal.BackColor = Color.FromArgb(220, 235, 252);
            lblBadgeTotal.TextAlign = ContentAlignment.MiddleCenter;
            lblBadgeTotal.Size = new Size(170, 34);
            lblBadgeTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblBadgeTotal.Location = new Point(this.ClientSize.Width - 195, 18);

            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Controls.Add(lblSubtitulo);
            pnlCabecera.Controls.Add(lblBadgeTotal);
        }

        // --------------------------------------------------------------------------------------
        // APARTADO 1: CONSTRUCCIÓN DEL FORMULARIO Y MATRIZ 2D
        // --------------------------------------------------------------------------------------
        private void ConstruirApartadoRegistro()
        {
            // Contenedor dividido en 2 columnas: Izquierda (Formulario) y Derecha (Matriz 2D)
            TableLayoutPanel tlpContenedor = new TableLayoutPanel();
            tlpContenedor.Dock = DockStyle.Fill;
            tlpContenedor.ColumnCount = 2;
            tlpContenedor.RowCount = 1;
            tlpContenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tlpContenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tlpContenedor.Padding = new Padding(15);

            // ----------------------------------------------------------------------------------
            // COLUMNA IZQUIERDA: FORMULARIO DE REGISTRO
            // ----------------------------------------------------------------------------------
            grpFormulario = new GroupBox();
            grpFormulario.Text = " Datos del Trámite a Ingresar ";
            grpFormulario.Dock = DockStyle.Fill;
            grpFormulario.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            grpFormulario.ForeColor = Color.FromArgb(20, 38, 68);
            grpFormulario.Padding = new Padding(15, 10, 15, 15);

            Panel pnlCampos = new Panel();
            pnlCampos.Dock = DockStyle.Fill;
            pnlCampos.AutoScroll = true;
            pnlCampos.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            int y = 10;
            int espaciado = 54;

            // Fila 1: Código (Autogenerado) y Fecha (Automática)
            Label lblCod = CrearLabelCampo("Código Autogenerado:", 15, y);
            txtCodigoAuto = new TextBox();
            txtCodigoAuto.Location = new Point(15, y + 20);
            txtCodigoAuto.Size = new Size(220, 26);
            txtCodigoAuto.ReadOnly = true;
            txtCodigoAuto.BackColor = Color.FromArgb(235, 240, 248);
            txtCodigoAuto.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            txtCodigoAuto.ForeColor = Color.FromArgb(14, 116, 144);

            Label lblFec = CrearLabelCampo("Fecha de Registro:", 255, y);
            txtFechaAuto = new TextBox();
            txtFechaAuto.Location = new Point(255, y + 20);
            txtFechaAuto.Size = new Size(220, 26);
            txtFechaAuto.ReadOnly = true;
            txtFechaAuto.BackColor = Color.FromArgb(235, 240, 248);
            txtFechaAuto.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            txtFechaAuto.Text = DateTime.Now.ToString("dd/MM/yyyy");

            pnlCampos.Controls.Add(lblCod);
            pnlCampos.Controls.Add(txtCodigoAuto);
            pnlCampos.Controls.Add(lblFec);
            pnlCampos.Controls.Add(txtFechaAuto);

            // Fila 2: DNI del Ciudadano (8 dígitos numéricos)
            y += espaciado;
            Label lblDni = CrearLabelCampo("DNI del Ciudadano (8 dígitos numéricos): *", 15, y);
            txtDni = new TextBox();
            txtDni.Location = new Point(15, y + 20);
            txtDni.Size = new Size(460, 26);
            txtDni.MaxLength = 8;
            txtDni.KeyPress += TxtDni_KeyPress; // Restringir a solo dígitos en teclado
            pnlCampos.Controls.Add(lblDni);
            pnlCampos.Controls.Add(txtDni);

            // Fila 3: Nombre del Ciudadano
            y += espaciado;
            Label lblCiud = CrearLabelCampo("Apellidos y Nombres Completos: *", 15, y);
            txtCiudadano = new TextBox();
            txtCiudadano.Location = new Point(15, y + 20);
            txtCiudadano.Size = new Size(460, 26);
            txtCiudadano.CharacterCasing = CharacterCasing.Upper;
            pnlCampos.Controls.Add(lblCiud);
            pnlCampos.Controls.Add(txtCiudadano);

            // Fila 4: Asunto del Trámite
            y += espaciado;
            Label lblAsu = CrearLabelCampo("Asunto / Petitorio del Trámite: *", 15, y);
            txtAsunto = new TextBox();
            txtAsunto.Location = new Point(15, y + 20);
            txtAsunto.Size = new Size(460, 52);
            txtAsunto.Multiline = true;
            txtAsunto.ScrollBars = ScrollBars.Vertical;
            pnlCampos.Controls.Add(lblAsu);
            pnlCampos.Controls.Add(txtAsunto);

            // Fila 5: Área de Destino y Estado Inicial
            y += espaciado + 32;
            Label lblArea = CrearLabelCampo("Área de Destino: *", 15, y);
            cboArea = new ComboBox();
            cboArea.Location = new Point(15, y + 20);
            cboArea.Size = new Size(220, 26);
            cboArea.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 0; i < ModuloMesaPartes.AREAS_VALIDAS.Length; i++)
            {
                cboArea.Items.Add(ModuloMesaPartes.AREAS_VALIDAS[i]);
            }
            if (cboArea.Items.Count > 0) cboArea.SelectedIndex = 0;

            Label lblEst = CrearLabelCampo("Estado Inicial: *", 255, y);
            cboEstado = new ComboBox();
            cboEstado.Location = new Point(255, y + 20);
            cboEstado.Size = new Size(220, 26);
            cboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            for (int i = 0; i < ModuloMesaPartes.ESTADOS_VALIDOS.Length; i++)
            {
                cboEstado.Items.Add(ModuloMesaPartes.ESTADOS_VALIDOS[i]);
            }
            if (cboEstado.Items.Count > 0) cboEstado.SelectedIndex = 0;

            pnlCampos.Controls.Add(lblArea);
            pnlCampos.Controls.Add(cboArea);
            pnlCampos.Controls.Add(lblEst);
            pnlCampos.Controls.Add(cboEstado);

            // Fila 6: Botones de Acción
            y += espaciado + 10;
            btnRegistrar = new Button();
            btnRegistrar.Text = "✔ Registrar Expediente";
            btnRegistrar.Location = new Point(15, y);
            btnRegistrar.Size = new Size(230, 38);
            btnRegistrar.BackColor = Color.FromArgb(16, 120, 70); // Verde institucional
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.Cursor = Cursors.Hand;
            btnRegistrar.Click += BtnRegistrar_Click;

            btnLimpiar = new Button();
            btnLimpiar.Text = "🧹 Limpiar Campos";
            btnLimpiar.Location = new Point(255, y);
            btnLimpiar.Size = new Size(220, 38);
            btnLimpiar.BackColor = Color.FromArgb(226, 232, 240);
            btnLimpiar.ForeColor = Color.FromArgb(30, 41, 59);
            btnLimpiar.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Cursor = Cursors.Hand;
            btnLimpiar.Click += BtnLimpiar_Click;

            y += 48;
            btnCargarPrueba = new Button();
            btnCargarPrueba.Text = "📂 Cargar 8 Datos de Prueba Reales (Demostración Rúbrica)";
            btnCargarPrueba.Location = new Point(15, y);
            btnCargarPrueba.Size = new Size(460, 36);
            btnCargarPrueba.BackColor = Color.FromArgb(14, 116, 144); // Azul cian oscuro
            btnCargarPrueba.ForeColor = Color.White;
            btnCargarPrueba.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnCargarPrueba.FlatStyle = FlatStyle.Flat;
            btnCargarPrueba.Cursor = Cursors.Hand;
            btnCargarPrueba.Click += BtnCargarPrueba_Click;

            pnlCampos.Controls.Add(btnRegistrar);
            pnlCampos.Controls.Add(btnLimpiar);
            pnlCampos.Controls.Add(btnCargarPrueba);

            grpFormulario.Controls.Add(pnlCampos);

            // ----------------------------------------------------------------------------------
            // COLUMNA DERECHA: MATRIZ BIDIMENSIONAL (2D) DE ESTADÍSTICAS
            // ----------------------------------------------------------------------------------
            grpEstadisticas2D = new GroupBox();
            grpEstadisticas2D.Text = " Matriz Bidimensional 2D: Trámites por Área y Estado [4 x 3] ";
            grpEstadisticas2D.Dock = DockStyle.Fill;
            grpEstadisticas2D.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            grpEstadisticas2D.ForeColor = Color.FromArgb(20, 38, 68);
            grpEstadisticas2D.Padding = new Padding(15, 10, 15, 15);

            Panel pnlMatrizInterno = new Panel();
            pnlMatrizInterno.Dock = DockStyle.Fill;

            lblExplicacionMatriz = new Label();
            lblExplicacionMatriz.Text = "Matriz int[4, 3] calculada dinámicamente mediante bucles 'for' anidados.\r\n" +
                                       "Filas = Dependencias (Áreas) | Columnas = Estados del Trámite.";
            lblExplicacionMatriz.Dock = DockStyle.Top;
            lblExplicacionMatriz.Height = 44;
            lblExplicacionMatriz.Font = new Font("Segoe UI", 8.5f, FontStyle.Italic);
            lblExplicacionMatriz.ForeColor = Color.FromArgb(71, 85, 105);

            dgvMatrizEstadisticas = new DataGridView();
            dgvMatrizEstadisticas.Dock = DockStyle.Fill;
            dgvMatrizEstadisticas.AllowUserToAddRows = false;
            dgvMatrizEstadisticas.AllowUserToDeleteRows = false;
            dgvMatrizEstadisticas.ReadOnly = true;
            dgvMatrizEstadisticas.RowHeadersVisible = false;
            dgvMatrizEstadisticas.BackgroundColor = Color.White;
            dgvMatrizEstadisticas.BorderStyle = BorderStyle.Fixed3D;
            dgvMatrizEstadisticas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMatrizEstadisticas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMatrizEstadisticas.MultiSelect = false;
            dgvMatrizEstadisticas.Font = new Font("Segoe UI", 9f, FontStyle.Regular);

            // Configurar columnas de la matriz 2D
            dgvMatrizEstadisticas.Columns.Add("colArea", "Área / Dependencia");
            dgvMatrizEstadisticas.Columns.Add("colRegistrado", "Registrado");
            dgvMatrizEstadisticas.Columns.Add("colTramite", "En Trámite");
            dgvMatrizEstadisticas.Columns.Add("colObservado", "Observado");
            dgvMatrizEstadisticas.Columns.Add("colTotal", "TOTAL ÁREA");

            // Formatear cabeceras
            dgvMatrizEstadisticas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 250);
            dgvMatrizEstadisticas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgvMatrizEstadisticas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvMatrizEstadisticas.EnableHeadersVisualStyles = false;

            pnlMatrizInterno.Controls.Add(dgvMatrizEstadisticas);
            pnlMatrizInterno.Controls.Add(lblExplicacionMatriz);
            grpEstadisticas2D.Controls.Add(pnlMatrizInterno);

            // Añadir al contenedor dividido
            tlpContenedor.Controls.Add(grpFormulario, 0, 0);
            tlpContenedor.Controls.Add(grpEstadisticas2D, 1, 0);
            tabRegistro.Controls.Add(tlpContenedor);
        }

        // --------------------------------------------------------------------------------------
        // APARTADO 2: CONSTRUCCIÓN DE BÚSQUEDA, LISTADO Y ORDENAMIENTO
        // --------------------------------------------------------------------------------------
        private void ConstruirApartadoListado()
        {
            Panel pnlListadoPrincipal = new Panel();
            pnlListadoPrincipal.Dock = DockStyle.Fill;
            pnlListadoPrincipal.Padding = new Padding(15);

            // 1. Barra Superior de Búsqueda y Ordenamiento
            grpBarraBusqueda = new GroupBox();
            grpBarraBusqueda.Text = " Algoritmos de Búsqueda y Ordenamiento ";
            grpBarraBusqueda.Dock = DockStyle.Top;
            grpBarraBusqueda.Height = 90;
            grpBarraBusqueda.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            grpBarraBusqueda.ForeColor = Color.FromArgb(20, 38, 68);

            Label lblBusquedaPrompt = new Label();
            lblBusquedaPrompt.Text = "Buscar por Código o DNI:";
            lblBusquedaPrompt.Location = new Point(15, 23);
            lblBusquedaPrompt.AutoSize = true;
            lblBusquedaPrompt.Font = new Font("Segoe UI", 9f, FontStyle.Regular);

            txtBuscar = new TextBox();
            txtBuscar.Location = new Point(15, 45);
            txtBuscar.Size = new Size(180, 25);
            txtBuscar.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            txtBuscar.PlaceholderText = "EXP-0001 ó 71234567";

            btnBuscarSecuencial = new Button();
            btnBuscarSecuencial.Text = "🔍 Búsqueda Secuencial O(n)";
            btnBuscarSecuencial.Location = new Point(205, 43);
            btnBuscarSecuencial.Size = new Size(195, 30);
            btnBuscarSecuencial.BackColor = Color.FromArgb(30, 58, 138); // Azul noche
            btnBuscarSecuencial.ForeColor = Color.White;
            btnBuscarSecuencial.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            btnBuscarSecuencial.FlatStyle = FlatStyle.Flat;
            btnBuscarSecuencial.Cursor = Cursors.Hand;
            btnBuscarSecuencial.Click += BtnBuscarSecuencial_Click;

            btnBuscarBinaria = new Button();
            btnBuscarBinaria.Text = "⚡ Búsqueda Binaria O(log n)";
            btnBuscarBinaria.Location = new Point(410, 43);
            btnBuscarBinaria.Size = new Size(195, 30);
            btnBuscarBinaria.BackColor = Color.FromArgb(14, 116, 144); // Cian petróleo
            btnBuscarBinaria.ForeColor = Color.White;
            btnBuscarBinaria.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            btnBuscarBinaria.FlatStyle = FlatStyle.Flat;
            btnBuscarBinaria.Cursor = Cursors.Hand;
            btnBuscarBinaria.Click += BtnBuscarBinaria_Click;

            btnOrdenarAZ = new Button();
            btnOrdenarAZ.Text = "▲ Ordenar A-Z (Código)";
            btnOrdenarAZ.Location = new Point(625, 43);
            btnOrdenarAZ.Size = new Size(155, 30);
            btnOrdenarAZ.BackColor = Color.FromArgb(241, 245, 249);
            btnOrdenarAZ.ForeColor = Color.FromArgb(15, 23, 42);
            btnOrdenarAZ.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            btnOrdenarAZ.FlatStyle = FlatStyle.Flat;
            btnOrdenarAZ.Cursor = Cursors.Hand;
            btnOrdenarAZ.Click += BtnOrdenarAZ_Click;

            btnOrdenarZA = new Button();
            btnOrdenarZA.Text = "▼ Ordenar Z-A (Código)";
            btnOrdenarZA.Location = new Point(788, 43);
            btnOrdenarZA.Size = new Size(155, 30);
            btnOrdenarZA.BackColor = Color.FromArgb(241, 245, 249);
            btnOrdenarZA.ForeColor = Color.FromArgb(15, 23, 42);
            btnOrdenarZA.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            btnOrdenarZA.FlatStyle = FlatStyle.Flat;
            btnOrdenarZA.Cursor = Cursors.Hand;
            btnOrdenarZA.Click += BtnOrdenarZA_Click;

            btnRefrescar = new Button();
            btnRefrescar.Text = "🔄 Refrescar";
            btnRefrescar.Location = new Point(950, 43);
            btnRefrescar.Size = new Size(110, 30);
            btnRefrescar.BackColor = Color.FromArgb(226, 232, 240);
            btnRefrescar.ForeColor = Color.FromArgb(15, 23, 42);
            btnRefrescar.Font = new Font("Segoe UI", 8.5f, FontStyle.Regular);
            btnRefrescar.FlatStyle = FlatStyle.Flat;
            btnRefrescar.Cursor = Cursors.Hand;
            btnRefrescar.Click += (s, e) => RefrescarTablaExpedientes();

            grpBarraBusqueda.Controls.Add(lblBusquedaPrompt);
            grpBarraBusqueda.Controls.Add(txtBuscar);
            grpBarraBusqueda.Controls.Add(btnBuscarSecuencial);
            grpBarraBusqueda.Controls.Add(btnBuscarBinaria);
            grpBarraBusqueda.Controls.Add(btnOrdenarAZ);
            grpBarraBusqueda.Controls.Add(btnOrdenarZA);
            grpBarraBusqueda.Controls.Add(btnRefrescar);

            // 2. Panel de Diagnóstico de Búsqueda (Complejidad y Pasos)
            pnlDiagnostico = new Panel();
            pnlDiagnostico.Dock = DockStyle.Top;
            pnlDiagnostico.Height = 56;
            pnlDiagnostico.BackColor = Color.FromArgb(238, 242, 255);
            pnlDiagnostico.BorderStyle = BorderStyle.FixedSingle;
            pnlDiagnostico.Padding = new Padding(12, 6, 12, 6);

            lblDiagnosticoTitulo = new Label();
            lblDiagnosticoTitulo.Text = "ℹ Panel de Sustentación de Algoritmos:";
            lblDiagnosticoTitulo.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            lblDiagnosticoTitulo.ForeColor = Color.FromArgb(49, 46, 129);
            lblDiagnosticoTitulo.Dock = DockStyle.Top;
            lblDiagnosticoTitulo.Height = 18;

            lblDiagnosticoDetalle = new Label();
            lblDiagnosticoDetalle.Text = "Ingrese un Código (ej. EXP-0001) o DNI y presione Búsqueda Secuencial u Búsqueda Binaria para comparar iteraciones.";
            lblDiagnosticoDetalle.Font = new Font("Segoe UI", 8.5f, FontStyle.Regular);
            lblDiagnosticoDetalle.ForeColor = Color.FromArgb(30, 41, 59);
            lblDiagnosticoDetalle.Dock = DockStyle.Fill;

            pnlDiagnostico.Controls.Add(lblDiagnosticoDetalle);
            pnlDiagnostico.Controls.Add(lblDiagnosticoTitulo);

            // 3. DataGridView Amplio de Expedientes
            dgvExpedientes = new DataGridView();
            dgvExpedientes.Dock = DockStyle.Fill;
            dgvExpedientes.AllowUserToAddRows = false;
            dgvExpedientes.AllowUserToDeleteRows = false;
            dgvExpedientes.ReadOnly = true;
            dgvExpedientes.RowHeadersVisible = false;
            dgvExpedientes.BackgroundColor = Color.White;
            dgvExpedientes.BorderStyle = BorderStyle.Fixed3D;
            dgvExpedientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvExpedientes.MultiSelect = false;
            dgvExpedientes.Font = new Font("Segoe UI", 9f, FontStyle.Regular);

            // Definición de Columnas
            dgvExpedientes.Columns.Add("colIndice", "N°");
            dgvExpedientes.Columns.Add("colCod", "Código");
            dgvExpedientes.Columns.Add("colDniExp", "DNI");
            dgvExpedientes.Columns.Add("colCiudExp", "Ciudadano / Solicitante");
            dgvExpedientes.Columns.Add("colAsuntoExp", "Asunto del Trámite");
            dgvExpedientes.Columns.Add("colAreaExp", "Área de Destino");
            dgvExpedientes.Columns.Add("colEstadoExp", "Estado Actual");
            dgvExpedientes.Columns.Add("colFechaExp", "Fecha");

            // Anchos y comportamientos de columnas
            dgvExpedientes.Columns[0].Width = 45;
            dgvExpedientes.Columns[1].Width = 100;
            dgvExpedientes.Columns[2].Width = 90;
            dgvExpedientes.Columns[3].Width = 220;
            dgvExpedientes.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvExpedientes.Columns[5].Width = 110;
            dgvExpedientes.Columns[6].Width = 115;
            dgvExpedientes.Columns[7].Width = 95;

            // Formato de encabezados
            dgvExpedientes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 38, 68);
            dgvExpedientes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvExpedientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvExpedientes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvExpedientes.EnableHeadersVisualStyles = false;
            dgvExpedientes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 252);

            // 4. Barra de estado inferior
            lblEstadoListado = new Label();
            lblEstadoListado.Dock = DockStyle.Bottom;
            lblEstadoListado.Height = 26;
            lblEstadoListado.Font = new Font("Segoe UI", 8.5f, FontStyle.Italic);
            lblEstadoListado.ForeColor = Color.FromArgb(100, 116, 139);
            lblEstadoListado.Text = "Cargando datos desde arreglos paralelos...";
            lblEstadoListado.TextAlign = ContentAlignment.MiddleLeft;

            // Ensamblado
            pnlListadoPrincipal.Controls.Add(dgvExpedientes);
            pnlListadoPrincipal.Controls.Add(lblEstadoListado);
            pnlListadoPrincipal.Controls.Add(pnlDiagnostico);
            pnlListadoPrincipal.Controls.Add(grpBarraBusqueda);

            tabListado.Controls.Add(pnlListadoPrincipal);
        }

        // --------------------------------------------------------------------------------------
        // EVENTOS Y MÉTODOS DEL FORMULARIO
        // --------------------------------------------------------------------------------------

        private void FrmPrincipal_Load(object? sender, EventArgs e)
        {
            // Cargar datos persistidos desde expedientes.txt si existe
            bool cargado = ModuloMesaPartes.CargarDesdeArchivo(out string mensajeCarga);

            if (!cargado)
            {
                // Si el archivo no existía, inicializar arreglos en blanco
                ModuloMesaPartes.InicializarEstructuras();
            }

            // Sincronizar vistas y controles
            ActualizarTodo();
        }

        /// <summary>
        /// Refresca tanto el formulario de registro como la matriz 2D y la grilla de expedientes.
        /// </summary>
        private void ActualizarTodo()
        {
            txtCodigoAuto.Text = ModuloMesaPartes.GenerarSiguienteCodigo();
            txtFechaAuto.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblBadgeTotal.Text = $"Expedientes: {ModuloMesaPartes.cantidadRegistros}";

            ActualizarTablaMatriz2D();
            RefrescarTablaExpedientes();
        }

        /// <summary>
        /// Carga y renderiza la Matriz Bidimensional 2D en el DataGridView utilizando bucles for anidados.
        /// </summary>
        private void ActualizarTablaMatriz2D()
        {
            dgvMatrizEstadisticas.Rows.Clear();

            // Asegurar que la matriz 2D tenga los cálculos al día
            ModuloMesaPartes.ActualizarMatrizEstadisticas();

            // Recorrer filas (Áreas) y columnas (Estados)
            for (int f = 0; f < ModuloMesaPartes.AREAS_VALIDAS.Length; f++)
            {
                string nombreArea = ModuloMesaPartes.AREAS_VALIDAS[f];
                int registrado = ModuloMesaPartes.matrizEstadisticas[f, 0];
                int enTramite = ModuloMesaPartes.matrizEstadisticas[f, 1];
                int observado = ModuloMesaPartes.matrizEstadisticas[f, 2];
                int totalFila = ModuloMesaPartes.CalcularTotalPorArea(f);

                int rowIndex = dgvMatrizEstadisticas.Rows.Add(
                    nombreArea,
                    registrado.ToString(),
                    enTramite.ToString(),
                    observado.ToString(),
                    totalFila.ToString()
                );

                // Alinear números al centro
                for (int c = 1; c <= 4; c++)
                {
                    dgvMatrizEstadisticas.Rows[rowIndex].Cells[c].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            // Fila de TOTAL GENERAL (sumatoria de columnas mediante bucles)
            int totalColReg = ModuloMesaPartes.CalcularTotalPorEstado(0);
            int totalColTra = ModuloMesaPartes.CalcularTotalPorEstado(1);
            int totalColObs = ModuloMesaPartes.CalcularTotalPorEstado(2);
            int granTotal = ModuloMesaPartes.CalcularTotalGeneralMatriz();

            int totalRowIndex = dgvMatrizEstadisticas.Rows.Add(
                "TOTAL GENERAL",
                totalColReg.ToString(),
                totalColTra.ToString(),
                totalColObs.ToString(),
                granTotal.ToString()
            );

            // Estilo visual distintivo para la fila totalizadora
            DataGridViewRow totalRow = dgvMatrizEstadisticas.Rows[totalRowIndex];
            totalRow.DefaultCellStyle.BackColor = Color.FromArgb(235, 245, 255);
            totalRow.DefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            totalRow.DefaultCellStyle.ForeColor = Color.FromArgb(14, 116, 144);
            for (int c = 1; c <= 4; c++)
            {
                totalRow.Cells[c].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        /// <summary>
        /// Carga manualmente los registros desde los 7 arreglos paralelos unidimensionales (1D)
        /// hacia el DataGridView mediante un bucle for tradicional (sin LINQ ni DataSource).
        /// </summary>
        private void RefrescarTablaExpedientes()
        {
            dgvExpedientes.Rows.Clear();

            // Carga manual elemento a elemento desde los arreglos paralelos
            for (int i = 0; i < ModuloMesaPartes.cantidadRegistros; i++)
            {
                int rowIndex = dgvExpedientes.Rows.Add(
                    (i + 1).ToString(),
                    ModuloMesaPartes.codigos[i],
                    ModuloMesaPartes.dnis[i],
                    ModuloMesaPartes.ciudadanos[i],
                    ModuloMesaPartes.asuntos[i],
                    ModuloMesaPartes.areas[i],
                    ModuloMesaPartes.estados[i],
                    ModuloMesaPartes.fechas[i]
                );

                // Colorear levemente la celda de Estado según su valor
                string est = ModuloMesaPartes.estados[i];
                if (est == "Registrado")
                {
                    dgvExpedientes.Rows[rowIndex].Cells[6].Style.ForeColor = Color.FromArgb(21, 128, 61); // Verde
                    dgvExpedientes.Rows[rowIndex].Cells[6].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
                else if (est == "En Trámite")
                {
                    dgvExpedientes.Rows[rowIndex].Cells[6].Style.ForeColor = Color.FromArgb(180, 83, 9); // Ámbar
                    dgvExpedientes.Rows[rowIndex].Cells[6].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
                else if (est == "Observado")
                {
                    dgvExpedientes.Rows[rowIndex].Cells[6].Style.ForeColor = Color.FromArgb(185, 28, 28); // Rojo
                    dgvExpedientes.Rows[rowIndex].Cells[6].Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                }
            }

            string estadoOrden = ModuloMesaPartes.estaOrdenadoAscendente
                ? "Ordenado A-Z por Código"
                : "Sin orden garantizado";

            lblEstadoListado.Text = $"Mostrando {ModuloMesaPartes.cantidadRegistros} expedientes cargados desde arreglos paralelos | Archivo: 'expedientes.txt' | Estado: {estadoOrden}";
        }

        // --------------------------------------------------------------------------------------
        // BOTÓN: REGISTRAR EXPEDIENTE
        // --------------------------------------------------------------------------------------
        private void BtnRegistrar_Click(object? sender, EventArgs e)
        {
            string dni = txtDni.Text;
            string ciudadano = txtCiudadano.Text;
            string asunto = txtAsunto.Text;
            string area = cboArea.SelectedItem?.ToString() ?? string.Empty;
            string estado = cboEstado.SelectedItem?.ToString() ?? string.Empty;
            string fecha = txtFechaAuto.Text;

            // Inserción en arreglos paralelos y persistencia en .txt mediante función modular
            bool exito = ModuloMesaPartes.RegistrarNuevoTramite(
                dni,
                ciudadano,
                asunto,
                area,
                estado,
                fecha,
                out string codigoGenerado,
                out string mensajeResultado
            );

            if (!exito)
            {
                MessageBox.Show(
                    mensajeResultado,
                    "Validación de Datos (Ley N.° 31170)",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            MessageBox.Show(
                mensajeResultado + "\n\nSe actualizaron la Matriz 2D y el archivo 'expedientes.txt'.",
                "Registro Exitoso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // Limpiar formulario y regenerar correlativo
            LimpiarFormulario();
            ActualizarTodo();
        }

        // --------------------------------------------------------------------------------------
        // BOTÓN: LIMPIAR CAMPOS
        // --------------------------------------------------------------------------------------
        private void BtnLimpiar_Click(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtDni.Clear();
            txtCiudadano.Clear();
            txtAsunto.Clear();
            if (cboArea.Items.Count > 0) cboArea.SelectedIndex = 0;
            if (cboEstado.Items.Count > 0) cboEstado.SelectedIndex = 0;
            txtCodigoAuto.Text = ModuloMesaPartes.GenerarSiguienteCodigo();
            txtFechaAuto.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDni.Focus();
        }

        // --------------------------------------------------------------------------------------
        // BOTÓN: CARGAR DATOS DE PRUEBA REALES (8 EXPEDIENTES MUNICIPALES)
        // --------------------------------------------------------------------------------------
        private void BtnCargarPrueba_Click(object? sender, EventArgs e)
        {
            DialogResult confirmacion = MessageBox.Show(
                "¿Desea cargar 8 expedientes de prueba reales de gestión municipal?\n" +
                "Estos se añadirán a los arreglos paralelos y se guardarán en 'expedientes.txt'.",
                "Cargar Datos de Demostración",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion == DialogResult.Yes)
            {
                int insertados = ModuloMesaPartes.CargarDatosDePruebaReales(out string mensaje);
                MessageBox.Show(mensaje, "Datos de Prueba Cargados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ActualizarTodo();
            }
        }

        // --------------------------------------------------------------------------------------
        // BOTÓN: BÚSQUEDA SECUENCIAL (LINEAL) - O(n)
        // --------------------------------------------------------------------------------------
        private void BtnBuscarSecuencial_Click(object? sender, EventArgs e)
        {
            string criterio = txtBuscar.Text;

            if (string.IsNullOrWhiteSpace(criterio))
            {
                MessageBox.Show(
                    "Por favor, ingrese un Código de expediente (ej. EXP-0001) o número de DNI para buscar.",
                    "Criterio Vacío",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtBuscar.Focus();
                return;
            }

            if (ModuloMesaPartes.cantidadRegistros == 0)
            {
                MessageBox.Show("No hay expedientes registrados en el sistema.", "Sin Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ejecución del algoritmo secuencial O(n)
            int indice = ModuloMesaPartes.BuscarSecuencial(criterio, out int pasos);

            if (indice != -1)
            {
                // Resaltar en la tabla
                ResaltarFilaEnTabla(indice);

                string detalle = $"✔ ENCONTRADO mediante BÚSQUEDA SECUENCIAL [Complejidad O(n)]\n\n" +
                                 $"• Índice en arreglos paralelos: [{indice}]\n" +
                                 $"• Comparaciones realizadas: {pasos} paso(s)\n" +
                                 $"• Código: {ModuloMesaPartes.codigos[indice]}\n" +
                                 $"• DNI: {ModuloMesaPartes.dnis[indice]}\n" +
                                 $"• Ciudadano: {ModuloMesaPartes.ciudadanos[indice]}\n" +
                                 $"• Asunto: {ModuloMesaPartes.asuntos[indice]}\n" +
                                 $"• Área: {ModuloMesaPartes.areas[indice]}\n" +
                                 $"• Estado: {ModuloMesaPartes.estados[indice]}\n" +
                                 $"• Fecha: {ModuloMesaPartes.fechas[indice]}";

                lblDiagnosticoTitulo.Text = $"✔ BÚSQUEDA SECUENCIAL (O(n)) — Trámite {ModuloMesaPartes.codigos[indice]} Encontrado";
                lblDiagnosticoTitulo.ForeColor = Color.FromArgb(16, 120, 70);
                lblDiagnosticoDetalle.Text = $"Índice: [{indice}] | Pasos/Comparaciones ejecutadas: {pasos} de {ModuloMesaPartes.cantidadRegistros} | Solicitante: {ModuloMesaPartes.ciudadanos[indice]}";

                MessageBox.Show(detalle, "Resultado de Búsqueda Secuencial (O(n))", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblDiagnosticoTitulo.Text = "❌ BÚSQUEDA SECUENCIAL (O(n)) — No se encontró coincidencia";
                lblDiagnosticoTitulo.ForeColor = Color.FromArgb(185, 28, 28);
                lblDiagnosticoDetalle.Text = $"Criterio: '{criterio}' no encontrado tras revisar los {pasos} registros en memoria.";

                MessageBox.Show(
                    $"No se encontró ningún expediente con el criterio '{criterio}'.\nSe realizaron {pasos} comparaciones (recorrido lineal completo).",
                    "No Encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        // --------------------------------------------------------------------------------------
        // BOTÓN: BÚSQUEDA BINARIA - O(log n)
        // --------------------------------------------------------------------------------------
        private void BtnBuscarBinaria_Click(object? sender, EventArgs e)
        {
            string codigo = txtBuscar.Text;

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show(
                    "La Búsqueda Binaria requiere el Código del expediente (ej. EXP-0001).\nPor favor, ingréselo en la caja de búsqueda.",
                    "Código Requerido",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtBuscar.Focus();
                return;
            }

            if (ModuloMesaPartes.cantidadRegistros == 0)
            {
                MessageBox.Show("No hay expedientes registrados en el sistema.", "Sin Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ejecución del algoritmo binario O(log n)
            int indice = ModuloMesaPartes.BuscarBinariaPorCodigo(codigo, out int pasos, out bool seReordeno);

            // Si fue necesario ordenar los arreglos para poder ejecutar la búsqueda binaria, refrescamos la grilla
            if (seReordeno)
            {
                RefrescarTablaExpedientes();
            }

            if (indice != -1)
            {
                ResaltarFilaEnTabla(indice);

                string mensajeReorden = seReordeno
                    ? "\n(Nota: Se ejecutó un ordenamiento previo automático Bubble Sort para cumplir con el requisito de arreglo ordenado)."
                    : "\n(Nota: El arreglo ya se encontraba ordenado).";

                string detalle = $"⚡ ENCONTRADO mediante BÚSQUEDA BINARIA [Complejidad O(log n)]\n\n" +
                                 $"• Índice en arreglos paralelos: [{indice}]\n" +
                                 $"• Iteraciones/Divisiones binarias: {pasos} paso(s)\n" +
                                 $"• Eficiencia: O(log₂ {ModuloMesaPartes.cantidadRegistros}) ≈ {Math.Ceiling(Math.Log2(Math.Max(1, ModuloMesaPartes.cantidadRegistros)))} pasos teóricos máximos\n" +
                                 $"• Código: {ModuloMesaPartes.codigos[indice]}\n" +
                                 $"• DNI: {ModuloMesaPartes.dnis[indice]}\n" +
                                 $"• Ciudadano: {ModuloMesaPartes.ciudadanos[indice]}\n" +
                                 $"• Asunto: {ModuloMesaPartes.asuntos[indice]}\n" +
                                 $"• Área: {ModuloMesaPartes.areas[indice]}\n" +
                                 $"• Estado: {ModuloMesaPartes.estados[indice]}\n" +
                                 $"• Fecha: {ModuloMesaPartes.fechas[indice]}" +
                                 mensajeReorden;

                lblDiagnosticoTitulo.Text = $"⚡ BÚSQUEDA BINARIA (O(log n)) — Trámite {ModuloMesaPartes.codigos[indice]} Encontrado";
                lblDiagnosticoTitulo.ForeColor = Color.FromArgb(14, 116, 144);
                lblDiagnosticoDetalle.Text = $"Índice: [{indice}] | Divisiones binarias: {pasos} iteraciones | Solicitante: {ModuloMesaPartes.ciudadanos[indice]} {(seReordeno ? "[Arreglo Reordenado]" : "")}";

                MessageBox.Show(detalle, "Resultado de Búsqueda Binaria (O(log n))", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblDiagnosticoTitulo.Text = "❌ BÚSQUEDA BINARIA (O(log n)) — Código no encontrado";
                lblDiagnosticoTitulo.ForeColor = Color.FromArgb(185, 28, 28);
                lblDiagnosticoDetalle.Text = $"Código: '{codigo}' no encontrado tras {pasos} divisiones de rango binario.";

                MessageBox.Show(
                    $"No se encontró ningún expediente con el código '{codigo}'.\nSe descartaron mitades en {pasos} iteraciones.",
                    "No Encontrado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        // --------------------------------------------------------------------------------------
        // BOTONES DE ORDENAMIENTO MANUAL BUBBLE SORT
        // --------------------------------------------------------------------------------------
        private void BtnOrdenarAZ_Click(object? sender, EventArgs e)
        {
            if (ModuloMesaPartes.cantidadRegistros <= 1)
            {
                MessageBox.Show("Se requieren al menos 2 registros para demostrar el ordenamiento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int comparaciones = 0;
            int intercambios = 0;

            // Ordenamiento manual burbuja por código (A-Z)
            ModuloMesaPartes.OrdenarBurbujaPorCodigo(true, ref comparaciones, ref intercambios);

            // Refrescar grilla
            RefrescarTablaExpedientes();

            lblDiagnosticoTitulo.Text = "▲ ORDENAMIENTO BUBBLE SORT (A-Z) COMPLETADO";
            lblDiagnosticoTitulo.ForeColor = Color.FromArgb(30, 58, 138);
            lblDiagnosticoDetalle.Text = $"Registros: {ModuloMesaPartes.cantidadRegistros} | Comparaciones O(n²): {comparaciones} | Intercambios sincronizados: {intercambios}";

            MessageBox.Show(
                $"Arreglos paralelos ordenados de forma ASCENDENTE (A-Z) por Código.\n\n" +
                $"• Algoritmo: Bubble Sort (Burbuja)\n" +
                $"• Elementos procesados: {ModuloMesaPartes.cantidadRegistros}\n" +
                $"• Comparaciones realizadas: {comparaciones}\n" +
                $"• Intercambios en arreglos paralelos: {intercambios}\n" +
                $"• Todos los 7 arreglos paralelos fueron sincronizados mediante Intercambiar(i, j).",
                "Ordenamiento Ascendente Concluido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void BtnOrdenarZA_Click(object? sender, EventArgs e)
        {
            if (ModuloMesaPartes.cantidadRegistros <= 1)
            {
                MessageBox.Show("Se requieren al menos 2 registros para demostrar el ordenamiento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int comparaciones = 0;
            int intercambios = 0;

            // Ordenamiento manual burbuja por código (Z-A)
            ModuloMesaPartes.OrdenarBurbujaPorCodigo(false, ref comparaciones, ref intercambios);

            // Refrescar grilla
            RefrescarTablaExpedientes();

            lblDiagnosticoTitulo.Text = "▼ ORDENAMIENTO BUBBLE SORT (Z-A) COMPLETADO";
            lblDiagnosticoTitulo.ForeColor = Color.FromArgb(30, 58, 138);
            lblDiagnosticoDetalle.Text = $"Registros: {ModuloMesaPartes.cantidadRegistros} | Comparaciones O(n²): {comparaciones} | Intercambios sincronizados: {intercambios}";

            MessageBox.Show(
                $"Arreglos paralelos ordenados de forma DESCENDENTE (Z-A) por Código.\n\n" +
                $"• Algoritmo: Bubble Sort (Burbuja)\n" +
                $"• Elementos procesados: {ModuloMesaPartes.cantidadRegistros}\n" +
                $"• Comparaciones realizadas: {comparaciones}\n" +
                $"• Intercambios en arreglos paralelos: {intercambios}\n" +
                $"• Todos los 7 arreglos paralelos fueron sincronizados mediante Intercambiar(i, j).",
                "Ordenamiento Descendente Concluido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // --------------------------------------------------------------------------------------
        // UTILIDADES GRÁFICAS
        // --------------------------------------------------------------------------------------

        private void ResaltarFilaEnTabla(int indice)
        {
            if (indice < 0 || indice >= dgvExpedientes.Rows.Count) return;

            // Limpiar selecciones previas y estilos
            for (int r = 0; r < dgvExpedientes.Rows.Count; r++)
            {
                dgvExpedientes.Rows[r].Selected = false;
                dgvExpedientes.Rows[r].DefaultCellStyle.BackColor = (r % 2 == 0) ? Color.White : Color.FromArgb(249, 250, 252);
            }

            // Seleccionar y resaltar con fondo llamativo
            dgvExpedientes.Rows[indice].Selected = true;
            dgvExpedientes.Rows[indice].DefaultCellStyle.BackColor = Color.FromArgb(254, 240, 138); // Amarillo suave de resaltado
            dgvExpedientes.FirstDisplayedScrollingRowIndex = indice;
        }

        private void TxtDni_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Permitir únicamente dígitos numéricos y la tecla de retroceso (Backspace)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private Label CrearLabelCampo(string texto, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = texto;
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(51, 65, 85);
            return lbl;
        }
    }
}
