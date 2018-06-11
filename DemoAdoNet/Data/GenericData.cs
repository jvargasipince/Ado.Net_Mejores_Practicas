using DemoAdoNet.Entities;
using System.Collections.Generic;

namespace DemoAdoNet.Data
{
   public static class GenericData
    {
        public static List<ItemList> ListarCursos()
        {
            List<ItemList> cursos = new List<ItemList>();

            cursos.Add(new ItemList()
            {
                Id = 1,
                Descripcion = "Programación Orientada Objetos I"
            });

            cursos.Add(new ItemList()
            {
                Id = 2,
                Descripcion = "Base Datos I"
            });

            cursos.Add(new ItemList()
            {
                Id = 3,
                Descripcion = "Aplicaciones Web I"
            });

            cursos.Add(new ItemList()
            {
                Id = 4,
                Descripcion = "Análisis y Diseño Sistemas I"
            });

            return cursos;
        }

        public static List<ItemList> ListarCiclos()
        {
            List<ItemList> ciclos = new List<ItemList>();

            ciclos.Add(new ItemList() 
            {
                Id = 1,
                Descripcion = "Primero"
            });

            ciclos.Add(new ItemList()
            {
                Id = 2,
                Descripcion = "Segundo"
            });

            ciclos.Add(new ItemList()
            {
                Id = 3,
                Descripcion = "Tercero"
            });

            ciclos.Add(new ItemList()
            {
                Id = 4,
                Descripcion = "Cuarto"
            });

            ciclos.Add(new ItemList()
            {
                Id = 5,
                Descripcion = "Quinto"
            });

            ciclos.Add(new ItemList()
            {
                Id = 6,
                Descripcion = "Sexto"
            });

            return ciclos;
        }

        public static  List<Evaluacion> ListarEvaluaciones()
        {
            List<Evaluacion> evaluaciones = new List<Evaluacion>();

            evaluaciones.Add(new Evaluacion
            {
                Id = 1,
                Tipo = "Evaluación Continua 1",
                Peso = 0.1m
            });

            evaluaciones.Add(new Evaluacion
            {
                Id = 2,
                Tipo = "Evaluación Continua 2",
                Peso = 0.15m
            });

            evaluaciones.Add(new Evaluacion
            {
                Id = 3,
                Tipo = "Evaluación Continua 3",
                Peso = 0.15m
            });

            evaluaciones.Add(new Evaluacion
            {
                Id = 4,
                Tipo = "Parcial",
                Peso = 0.2m
            });

            evaluaciones.Add(new Evaluacion
            {
                Id = 5,
                Tipo = "Final",
                Peso = 0.4m
            });

            return evaluaciones;

        }
    }
}
