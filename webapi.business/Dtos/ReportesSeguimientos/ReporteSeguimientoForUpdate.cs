namespace webapi.business.Dtos.ReportesSeguimientos
{
    public class ReporteSeguimientoForUpdate
    {
        public int Id { get; set; }
        public string Observaciones { get; set; }
        public int SeguimientoId { get; set; }
        public string Estado { get; }
        public ReporteSeguimientoForUpdate()
        {
            Estado = "Enviado";
        }
    }
}
