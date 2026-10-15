using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace parcial2progra
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Server=DESKTOP-D2LM5MQ; Database=ParcialProgramacion1; Trusted_Connection=True;";
        private int idProductoSeleccionado = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void CargarProductos()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    string sql = "SELECT IdProducto, Nombre, Categoria, Precio, Existencias FROM dbo.Productos";
                    SqlDataAdapter da = new SqlDataAdapter(sql, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvProductos.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnConsultar_Click_1(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void BtnGuardar_Click_1(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            string sql = "INSERT INTO dbo.Productos (Nombre, Categoria, Precio, Existencias) VALUES (@Nombre, @Categoria, @Precio, @Existencias)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Categoria", txtCategoria.Text.Trim());
                    cmd.Parameters.AddWithValue("@Precio", decimal.Parse(txtPrecio.Text));
                    cmd.Parameters.AddWithValue("@Existencias", int.Parse(txtExistencias.Text));

                    try
                    {
                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("¡Producto registrado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            CargarProductos();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnActualizar_Click_1(object sender, EventArgs e)
        {
            if (idProductoSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un producto de la tabla para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarCampos()) return;

            string sql = "UPDATE dbo.Productos SET Nombre = @Nombre, Categoria = @Categoria, Precio = @Precio, Existencias = @Existencias WHERE IdProducto = @IdProducto";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@IdProducto", idProductoSeleccionado);
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Categoria", txtCategoria.Text.Trim());
                    cmd.Parameters.AddWithValue("@Precio", decimal.Parse(txtPrecio.Text));
                    cmd.Parameters.AddWithValue("@Existencias", int.Parse(txtExistencias.Text));

                    try
                    {
                        con.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("¡Producto actualizado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            CargarProductos();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnEliminar_Click_1(object sender, EventArgs e)
        {
            if (idProductoSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un producto de la tabla para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("¿Está seguro de que desea eliminar este producto?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "DELETE FROM dbo.Productos WHERE IdProducto = @IdProducto";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@IdProducto", idProductoSeleccionado);

                        try
                        {
                            con.Open();
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("¡Producto eliminado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LimpiarCampos();
                                CargarProductos();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void BtnLimpiar_Click_1(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];
                idProductoSeleccionado = Convert.ToInt32(fila.Cells["IdProducto"].Value);
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtCategoria.Text = fila.Cells["Categoria"].Value.ToString();
                txtPrecio.Text = fila.Cells["Precio"].Value.ToString();
                txtExistencias.Text = fila.Cells["Existencias"].Value.ToString();
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCategoria.Text))
            {
                MessageBox.Show("El nombre y la categoría son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("El precio debe ser un número mayor que cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtExistencias.Text, out int existencias) || existencias < 0)
            {
                MessageBox.Show("Las existencias deben ser un número entero mayor o igual a cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            idProductoSeleccionado = 0;
            txtNombre.Clear();
            txtCategoria.Clear();
            txtPrecio.Clear();
            txtExistencias.Clear();
            txtNombre.Focus();
        }
    }
}
