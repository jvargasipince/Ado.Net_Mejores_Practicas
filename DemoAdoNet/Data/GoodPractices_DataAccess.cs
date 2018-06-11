using DemoAdoNet.Entities;
using DemoAdoNet.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace DemoAdoNet.Data
{
   public class GoodPractices_DataAccess : ILogic
    {

        string connectionString = ConfigurationManager.ConnectionStrings["CibertecDB"].ToString();
        const string Estado_Activo = "1";
        const string Estado_Inactivo = "0";

        public GoodPractices_DataAccess()
        {
        }

        public List<Alumno> ListarAlumnos()
        {
            List<Alumno> alumnos = new List<Alumno>();

            using (var  conn = new SqlConnection(connectionString))
            {

                conn.Open();

                using (var command = new SqlCommand("select Id, Codigo, Nombre, Ciclo, Curso from Alumno where Estado = @activo", conn))
                {
                    command.Parameters.Add(new SqlParameter("@activo", Estado_Activo));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Alumno alumno = new Alumno()
                            {
                                Id = (int)reader["Id"],
                                Codigo = (string)reader["Codigo"],
                                Nombre = (string)reader["Nombre"],
                                Ciclo = (int)reader["Ciclo"],
                                Curso = (int)reader["Curso"]
                            };

                        alumnos.Add(alumno);

                        }
                    }
                }
            }

            return alumnos;

        }

        public List<Calificacion> ListarNotas()
        {
            List<Calificacion> calificaciones = new List<Calificacion>();

            using (var conn = new SqlConnection(connectionString))
            {

                conn.Open();

                using (var command = new SqlCommand("select IdAlumno, IdTipoEvaluacion, Nota from Calificacion", conn))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Calificacion calificacion = new Calificacion()
                            {
                                IdAlumno = (int)reader["IdAlumno"],
                                IdTipoEvaluacion = (int)reader["IdTipoEvaluacion"],
                                Nota = (reader["Nota"] == DBNull.Value) ? 0 : (decimal)reader["Nota"]
                            };

                            calificaciones.Add(calificacion);

                        }
                    }
                }

                return calificaciones;
            }
        }

        public List<Calificacion> ObtenerNotaPorAlumno(int IdAlumno)
        {
            List<Calificacion> calificaciones = new List<Calificacion>();

            using (var conn = new SqlConnection(connectionString))
            {

                conn.Open();

                using (var command = new SqlCommand("select Id, IdAlumno, IdTipoEvaluacion, Nota from Calificacion where IdAlumno = @IdAlumno", conn))
                {

                    command.Parameters.Add(new SqlParameter("@IdAlumno", IdAlumno));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Calificacion calificacion = new Calificacion()
                            {
                                Id = (int)reader["Id"],
                                IdAlumno = (int)reader["IdAlumno"],
                                IdTipoEvaluacion = (int)reader["IdTipoEvaluacion"],
                                Nota = (reader["Nota"] == DBNull.Value) ? 0 : (decimal)reader["Nota"]
                            };

                            calificaciones.Add(calificacion);

                        }
                    }
                }

                return calificaciones;
            }
        }

        public Alumno ObtenerAlumno(string codigoAlumno)
        {
            Alumno alumno = new Alumno();

            using (var conn = new SqlConnection(connectionString))
            {

                conn.Open();

                using (var command = new SqlCommand("select Id, Codigo, Nombre, Ciclo, Curso, CONVERT(INT,Version)" +
                    " Version from Alumno where Estado = 1 and Codigo = @codigoAlumno", conn))
                {
                    command.Parameters.Add(new SqlParameter("@codigoAlumno", codigoAlumno));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            alumno = new Alumno()
                            {
                                Id = (int)reader["Id"],
                                Codigo = (string)reader["Codigo"],
                                Nombre = (string)reader["Nombre"],
                                Ciclo = (int)reader["Ciclo"],
                                Curso = (int)reader["Curso"],
                                Version = (int)reader["Version"]
                            };

                            alumno.Notas = ObtenerNotaPorAlumno(alumno.Id);

                        }
                    }
                }
            }

            return alumno;
        }

        public bool ActualizarAlumno(Alumno alumno)
        {
            //Solo Cabecera

            bool isSuccess = false;

            try
            {
                //Declaramos la transaccion
                using (TransactionScope scope = new TransactionScope())
                {
                    //Establecemos la conexion
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string actualizaAlumno = "update Alumno " +
                                                " set Codigo = @Codigo, " +
                                                "Nombre = @Nombre, " +
                                                "Ciclo = @Ciclo, " +
                                                "Curso = @Curso " +
                                                "where" +
                                                " id = @Id and " +
                                                "CONVERT(INT,Version) = @Version";

                        using (var commandAlumno = new SqlCommand(actualizaAlumno, conn))
                        {
                            commandAlumno.Parameters.Add(new SqlParameter("@Codigo", alumno.Codigo));
                            commandAlumno.Parameters.Add(new SqlParameter("@Nombre", alumno.Nombre));
                            commandAlumno.Parameters.Add(new SqlParameter("@Ciclo", alumno.Ciclo));
                            commandAlumno.Parameters.Add(new SqlParameter("@Curso", alumno.Curso));
                            commandAlumno.Parameters.Add(new SqlParameter("@Id", alumno.Id));
                            commandAlumno.Parameters.Add(new SqlParameter("@Version", alumno.Version));

                            var filasActualizadas = commandAlumno.ExecuteNonQuery();

                            if (filasActualizadas > 0)
                            {
                                isSuccess = true;
                                scope.Complete();
                            }
                            else
                            {
                                scope.Dispose();
                            }
                        }

                    }

                }

                return isSuccess;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public bool EliminarAlumno(int idAlumno, int version)
        {
            bool isSuccess = false;

            try
            {
                //Declaramos la transaccion
                using (TransactionScope scope = new TransactionScope())
                {
                    //Establecemos la conexion
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string eliminaCalificaciones = "delete from Calificacion " +
                                "where IdAlumno = @IdAlumno";

                        using (var commandCalificaciones = new SqlCommand(eliminaCalificaciones, conn))
                        {

                            commandCalificaciones.Parameters.Add(new SqlParameter("@IdAlumno", idAlumno));
                            var notasEliminadas = commandCalificaciones.ExecuteNonQuery();

                            if (notasEliminadas > 0)
                            {
                                string actualizaAlumno = "update Alumno " +
                                        " set Estado = @Estado " +
                                        "where id = @Id and CONVERT(INT,Version) = @Version";

                                using (var commandAlumno = new SqlCommand(actualizaAlumno, conn))
                                {
                                    commandAlumno.Parameters.Add(new SqlParameter("@Id", idAlumno));
                                    commandAlumno.Parameters.Add(new SqlParameter("@Estado", Estado_Inactivo));
                                    commandAlumno.Parameters.Add(new SqlParameter("@Version", version));

                                    var alumnoEliminado = commandAlumno.ExecuteNonQuery();

                                    if (alumnoEliminado > 0)
                                    {
                                        isSuccess = true;
                                        scope.Complete();
                                    }
                                    else
                                    {
                                        scope.Dispose();
                                    }

                                }

                            }
                            else
                            {
                                scope.Dispose();
                            }
                        }

                    }

                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
                        
         }          

        public int RegistrarAlumno(Alumno alumno)
        {
            int idAlumno = 0;

            try
            {
                //Declaramos la transaccion
                using (TransactionScope scope = new TransactionScope())
                {
                    //Establecemos la conexion
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string insertAlumno = "insert into Alumno (Codigo, Nombre, Ciclo, Curso) " +
                                                "output INSERTED.ID" +
                                                " values (@Codigo, @Nombre, @Ciclo, @Curso)";

                        using (var commandAlumno = new SqlCommand(insertAlumno, conn))
                        {
                            commandAlumno.Parameters.Add(new SqlParameter("@Codigo", alumno.Codigo));
                            commandAlumno.Parameters.Add(new SqlParameter("@Nombre", alumno.Nombre));
                            commandAlumno.Parameters.Add(new SqlParameter("@Ciclo", alumno.Ciclo));
                            commandAlumno.Parameters.Add(new SqlParameter("@Curso", alumno.Curso));
                            
                            var valorRetornadoAlumno = commandAlumno.ExecuteScalar();
                            if (valorRetornadoAlumno != null)
                                int.TryParse(valorRetornadoAlumno.ToString(), out idAlumno);

                            if (idAlumno > 0)
                            {
                                bool IsSuccess = true;

                                string insertCalificaciones = "insert into Calificacion (IdAlumno, IdTipoEvaluacion, Nota) " +
                                        "output INSERTED.ID" +
                                        " values (@IdAlumno, @IdTipoEvaluacion, @Nota)";

                               
                                foreach (var nota in alumno.Notas)
                                {
                                    using (var commandCalificaciones = new SqlCommand(insertCalificaciones, conn))
                                    {

                                        commandCalificaciones.Parameters.Add(new SqlParameter("@IdAlumno", idAlumno));
                                        commandCalificaciones.Parameters.Add(new SqlParameter("@IdTipoEvaluacion", nota.IdTipoEvaluacion));
                                        commandCalificaciones.Parameters.Add(new SqlParameter("@Nota", nota.Nota));

                                        int idNota = 0;
                                        var valorRetornadoNota = commandCalificaciones.ExecuteScalar();
                                        if (valorRetornadoNota != null)
                                            int.TryParse(valorRetornadoNota.ToString(), out idNota);

                                        if (idNota == 0)
                                        {
                                            IsSuccess = false;
                                            break;
                                        }   
                                    }
                                }

                                if (IsSuccess)                                
                                    scope.Complete();
                                
                            }
                            else
                            {
                                scope.Dispose();
                            }
                        }

                    }

                }

                return idAlumno;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
