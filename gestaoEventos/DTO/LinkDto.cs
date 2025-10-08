namespace APIMongoDB.DTO
{
    // Classe que representa um link HATEOAS (Hypermedia as the Engine of Application State)
    // Cada instância desta classe descreve uma ação possível relacionada ao recurso principal (GET, PUT, DELETE, etc.)
    public class LinkDto
    {
        public string Href { get; set; } = string.Empty;  // URL para acessar o recurso
        public string Rel { get; set; } = string.Empty;   // Tipo de relação (ex: "self", "update", "delete")
        public string Method { get; set; } = string.Empty; // Método HTTP associado (ex: "GET", "POST", "PUT", "DELETE")
    }
}
