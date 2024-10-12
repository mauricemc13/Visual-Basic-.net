Imports System.Data.SqlClient
Public Class Form1
    ' Define la cadena de conexión
    Private connectionString As String = "Server=DESKTOP-49B259R\SQLEXPRESS;Database=conexion1;Integrated Security=True"
    Private connection As SqlConnection
    Private Sub LimpiarCampos()
        txtNombre.Clear()
        txtPrecio.Clear()
        txtCantidad.Clear()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Inicializar la conexión
        connection = New SqlConnection(connectionString)

        Try
            ' Abrir la conexión a SQL Server
            connection.Open()
            MessageBox.Show("Conexión establecida con éxito.")
        Catch ex As Exception
            ' Mostrar mensaje en caso de error
            MessageBox.Show("Error al conectar: " & ex.Message)
        Finally
            ' Cerrar la conexión
            connection.Close()
        End Try
        CargarDatos()
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        ' Verificar que los campos no estén vacíos
        If txtNombre.Text <> "" AndAlso txtPrecio.Text <> "" AndAlso txtCantidad.Text <> "" Then
            Try
                connection.Open()
                ' Crear comando SQL para insertar el producto sin la columna id
                Dim query As String = "INSERT INTO productos (nombre, precio, cantidad) VALUES (@nombre, @precio, @cantidad)"
                Dim command As New SqlCommand(query, connection)
                ' Agregar parámetros
                command.Parameters.AddWithValue("@nombre", txtNombre.Text)
                command.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text))
                command.Parameters.AddWithValue("@cantidad", Convert.ToInt32(txtCantidad.Text))
                ' Ejecutar el comando
                command.ExecuteNonQuery()
                MessageBox.Show("Producto agregado con éxito.")
                LimpiarCampos() ' Limpiar los campos después de agregar
            Catch ex As Exception
                MessageBox.Show("Error al agregar el producto: " & ex.Message)
            Finally
                connection.Close()
            End Try
            CargarDatos()
        Else
            MessageBox.Show("Por favor, completa todos los campos.")
        End If
    End Sub
    Private Sub CargarDatos()
        Try
            connection.Open()
            ' Crear comando SQL para seleccionar todos los productos
            Dim query As String = "SELECT * FROM productos"
            Dim adapter As New SqlDataAdapter(query, connection)
            Dim table As New DataTable()
            ' Llenar el DataGridView con los datos
            adapter.Fill(table)
            dataGridViewProductos.DataSource = table
        Catch ex As Exception
            MessageBox.Show("Error al cargar los productos: " & ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub

    Private Sub btnLeer_Click(sender As Object, e As EventArgs) Handles btnLeer.Click
        CargarDatos()
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        If dataGridViewProductos.SelectedRows.Count = 1 Then
            Try
                connection.Open()
                ' Obtener el ID del producto seleccionado
                Dim id As Integer = Convert.ToInt32(dataGridViewProductos.SelectedRows(0).Cells("id").Value)
                ' Crear comando SQL para actualizar el producto
                Dim query As String = "UPDATE productos SET nombre = @nombre, precio = @precio, cantidad = @cantidad WHERE id = @id"
                Dim command As New SqlCommand(query, connection)
                ' Agregar parámetros
                command.Parameters.AddWithValue("@nombre", txtNombre.Text)
                command.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text))
                command.Parameters.AddWithValue("@cantidad", Convert.ToInt32(txtCantidad.Text))
                command.Parameters.AddWithValue("@id", id)
                ' Ejecutar el comando
                command.ExecuteNonQuery()
                MessageBox.Show("Producto actualizado con éxito.")
                LimpiarCampos()
            Catch ex As Exception
                MessageBox.Show("Error al actualizar el producto: " & ex.Message)
            Finally
                connection.Close()
            End Try
            CargarDatos()
        Else
            MessageBox.Show("Por favor selecciona un producto para actualizar.")
        End If
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dataGridViewProductos.SelectedRows.Count = 1 Then
            Try
                connection.Open()
                ' Obtener el ID del producto seleccionado
                Dim id As Integer = Convert.ToInt32(dataGridViewProductos.SelectedRows(0).Cells("id").Value)
                ' Crear comando SQL para eliminar el producto
                Dim query As String = "DELETE FROM productos WHERE id = @id"
                Dim command As New SqlCommand(query, connection)
                ' Agregar parámetro
                command.Parameters.AddWithValue("@id", id)
                ' Ejecutar el comando
                command.ExecuteNonQuery()
                MessageBox.Show("Producto eliminado con éxito.")
                LimpiarCampos() ' Limpiar los campos después de eliminar
            Catch ex As Exception
                MessageBox.Show("Error al eliminar el producto: " & ex.Message)
            Finally
                connection.Close()
            End Try
            CargarDatos()
        Else
            MessageBox.Show("Por favor selecciona un producto para eliminar.")
        End If
    End Sub

    Private Sub dataGridViewProductos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataGridViewProductos.CellContentClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dataGridViewProductos.Rows(e.RowIndex)
            txtNombre.Text = row.Cells("nombre").Value.ToString()
            txtPrecio.Text = row.Cells("precio").Value.ToString()
            txtCantidad.Text = row.Cells("cantidad").Value.ToString()
        End If
    End Sub


End Class
