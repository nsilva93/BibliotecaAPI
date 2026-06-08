using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        #region Get()

        [HttpGet]
        public async Task<IEnumerable<Libro>> Get()
        {
            return await context.Libros.
                ToListAsync();
        }

        #endregion

        #region Get(int id)

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            var libro = await context.Libros.
                Include(x => x.Autor).
                FirstOrDefaultAsync(x => x.Id == id);
            if (libro is null)
            {
                return NotFound();
            }
            return libro;
        }

        #endregion

        #region Post(Libro libro)

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);
            if (!existeAutor)
            {
                return BadRequest($"El autor de id {libro.AutorId} no existe");
            }
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        #endregion

        #region Put(int id, Libro libro)

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest("Los ids deben coincidir");
            }
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);
            if (!existeAutor)
            {
                return BadRequest($"El autor de id {libro.AutorId} no existe");
            }
            context.Update(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        #endregion

        #region Delete(int id)

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registrosBorrado = await context.Libros.Where(x => x.Id == id).ExecuteDeleteAsync();
            if (registrosBorrado == 0)
            {
                return NotFound();
            }
            return Ok();
        }

        #endregion

    }
}
