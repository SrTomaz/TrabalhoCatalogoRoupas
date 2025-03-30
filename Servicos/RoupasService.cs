namespace CatalogoRoupas.Servicos
{
    using CatalogoRoupas.Models;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    public class RoupasService
    {
        public RoupasService() 
        {
        }

        public void Create(Roupa roupa)
        {
            if (string.IsNullOrEmpty(roupa.nomePeca))
            {
                throw new ArgumentException("Nome da peça não pode ser vazio");
            }
            Roupa.InserirRoupa(roupa);                      
           
        }

        public Roupa? Read(int id)
        {           
            Roupa oRoupas;          

            oRoupas = Roupa.ObterRoupaUnica(id);
             
            return oRoupas;
        }

        public void Update(Roupa roupa)
        {
            var ProcuraRoupa = Read(roupa.idRoupa);
                if (ProcuraRoupa == null)
                {
                    throw new KeyNotFoundException("Roupa não encontrada.");
                }

                Roupa.AtualizaPeca(roupa);                    
        }

        public void Delete(int id)
        {
           
                var roupa = Read(id);
                if (roupa == null)
                {
                     throw new KeyNotFoundException("Roupa não encontrada.");                    
                }
                Roupa.DeletarPeca(id);                           
            
        }
    }

}
