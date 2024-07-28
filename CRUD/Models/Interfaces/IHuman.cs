namespace CRUD.Models.Interfaces
{
    public interface IHuman
    {
        byte Edad { get; set; }
        int Id { get; set; }
        int IdTipoIdentificacion { get; set; }
        string Nombre { get; set; }
        string NumeroIdentificacion { get; set; }
    }
}