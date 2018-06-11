namespace DemoAdoNet.Entities
{
    public class Calificacion
    {
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public int IdTipoEvaluacion { get; set; }             
        public decimal Nota { get; set; }

    }
}
