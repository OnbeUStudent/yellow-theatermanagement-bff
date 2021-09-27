using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dii_MovieCatalogSvc.Fake.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
//test
namespace Dii_MovieCatalogSvc.Fake.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieCatalogSvcContext _context;

        public MoviesController(MovieCatalogSvcContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movie
                .Include(movie => movie.MovieMetadata)
                .ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(string id)
        {
            if (!Guid.TryParse(id, out Guid movieIdAsGuid))
            {
                return NotFound();
            }
            var movie = await _context.Movie
                .Include(movie => movie.MovieMetadata)
                .SingleOrDefaultAsync(movie => movie.MovieId == movieIdAsGuid);
            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutMovie(string id, Movie movie)
        {
            if (!Guid.TryParse(id, out Guid movieIdAsGuid))
            {
                return NotFound();
            }
            if (movieIdAsGuid != movie.MovieId)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movieIdAsGuid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.MovieId }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/MovieMetadatas/5/MovieMetadatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/MovieMetadatas")]
        public async Task<IActionResult> PutMovieMetadata(string id, MovieMetadata movieMetadata)
        {
            if (!Guid.TryParse(id, out Guid movieIdAsGuid))
            {
                return NotFound();
            }
            if (movieIdAsGuid != movieMetadata.MovieMetadataId)
            {
                return BadRequest();
            }

            _context.Entry(movieMetadata).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movieIdAsGuid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool MovieExists(Guid id)
        {
            return _context.Movie.Any(e => e.MovieId == id);
        }
    }
}
