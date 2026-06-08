using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresControllers : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresControllers(ApplicationDbContext context)
        {
            this.context = context;
        }

        #region Get()

        [HttpGet]
        public async Task<IEnumerable<Autor>> Get()
        {
            return await context.Autores.ToListAsync();
        }

        #endregion

        #region Get(int id)

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await context.Autores
                .Include(x => x.Libros)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (autor is null)
            {
                return NotFound();
            }
            return autor;
        }

        #endregion

        #region GetPrimerAutor()

        [HttpGet("primero")]
        public async Task<Autor> GetPrimerAutor()
        {
            return await context.Autores
                .Include(x => x.Libros)
                .FirstAsync();
        }

        #endregion

        #region Post(Autor autor)

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        #endregion

        #region [HttpPut("{id:int}")]

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest("Los ids deben coincidir");
            }
            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        #endregion

        #region [HttpDelete("{id:int}")]

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registrosBorrado = await context.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();
            if (registrosBorrado == 0)
            {
                return NotFound();
            }
            return Ok();
        }

        #endregion

    }
}