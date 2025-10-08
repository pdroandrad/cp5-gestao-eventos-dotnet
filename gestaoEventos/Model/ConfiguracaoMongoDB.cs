namespace APIMongoDB.Model
{
    public class ConfiguracaoMongoDB
    {
        public string StringConexao { get; set; } = null!;
        public string NomeBanco { get; set; } = null!;
        public string NomeColecaoAlunos { get; set; } = "Alunos";
    }
}