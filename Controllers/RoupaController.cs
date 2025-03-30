using Microsoft.AspNetCore.Mvc;
using CatalogoRoupas.Models;
using System.Collections.Generic;

namespace CatalogoRoupas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoupaController : Controller
    {
        [HttpGet(nameof(ProcuraRoupas))]
        public ActionResult<IEnumerable<Roupa>> ProcuraRoupas()
        {
            List<Roupa> oRoupas = new List<Roupa>();

            oRoupas = Roupa.ObterRoupas();

            return Ok(oRoupas);
        }

        // GET: api/roupa/1
        [HttpGet($"{nameof(ProcuraRoupa)}/{{id}}")]
        public ActionResult<Roupa> ProcuraRoupa(int id)
        {
            Roupa oRoupas = new Roupa();

            oRoupas = Roupa.ObterRoupaUnica(id);

            if (oRoupas == null)
            {
                return NotFound();
            }

            return Ok(oRoupas);
        }

        // POST: api/roupa
        [HttpPost(nameof(InsereRoupa))]
        public ActionResult<Roupa> InsereRoupa(Roupa novaRoupa)
        {
            try
            {              
                if(novaRoupa.valorPeca <= 0)
                {
                    return BadRequest("valor da peca nao pode ser menor ou igual a zero!");
                }
                 Roupa.InserirRoupa(novaRoupa);               
                return Ok("roupa inserida com sucesso!");
            }
            catch (Exception ex)
            {

                return BadRequest($"Ocorreu um erro {ex.Message}");
            }
          
        }

        // PUT: api/roupa/1
        [HttpPut($"{nameof(AtualizaRoupa)}")]        
        public IActionResult AtualizaRoupa(Roupa roupaAtualizada)
        {
            try
            {
                if (roupaAtualizada.idRoupa <= 0)
                {
                    return BadRequest("id da peca não pode ser menor ou igual a zero!");
                }

                Roupa.AtualizaPeca(roupaAtualizada);
              
                    return Ok("peça atualizada com sucesso!");              
            }
            catch (Exception ex)
            {

                return BadRequest($"ocorreu um erro: {ex.Message}");
            }
        }


        // DELETE: api/roupa/1
        [HttpDelete($"{nameof(DeletaRoupa)}/{{id}}")]
        public IActionResult DeletaRoupa(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("id da peca não pode ser menor ou igual a zero!");
                }


                Roupa.DeletarPeca(id);

                return Ok("Roupa deletada com sucesso!");
            }
            catch (Exception ex)
            {

                return BadRequest($"ocorreu um erro: {ex.Message}");
            }
        }
    }
}
