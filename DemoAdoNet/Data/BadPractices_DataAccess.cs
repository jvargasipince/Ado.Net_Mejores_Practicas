using DemoAdoNet.Entities;
using DemoAdoNet.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DemoAdoNet.Data
{
    public class BadPractices_DataAccess : ILogic
    {
        string connectionString = @"Data Source = JVARGAS\SQL2014;Initial Catalog = CIBERTEC_ADO; Persist Security Info=True;User ID = sa; Password=sql";
       
        public BadPractices_DataAccess()
        {
        }
 
        public List<Alumno> ListarAlumnos()
        {
            List<Alumno> alumnos = new List<Alumno>();

            var conn = new SqlConnection(connectionString);

            conn.Open();

            string queryString =  "select * from Alumno where Estado = 1";
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, conn);
            DataSet data = new DataSet();
            adapter.Fill(data, "Alumnos");

            DataTable dataTable = data.Tables["Alumnos"];

            foreach (DataRow row in dataTable.Rows)
            {
                Alumno alumno = new Alumno()
                {
                    Id = (int)row["Id"],
                    Codigo = (string)row["Codigo"],
                    Nombre = (string)row["Nombre"],
                    Ciclo = (int)row["Ciclo"],
                    Curso = (int)row["Curso"]
                };

                alumnos.Add(alumno);
            }

            //Liberar Recursos
            conn.Close();
            adapter = null;
            data = null;

            return alumnos;

        }

        public List<Calificacion> ListarNotas()
        {
            List<Calificacion> calificaciones = new List<Calificacion>();

            var conn = new SqlConnection(connectionString);

            conn.Open();

            string queryString = "select * from Calificacion";
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, conn);
            DataSet data = new DataSet();
            adapter.Fill(data, "Calificaciones");

            DataTable dataTable = data.Tables["Calificaciones"];

            foreach (DataRow row in dataTable.Rows)
            {
                Calificacion calificacion = new Calificacion()
                {
                    IdAlumno = (int)row["IdAlumno"],
                    IdTipoEvaluacion = (int)row["IdTipoEvaluacion"],
                    Nota = (decimal)row["Nota"]
                };

                calificaciones.Add(calificacion);
            }

            //Liberar Recursos
            conn.Close();
            adapter = null;
            data = null;

            return calificaciones;
        }

        public List<Calificacion> ObtenerNotaPorAlumno(int idAlumno)
        {
            List<Calificacion> calificaciones = new List<Calificacion>();

            var conn = new SqlConnection(connectionString);

            conn.Open();

            string queryString = String.Format("select * from Calificacion" +
                    "where IdAlumno = {0}", idAlumno);

            SqlDataAdapter adapter = new SqlDataAdapter(queryString, conn);
            DataSet data = new DataSet();
            adapter.Fill(data, "Calificaciones");

            DataTable dataTable = data.Tables["Calificaciones"];

            foreach (DataRow row in dataTable.Rows)
            {
                Calificacion calificacion = new Calificacion()
                {
                    IdAlumno = (int)row["IdAlumno"],
                    IdTipoEvaluacion = (int)row["IdTipoEvaluacion"],
                    Nota = (decimal)row["Nota"]
                };

                calificaciones.Add(calificacion);
            }

            //Liberar Recursos
            conn.Close();
            adapter = null;
            data = null;

            return calificaciones;
        }


        public Alumno ObtenerAlumno(string codigoAlumno)
        {
            Alumno alumno = new Alumno();

            var conn = new SqlConnection(connectionString);

            conn.Open();

            string queryString = String.Format("select * from Alumno " +
                    "where Estado = 1 and Codigo = {0}", codigoAlumno);

            SqlDataAdapter adapter = new SqlDataAdapter(queryString, conn);
            DataSet data = new DataSet();
            adapter.Fill(data, "Alumno");

            DataTable dataTable = data.Tables["Alumno"];

            foreach (DataRow row in dataTable.Rows)
            {
                alumno = new Alumno()
                {
                    Id = (int)row["Id"],
                    Codigo = (string)row["Codigo"],
                    Nombre = (string)row["Nombre"],
                    Ciclo = (int)row["Ciclo"],
                    Curso = (int)row["Curso"],
                    Version = (int)row["Version"]
                };
            }

            //Liberar Recursos
            conn.Close();
            adapter = null;
            data = null;

            return alumno;
        }

        public int RegistrarAlumno(Alumno alumno)
        {

            int idAlumno = 0;
  
            var cmdInsertarAlumno = String.Format(
                    "INSERT INTO Alumnos (codigo, nombre, ciclo, curso) " +
                    "output INSERTED.ID" +
                    "VALUES ('{0}', '{1}', '{2}', '{3}')",
                    alumno.Codigo, alumno.Nombre, alumno.Ciclo, alumno.Curso);

            using (var command = new SqlCommand(cmdInsertarAlumno, new SqlConnection(connectionString)))
            {
                idAlumno = (int) command.ExecuteScalar();
            }
            
            foreach (var nota in alumno.Notas)
            {
                string insertCalificaciones = String.Format("insert into Calificacion (IdAlumno, IdTipoEvaluacion, Nota) " +
                "output INSERTED.ID" +
                " values ('{0}', '{1}', '{2}')", idAlumno, nota.IdTipoEvaluacion, nota.Nota);

                using (var commandCalificaciones = new SqlCommand(insertCalificaciones, new SqlConnection(connectionString)))
                {
                   int idNota = (int)commandCalificaciones.ExecuteScalar();
                }
            }

            return idAlumno;

        }

        public bool ActualizarAlumno(Alumno alumno)
        {

            bool success = false;

            string actualizaAlumno = String.Format("update Alumno" +
                                                " set Codigo = {0}, " +
                                                "Nombre = {1}, " +
                                                "Ciclo = {2}, " +
                                                "Curso = {3}" +
                                                "where" +
                                                " id = {4} ", 
                                                alumno.Codigo, 
                                                alumno.Nombre, 
                                                alumno.Ciclo,
                                                alumno.Curso,
                                                alumno.Id);

            using (var command = new SqlCommand(actualizaAlumno, new SqlConnection(connectionString)))
            {
                int filas = (int) command.ExecuteNonQuery();

                if (filas > 0)                
                    success = true;
            }

            return success;

        }

        public bool EliminarAlumno(int idAlumno, int version)
        {
            bool isSuccess = false;

            string eliminaCalificaciones = String.Format("delete from Calificacion" +
                                "where IdAlumno = {0}", idAlumno);

            using (var command = new SqlCommand(eliminaCalificaciones, new SqlConnection(connectionString)))
            {
                var rows = command.ExecuteNonQuery();
            }

            string actualizaAlumno = String.Format("update Alumno " +
                    " set Estado = 1 where id = {0}", idAlumno);

            using (var command = new SqlCommand(actualizaAlumno, new SqlConnection(connectionString)))
            {
                var rows = command.ExecuteNonQuery();
            }

            isSuccess = true;

            return isSuccess;
        }


    }
}
