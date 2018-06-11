using DemoAdoNet.Entities;
using System;
using System.Collections.Generic;

namespace DemoAdoNet.Interface
{
    public interface ILogic
    {

        List<Alumno> ListarAlumnos();

        List<Calificacion> ListarNotas();

        List<Calificacion> ObtenerNotaPorAlumno(int IdAlumno);

        Alumno ObtenerAlumno(string codigoAlumno);

        int RegistrarAlumno(Alumno alumno);

        bool ActualizarAlumno(Alumno alumno);

        bool EliminarAlumno(int idAlumno, int version);
    }
}
