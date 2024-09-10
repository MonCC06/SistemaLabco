using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ET;
using BL;
using System.Security.Cryptography;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace SistemaLabco
{
    public partial class FrmInicio : Form
    {
        public FrmInicio()
        {
            InitializeComponent();
            CargarEstadoComboBox();
            DgvFacturaProducto.CellValueChanged += DgvFacturaProducto_CellValueChanged;
            DgvFacturaProducto.CellEndEdit += DgvFacturaProducto_CellEndEdit;
            

        }



        private void CargarEstadoComboBox()
        {
            // Añadir ítems al ComboBox
            comboBoxEstado.Items.Add("Anulada");
            comboBoxEstado.Items.Add("Completada");
            comboBoxEstado.Items.Add("En Proceso");

            // Establecer un valor predeterminado (opcional)
            comboBoxEstado.SelectedIndex = 0; // Selecciona el primer ítem por defecto
        }
        //CLIENTE//

        int EstadoGuarda = 1;
        int IDCliente = 0;
        int IDTrabajador = 0;
        int IDServicio = 0;
        int IdMarca = 0;
        int IDProducto = 0;
        int IdVehiculo = 0;
        int seleccionCl = 0;
        int selecionMa = 0;
        bool seleccion = true;
        private void FormatoCL()
        {
            if (DgvCliente.Columns.Count >= 6) // Verifica si hay al menos 6 columnas
            {
                DgvCliente.Columns[0].Width = 100;
                DgvCliente.Columns[0].HeaderText = "ID_Cliente";
                DgvCliente.Columns[1].Width = 100;
                DgvCliente.Columns[1].HeaderText = "Nombre";
                DgvCliente.Columns[2].Width = 100;
                DgvCliente.Columns[2].HeaderText = "Cedula";
                DgvCliente.Columns[3].Width = 100;
                DgvCliente.Columns[3].HeaderText = "Telefono";
                DgvCliente.Columns[4].Width = 100;
                DgvCliente.Columns[4].HeaderText = "Correo";
                DgvCliente.Columns[5].Width = 100;
                DgvCliente.Columns[5].HeaderText = "Estado";
            }

        }

        private void ListadoCL(string tTexto)
        {
            try
            {
                DgvCliente.DataSource = BLCliente.ListadoCL(tTexto);
                this.FormatoCL();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            //datasource nos dice de donde vamos a consumir los datos
        }

        private void BotonesCliente(bool LEstado)
        {
            this.btnGuardarNuevoCliente.Enabled = LEstado;
            this.btnBuscarCliente.Enabled = LEstado;
            this.btnModficarCliente.Enabled = LEstado;
            this.btnCancelarNuevoCliente.Enabled = LEstado;
            this.btnEliminarCliente.Enabled = LEstado;
        }

        private void SeleccionaCliente()
        {
            //convertir dato de string a un valor 
            if (string.IsNullOrEmpty(Convert.ToString(DgvCliente.CurrentRow.Cells["IDCliente"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            else
            {
                //convertir el id del cliente de la celda seleccionada de string a entero
                this.IDCliente = Convert.ToInt32(DgvCliente.CurrentRow.Cells["IDCliente"].Value);
                txtNuevoNombreCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Nombre"].Value);
                txtNuevoCedulaCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Cedula"].Value);
                txtNuevoCorreoCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Correo"].Value);
                txtNuevoTelefonoCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Telefono"].Value);
            }
        }


        private void ActualizarCliente()
        {
            // Aquí puedes utilizar el método GuardarCliente o crear un método específico para actualización.
            // Asumiendo que `GuardarCliente` se usa para ambos casos:
            GuardarCliente();
            this.ListadoCL("%");
        }

        private void ModificarCliente()
        {
            EstadoGuarda = 2; // Indica que se trata de una actualización

            // Verificar si hay una fila seleccionada en el DataGridView
            if (DgvCliente.CurrentRow != null)
            {
                // Obtener el IDCliente de la fila seleccionada
                this.IDCliente = Convert.ToInt32(DgvCliente.CurrentRow.Cells["IDCliente"].Value);

                // Poblar los campos con los datos actuales del cliente
                txtNuevoNombreCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Nombre"].Value);
                txtNuevoCedulaCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Cedula"].Value);
                txtNuevoCorreoCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Correo"].Value);
                txtNuevoTelefonoCliente.Text = Convert.ToString(DgvCliente.CurrentRow.Cells["Telefono"].Value);


                // Establecer el enfoque en el primer campo editable
                txtNuevoNombreCliente.Focus();
            }
            else
            {
                MessageBox.Show("Seleccione un cliente para modificar.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GuardarCliente()
        {
            if (txtNuevoNombreCliente.Text == String.Empty)
            {
                MessageBox.Show("Nombre del Cliente requerido(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
            if (txtNuevoCedulaCliente.Text == String.Empty)
            {
                MessageBox.Show("Cedula del Cliente requerida(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
            if (txtNuevoCorreoCliente.Text == String.Empty)
            {
                MessageBox.Show("Correo del Cliente requerido(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
            if (txtNuevoTelefonoCliente.Text == String.Empty)
            {
                MessageBox.Show("Telefono del  requerido(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
            else
            {
                ETCliente eTCliente = new ETCliente();
                //Respuesta de la bl y la que recibe de dal cuando se hace el insert (cuando guardamos)
                //para saber si el proceso fue exitoso o no
                String Rpta = "";
                eTCliente.IDCliente = this.IDCliente;
                //lo que el usuario digite en ese campo se va a capturar y se va a enviar a la bd como propiedad
                eTCliente.Nombre = txtNuevoNombreCliente.Text.Trim();
                eTCliente.Cedula = txtNuevoCedulaCliente.Text.Trim();
                eTCliente.Correo = txtNuevoCorreoCliente.Text.Trim();
                eTCliente.Telefono = txtNuevoTelefonoCliente.Text.Trim();
                //respuesta igual a los que nos retorne, enviar parametros para saber si es nuevo o no
                try
                {
                    Rpta = BLCliente.GuardarCL(EstadoGuarda, eTCliente);

                    if (Rpta == "OK")
                    {

                        MessageBox.Show("Los datos se han registrado", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        EstadoGuarda = 1;
                        this.BotonesCliente(true);
                        txtNuevoNombreCliente.Text = "";
                        txtNuevoCedulaCliente.Text = "";
                        txtNuevoCorreoCliente.Text = "";
                        txtNuevoTelefonoCliente.Text = "";
                        this.IDCliente = 0;
                    }
                    else
                    {
                        MessageBox.Show("No se logró registrar el dato: " + Rpta, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void btnGuardarNuevoCliente_Click(object sender, EventArgs e)
        {
            if (EstadoGuarda == 2)
            {
                // Actualización del cliente
                ActualizarCliente();
            }
            else if (EstadoGuarda == 1)
            {
                // Inserción de nuevo cliente
                GuardarCliente();
            }
            else
            {
                MessageBox.Show("El estado de guardado no está definido.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void btnEliminarCliente_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Convert.ToString(DgvCliente.CurrentRow.Cells["IDCliente"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            else
            {
                DialogResult opcion;
                //preguntar si se quiere realizar procedimiento y a opcion se le asignael valor de la respuesta
                opcion = MessageBox.Show("¿Está seguro de eliminar el registro seleccionado?", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                   MessageBoxIcon.Question);
                if (opcion == DialogResult.Yes)
                {
                    String Rpta = "";
                    //convertir a int
                    this.IDCliente = Convert.ToInt32(DgvCliente.CurrentRow.Cells["IDCliente"].Value);
                    Rpta = BLCliente.EliminaCL(this.IDCliente);

                    if (Rpta.Equals("OK"))
                    {
                        this.IDCliente = 0;
                        MessageBox.Show("Registro Eliminado", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Exclamation);

                    }
                }

            }

        }

        private void btnModficarCliente_Click(object sender, EventArgs e)
        {
            ModificarCliente();
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            this.ListadoCL(TxtBuscarCliente.Text.Trim());
        }

        private void btnCancelarNuevoCliente_Click(object sender, EventArgs e)
        {
            this.BotonesCliente(true);
            txtNuevoNombreCliente.Text = "";
            txtNuevoCedulaCliente.Text = "";
            txtNuevoCorreoCliente.Text = "";
            txtNuevoTelefonoCliente.Text = "";

        }

        private void ckbNombreCliente_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoCL(TxtBuscarCliente.Text.Trim());

        }

        private void ckbCedulaCliente_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoCL(TxtBuscarCliente.Text.Trim());
        }

        //TRABAJADOR//

        private void FormatoTrabajdor()
        {
            if (DgvCliente.Columns.Count >= 6) // Verifica si hay al menos 6 columnas
            {
                DgvTrabajador.Columns[0].Width = 100;
                DgvTrabajador.Columns[0].HeaderText = "IDTrabajador";
                DgvTrabajador.Columns[1].Width = 100;
                DgvTrabajador.Columns[1].HeaderText = "Nombre";
                DgvTrabajador.Columns[2].Width = 100;
                DgvTrabajador.Columns[2].HeaderText = "Cedula";
                DgvTrabajador.Columns[3].Width = 100;
                DgvTrabajador.Columns[3].HeaderText = "Telefono";
                DgvTrabajador.Columns[4].Width = 100;
                DgvTrabajador.Columns[4].HeaderText = "Correo";

            }

        }
        private void ListadoTrabajador(string tTexto)
        {
            try
            {
                DgvTrabajador.DataSource = BLTrabajador.ListadoTR(tTexto);
                this.FormatoTrabajdor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            //datasource nos dice de donde vamos a consumir los datos
        }
        private void BotonesTrabajador(bool LEstado)
        {
            this.btnGuardarNuevoTrabajador.Enabled = LEstado;
            this.btnBuscarTrabajador.Enabled = LEstado;
            this.btnModficarTrabajador.Enabled = LEstado;
            this.btnCancelarNuevoTrabajador.Enabled = LEstado;
            this.btnEliminarTrabajador.Enabled = LEstado;
        }
        private void SeleccionaTrabajador()
        {
            //convertir dato de string a un valor 
            if (string.IsNullOrEmpty(Convert.ToString(DgvTrabajador.CurrentRow.Cells["IDTrabajador"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            else
            {
                //convertir el id del cliente de la celda seleccionada de string a entero
                this.IDTrabajador = Convert.ToInt32(DgvTrabajador.CurrentRow.Cells["IDTrabajador"].Value);
                txtNuevoNombreTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Nombre"].Value);
                txtNuevoCedulaTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Cedula"].Value);
                txtNuevoCorreoTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Correo"].Value);
                txtNuevoTelefonoTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Telefono"].Value);
            }
        }
        private void ActualizarTrabajador()
        {
            // Aquí puedes utilizar el método GuardarCliente o crear un método específico para actualización.
            // Asumiendo que `GuardarCliente` se usa para ambos casos:
            GuardarTrabajador();
            this.ListadoTrabajador("%");
        }
        private void ModificarTrabajador()
        {
            EstadoGuarda = 2; // Indica que se trata de una actualización

            // Verificar si hay una fila seleccionada en el DataGridView
            if (DgvTrabajador.CurrentRow != null)
            {
                // Obtener el IDCliente de la fila seleccionada
                this.IDTrabajador = Convert.ToInt32(DgvTrabajador.CurrentRow.Cells["IDTrabajador"].Value);

                // Poblar los campos con los datos actuales del cliente
                txtNuevoNombreTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Nombre"].Value);
                txtNuevoCedulaTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Cedula"].Value);
                txtNuevoCorreoTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Correo"].Value);
                txtNuevoTelefonoTrabajador.Text = Convert.ToString(DgvTrabajador.CurrentRow.Cells["Telefono"].Value);


                // Establecer el enfoque en el primer campo editable
                txtNuevoNombreTrabajador.Focus();
            }
            else
            {
                MessageBox.Show("Seleccione un Trabaajador para modificar.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void GuardarTrabajador()
        {
            // Verificación de campos vacíos en una sola condición
            if (string.IsNullOrWhiteSpace(txtNuevoNombreTrabajador.Text) ||
                string.IsNullOrWhiteSpace(txtNuevoCedulaTrabajador.Text) ||
                string.IsNullOrWhiteSpace(txtNuevoCorreoTrabajador.Text) ||
                string.IsNullOrWhiteSpace(txtNuevoTelefonoTrabajador.Text))
            {
                MessageBox.Show("Todos los campos son requeridos(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return; // Para detener la ejecución si algún campo está vacío
            }

            ETTrabajador eTTrabajador = new ETTrabajador();
            String Rpta = "";

            // Asignación de valores cliente
            eTTrabajador.IDTrabajador = this.IDTrabajador;
            eTTrabajador.Nombre = txtNuevoNombreTrabajador.Text.Trim();
            eTTrabajador.Cedula = txtNuevoCedulaTrabajador.Text.Trim();
            eTTrabajador.Correo = txtNuevoCorreoTrabajador.Text.Trim();
            eTTrabajador.Telefono = txtNuevoTelefonoTrabajador.Text.Trim();

            try
            {
                Rpta = BLTrabajador.GuardarTR(EstadoGuarda, eTTrabajador);

                if (Rpta == "OK")
                {
                    MessageBox.Show("Los datos se han registrado", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reinicio de valores
                    EstadoGuarda = 1;
                    this.BotonesTrabajador(true);
                    txtNuevoNombreTrabajador.Text = "";
                    txtNuevoCedulaTrabajador.Text = "";
                    txtNuevoCorreoTrabajador.Text = "";
                    txtNuevoTelefonoTrabajador.Text = "";
                    this.IDTrabajador = 0;
                }
                else
                {
                    MessageBox.Show("No se logró registrar el dato: " + Rpta, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardarNuevoTrabajador_Click(object sender, EventArgs e)
        {
            if (EstadoGuarda == 2)
            {
                // Actualización de Trabajador
                ActualizarTrabajador();
            }
            else if (EstadoGuarda == 1)
            {
                // Inserción de nuevo Trabajador
                GuardarTrabajador();
            }
            else
            {
                MessageBox.Show("El estado de guardado no está definido.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnCancelarNuevoTrabajador_Click(object sender, EventArgs e)
        {
            this.BotonesTrabajador(true);
            txtNuevoNombreTrabajador.Text = "";
            txtNuevoCedulaTrabajador.Text = "";
            txtNuevoCorreoTrabajador.Text = "";
            txtNuevoTelefonoTrabajador.Text = "";

        }

        private void btnBuscarTrabajador_Click(object sender, EventArgs e)
        {
            this.ListadoTrabajador(txtBuscarTrabajador.Text.Trim());

        }

        private void btnModficarTrabajador_Click(object sender, EventArgs e)
        {
            ModificarTrabajador();

        }

        private void btnEliminarTrabajador_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Convert.ToString(DgvTrabajador.CurrentRow.Cells["IDTrabajador"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            else
            {
                DialogResult opcion;
                //preguntar si se quiere realizar procedimiento y a opcion se le asignael valor de la respuesta
                opcion = MessageBox.Show("¿Está seguro de eliminar el registro seleccionado?", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                   MessageBoxIcon.Question);
                if (opcion == DialogResult.Yes)
                {
                    String Rpta = "";
                    //convertir a int
                    this.IDTrabajador = Convert.ToInt32(DgvTrabajador.CurrentRow.Cells["IDTrabajador"].Value);
                    Rpta = BLTrabajador.EliminaTR(this.IDTrabajador);

                    if (Rpta.Equals("OK"))
                    {
                        this.IDTrabajador = 0;
                        MessageBox.Show("Registro Eliminado", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Exclamation);

                    }
                }

            }

        }

        private void ckbNombreTrabajador_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoTrabajador(txtBuscarTrabajador.Text.Trim());

        }

        private void ckbCedulaTrabajador_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoTrabajador(txtBuscarTrabajador.Text.Trim());

        }


        //SERVICIO//

        private void FormatoServicio()
        {
            if (DGVServicio.Columns.Count >= 4) // Verifica si hay al menos 6 columnas
            {
                DGVServicio.Columns[0].Width = 100;
                DGVServicio.Columns[0].HeaderText = "IDServico";
                DGVServicio.Columns[1].Width = 100;
                DGVServicio.Columns[1].HeaderText = "Descripcion";
                DGVServicio.Columns[2].Width = 100;
                DGVServicio.Columns[2].HeaderText = "Monto";


            }


        }
        private void ListadoServicio(string tTexto)
        {
            try
            {
                DGVServicio.DataSource = BLServicio.ListadoSE(tTexto);
                this.FormatoServicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            //datasource nos dice de donde vamos a consumir los datos
        }
        private void BotonesServicio(bool LEstado)
        {
            this.BTGuardarServicio.Enabled = LEstado;
            this.BTBuscarServicio.Enabled = LEstado;
            this.BTModificarServicio.Enabled = LEstado;
            this.BTCancelarServicio.Enabled = LEstado;
            this.BTEliminarServicio.Enabled = LEstado;
        }
        private void SeleccionaServicio()
        {
            //convertir dato de string a un valor 
            if (string.IsNullOrEmpty(Convert.ToString(DGVServicio.CurrentRow.Cells["IDServicio"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            else
            {
                //convertir el id del cliente de la celda seleccionada de string a entero
                this.IDServicio = Convert.ToInt32(DGVServicio.CurrentRow.Cells["IDTrabajador"].Value);
                TBDescripcionServicio.Text = Convert.ToString(DGVServicio.CurrentRow.Cells["Descripcion"].Value);
                TBPrecioServicio.Text = Convert.ToString(DGVServicio.CurrentRow.Cells["Monto"].Value);

            }
        }
        private void ActualizarServicio()
        {
            // Aquí puedes utilizar el método GuardarCliente o crear un método específico para actualización.
            // Asumiendo que `GuardarCliente` se usa para ambos casos:
            GuardarServicio();
            this.ListadoServicio("%");
        }
        private void ModificarServicio()
        {
            EstadoGuarda = 2; // Indica que se trata de una actualización

            // Verificar si hay una fila seleccionada en el DataGridView
            if (DGVServicio.CurrentRow != null)
            {
                // Obtener el IDCliente de la fila seleccionada
                this.IDServicio = Convert.ToInt32(DGVServicio.CurrentRow.Cells["IDServicio"].Value);

                // Poblar los campos con los datos actuales del cliente
                TBDescripcionServicio.Text = Convert.ToString(DGVServicio.CurrentRow.Cells["Descripcion"].Value);
                TBPrecioServicio.Text = Convert.ToString(DGVServicio.CurrentRow.Cells["Monto"].Value);



                // Establecer el enfoque en el primer campo editable
                TBDescripcionServicio.Focus();
            }
            else
            {
                MessageBox.Show("Seleccione un Trabaajador para modificar.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void GuardarServicio()
        {
            // Verificación de campos vacíos en una sola condición
            if (string.IsNullOrWhiteSpace(TBDescripcionServicio.Text) ||
                string.IsNullOrWhiteSpace(TBPrecioServicio.Text))
            {
                MessageBox.Show("Todos los campos son requeridos(*)", "Aviso del Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return; // Para detener la ejecución si algún campo está vacío
            }

            ETServicio eTServicio = new ETServicio();
            String Rpta = "";

            // Asignación de valores cliente
            eTServicio.IDServicio = this.IDServicio;
            eTServicio.Descripcion = TBDescripcionServicio.Text.Trim();
            eTServicio.Monto = float.Parse(TBPrecioServicio.Text.Trim());



            try
            {
                Rpta = BLServicio.GuardarSE(EstadoGuarda, eTServicio);

                if (Rpta == "OK")
                {
                    MessageBox.Show("Los datos se han registrado", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reinicio de valores
                    EstadoGuarda = 1;
                    this.BotonesServicio(true);
                    TBDescripcionServicio.Text = "";
                    TBPrecioServicio.Text = "";

                    this.IDServicio = 0;
                }
                else
                {
                    MessageBox.Show("No se logró registrar el dato: " + Rpta, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BTCancelarServicio_Click(object sender, EventArgs e)
        {
            this.BotonesServicio(true);
            TBDescripcionServicio.Text = "";

            TBPrecioServicio.Text = "";

        }

        private void BTGuardarServicio_Click(object sender, EventArgs e)
        {

            if (EstadoGuarda == 2)
            {
                // Actualización de Trabajador
                ActualizarServicio();
            }
            else if (EstadoGuarda == 1)
            {
                // Inserción de nuevo Trabajador
                GuardarServicio();
            }
            else
            {
                MessageBox.Show("El estado de guardado no está definido.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void BTBuscarServicio_Click(object sender, EventArgs e)
        {
            this.ListadoServicio(TBBuscarServicio.Text.Trim());

        }

        private void BTModificarServicio_Click(object sender, EventArgs e)
        {
            ModificarServicio();

        }

        private void BTEliminarServicio_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Convert.ToString(DGVServicio.CurrentRow.Cells["IDServicio"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            else
            {
                DialogResult opcion;
                //preguntar si se quiere realizar procedimiento y a opcion se le asignael valor de la respuesta
                opcion = MessageBox.Show("¿Está seguro de eliminar el registro seleccionado?", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                   MessageBoxIcon.Question);
                if (opcion == DialogResult.Yes)
                {
                    String Rpta = "";
                    //convertir a int
                    this.IDServicio = Convert.ToInt32(DGVServicio.CurrentRow.Cells["IDServicio"].Value);
                    Rpta = BLServicio.EliminaSE(this.IDServicio);

                    if (Rpta.Equals("OK"))
                    {
                        this.IDServicio = 0;
                        MessageBox.Show("Registro Eliminado", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Exclamation);

                    }
                }

            }

        }

        //MARCA//

        private void FormatoMa()
        {
            dataGridViewMarca.Columns[0].Width = 100;
            dataGridViewMarca.Columns[0].HeaderText = "ID Marca";
            dataGridViewMarca.Columns[1].Width = 100;
            dataGridViewMarca.Columns[1].HeaderText = "Marca";

        }

        private void ListadoMarca(string cTexto)
        {

            try
            {
                dataGridViewMarca.DataSource = BLMarca.ListadoMA(cTexto);
                this.FormatoMa();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }



        }


        private void ActualizarMarca()
        {

            GuardarMarca();
            this.ListadoMarca("%");
        }



        private void SeleccionaItemMarca()
        {
            //Validasmos que el DATAGEIP tenga datos para que no nos de error

            if (string.IsNullOrEmpty(Convert.ToString(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value)))
            {
                MessageBox.Show("No hay datos que mostrar", "Aviso del sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                this.IdMarca = Convert.ToInt32(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value);
                TxTNombreMarca.Text = Convert.ToString(dataGridViewMarca.CurrentRow.Cells["Nombre"].Value);

            }

        }


        private void ModificarMarca()
        {
            EstadoGuarda = 2; // Indica que se trata de una actualización

            // Verificar si hay una fila seleccionada en el DataGridView
            if (dataGridViewMarca.CurrentRow != null)
            {
                // Obtener el IDCliente de la fila seleccionada
                this.IdMarca = Convert.ToInt32(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value);

                // Poblar los campos con los datos actuales del cliente
                TxTNombreMarca.Text = Convert.ToString(dataGridViewMarca.CurrentRow.Cells["Nombre"].Value);


                // Establecer el enfoque en el primer campo editable
                TxTNombreMarca.Focus();
            }
            else
            {
                MessageBox.Show("Seleccione un cliente para modificar.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void GuardarMarca()
        {

            if (TxTNombreMarca.Text == String.Empty)
            {
                MessageBox.Show("Falta ingresar datos requeridos(*)", "Aviso del sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {

                ETMarca etmarca = new ETMarca();
                string Rpta = "";
                etmarca.IDMarca = this.IdMarca;
                etmarca.Nombre = TxTNombreMarca.Text.Trim();
                Rpta = BLMarca.GuardarMA(EstadoGuarda, etmarca);


                if (Rpta == "OK")
                {
                    this.ListadoMarca("%");
                    MessageBox.Show("Los datos se han registrado", "Aviso del sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
                    EstadoGuarda = 1; // si no guardo nada
                    TxTNombreMarca.Text = "";
                    TxTNombreMarca.ReadOnly = false;
                    this.IdMarca = 0;

                }
                else
                {
                    MessageBox.Show(Rpta, "Aviso del sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);

                }

            }



        }

        private void buttonGuardarMarca_Click(object sender, EventArgs e)
        {
            if (EstadoGuarda == 2)
            {
                // Actualización del cliente
                ActualizarMarca();
            }
            else if (EstadoGuarda == 1)
            {
                // Inserción de nuevo cliente
                GuardarMarca();
            }
            else
            {
                MessageBox.Show("El estado de guardado no está definido.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void buttonCancelarMarca_Click(object sender, EventArgs e)
        {
            TxTNombreMarca.Text = "";
        }

        private void buttonBuscarMarca_Click(object sender, EventArgs e)
        {

            this.ListadoMarca(TxTBuscarMarca.Text.Trim());


        }

        private void buttonModificarMarca_Click(object sender, EventArgs e)
        {
            ModificarMarca();

        }

        private void buttonEliminarMarca_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value)))
            {
                MessageBox.Show("No hay datos que mostrar", "Aviso del sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                DialogResult opcion;
                opcion = MessageBox.Show("Esta seguro de eliminar el registro seleccionado ?", "Aviso del sistema", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (opcion == DialogResult.Yes)
                {
                    string Rpta = "";
                    this.IdMarca = Convert.ToInt32(dataGridViewMarca.CurrentRow.Cells["IDMarca"].Value);
                    Rpta = BLMarca.EliminarMA(this.IdMarca);

                    if (Rpta.Equals("OK"))
                    {
                        this.ListadoMarca("%");//LLAMAMOS EL METODO PARA ACTUALIZAR LA LISTA
                        this.IdMarca = 0;
                        MessageBox.Show("Registro eliminado", "Aviso del sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Exclamation);
                    }

                }


            }

        }

        //VEHICULO//


        private void FormatoVe()
        {
            DgvLClVh.Columns[0].Width = 100;
            DgvLClVh.Columns[0].HeaderText = "ID Vehiculo";
            DgvLClVh.Columns[1].Width = 100;
            DgvLClVh.Columns[1].HeaderText = "Vehiculo";

        }

        private void ActualizarVehiculo()
        {
            GuardarVehiculo();
            this.ListadoVe("%");
        }

        private void ListadoVe(string cTexto)
        {

            try
            {
                DgvLClVh.DataSource = BLVehiculo.ListadoVE(cTexto);
                this.FormatoVe();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }

        }

        private void SeleccionaItemVehiculo()
        {
            //Validasmos que el DATAGEIP tenga datos para que no nos de error

            if (string.IsNullOrEmpty(Convert.ToString(DgvLClVh.CurrentRow.Cells["IDVehiculo"].Value)))
            {
                MessageBox.Show("No hay datos que mostrar", "Aviso del sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {

                this.IdVehiculo = Convert.ToInt32(DgvLClVh.CurrentRow.Cells["IDVehiculo"].Value);
                TxTPlacaVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["Placa"].Value);
                TxTCliente.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["IDCliente"].Value);
                TxTModeloVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["Modelo"].Value);
                TxTModeloVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["IDMarca"].Value);
                DistanciaTxTVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["Anno"].Value);
                TxTVINVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["VIN"].Value);
                DistanciaTxTVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["DistanciaRecorrida"].Value);
            }

        }

        private void GuardarVehiculo()
        {
            // Verificar que todos los campos requeridos estén completos
            if (TxTPlacaVehiculo.Text == String.Empty ||
                TxTCliente.Text == String.Empty ||
                TxTModeloVehiculo.Text == String.Empty ||
                TxTModeloVehiculo.Text == String.Empty ||
                DistanciaTxTVehiculo.Text == String.Empty ||
                TxTVINVehiculo.Text == String.Empty ||
                DistanciaTxTVehiculo.Text == String.Empty)
            {
                MessageBox.Show("Falta ingresar datos requeridos(*)", "Aviso del sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return; // Salir si falta algún dato
            }
            if(chkK.Checked)
            {
                bool seleccion = true;

            }
            else
            {
                bool seleccion = false;
            }

            ETVehiculo etvehiculo = new ETVehiculo();

            string Rpta = "";

            // Asignaciones de campos de texto
            etvehiculo.IDVehiculo = IdVehiculo;
            etvehiculo.Placa = TxTPlacaVehiculo.Text.Trim();
            etvehiculo.Modelo = TxTModeloVehiculo.Text.Trim();
            etvehiculo.Anno = DistanciaTxTVehiculo.Text.Trim();
            etvehiculo.VIN = TxTVINVehiculo.Text.Trim();
            etvehiculo.IDCliente = seleccionCl;
            etvehiculo.IDMarca = selecionMa;
            etvehiculo.TipodeDistancia = seleccion;
            

            // Convertir distancia a decimal
            decimal distancia;
            if (decimal.TryParse(DistanciaTxTVehiculo.Text.Trim(), out distancia))
            {
                // Determinar tipo de distancia según el estado del CheckBox
                if (chkK.Checked) // CheckBox seleccionado para "Kilómetros"
                {
                    // Asignar solo el valor decimal sin la unidad
                    etvehiculo.DistanciaRecorrida = distancia;
                    etvehiculo.TipodeDistancia = true;
                }
                else // CheckBox no seleccionado para "Millas"
                {
                    // Asignar solo el valor decimal sin la unidad
                    etvehiculo.DistanciaRecorrida = distancia;
                    etvehiculo.TipodeDistancia = false;
                }
            }
            else
            {
                MessageBox.Show("La distancia ingresada no es válida.", "Aviso del sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return; // Salir si la distancia no es válida
            }

            // Guardar vehículo
            Rpta = BLVehiculo.GuardarVE(EstadoGuarda, etvehiculo);
            if (Rpta == "OK")
            {
                this.ListadoMarca("%");
                MessageBox.Show("Los datos se han registrado", "Aviso del sistema", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                EstadoGuarda = 1; // Estado que indica que los datos fueron guardados correctamente

                // Limpiar campos
                TxTPlacaVehiculo.Text = "";
                TxTCliente.Text = "";
                TxTModeloVehiculo.Text = "";
                TxTModeloVehiculo.Text = "";
                TxTVINVehiculo.Text = "";
                DistanciaTxTVehiculo.Text = "";
                DistanciaTxTVehiculo.Text = ""; // Corrige el nombre del TextBox si es necesario
                IdVehiculo = 0;
            }
            else
            {
                MessageBox.Show(Rpta, "Aviso del sistema", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }


        private void ModificarVehiculo()
        {
            EstadoGuarda = 2; // Indica que se trata de una actualización

            // Verificar si hay una fila seleccionada en el DataGridView
            if (DgvLClVh.CurrentRow != null)
            {
                // Obtener el IDCliente de la fila seleccionada
                this.IdVehiculo = Convert.ToInt32(DgvLClVh.CurrentRow.Cells["IDVehiculo"].Value);

                // Poblar los campos con los datos actuales del cliente
                TxTCliente.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["IDCliente"].Value);
                TxTModeloVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["Modelo"].Value);
                TxTPlacaVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["Placa"].Value);
                TxTModeloVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["IDMarca"].Value);
                DistanciaTxTVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["Anno"].Value);
                TxTVINVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["VIN"].Value);
                DistanciaTxTVehiculo.Text = Convert.ToString(DgvLClVh.CurrentRow.Cells["DistanciaRecorrida"].Value);

                // Establecer el enfoque en el primer campo editable
                txtNuevoNombreTrabajador.Focus();
            }
            else
            {
                MessageBox.Show("Seleccione un Trabaajador para modificar.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }



        //PRODUCTO//

        private void FormatoPR()
        {
            DGVProducto.Columns[0].Width = 90;
            DGVProducto.Columns[0].HeaderText = "IDProducto";
            DGVProducto.Columns[1].Width = 240;
            DGVProducto.Columns[1].HeaderText = "Descripcion";
            DGVProducto.Columns[2].Width = 150;
            DGVProducto.Columns[2].HeaderText = "Stock_Actual";
            DGVProducto.Columns[3].Width = 120;
            DGVProducto.Columns[3].HeaderText = "Precio";
        }


        private void ListadoPR(string cTexto)
        {
            try
            {
                DGVProducto.DataSource = BLProducto.ListadoPR(cTexto);
                this.FormatoPR();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            finally
            {

            }
        }

        private void BotonesProducto(bool LEstado)
        {
            this.BTGuardarProducto.Enabled = LEstado;
            this.BTEliminararProducto.Enabled = LEstado;
            this.BTModificarProducto.Enabled = LEstado;
            this.BTBusarProducto.Enabled = LEstado;
            this.BTCancelarProducto.Enabled = LEstado;
        }



        private void ActualizarProducto()
        {

            GuardarProducto();
            this.ListadoPR("%");
        }


        private void ModificarProducto()
        {

            EstadoGuarda = 2;

            if (DGVProducto.CurrentRow != null)
            {

                this.IDProducto = Convert.ToInt32(DGVProducto.CurrentRow.Cells["IDProducto"].Value);


                TBDescripcionProducto.Text = Convert.ToString(DGVProducto.CurrentRow.Cells["Descripcion"].Value);
                TBPrecioProducto.Text = Convert.ToString(DGVProducto.CurrentRow.Cells["Precio"].Value);
                TBStockProducto.Text = Convert.ToString(DGVProducto.CurrentRow.Cells["Stock_Actual"].Value);
                TBDescripcionServicio.Focus();

            }

            else
            {

                MessageBox.Show("Seleccione un producto para modificar", "Aviso del sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void GuardarProducto()
        {

            if (TBDescripcionProducto.Text == String.Empty)
            {


                MessageBox.Show("Descripcion del producto requerida(*)", "Aviso del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            if (TBPrecioProducto.Text == String.Empty)
            {


                MessageBox.Show("Precio del producto requerido(*)", "Aviso del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            if (TBStockProducto.Text == String.Empty)
            {


                MessageBox.Show("Stock del producto requerido(*)", "Aviso del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {

                ETProducto eTProducto = new ETProducto();

                String Rpta = "";
                eTProducto.IDProducto = this.IDProducto;


                eTProducto.Descripcion = TBDescripcionProducto.Text.Trim();
                eTProducto.Precio = float.Parse(TBPrecioProducto.Text.Trim()); // Convierte el precio
                eTProducto.StockActual = int.Parse(TBStockProducto.Text.Trim()); // Convierte el stock

                try
                {

                    Rpta = BLProducto.GuardarPR(EstadoGuarda, eTProducto);

                    if (Rpta == "OK")
                    {

                        MessageBox.Show("Los datos se han registrado", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        EstadoGuarda = 1;
                        this.BotonesProducto(true);

                        TBDescripcionProducto.Text = "";
                        TBPrecioProducto.Text = "";
                        TBStockProducto.Text = "";
                        this.IDProducto = 0;
                    }


                    else
                    {
                        MessageBox.Show("No se logró registrar el dato: " + Rpta, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void BTCancelarProducto_Click(object sender, EventArgs e)
        {

            this.BotonesProducto(true);
            TBDescripcionProducto.Text = "";
            TBPrecioProducto.Text = "";
            TBStockProducto.Text = "";

        }

        private void BTGuardarProducto_Click(object sender, EventArgs e)
        {
            if (EstadoGuarda == 2)
            {
                ActualizarProducto();
            }

            else if (EstadoGuarda == 1)
            {
                GuardarProducto();
            }

            else
            {
                MessageBox.Show("El estado de guardado no está definido.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void BTBusarProducto_Click(object sender, EventArgs e)
        {
            this.ListadoPR(TBBuscarProducto.Text.Trim());
        }

        private void BTModificarProducto_Click(object sender, EventArgs e)
        {
            ModificarProducto();
        }

        private void BTEliminararProducto_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Convert.ToString(DGVProducto.CurrentRow.Cells["IDProducto"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
            }
            else
            {
                DialogResult opcion;
                //preguntar si se quiere realizar procedimiento y a opcion se le asignael valor de la respuesta
                opcion = MessageBox.Show("¿Está seguro de eliminar el registro seleccionado?", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                   MessageBoxIcon.Question);
                if (opcion == DialogResult.Yes)
                {
                    String Rpta = "";

                    this.IDProducto = Convert.ToInt32(DGVProducto.CurrentRow.Cells["IDProducto"].Value);

                    Rpta = BLProducto.EliminaPR(this.IDProducto);


                    if (Rpta.Equals("OJ"))
                    {

                        this.IDProducto = 0;
                        MessageBox.Show("Registro Eliminado", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Exclamation);
                    }
                }
            }
        }


        ////////////////////////////////////////FACTURAAAAA/////////////////////////////////////////

        private void Calcular_Totales()
        {
            decimal nCantidad = 0;
            decimal nSubtotal = 0;
            decimal nIva = 0;
            decimal nPrecio = 0;
            decimal nTotal = 0;

            if (DgvFacturaProducto.Rows.Count == 0)
            {
                nSubtotal = 0;
                nIva = 0;
                nTotal = 0;
            }
            else
            {
                // Recorrer todas las filas del DataGridView
                foreach (DataGridViewRow FilaTemp in DgvFacturaProducto.Rows)
                {
                    if (FilaTemp.Cells["Precio"].Value != null && FilaTemp.Cells["Cantidad"].Value != null)
                    {
                        // Sumar precios y cantidades
                        nPrecio = Convert.ToDecimal(FilaTemp.Cells["Precio"].Value); // Obtener el precio
                        nCantidad = Convert.ToDecimal(FilaTemp.Cells["Cantidad"].Value); // Obtener la cantidad

                        // Calcular el subtotal de la fila (precio * cantidad)
                        nSubtotal += nPrecio * nCantidad;
                    }
                }

                // Calcular el IVA (13%)
                nIva = nSubtotal * 0.13m;

                // Calcular el total (subtotal + IVA)
                nTotal = nSubtotal + nIva;

                // Actualizar los TextBox con los resultados redondeados a 2 decimales
                TxtSubtotal.Text = decimal.Round(nSubtotal, 2).ToString("#0.00");
                TxtIVA.Text = "13%";// Aquí mostrarás el valor de IVA
                textBox5.Text= decimal.Round(nIva, 2).ToString("#0.00");
                TxtTotal.Text = decimal.Round(nTotal, 2).ToString("#0.00");
            }
        }


        //CLIENTE//

        private void BtnRetornar2_Click(object sender, EventArgs e)
        {
            PnlListaCL.Visible = false;

        }

        private void DgvListaCL_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica si se hizo clic en una celda válida y en la columna del IDCliente
            if (e.RowIndex >= 0 && DgvListaCL.Columns[e.ColumnIndex].Name == "IDCliente")
            {
                // Obtener la fila seleccionada.
                DataGridViewRow row = DgvListaCL.Rows[e.RowIndex];

                if (row != null)
                {
                    string idClienteSeleccionado = row.Cells["IDCliente"].Value.ToString();

                    // Verificar si el cliente ya está seleccionado (ya está en el TextBox de cédula)
                    if (TxtCedulaCliente.Text == idClienteSeleccionado)
                    {
                        MessageBox.Show("Este cliente ya ha sido seleccionado.", "Aviso");
                        return; // Salir para evitar agregarlo dos veces
                    }

                    // Asignar los valores de la fila a los TextBox correspondientes.
                    TxtCedulaCliente.Text = idClienteSeleccionado;
                    TxtNombreCliente.Text = row.Cells["Nombre"].Value.ToString();
                    TxtTelefonoCliente.Text = row.Cells["Telefono"].Value.ToString();
                    TxtEmailCliente.Text = row.Cells["Correo"].Value.ToString();

                    // Ocultar el panel de lista de clientes
                    PnlListaCL.Visible = false;
                }
                else
                {
                    MessageBox.Show("No se pudo obtener la fila seleccionada.", "Error");
                }
            }
            else
            {
                MessageBox.Show("Haga clic en el IDCliente para seleccionar el cliente.", "Aviso");
            }

        }

        private void BtnBuscar2_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamar al método ListadoCL y asignar el resultado al DataGridView
                DgvListaCL.DataSource = BLCliente.ListadoCL(TxtListaCL.Text);

                // Verificar si se han cargado los datos
                if (DgvListaCL.Rows.Count == 0)
                {

                    MessageBox.Show("No se encontraron clientes.");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }

        }

        private void BtnLupa1_Click(object sender, EventArgs e)
        {
            this.PnlListaCL.Location = TxtEmailCliente.Location;
            this.PnlListaCL.Visible = true;

        }

        //TRABAJADOR//
        private void btnRetornar_encargado_Click(object sender, EventArgs e)
        {
            PnEncargado.Visible = false;

        }

        private void btnBuscar_encargado_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamar al método ListadoCL y asignar el resultado al DataGridView
                DGVEncargado.DataSource = BLTrabajador.ListadoTR(TxtListaCL.Text);

                // Verificar si se han cargado los datos
                if (DGVEncargado.Rows.Count == 0)
                {

                    MessageBox.Show("No se encontraron clientes.");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }

        }

        private void DGVEncargado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica si se hizo clic en una celda válida y en la columna del IDCliente
            if (e.RowIndex >= 0 && DGVEncargado.Columns[e.ColumnIndex].Name == "IDTrabajador")
            {
                // Obtener la fila seleccionada.
                DataGridViewRow row = DGVEncargado.Rows[e.RowIndex];

                if (row != null)
                {
                    // Muestra los valores de la fila seleccionada.
                    MessageBox.Show("Trabajador seleccionado: " + row.Cells["IDTrabajador"].Value.ToString());

                    // Asignar los valores de la fila a los TextBox correspondientes.
                    textEncargado.Text = row.Cells["Nombre"].Value.ToString();


                    // Ocultar el panel de lista de clientes
                    PnEncargado.Visible = false;
                }
                else
                {
                    MessageBox.Show("No se pudo obtener la fila seleccionada.", "Error");
                }
            }
            else
            {
                MessageBox.Show("Haga clic en el IDTrabajador para seleccionar el trabajador.", "Aviso");
            }

        }

        private void BtnBuscarEncargado_Click(object sender, EventArgs e)
        {
            this.PnEncargado.Visible = true;
        }

        //DETALLE
        private void BtnLupaPR_Click(object sender, EventArgs e)
        {
            this.PnlListaPR.Visible = true;
        }
        // Función que actualiza el subtotal total de todos los productos
        private void ActualizarSubtotal()
        {
            decimal subtotal = 0;

            // Iterar sobre todas las filas en DgvFacturaProducto para calcular el subtotal total.
            foreach (DataGridViewRow row in DgvFacturaProducto.Rows)
            {
                if (row.Cells[2].Value != null && row.Cells[3].Value != null)
                {
                    decimal precio = Convert.ToDecimal(row.Cells[2].Value);
                    int cantidad = Convert.ToInt32(row.Cells[3].Value);
                    subtotal += precio * cantidad;
                }
            }

            // Mostrar el subtotal actualizado en TxtSubtotal.
            TxtSubtotal.Text = subtotal.ToString("N2"); // Formato con 2 decimales
            Calcular_Totales();
        }

        // Evento cuando cambia el valor de la cantidad en DgvFacturaProducto
        private void DgvFacturaProducto_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si el cambio fue en la columna de cantidad (en este caso la columna 3).
            if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                // Actualizar el subtotal cada vez que cambie la cantidad.
                ActualizarSubtotal();
            }
        }

        // Evento para detectar cuando el usuario termina de editar una celda.
        private void DgvFacturaProducto_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Al terminar de editar, recalcular el subtotal si es necesario.
            if (e.ColumnIndex == 3) // Verificar si fue en la columna de cantidad
            {
                ActualizarSubtotal();
            }
        }
        private void DGVProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && DGVProductos.Columns[e.ColumnIndex].Name == "IDProducto") // Asegúrate que sea la columna correcta
            {
                // Obtener la fila seleccionada.
                DataGridViewRow row = DGVProductos.Rows[e.RowIndex];

                if (row != null)
                {
                    // Verificar si las celdas existen y tienen datos válidos
                    if (row.Cells["IDProducto"].Value != null && row.Cells["Descripcion"].Value != null && row.Cells["Precio"].Value != null)
                    {
                        string idProducto = row.Cells["IDProducto"].Value.ToString();
                        string descripcion = row.Cells["Descripcion"].Value.ToString();
                        decimal precio = Convert.ToDecimal(row.Cells["Precio"].Value);

                        // Verificar si el producto ya está en DgvFacturaProducto
                        bool productoEncontrado = false;
                        foreach (DataGridViewRow facturaRow in DgvFacturaProducto.Rows)
                        {
                            if (facturaRow.Cells["IDProducto"].Value != null && facturaRow.Cells["IDProducto"].Value.ToString() == idProducto)
                            {
                                // Si el producto ya está, aumentar la cantidad
                                int cantidadActual = Convert.ToInt32(facturaRow.Cells["Cantidad"].Value);
                                facturaRow.Cells["Cantidad"].Value = cantidadActual + 1;

                                // Actualizar el subtotal y detener el proceso
                                ActualizarSubtotal();
                                productoEncontrado = true;
                                break;
                            }
                        }

                        // Si el producto no está en la factura, añadirlo como una nueva fila
                        if (!productoEncontrado)
                        {
                            DataGridViewRow newRow = new DataGridViewRow();
                            newRow.CreateCells(DgvFacturaProducto);

                            // Verifica si las columnas existen antes de asignar valores
                            if (DgvFacturaProducto.Columns["IDProducto"] != null &&
                                DgvFacturaProducto.Columns["Descripcion"] != null &&
                                DgvFacturaProducto.Columns["Precio"] != null &&
                                DgvFacturaProducto.Columns["Cantidad"] != null)
                            {
                                newRow.Cells[0].Value = idProducto;   // IDProducto
                                newRow.Cells[1].Value = descripcion;  // Descripción
                                newRow.Cells[2].Value = precio;       // Precio
                                newRow.Cells[3].Value = 1;            // Cantidad inicial 1

                                DgvFacturaProducto.Rows.Add(newRow);

                                // Calcular el nuevo subtotal
                                ActualizarSubtotal();
                                PnlListaPR.Visible = false;
                            }
                            else
                            {
                                MessageBox.Show("Error: Las columnas del DataGridView no están correctamente definidas.", "Error");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener los datos de la fila seleccionada. Verifique que las columnas existan y tengan valores válidos.", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo obtener la fila seleccionada.", "Error");
                }
            }
            else
            {
                MessageBox.Show("Haga clic en el IDProducto para seleccionar el producto.", "Aviso");
            }

        }

        private void BtnRetornarPrListado_Click(object sender, EventArgs e)
        {
            this.PnlListaPR.Visible = false;

        }

        private void BtnBuscarPrlistado_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamar al método ListadoCL y asignar el resultado al DataGridView
                DGVProductos.DataSource = BLProducto.ListadoPR(TxtProductos.Text);

                // Verificar si se han cargado los datos
                if (DGVProductos.Rows.Count == 0)
                {

                    MessageBox.Show("No se encontraron clientes.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }

        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {
            PnlListaPR.Visible = false;
            PnlListaCL.Visible = false;
            PnEncargado.Visible = false;
            PnlListaVE.Visible = false;
            PnlCliente.Visible = false;
            PNLMARC.Visible = false;


            //producto//

            // Crear las columnas del DgvFacturaProducto si aún no existen
            if (DgvFacturaProducto.Columns.Count == 0)
            {
                // Agregar la columna para el ID del producto.
                DgvFacturaProducto.Columns.Add("IDProducto", "ID Producto");

                // Agregar la columna para la descripción del producto.
                DgvFacturaProducto.Columns.Add("Descripcion", "Descripción");

                // Agregar la columna para el precio del producto.
                DgvFacturaProducto.Columns.Add("Precio", "Precio");

                DgvFacturaProducto.Columns.Add("Cantidad", "Cantidad");
            }
        }

        private void BtnEliminarProducto_Click(object sender, EventArgs e)
        {
            if (DGVProductos.CurrentRow != null)
            {
                // Obtener el ID del producto de la fila seleccionada
                string idProducto = DGVProductos.CurrentRow.Cells["IDProducto"].Value.ToString();

                // Buscar la fila en DgvFacturaProducto por el IDProducto
                foreach (DataGridViewRow row in DgvFacturaProducto.Rows)
                {
                    if (row.Cells["IDProducto"].Value.ToString() == idProducto)
                    {
                        // Eliminar la fila si coincide el IDProducto
                        DgvFacturaProducto.Rows.Remove(row);
                        MessageBox.Show("Producto eliminado de la factura.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // Salir del bucle una vez eliminado
                    }
                }

                // Si no se encontró la fila, mostrar un mensaje
                MessageBox.Show("El producto no está en la factura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnBuscarVE_Click(object sender, EventArgs e)
        {
            this.PnlListaVE.Location = TxtAnnoVehiculoFactura.Location;
            this.PnlListaVE.Visible = true;

        }

        private void btnBuscar_Ve_Click(object sender, EventArgs e)
        {
            try
            {
                DgvListaVE.DataSource = BLVehiculo.ListadoVE(TxtListaVE.Text);

                if (DgvListaVE.Rows.Count == 0)
                {
                    MessageBox.Show("No se logro encontrar ningun vehiculo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }

        }

        private void DgvListaVE_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && DgvListaVE.Columns[e.ColumnIndex].Name == "IDVehiculo")
            {
                DataGridViewRow row = DgvListaCL.Rows[e.RowIndex];

                if (row != null)
                {
                    MessageBox.Show("Vehiculo seleccionado: " + row.Cells["IDVehiculo"].Value.ToString());

                    TxtPlacaVehiculoFactura.Text = row.Cells["Placa"].Value.ToString();
                    TxtMarcaVehiculoFactura.Text = row.Cells["Marca"].Value.ToString();
                    TxtAnnoVehiculoFactura.Text = row.Cells["Anno"].Value.ToString();
                    TxtDistanciaVehiculoFactura.Text = row.Cells["Distancia"].Value.ToString();

                    PnlListaCL.Visible = false;
                }
                else
                {
                    MessageBox.Show("No se pudo obtener la fila seleccionada.", "Error");
                }
            }
            else
            {
                MessageBox.Show("Haga clic en la Placa del Vehiculo para seleccionar el cliente.", "Aviso");
            }
        }

        private void btnRetornar_VE_Click(object sender, EventArgs e)
        {
            PnlListaVE.Visible = false;

        }

        //BUSCAR FACTURA
        private void Botonesfactura(bool LEstado)
        {
            this.btnBuscarFactura.Enabled = LEstado;

        }
        private void FormatoFA()
        {
            if (DgvBuscarFactura.Columns.Count >= 3) // Verifica si hay al menos 6 columnas
            {
                DgvCliente.Columns[0].Width = 100;
                DgvCliente.Columns[0].HeaderText = "Fecha";
                DgvCliente.Columns[1].Width = 100;
                DgvCliente.Columns[1].HeaderText = "Nombre";
                DgvCliente.Columns[5].Width = 100;
                DgvCliente.Columns[5].HeaderText = "Estado";
            }

        }
        private void ListadoFA(string tTexto)
        {
            try
            {
                DgvBuscarFactura.DataSource = BLFactura.ListadoFA(tTexto);
                this.FormatoFA();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            //datasource nos dice de donde vamos a consumir los datos
        }

        private void btnBuscarFactura_Click(object sender, EventArgs e)
        {
            this.ListadoFA(txtBuscarFactura.Text.Trim());
        }

        private void ckbBuscarNombreFactura_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoFA(txtBuscarFactura.Text.Trim());
        }

        private void ckbBuscarEstadoFactura_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoFA(txtBuscarFactura.Text.Trim());
        }

        private void ckbBuscarFechaFactura_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoFA(txtBuscarFactura.Text.Trim());

        }

        private void DgvBuscarFactura_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && DgvBuscarFactura.Columns[e.ColumnIndex].Name == "IDFactura")
            {
                DataGridViewRow row = DgvBuscarFactura.Rows[e.RowIndex];

                if (row != null)
                {
                    MessageBox.Show("Factura seleccionada: " + row.Cells["IDFactura"].Value.ToString());

                    TxtCedulaCliente.Text = row.Cells["Cedula"].Value.ToString();
                    TxtNombreCliente.Text = row.Cells["Nombre"].Value.ToString();
                    TxtTelefonoCliente.Text = row.Cells["Telefono"].Value.ToString();
                    TxtEmailCliente.Text = row.Cells["Email"].Value.ToString();

                    comboBoxEstado.Text = row.Cells["Estado"].Value.ToString();
                    dateTimePicker1.Text = row.Cells["Fecha"].Value.ToString();
                    textEncargado.Text = row.Cells["Anno"].Value.ToString();

                    TxtPlacaVehiculoFactura.Text = row.Cells["Placa"].Value.ToString();
                    TxtMarcaVehiculoFactura.Text = row.Cells["Marca"].Value.ToString();
                    TxtAnnoVehiculoFactura.Text = row.Cells["Anno"].Value.ToString();
                    TxtDistanciaVehiculoFactura.Text = row.Cells["Distancia"].Value.ToString();

                    TxtSubtotal.Text = row.Cells["Subtotal"].Value.ToString();
                    TxtIVA.Text = row.Cells["IVA"].Value.ToString();
                    textBox5.Text = row.Cells["Descuento"].Value.ToString();
                    TxtTotal.Text = row.Cells["total"].Value.ToString();

                    DgvFacturaProducto.CurrentRow.Cells["Nombre"].Value = DgvBuscarFactura.CurrentRow.Cells["Nombre"].Value;
                }
                else
                {
                    MessageBox.Show("No se pudo obtener la fila seleccionada.", "Error");
                }
            }
        }

        private void buttonGuardarVehiculo_Click(object sender, EventArgs e)
        {

            if (EstadoGuarda == 2)
            {
                // Actualización del cliente
                ActualizarVehiculo();
            }
            else if (EstadoGuarda == 1)
            {
                // Inserción de nuevo cliente
                GuardarVehiculo();
            }
            else
            {
                MessageBox.Show("El estado de guardado no está definido.", "Aviso del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void buttonBuscarVehiculo_Click(object sender, EventArgs e)
        {
            this.ListadoVe(TxTBuscarVehiculo.Text.Trim());
        }

        private void buttonModificarVehiculo_Click(object sender, EventArgs e)
        {
            ModificarVehiculo();

        }

        private void chkplacavehiculo_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoVe(TxTBuscarVehiculo.Text.Trim());

        }

        private void chkceduvehi_CheckedChanged(object sender, EventArgs e)
        {
            this.ListadoVe(TxTBuscarVehiculo.Text.Trim());

        }

        private void buttonEliminarVehiculo_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(Convert.ToString(DgvLClVh.CurrentRow.Cells["IDVehiculo"].Value)))
            {
                MessageBox.Show("No hay datos que mostar", "Aviso del Sistema", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
            else
            {
                DialogResult opcion;
                //preguntar si se quiere realizar procedimiento y a opcion se le asignael valor de la respuesta
                opcion = MessageBox.Show("¿Está seguro de eliminar el registro seleccionado?", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                   MessageBoxIcon.Question);
                if (opcion == DialogResult.Yes)
                {
                    String Rpta = "";
                    //convertir a int
                    this.IdVehiculo = Convert.ToInt32(DgvLClVh.CurrentRow.Cells["IDVehiculo"].Value);
                    Rpta = BLVehiculo.EliminaVE(this.IdVehiculo);

                    if (Rpta.Equals("OK"))
                    {
                        this.IDTrabajador = 0;
                        MessageBox.Show("Registro Eliminado", "Aviso del Sistema", MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Exclamation);

                    }
                }

            }
        }

        private void buttonCancelarVehiculo_Click(object sender, EventArgs e)
        {
            this.IdVehiculo = 0;
            TxTPlacaVehiculo.Text = "";
            TxTVINVehiculo.Text = "";
            TxTModeloVehiculo.Text = "";
            DistanciaTxTVehiculo.Text = "0";
        }

        private void BtnCliente_Click(object sender, EventArgs e)
        {
            this.PnlCliente.Visible = true;
        }

        private void BtnMarcas_Click(object sender, EventArgs e)
        {
            this.PNLMARC.Visible = true;
        }

        private void BuscarClientes_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamar al método ListadoCL y asignar el resultado al DataGridView
                PnlCV.DataSource = BLCliente.ListadoCL(TxtListaCL.Text);

                // Verificar si se han cargado los datos
                if (DgvLClVh.Rows.Count == 0)
                {

                    MessageBox.Show("No se encontraron clientes.");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        private void CancelarCLI_Click(object sender, EventArgs e)
        {
            PnlCliente.Visible = false;
        }

        private void BuscarMarcas_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamar al método ListadoCL y asignar el resultado al DataGridView
                DgvMVc.DataSource = BLMarca.ListadoMA(TxTModeloVehiculo.Text);

                // Verificar si se han cargado los datos
                if (DgvLClVh.Rows.Count == 0)
                {

                    MessageBox.Show("No se encontraron clientes.");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        private void CancelarMa_Click(object sender, EventArgs e)
        {
            PNLMARC.Visible = false;
        }

      

        private void DgvMVc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && DgvMVc.Columns[e.ColumnIndex].Name == "IDMarca")
            {
                // Obtener la fila seleccionada.
                DataGridViewRow row = DgvMVc.Rows[e.RowIndex];

                if (row != null)
                {
                    // Muestra los valores de la fila seleccionada.
                    MessageBox.Show("Marca seleccionado: " + row.Cells["IDMarca"].Value.ToString());

                    // Asignar los valores de la fila a los TextBox correspondientes.
                    selecionMa =int.Parse(row.Cells["IDmarca"].Value.ToString());
                    TxTModeloVehiculo.Text = row.Cells["IDMarca"].Value.ToString();

                    
                    // Ocultar el panel de lista de clientes
                    DgvMVc.Visible = false;
                }
                else
                {
                    MessageBox.Show("No se pudo obtener la fila seleccionada.", "Error");
                }
            }
            else
            {
                MessageBox.Show("Haga clic en el IDCliente para seleccionar el cliente.", "Aviso");
            }
        }

        private void PnlCV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            // Verifica si se hizo clic en una celda válida y en la columna del IDCliente
            // Verifica si se hizo clic en una celda válida
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < PnlCV.Columns.Count)
            {
                // Verifica si es la columna del IDCliente
                if (PnlCV.Columns[e.ColumnIndex].Name == "IDCliente")
                {
                    // Obtener la fila seleccionada.
                    DataGridViewRow row = PnlCV.Rows[e.RowIndex];

                    if (row != null)
                    {
                        string idClienteSeleccionado = row.Cells["IDCliente"].Value.ToString();

                        // Verificar si el cliente ya está seleccionado
                        if (TxtCedulaCliente.Text == idClienteSeleccionado)
                        {
                            MessageBox.Show("Este cliente ya ha sido seleccionado.", "Aviso");
                            return; // Salir para evitar agregarlo dos veces
                        }

                        // Asignar los valores de la fila a los TextBox correspondientes.
                        TxTCliente.Text = row.Cells["Nombre"].Value.ToString();
                        seleccionCl = int.Parse(row.Cells["IDCliente"].Value.ToString());
                        // Ocultar el panel de lista de clientes
                        PnlCliente.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener la fila seleccionada.", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("Haga clic en el IDCliente para seleccionar el cliente.", "Aviso");
                }
            }
            else
            {
                MessageBox.Show("El índice de la fila o columna está fuera de rango.", "Error");
            }
        }

        
    }
}







