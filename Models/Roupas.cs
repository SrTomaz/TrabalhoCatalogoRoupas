using Dapper;
using Microsoft.AspNetCore.Hosting.Server;
using System.Data;
using System.Data.SqlClient;

namespace CatalogoRoupas.Models
{
    public class Roupa
    {

        public Roupa()
        {
        }

        public Roupa(int id, string? nome, string? tamanho, string? cor, decimal preco)
        {
            idRoupa = id;
            nomePeca = nome;
            tamanhoPeca = tamanho;
            corpeca = cor;
            valorPeca = preco;
        }


        public Roupa(string? nome, string? tamanho, string? cor, decimal preco)
        {           
            nomePeca = nome;
            tamanhoPeca = tamanho;
            corpeca = cor;
            valorPeca = preco;
        }



        public int idRoupa { get; set; }
        public string? nomePeca { get; set; }
        public string? tamanhoPeca { get; set; }
        public string? corpeca { get; set; }
        public decimal valorPeca { get; set; }

        internal static void AtualizaPeca(Roupa roupaAtualizada)
        {
            SqlConnection conn;
            DynamicParameters p;
            try
            {               
                p = new DynamicParameters();
                conn = new SqlConnection("Server = DESKTOP-AOD1MJI\\SQLEXPRESS; Database = BD_ROUPAS; Trusted_Connection = True;");
                
                p.Add("@idRoupa", roupaAtualizada.idRoupa);
                p.Add("@Nome", roupaAtualizada.nomePeca);
                p.Add("@Tamanho", roupaAtualizada.tamanhoPeca);
                p.Add("@Cor", roupaAtualizada.corpeca);
                p.Add("@Preco", roupaAtualizada.valorPeca);
                conn.Execute("SP_Atualiza_Roupas", p, commandType: CommandType.StoredProcedure);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"erro: {ex.Message}");
                
            }
        }

        internal static void DeletarPeca(int id)
        {
            SqlConnection conn;
            DynamicParameters p;
            
            try
            {
                p = new DynamicParameters();
                conn = new SqlConnection("Server = DESKTOP-AOD1MJI\\SQLEXPRESS; Database = BD_ROUPAS; Trusted_Connection = True;");
                
                p.Add("@idRoupa", id);
                conn.Execute("SP_Deleta_Roupas", p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"erro: {ex.Message}");               
            }
        }

        internal static void InserirRoupa(Roupa novaRoupa)
        {
            SqlConnection conn;
            DynamicParameters p;
            
            try
            {
                p = new DynamicParameters();
                conn = new SqlConnection("Server = DESKTOP-AOD1MJI\\SQLEXPRESS; Database = BD_ROUPAS; Trusted_Connection = True;");

                p.Add("@Nome", novaRoupa.nomePeca);
                p.Add("@Tamanho", novaRoupa.tamanhoPeca);
                p.Add("@Cor", novaRoupa.corpeca);
                p.Add("@Preco", novaRoupa.valorPeca);
                conn.Execute("SP_Insere_Roupas", p, commandType: CommandType.StoredProcedure);              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"erro: {ex.Message}");               
            }
        }

        internal static List<Roupa> ObterRoupas()
        {
            SqlConnection conn;
            DynamicParameters p;
            List<Roupa> ret;

            try
            {
                p= new DynamicParameters();
                conn = new SqlConnection("Server = DESKTOP-AOD1MJI\\SQLEXPRESS; Database = BD_ROUPAS; Trusted_Connection = True;");
                ret = new List<Roupa>();
               
                ret = conn.Query<Roupa>("SP_Seleciona_Roupas", p, commandType: CommandType.StoredProcedure).ToList();

                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"erro: {ex.Message}");
                return new List<Roupa>();
            }

        }


        internal static Roupa ObterRoupaUnica(int id)
        {
            SqlConnection conn;
            DynamicParameters p;
            Roupa ret;

            try
            {
                p = new DynamicParameters();
                conn = new SqlConnection("Server = DESKTOP-AOD1MJI\\SQLEXPRESS; Database = BD_ROUPAS; Trusted_Connection = True;");
                ret = new Roupa();

                p.Add("@idRoupa", id);
                ret = conn.Query<Roupa>("SP_Seleciona_Roupas", p, commandType: CommandType.StoredProcedure).FirstOrDefault();

                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"erro: {ex.Message}");
                return new Roupa();
            }

        }



    }
}
