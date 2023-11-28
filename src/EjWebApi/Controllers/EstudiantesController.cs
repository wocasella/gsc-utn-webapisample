using EjWebApi.DataAccess;
using EjWebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EjWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly AcademiaDbContext context;

        public EstudiantesController(AcademiaDbContext context) => this.context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetAll()
        {
            return await this.context.Estudiantes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante?>> GetById(int id)
        {
            var estudiante = await this.context.FindAsync<Estudiante>(id);

            if (estudiante == null)
                return this.NotFound();

            return estudiante;
        }

        [HttpPost]
        public async Task<ActionResult<Estudiante>> Create(Estudiante estudiante)
        {
            await this.context.AddAsync(estudiante);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction(nameof(GetById), new { id = estudiante.Id }, estudiante);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Estudiante estudiante)
        {
            this.context.Update(estudiante);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var estudiante = await this.context.FindAsync<Estudiante>(id);

            if (estudiante == null)
                return this.NotFound();

            this.context.Remove(estudiante);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }
    }
}
